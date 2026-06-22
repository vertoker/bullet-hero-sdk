using BH.SDK.Models;
using BH.SDK.Rules;

namespace BH.SDK.Generators
{
    public abstract class BaseGenerator<TInputParameters>
        where TInputParameters : BaseGenerator<TInputParameters>.BaseInputParameters
    {
        public abstract void Generate(Level level, TInputParameters parameters);
        
        public abstract class BaseInputParameters
        {
            public readonly int StartFrame;
            public readonly int EndFrame;
            public readonly int Layer;

            protected BaseInputParameters(int startFrame, int endFrame, int layer = ValueRules.DefaultLayer)
            {
                StartFrame = startFrame;
                EndFrame = endFrame;
                Layer = layer;
            }
        }
    }
}