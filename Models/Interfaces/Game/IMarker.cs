using BHSDK.Interfaces;
using BHSDK.Models.V1.Game;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Game
{
    public interface IMarker : IModelReflection<MarkerV1, IMarker>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Frame { get; set; }
        public Color32 Color { get; set; }
    }
}