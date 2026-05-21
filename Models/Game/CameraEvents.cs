using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    public class CameraEvents : ICopyable<CameraEvents>, IEquatable<CameraEvents>
    {
        // Camera - is a unique instance. It exists all level lifetime and any Object can be child of camera. 
        // Camera has instanceId and this is always -1 (because 0 - is a fallback).
        // Camera GO != camera info for parenting, this info just duplicate from camera.transform 
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxCameraPositions)]
        [RuleCollectionSorted(nameof(Pos.Frame))]
        [RuleCollectionUnique(nameof(Pos.Frame))]
        [JsonProperty(Names.Position)]
        public List<Pos> Positions { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxCameraRotations)]
        [RuleCollectionSorted(nameof(Rot.Frame))]
        [RuleCollectionUnique(nameof(Rot.Frame))]
        [JsonProperty(Names.Rotation)]
        public List<Rot> Rotations { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxCameraZooms)]
        [RuleCollectionSorted(nameof(Zoom.Frame))]
        [RuleCollectionUnique(nameof(Zoom.Frame))]
        [JsonProperty(Names.Zoom)]
        public List<Zoom> Zooms { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxCameraShakes)]
        [RuleCollectionSorted(nameof(Shake.Frame))]
        [RuleCollectionUnique(nameof(Shake.Frame))]
        [JsonProperty(Names.Shake)]
        public List<Shake> Shakes { get; set; }

        public CameraEvents()
        {
            Positions = new List<Pos>();
            Rotations = new List<Rot>();
            Zooms = new List<Zoom>();
            Shakes = new List<Shake>();
        }
        public CameraEvents(List<Pos> positions, List<Rot> rotations, List<Zoom> zooms, List<Shake> shakes)
        {
            Positions = positions;
            Rotations = rotations;
            Zooms = zooms;
            Shakes = shakes;
        }

        public object Clone() => Copy();
        public CameraEvents Copy() => new(Positions.CopyList(), Rotations.CopyList(), Zooms.CopyList(), Shakes.CopyList());

        public override bool Equals(object obj) => obj is CameraEvents value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Positions.GetListHashCode(),
            Rotations.GetListHashCode(), Zooms.GetListHashCode(), Shakes.GetListHashCode());

        public bool Equals(CameraEvents other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Positions.ListEquals(other.Positions)
                         && Rotations.ListEquals(other.Rotations)
                         && Zooms.ListEquals(other.Zooms)
                         && Shakes.ListEquals(other.Shakes);
            return result;
        }
    }
}