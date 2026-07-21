using BH.SDK.Models.Enum.Values;

namespace BH.SDK.Models.Interfaces.Values
{
    public interface IColor3 : IModel<IColor3>
    {
        public ColorType GetModelType();
    }
}
