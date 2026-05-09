using System.Collections.Generic;
using BHSDK.Models;
using BHSDK.Validations.Rules;

namespace BHSDK.Validations
{
    public class LevelValidator
    {
        private readonly List<ILevelValidator> _validators;

        public LevelValidator(IEnumerable<ILevelValidator> rules)
        {
            _validators = new List<ILevelValidator>(rules);
        }

        public List<LevelIssue> Validate(Level level, LevelValidatorSettings settings)
        {
            var results = new List<LevelIssue>();

            foreach (var validator in _validators)
                validator.Validate(level, settings, results);

            return results;
        }

        public static LevelValidator CreateDefault()
        {
            return new LevelValidator(new ILevelValidator[]
            {
                new Rules.LevelValidator(),
            });
        }
    }
}