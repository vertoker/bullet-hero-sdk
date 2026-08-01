using System.Collections.Generic;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Models.Interfaces
{
    // PrefabObject placements (whether hand-placed in a level or nested inside another Prefab
    // template) live directly in Objects, identified by GetModelType() == ObjectType.PrefabObject -
    // there is no separate list, unlike the SDK's original design.
    public interface IObjectScope
    {
        public Dictionary<ObjectId, RectObject> Objects { get; set; }
    }
}