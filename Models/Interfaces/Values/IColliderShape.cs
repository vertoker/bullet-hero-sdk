using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IColliderShape
    {
        public ColliderShapeType GetModelType();
    }
}