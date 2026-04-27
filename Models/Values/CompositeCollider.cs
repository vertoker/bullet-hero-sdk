using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class CompositeCollider
    {
        public const int MaxCount = 64;
        
        // id for user-defined colliders, allowed only negative (started with -1, 0 is uninitialized)
        // this works like resource, but integrated in model. Works only with TextureObject
        
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [JsonProperty(Names.TrianglesShort)]
        public List<TriangleCollider> Triangles { get; set; }

        public CompositeCollider()
        {
            ColliderId = 0;
            Triangles = new List<TriangleCollider>();
        }
        public CompositeCollider(int colliderId, List<TriangleCollider> triangles)
        {
            ColliderId = colliderId;
            Triangles = triangles;
        }
    }
}