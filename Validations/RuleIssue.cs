using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;

namespace BH.SDK.Validations
{
    public readonly struct RuleIssue
    {
        public readonly BaseRuleAttribute Rule;
        public readonly object Root;
        public readonly List<RulePath> Trace;

        public RuleIssue(BaseRuleAttribute rule, object root, List<RulePath> trace)
        {
            Rule = rule;
            Root = root;
            Trace = trace;
        }

        public object GetValue()
        {
            var result = Root;
            foreach (var path in Trace)
            {
                result = path.Property.GetValue(result);
                if (path.HasKey) result = GetCollectionItem(result, path);
            }
            return result;
        }
        public (object, PropertyInfo) GetContextAndProperty()
        {
            var result = Root;
            for (var i = 0; i < Trace.Count - 1; i++)
            {
                var path = Trace[i];
                result = path.Property.GetValue(result);
                if (path.HasKey) result = GetCollectionItem(result, path);
            }
            var resultProperty = Trace[^1].Property;
            return (result, resultProperty);
        }

        private static object GetCollectionItem(object collection, RulePath path)
        {
            return collection switch
            {
                IDictionary dictionary => dictionary[path.Key],
                IList list => list[(int)path.Key],
                _ when collection.GetType().IsArray => ((Array)collection).GetValue((int)path.Key),
                _ => collection,
            };
        }

        public string GetPath() => Trace.GetPath();
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Issue, Rule: ");
            builder.Append(Rule.GetType().Name);
            builder.Append(", Trace: ");
            Trace.BuildTracePath(builder);
            return builder.ToString();
        }
    }
}