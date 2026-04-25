using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface ICollisionShape
    {
        public ShapeType GetModelType();
    }
}