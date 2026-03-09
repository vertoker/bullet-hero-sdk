using BHSDK.Models.Enum.Values;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IColor
    {
        public ColorType Type { get; }
        public Color Get();
    }
}