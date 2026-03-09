using BHSDK.Models.Interfaces.Game;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Values;
using UnityEngine;

namespace BHSDK.Models.V1.Game
{
    public class CheckpointV1 : ICheckpoint
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Frame { get; set; }
        public IColor Color { get; set; }

        public CheckpointV1()
        {
            Name = string.Empty;
            Active = true;
            Frame = 0;
            Color = new ColorValueV1(1f, 1f, 1f, 1f);
        }
        public CheckpointV1(string name, bool active, int frame, IColor color)
        {
            Name = name;
            Active = active;
            Frame = frame;
            Color = color;
        }
    }
}