using System.Collections.Generic;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Resources
{
    public class CompositeColliderShape
    {
        public const int MaxCount = 128;
        
        // id for user-defined colliders, allowed only negative (started with -1, 0 is uninitialized)
        // this works like resource, but integrated in model. Works only with TextureObject
        
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [JsonProperty(Names.Shapes)]
        public List<IColliderShape> Shapes { get; set; }

        public CompositeColliderShape()
        {
            ColliderId = 0;
            Shapes = new List<IColliderShape>();
        }
        public CompositeColliderShape(int colliderId, List<IColliderShape> shapes)
        {
            ColliderId = colliderId;
            Shapes = shapes;
        }
    }
}