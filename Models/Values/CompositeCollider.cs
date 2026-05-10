using System.Collections.Generic;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class CompositeCollider
    {
        public const int MaxCount = 64;
        
        // id for user-defined colliders, allowed only negative (started with -1, 0 is uninitialized)
        // this works like resource, but integrated in model. Works only with TextureObject
        
        [RuleMax(0)]
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(MaxCount)]
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