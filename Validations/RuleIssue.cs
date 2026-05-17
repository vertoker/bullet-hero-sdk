using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;

namespace BHSDK.Validations
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
                if (!path.HasIndex) continue;
                if (result is IList list)
                    result = list[path.Index];
                else if (result.GetType().IsArray)
                    result = ((Array)result).GetValue(path.Index);
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

                if (!path.HasIndex) continue;
                if (result is IList list)
                    result = list[path.Index];
                else if (result.GetType().IsArray)
                    result = ((Array)result).GetValue(path.Index);
            }
            var resultProperty = Trace[^1].Property;
            return (result, resultProperty);
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