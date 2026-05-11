using System.Collections.Generic;
using BHSDK.Models;

namespace BHSDK.Validations
{
    public class LevelFixer
    {
        public void Fix(List<LevelIssue> issues, Level level, LevelFixerSettings settings)
        {
            Cat.Meow("Fix");
            foreach (var issue in issues)
            {
                var (context, property) = issue.GetContextAndProperty();
                issue.Rule.Fix(context, property, level);
            }
        }
    }
}