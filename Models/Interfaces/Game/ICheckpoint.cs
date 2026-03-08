using UnityEngine;

namespace BHSDK.Models.Interfaces.Game
{
    public interface ICheckpoint
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Frame { get; set; }
        public Color32 Color { get; set; }
    }
}