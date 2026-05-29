using System.Collections.Generic;

namespace BH.SDK.Validations
{
    public class RuleFixer
    {
        public void Fix(List<RuleIssue> issues, object obj, RuleFixerSettings settings)
        {
            Cat.Meow("Fix");
            
            // inverse fixing is same variant because some fixes invalidate data for next deeper issues
            // Especially if you using RuleAnalyzerSettings.analyzeAllRecursiveRules
            for (var i = issues.Count - 1; i >= 0; i--)
            {
                var issue = issues[i];
                var (context, property) = issue.GetContextAndProperty();
                issue.Rule.Fix(context, property, obj);
            }
        }
    }
}