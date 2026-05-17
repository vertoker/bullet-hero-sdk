using System.Collections.Generic;
using BHSDK.Models;

namespace BHSDK.Validations
{
    public class RuleFixer
    {
        public void Fix(List<RuleIssue> issues, object obj, RuleFixerSettings settings)
        {
            Cat.Meow("Fix");
            foreach (var issue in issues)
            {
                var (context, property) = issue.GetContextAndProperty();
                issue.Rule.Fix(context, property, obj);
            }
        }
    }
}