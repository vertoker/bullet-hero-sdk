using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class ObjectIdModification
    {
        [JsonProperty(Names.PrevObjectId)]
        public int PrevObjectId { get; set; }
        
        [JsonProperty(Names.NextObjectId)]
        public int NextObjectId { get; set; }

        public ObjectIdModification()
        {
            PrevObjectId = ObjectStatic.UndefinedId;
            NextObjectId = ObjectStatic.UndefinedId;
        }
        public ObjectIdModification(int prevObjectId, int nextObjectId)
        {
            PrevObjectId = prevObjectId;
            NextObjectId = nextObjectId;
        }
    }
}