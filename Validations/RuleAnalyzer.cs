using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;

namespace BH.SDK.Validations
{
    public class RuleAnalyzer
    {
        private readonly List<RulePath> _trace = new(16);
        private readonly Dictionary<Type, PropertyInfo[]> _typesCache = new(32);
        private readonly Dictionary<PropertyInfo, BasePropertyRuleAttribute[]> _rulesCache = new(32);
        private readonly Dictionary<Type, BaseObjectRuleAttribute[]> _objectRulesCache = new(32);
        private readonly Stack<List<(object, PropertyInfo)>> _nextObjectsPool;

        public RuleAnalyzer()
        {
            _nextObjectsPool = new Stack<List<(object, PropertyInfo)>>(16);
            for (var i = 0; i < 16; i++)
                _nextObjectsPool.Push(new List<(object, PropertyInfo)>(8));

            // Warm the caches for every [RuleContainer] type in the assembly - not just typeof(Level) -
            // so aggregate roots that aren't reachable from Level's own object graph (UserSettings,
            // LevelMeta, and any future one) get cached too. Analyze() itself never hardcoded Level;
            // this was only ever a warm-up gap.
            var visited = new HashSet<Type>();
            foreach (var contextType in GetType().Assembly.GetTypes())
            {
                if (contextType.GetCustomAttribute<RuleContainerAttribute>() != null)
                    CacheRecursively(contextType, visited);
            }
        }

        private void CacheRecursively(Type contextType, HashSet<Type> visited)
        {
            if (!visited.Add(contextType)) return;

            var ruleContainer = contextType.GetCustomAttribute<RuleContainerAttribute>();
            if (ruleContainer == null) return;

            GetObjectRules(contextType);
            var objProperties = GetObjProperties(contextType);

            foreach (var property in objProperties)
            {
                GetRules(property);

                if (property.PropertyType.IsList())
                    CacheRecursively(property.PropertyType.GetGenericArguments()[0], visited);
                else if (property.PropertyType.IsArray)
                    CacheRecursively(property.PropertyType.GetElementType(), visited);
                else if (property.PropertyType.IsDictionary())
                    CacheRecursively(property.PropertyType.GetDictionaryValueGenericParameterOrDefault(), visited);
                else CacheRecursively(property.PropertyType, visited);
            }
        }

        public List<RuleIssue> Analyze(object obj, RuleAnalyzerSettings settings)
        {
            var result = new List<RuleIssue>(8);

            // Cat.Meow("Analyze");
            try
            {
                AnalyzeRecursive(obj, settings, result, RuleContext.ForRoot(obj));
            }
            finally
            {
                // A misapplied rule aborts the walk by design, and the analyzer is meant to be
                // reusable afterwards - so the trace is reset unconditionally, not only on success.
                // Leaving it behind poisons every later issue's path with a dead prefix.
                _trace.Clear();
            }

            return result;
        }

