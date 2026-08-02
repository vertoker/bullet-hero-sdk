using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Interfaces
{
    // Implemented by whichever model owns its own independent timeline length - LevelSettings for a
    // level's own timeline, Prefab for a template's own local timeline (see PrefabRules.
    // DefaultFrameLength / Prefab.FrameLength's own comment). Lets timeline-bound editor code (the
    // in-game editor's Prefab Timeline) size itself generically without caring which kind of scope
    // it's bounding, same reasoning as IObjectScope/IObjectIdCounter's own split.
    public interface IFrameLength
    {
        [RuleMin(FrameRules.MinFrameLength)]
        [JsonProperty(Names.FrameLengthShort)]
        public int FrameLength { get; set; }
    }
}
