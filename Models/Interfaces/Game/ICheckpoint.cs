using BHSDK.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.V1.Game;
using UnityEngine;

namespace BHSDK.Models.Interfaces.Game
{
    public interface ICheckpoint : IModelReflection<CheckpointV1, ICheckpoint>
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Frame { get; set; }
        public IColor Color { get; set; }
    }
}