        private void AnalyzeRecursive(object target, RuleAnalyzerSettings settings,
            List<RuleIssue> result, RuleContext context)
        {
            if (target == null) return;
            var targetType = target.GetType();

            var ruleContainer = targetType.GetCustomAttribute<RuleContainerAttribute>();
            if (ruleContainer == null) return;

            // Entering a scope of its own (a prefab template) rebases everything scope-relative -
            // frames against its own timeline, parent ids against its own reserved targets - for
            // this subtree and everything below it.
            if (target is IFrameScope frameScope) context = context.WithScope(frameScope);

            // Object rules run before the property walk, so an issue spanning two properties is
            // reported against the object owning both - and lands in the trace before either of
            // them, which is what makes the reverse-order fix pass repair the pair first.
            foreach (var rule in GetObjectRules(targetType))
            {
                if (!rule.IsValidType(targetType))
                {
                    throw new ArgumentException($"Can't apply rule {rule.GetType().Name} " +
                                                $"to type {targetType.Name}, path: {_trace.GetPath()}");
                }

                if (!settings.Reports(rule.Group)) continue;
                if (rule.IsValid(target, context)) continue;

                var objectIssue = new RuleIssue(rule, context, new List<RulePath>(_trace));
                result.Add(objectIssue);
                Cat.MeowWarn(objectIssue);
            }

            var objProperties = GetObjProperties(targetType);

            // The pool is a reuse optimization, never a capacity limit: a graph deeper than the
            // prewarmed count is legal, so an exhausted pool allocates instead of throwing. Returning
            // the buffer through finally is what keeps it a pool at all - the misapplied-rule throw
            // below is by design, and leaking one buffer per such throw drained a shared analyzer
            // dry, after which every later Analyze died on an empty stack instead of reporting rules.
            var nextObjects = _nextObjectsPool.Count > 0
                ? _nextObjectsPool.Pop()
                : new List<(object, PropertyInfo)>(8);

            try
            {
                foreach (var property in objProperties)
                {
                    var rules = GetRules(property);
                    var nextObj = property.GetValue(target);
                    _trace.Add(new RulePath(property));

                    var hasInvalidRule = false;
                    foreach (var rule in rules)
                    {
                        // TODO move to Roslyn Analyzer, this must not be in runtime
                        if (!rule.IsValidType(property))
                        {
                            throw new ArgumentException($"Can't apply rule {rule.GetType().Name} " +
                                                     $"to type {nextObj?.GetType().Name}, path: {_trace.GetPath()}");
                        }

                        if (!settings.Reports(rule.Group)) continue;

                        if (!rule.IsValid(nextObj, context))
                        {
                            hasInvalidRule = true;
                            var issue = new RuleIssue(rule, context, new List<RulePath>(_trace));
                            result.Add(issue);
                            Cat.MeowWarn(issue);

                            if (!settings.analyzeAllPropertyRules) break;
                        }
                    }

                    if (hasInvalidRule)
                    {
                        if (settings.analyzeAllRecursiveRules && nextObj != null)
                            nextObjects.Add((nextObj, property));
                    }
                    else if (nextObj != null)
                    {
                        nextObjects.Add((nextObj, property));
                    }

                    _trace.RemoveAt(_trace.Count - 1);
                }

                foreach (var (nextObj, nextProp) in nextObjects)
                {
                    if (nextObj is IList list)
                    {
                        for (var i = 0; i < list.Count; i++)
                        {
                            _trace.Add(new RulePath(nextProp, i));
                            AnalyzeRecursive(list[i], settings, result, context);
                            _trace.RemoveAt(_trace.Count - 1);
                        }
                    }
                    else if (nextObj is IDictionary dictionary)
                    {
                        // Values only, deliberately. A RulePath addresses "this property, at this key",
                        // which is how RuleFixer finds its way back to a value - there is no way to
                        // express "the key itself", so an issue raised on a key would be repaired into
                        // the value instead. Keys stay safe without the walk: every key in the format is
                        // an id struct, and a struct can never be a [RuleContainer] anyway (it is boxed
                        // on the way in, so any Fix would write into a copy and vanish). Anything a key
                        // needs checking for - above all whether it agrees with its value's own id - is
                        // relational, and belongs to the graph pass rather than here.
                        foreach (DictionaryEntry entry in dictionary)
                        {
                            _trace.Add(new RulePath(nextProp, entry.Key));
                            AnalyzeRecursive(entry.Value, settings, result, context);
                            _trace.RemoveAt(_trace.Count - 1);
                        }
                    }
                    else if (nextObj.GetType().IsArray)
                    {
                        var array = (Array)nextObj;
                        for (var i = 0; i < array.Length; i++)
                        {
                            _trace.Add(new RulePath(nextProp, i));
                            AnalyzeRecursive(array.GetValue(i), settings, result, context);
                            _trace.RemoveAt(_trace.Count - 1);
                        }
                    }
                    else
                    {
                        _trace.Add(new RulePath(nextProp));
                        AnalyzeRecursive(nextObj, settings, result, context);
                        _trace.RemoveAt(_trace.Count - 1);
                    }
                }
            }
            finally
            {
                nextObjects.Clear();
                _nextObjectsPool.Push(nextObjects);
            }
        }

        private PropertyInfo[] GetObjProperties(Type objType)
        {
            if (!_typesCache.TryGetValue(objType, out var objProperties))
            {
                objProperties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(property => property.CanRead && property.CanWrite).ToArray();

                _typesCache.Add(objType, objProperties);
            }
            return objProperties;
        }
        private BasePropertyRuleAttribute[] GetRules(PropertyInfo property)
        {
            if (!_rulesCache.TryGetValue(property, out var rules))
            {
                rules = property.GetCustomAttributes<BasePropertyRuleAttribute>(true).ToArray();
                _rulesCache.Add(property, rules);
            }
            return rules;
        }

        private BaseObjectRuleAttribute[] GetObjectRules(Type type)
        {
            if (!_objectRulesCache.TryGetValue(type, out var rules))
            {
                rules = type.GetCustomAttributes<BaseObjectRuleAttribute>(true).ToArray();
                _objectRulesCache.Add(type, rules);
            }
            return rules;
        }
    }
}
