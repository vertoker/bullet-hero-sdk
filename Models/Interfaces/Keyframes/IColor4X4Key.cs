using BH.SDK.Models.Enum.Keyframes;

namespace BH.SDK.Models.Interfaces.Keyframes
{
    public interface IColor4X4Key : IKeyframe, IModel<IColor4X4Key>
    {
        public Color4X4KeyType GetModelType();
    }
}