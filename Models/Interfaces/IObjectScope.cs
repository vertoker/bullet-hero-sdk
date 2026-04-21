using System.Collections.Generic;
using BHSDK.Models.Objects;

namespace BHSDK.Models.Interfaces
{
    public interface IObjectScope
    {
        public List<Object> Objects { get; set; }
        public List<PrefabObject> PrefabObjects { get; set; }
    }
}