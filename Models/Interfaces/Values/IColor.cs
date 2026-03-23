using BHSDK.Models.Enum.Values;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IColor
    {
        public ColorType GetModelType();
        public Color Get();
    }
}