using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Game
{
    public class MarkerV1 : IMarker
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Frame { get; set; }
        public IColor Color { get; set; }

        public MarkerV1()
        {
            Name = string.Empty;
            Description = string.Empty;
            Frame = 0;
            Color = new ColorValueV1();
        }
        public MarkerV1(string name, string description, int frame, IColor color)
        {
            Name = name;
            Description = description;
            Frame = frame;
            Color = color;
        }
    }
}