using BHSDK.Models.Interfaces.Game;
using UnityEngine;

namespace BHSDK.Models.V1.Game
{
    public class MarkerV1 : IMarker
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Frame { get; set; } = 0;
        public Color32 Color { get; set; } = new(255, 255, 255, 255);
    }
}