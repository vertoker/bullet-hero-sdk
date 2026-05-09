using System.Collections.Generic;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class CameraEvents
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
    }
}