using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;

namespace BH.SDK.Validations
{
    public class RuleAnalyzer
    {
        private readonly List<RulePath> _trace = new(16);
        private readonly Dictionary<Type, PropertyInfo[]> _typesCache = new(32);
        private readonly Dictionary<PropertyInfo, BaseRuleAttribute[]> _rulesCache = new(32);

        public RuleAnalyzer()
        {
            CacheRecursively(typeof(Level));
        }

        private void CacheRecursively(Type contextType)
        {
            var ruleContainer = contextType.GetCustomAttribute<RuleContainerAttribute>();
            if (ruleContainer == null) return;
            
            var objProperties = GetObjProperties(contextType);

            foreach (var property in objProperties)
            {
                GetRules(property);
                
                if (property.PropertyType.IsList())
                    CacheRecursively(property.PropertyType.GetGenericArguments()[0]);
                else if (property.PropertyType.IsArray)
                    CacheRecursively(property.PropertyType.GetElementType());
                else CacheRecursively(property.PropertyType);
            }
        }

        public List<RuleIssue> Analyze(object obj, RuleAnalyzerSettings settings)
        {
            var result = new List<RuleIssue>(4);

            Cat.Meow("Analyze");
            AnalyzeRecursive(obj, settings, result, obj);
            _trace.Clear();
            
            return result;
        }
        
        private void AnalyzeRecursive(object context, RuleAnalyzerSettings settings, List<RuleIssue> result, object obj)
        {
            if (context == null) return;
            var contextType = context.GetType();
            
            var ruleContainer = contextType.GetCustomAttribute<RuleContainerAttribute>();
            if (ruleContainer == null) return;
            
            var objProperties = GetObjProperties(contextType);
            
            var nextObjects = new List<(object, PropertyInfo)>();
            foreach (var property in objProperties)
            {
                var rules = GetRules(property);
                var nextObj = property.GetValue(context);
                _trace.Add(new RulePath(property));
                
                BaseRuleAttribute invalidRule = null;
                foreach (var rule in rules)
                {
                    // TODO move to Roslyn Analyzer, this must not be in runtime
                    if (!rule.IsValidType(property))
                    {
                        throw new ArgumentException($"Can't apply rule {rule.GetType().Name} " +
                                                 $"to type {nextObj?.GetType().Name}, path: {_trace.GetPath()}");
                    }
                    
                    if (!rule.IsValid(nextObj, obj))
                    {
                        invalidRule = rule;
                        break;
                    }
                }

                if (invalidRule != null)
                {
                    var issue = new RuleIssue(invalidRule, obj, new List<RulePath>(_trace));
                    result.Add(issue);
                    Cat.MeowWarn(issue);
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
                        AnalyzeRecursive(list[i], settings, result, obj);
                        _trace.RemoveAt(_trace.Count - 1);
                    }
                }
                else if (nextObj.GetType().IsArray)
                {
                    var array = (Array)nextObj;
                    for (var i = 0; i < array.Length; i++)
                    {
                        _trace.Add(new RulePath(nextProp, i));
                        AnalyzeRecursive(array.GetValue(i), settings, result, obj);
                        _trace.RemoveAt(_trace.Count - 1);
                    }
                }
                else
                {
                    _trace.Add(new RulePath(nextProp));
                    AnalyzeRecursive(nextObj, settings, result, obj);
                    _trace.RemoveAt(_trace.Count - 1);
                }
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
        private BaseRuleAttribute[] GetRules(PropertyInfo property)
        {
            if (!_rulesCache.TryGetValue(property, out var rules))
            {
                rules = property.GetCustomAttributes<BaseRuleAttribute>(true).ToArray();
                _rulesCache.Add(property, rules);
            }
            return rules;
        }
    }
}