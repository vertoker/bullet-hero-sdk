using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    [RuleContainer]
    public class ObjectIdModification : ICopyable<ObjectIdModification>
    {
        [JsonProperty(Names.PrevObjectId)]
        public int PrevObjectId { get; set; }
        
        [JsonProperty(Names.NextObjectId)]
        public int NextObjectId { get; set; }

        public ObjectIdModification()
        {
            PrevObjectId = ObjectRules.UndefinedId;
            NextObjectId = ObjectRules.UndefinedId;
        }
        public ObjectIdModification(int prevObjectId, int nextObjectId)
        {
            PrevObjectId = prevObjectId;
            NextObjectId = nextObjectId;
        }

        public object Clone() => Copy();
        public ObjectIdModification Copy() => new(PrevObjectId, NextObjectId);
    }
}