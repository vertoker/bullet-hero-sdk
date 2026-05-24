using BH.SDK.Rules;

namespace BH.SDK.Generators
{
    public abstract class ObjectsParameters
    {
        public readonly int StartFrame;
        public readonly int EndFrame;
        public readonly int Layer;

        protected ObjectsParameters(int startFrame, int endFrame, int layer = ObjectRules.DefaultLayer)
        {
            StartFrame = startFrame;
            EndFrame = endFrame;
            Layer = layer;
        }
    }
}