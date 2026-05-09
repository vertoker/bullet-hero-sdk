using System.Collections.Generic;
using BHSDK.Models;

namespace BHSDK.Validations.Rules
{
    public class LevelValidator : ILevelValidator
    {
        public void Validate(Level level, LevelValidatorSettings settings, List<LevelIssue> results)
        {
            if (settings.EnableNullChecks)
            {
                if (level == null)
                {
                    results.Add(LevelIssue.Error(LevelErrorType.NullReference));
                    return;
                }
                
                if (level.Settings == null)
                    results.Add(LevelIssue.Error(LevelErrorType.NullReference, level, nameof(level.Settings)));
                if (level.Meta == null)
                    results.Add(LevelIssue.Error(LevelErrorType.NullReference, level, nameof(level.Meta)));
                if (level.Game == null)
                    results.Add(LevelIssue.Error(LevelErrorType.NullReference, level, nameof(level.Game)));
                if (level.Audio == null)
                    results.Add(LevelIssue.Error(LevelErrorType.NullReference, level, nameof(level.Audio)));
                if (level.Resources == null)
                    results.Add(LevelIssue.Error(LevelErrorType.NullReference, level, nameof(level.Resources)));
            }
        }
    }
}