using System.Collections.Generic;
using BH.SDK.Models.Objects;

namespace BH.SDK.Models.Interfaces
{
    public interface IObjectScope
    {
        public List<Object> Objects { get; set; }
        public List<PrefabObject> PrefabObjects { get; set; }
    }
}