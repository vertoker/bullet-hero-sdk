using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class CompositeColliderShape
    {
        public const int MaxCount = 64;
        
        // id for user-defined colliders, allowed only negative (started with -1, 0 is uninitialized)
        // this works like resource, but integrated in model. Works only with TextureObject
        
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [JsonProperty(Names.TrianglesShort)]
        public List<TriangleColliderShape> Triangles { get; set; }

        public CompositeColliderShape()
        {
            ColliderId = 0;
            Triangles = new List<TriangleColliderShape>();
        }
        public CompositeColliderShape(int colliderId, List<TriangleColliderShape> triangles)
        {
            ColliderId = colliderId;
            Triangles = triangles;
        }
    }
}