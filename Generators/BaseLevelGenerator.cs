using BH.SDK.Models;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules;

namespace BH.SDK.Generators
{
    public abstract class BaseLevelGenerator<TInputParameters>
        where TInputParameters : BaseLevelGenerator<TInputParameters>.BaseInputParameters
    {
        public abstract Level GenerateLevel(TInputParameters parameters);
        public abstract LevelMeta GenerateMeta(TInputParameters parameters);
        
        public abstract class BaseInputParameters
        {
            public readonly IString LevelName;
            public readonly IString LevelDescription;
            
            protected BaseInputParameters(IString levelName, IString levelDescription)
            {
                LevelName = levelName;
                LevelDescription = levelDescription;
            }
        }
    }
}