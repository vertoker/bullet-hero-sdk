using BH.SDK.Models;
using BH.SDK.Models.Interfaces.Values;

namespace BH.SDK.Generators
{
    public class EmptyLevelGenerator : BaseLevelGenerator<EmptyLevelGenerator.InputParameters>
    {
        public override Level GenerateLevel(InputParameters parameters)
        {
            var level = new Level();
            return level;
        }
        public override LevelMeta GenerateMeta(InputParameters parameters)
        {
            var meta = new LevelMeta
            {
                LevelName = parameters.LevelName.Copy(),
                LevelDescription = parameters.LevelDescription.Copy(),
            };
            return meta;
        }

        public class InputParameters : BaseInputParameters
        {
            public InputParameters(IString levelName, IString levelDescription) : base(levelName, levelDescription)
            {
                
            }
        }
    }
}