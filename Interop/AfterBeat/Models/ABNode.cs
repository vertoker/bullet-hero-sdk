using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Interop.AfterBeat.Models
{
    // Every node in this format keeps what it did not recognise, and that is the single most
    // important decision in this folder. The wiki these models were transcribed from is marked
    // "mostly copied directly" and is openly behind the game on at least one feature (custom
    // polygon shapes are in the editor and their keys are undocumented), so a real level almost
    // certainly carries keys nothing here has a property for. Without this they would vanish on the
    // first export, silently, and only in somebody else's level.
    //
    // It is also what makes a round trip a meaningful test rather than a comparison of the subset
    // this file happens to know about.

    /// <summary>
    /// Base of every Afterbeat wire model. Deliberately does NOT implement the SDK's IModel&lt;T&gt;
    /// pattern - these are one-shot deserialization targets like the Versions/VX_Y/ snapshots, not
    /// domain objects.
    /// </summary>
    public abstract class ABNode
    {
        /// <summary> Keys this build has no property for, preserved verbatim across a round trip. </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> Unknown { get; set; } = new Dictionary<string, JToken>();
    }
}
