using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class Vector3RectStep : IVector3, ICopyable<Vector3RectStep>, IEquatable<Vector3RectStep>
    {
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        [JsonProperty(Names.MinZ)]
        public float MinZ { get; set; }
        
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        [JsonProperty(Names.MaxZ)]
        public float MaxZ { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Step)]
        public float Step { get; set; }

        public Vector3RectStep()
        {
            MinX = 0f;
            MinY = 0f;
            MinZ = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            MaxZ = 1f;
            
            Step = 1f;
        }
        public Vector3RectStep(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float step)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
            
            Step = step;
        }

        public VectorType GetModelType() => VectorType.RandomRectStep;

        public object Clone() => Copy();
        IVector3 ICopyable<IVector3>.Copy() => new Vector3RectStep(MinX, MinY, MinZ, MaxX, MaxY, MaxZ, Step);
        public Vector3RectStep Copy() => new(MinX, MinY, MinZ, MaxX, MaxY, MaxZ, Step);

        public override bool Equals(object obj) => obj is Vector3RectStep value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinX, MinY, MinZ, MaxX, MaxY, MaxZ, Step);
        
        public bool Equals(IVector3 other) => other is Vector3RectStep value && Equals(value);
        public bool Equals(Vector3RectStep other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinX.Equals(other.MinX)
                         && MinY.Equals(other.MinY)
                         && MinZ.Equals(other.MinZ)
                         && MaxX.Equals(other.MaxX)
                         && MaxY.Equals(other.MaxY)
                         && MaxZ.Equals(other.MaxZ)
                         && Step.Equals(other.Step);
            return result;
        }
    }
}