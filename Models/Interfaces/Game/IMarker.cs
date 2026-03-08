using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Game;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Game
{
    public interface IMarker : IModelReflection<MarkerV1, IMarker>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Frame { get; set; }
        public IColor Color { get; set; }
    }
}