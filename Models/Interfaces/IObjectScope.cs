using System.Collections.Generic;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Models.Interfaces
{
    public interface IObjectScope
    {
        public Dictionary<ObjectId, RectObject> Objects { get; set; }
        public List<PrefabObject> PrefabObjects { get; set; }
    }
}