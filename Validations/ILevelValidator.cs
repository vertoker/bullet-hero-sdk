using System.Collections.Generic;
using BHSDK.Models;

namespace BHSDK.Validations
{
    public interface ILevelValidator
    {
        public void Validate(Level level, LevelValidatorSettings settings, List<LevelIssue> results);
    }
}