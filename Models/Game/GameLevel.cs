using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Game
{
    [RuleContainer]
    [DataVersion(DataDomains.GameLevel, 1, 0)]
    public class GameLevel : IObjectScope, IModel<GameLevel>
    {
        [RuleNotNull]
        [JsonProperty(Names.Events)]
        public GameEvents Events { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.CameraEvents)]
        public CameraEvents CameraEvents { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.PostProcessingEvents)]
        public PostProcessingEvents PostProcessingEvents { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.PlayerEvents)]
        public PlayerEvents PlayerEvents { get; set; }
        
        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ObjectId)
        // Placed PrefabObject instances live directly in here too (GetModelType() ==
        // ObjectType.PrefabObject) - see IObjectScope's own comment.
        [RuleNotNull]
        [JsonProperty(Names.Objects)]
        public Dictionary<ObjectId, RectObject> Objects { get; set; }

        public GameLevel()
        {
            Events = new GameEvents();
            CameraEvents = new CameraEvents();
            PostProcessingEvents = new PostProcessingEvents();
            PlayerEvents = new PlayerEvents();

            Objects = new Dictionary<ObjectId, RectObject>();
        }
        public GameLevel(GameEvents events, CameraEvents cameraEvents, PostProcessingEvents postProcessingEvents,
            PlayerEvents playerEvents, Dictionary<ObjectId, RectObject> objects)
        {
            Events = events;
            CameraEvents = cameraEvents;
            PostProcessingEvents = postProcessingEvents;
            PlayerEvents = playerEvents;

            Objects = objects;
        }
        public void Reset()
        {
            Events.Reset();
            CameraEvents.Reset();
            PostProcessingEvents.Reset();
            PlayerEvents.Reset();

            Objects.Clear();
        }

        public object Clone() => Copy();
        public GameLevel Copy() => new(Events.Copy(), CameraEvents.Copy(), PostProcessingEvents.Copy(),
            PlayerEvents.Copy(), Objects.CopyDictionary());

        public override bool Equals(object obj) => obj is GameLevel value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Events, CameraEvents, PostProcessingEvents, PlayerEvents,
            Objects.GetDictionaryHashCode());

        public bool Equals(GameLevel other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Events.Equals(other.Events)
                         && CameraEvents.Equals(other.CameraEvents)
                         && PostProcessingEvents.Equals(other.PostProcessingEvents)
                         && PlayerEvents.Equals(other.PlayerEvents)
                         && Objects.DictionaryEquals(other.Objects);
            return result;
        }
    }
}