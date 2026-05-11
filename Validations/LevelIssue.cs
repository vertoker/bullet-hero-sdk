using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;

namespace BHSDK.Validations
{
    public readonly struct LevelIssue
    {
        public readonly BaseRuleAttribute Rule;
        public readonly object LevelRoot;
        public readonly List<LevelPath> Trace;

        public LevelIssue(BaseRuleAttribute rule, object levelRoot, List<LevelPath> trace)
        {
            Rule = rule;
            LevelRoot = levelRoot;
            Trace = trace;
        }

        public object GetValue()
        {
            var result = LevelRoot;
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
            var result = LevelRoot;
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
        
        public string GetPath()
        {
            if (Trace.Count == 0) return string.Empty;
            var builder = new StringBuilder();
            BuildTrace(builder);
            return builder.ToString();
        }
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Issue, Rule: ");
            builder.Append(Rule.GetType().Name);
            builder.Append(", Trace: ");
            BuildTrace(builder);
            return builder.ToString();
        }
        private void BuildTrace(StringBuilder builder)
        {
            if (Trace.Count == 0) return;
            for (var i = 0; i < Trace.Count - 1; i++)
            {
                var path = Trace[i];
                path.Append(builder);
                builder.Append('.');
            }
            Trace[^1].Append(builder);
        }
    }
}