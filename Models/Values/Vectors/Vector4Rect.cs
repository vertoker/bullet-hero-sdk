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
    public class Vector4Rect : IVector4, ICopyable<Vector4Rect>, IEquatable<Vector4Rect>
    {
        [JsonProperty(Names.MinX)]
        public float MinX { get; set; }
        
        [JsonProperty(Names.MinY)]
        public float MinY { get; set; }
        
        [JsonProperty(Names.MinZ)]
        public float MinZ { get; set; }
        
        [JsonProperty(Names.MinW)]
        public float MinW { get; set; }
        
        [JsonProperty(Names.MaxX)]
        public float MaxX { get; set; }
        
        [JsonProperty(Names.MaxY)]
        public float MaxY { get; set; }
        
        [JsonProperty(Names.MaxZ)]
        public float MaxZ { get; set; }
        
        [JsonProperty(Names.MaxW)]
        public float MaxW { get; set; }

        public Vector4Rect()
        {
            MinX = 0f;
            MinY = 0f;
            MinZ = 0f;
            MinW = 0f;
            
            MaxX = 1f;
            MaxY = 1f;
            MaxZ = 1f;
            MaxW = 1f;
        }
        public Vector4Rect(float minX, float minY, float minZ, float minW, 
            float maxX, float maxY, float maxZ, float maxW)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MinW = minW;
            
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
            MaxW = maxW;
        }

        public VectorType GetModelType() => VectorType.RandomRect;

        public object Clone() => Copy();
        IVector4 ICopyable<IVector4>.Copy() => new Vector4Rect(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW);
        public Vector4Rect Copy() => new(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW);

        public override bool Equals(object obj) => obj is Vector4Rect value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(MinX, MinY, MinZ, MinW, MaxX, MaxY, MaxZ, MaxW);
        
        public bool Equals(IVector4 other) => other is Vector4Rect value && Equals(value);
        public bool Equals(Vector4Rect other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MinX.Equals(other.MinX)
                         && MinY.Equals(other.MinY)
                         && MinZ.Equals(other.MinZ)
                         && MinW.Equals(other.MinW)
                         && MaxX.Equals(other.MaxX)
                         && MaxY.Equals(other.MaxY)
                         && MaxZ.Equals(other.MaxZ)
                         && MaxW.Equals(other.MaxW);
            return result;
        }
    }
}