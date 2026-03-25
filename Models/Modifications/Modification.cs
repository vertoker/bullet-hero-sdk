using Newtonsoft.Json;

namespace BHSDK.Models.Modifications
{
    // Limitations for modifications
    
    // 1. Works only for Object and Prefab, not applied to anything else
    
    // 2. You can't make Object a child or any Object in Prefab, no parenting from low levels
    // (but you still can make root of prefab inherit from outside Object, parenting from high levels is allowed)
    
    // 3. Modification works only for prefab scope where it's located.
    // No deep inheritance of changes
    
    public class Modification
    {
        [JsonProperty(ModelNames.ObjectId)]
        public int ObjectId { get; set; } // to which Object this modification is applied
        
        [JsonProperty(ModelNames.Path)]
        public string Path { get; set; }
        
        [JsonProperty(ModelNames.Value)]
        public object Value { get; set; }

        public Modification()
        {
            ObjectId = 0;
            Path = string.Empty;
            Value = null;
        }
        public Modification(int objectId, string path, object value)
        {
            ObjectId = objectId;
            Path = path;
            Value = value;
        }
    }
}