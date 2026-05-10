using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BHSDK.Models;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;

namespace BHSDK.Validations
{
    public class LevelValidator
    {
        private readonly List<LevelPath> _paths = new(16);
        
        public List<LevelIssue> Validate(Level level, LevelValidatorSettings settings)
        {
            var result = new List<LevelIssue>();

            ValidateRecursive(level, settings, result, level);
            _paths.Clear();
            
            return result;
        }
        
        private void ValidateRecursive(object context, LevelValidatorSettings settings, List<LevelIssue> result, Level level)
        {
            if (context == null) return;
            
            var objType = context.GetType();
            var ruleContainer = objType.GetCustomAttribute<RuleContainerAttribute>();
            if (ruleContainer == null) return;
            
            // Cat.Meow(objType);
            var objProperties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            var nextObjects = new List<object>();
            var nextProperties = new List<PropertyInfo>();
            foreach (var property in objProperties)
            {
                var nextObj = property.GetValue(context);
                var rules = property.GetCustomAttributes<BaseRuleAttribute>(true);

                var validRule = rules.FirstOrDefault(rule => rule.HasIsValid && !rule.IsValid(nextObj, level));

                _paths.Add(new LevelPath(property));
                if (validRule != null)
                {
                    var issue = new LevelIssue(validRule, context, new List<LevelPath>(_paths));
                    result.Add(issue);
                    Cat.MeowWarn(issue);
                }
                else if (nextObj != null)
                {
                    nextObjects.Add(nextObj);
                    nextProperties.Add(property);
                }
                _paths.RemoveAt(_paths.Count - 1);
            }

            for (var index = 0; index < nextObjects.Count; index++)
            {
                var nextObj = nextObjects[index];
                var nextProp = nextProperties[index];
                
                if (nextObj is IList list)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        _paths.Add(new LevelPath(nextProp, i));
                        ValidateRecursive(list[i], settings, result, level);
                        _paths.RemoveAt(_paths.Count - 1);
                    }
                }
                else if (nextObj.GetType().IsArray)
                {
                    var array = (Array)nextObj;
                    for (var i = 0; i < array.Length; i++)
                    {
                        _paths.Add(new LevelPath(nextProp, i));
                        ValidateRecursive(array.GetValue(i), settings, result, level);
                        _paths.RemoveAt(_paths.Count - 1);
                    }
                }
                else
                {
                    _paths.Add(new LevelPath(nextProp));
                    ValidateRecursive(nextObj, settings, result, level);
                    _paths.RemoveAt(_paths.Count - 1);
                }
            }
        }
        
        private static bool IsTerminalType(Type type)
        {
            if (type.IsPrimitive) return true;
            if (type.IsEnum) return true;
            
            if (type == typeof(string)) return true;
            if (type == typeof(decimal)) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(DateTimeOffset)) return true;
            if (type == typeof(Guid)) return true;
            if (type == typeof(TimeSpan)) return true;

            // if (type.Namespace != null && type.Namespace.StartsWith("System"))
            //     return true;
            
            return false;
        }
    }
}