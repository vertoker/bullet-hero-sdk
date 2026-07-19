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
        // Camera - is a unique form of default RectObject. It cut some data, here's all
        // Positions - unchanged
        // Layers - camera is not an TextureObject, you can't see it, it's otherwise, remove
        // Rotations - unchanged
        // Scales - always (1f, 1f) because of aspect, remove
        // Size - camera aspect is locked by user screen, also Level.Settings provide tool for
        // manual setup aspect (IScreenLimit). Because of that
        // changed to Zooms - Size with 1 value applied to xy simultaneously
        // AnchorsMin/AnchorsMax - camera doesn't have any parents, remove
        // Pivot - unchanged
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxCameraKeys)]
        [RuleCollectionUnique(nameof(PosKey.Frame))]
        [JsonProperty(Names.Position)]
        public List<PosKey> Positions { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxCameraKeys)]
        [RuleCollectionUnique(nameof(AngleKey.Frame))]
        [JsonProperty(Names.Rotation)]
        public List<AngleKey> Rotations { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxCameraKeys)]
        [RuleCollectionUnique(nameof(ZoomKey.Frame))]
        [JsonProperty(Names.Zoom)]
        public List<ZoomKey> Zooms { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxObjectKeys)]
        [RuleCollectionUnique(nameof(AlignmentKey.Frame))]
        [JsonProperty(Names.Pivot)]
        public List<AlignmentKey> Pivots { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxCameraKeys)]
        [RuleCollectionUnique(nameof(ShakeKey.Frame))]
        [JsonProperty(Names.Shake)]
        public List<ShakeKey> Shakes { get; set; }

        public CameraEvents()
        {
            Positions = new List<PosKey>();
            Rotations = new List<AngleKey>();
            Zooms = new List<ZoomKey>();
            Pivots = new List<AlignmentKey>();
            Shakes = new List<ShakeKey>();
        }
        public CameraEvents(List<PosKey> positions, List<AngleKey> rotations,
            List<ZoomKey> zooms, List<AlignmentKey> pivots, List<ShakeKey> shakes)
        {
            Positions = positions;
            Rotations = rotations;
            Zooms = zooms;
            Pivots = pivots;
            Shakes = shakes;
        }

        public object Clone() => Copy();
        public CameraEvents Copy() => new(Positions.CopyList(), Rotations.CopyList(),
            Zooms.CopyList(), Pivots.CopyList(), Shakes.CopyList());

        public override bool Equals(object obj) => obj is CameraEvents value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Positions.GetListHashCode(),
            Rotations.GetListHashCode(), Zooms.GetListHashCode(), Pivots.GetListHashCode(), Shakes.GetListHashCode());

        public bool Equals(CameraEvents other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Positions.ListEquals(other.Positions)
                         && Rotations.ListEquals(other.Rotations)
                         && Zooms.ListEquals(other.Zooms)
                         && Pivots.ListEquals(other.Pivots)
                         && Shakes.ListEquals(other.Shakes);
            return result;
        }
    }
}