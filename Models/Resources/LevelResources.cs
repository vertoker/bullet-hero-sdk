using System;
using System.Collections.Generic;
using BH.SDK.Models.Data;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    [DataVersion(DataDomains.LevelResources, 1, 0)]
    public class LevelResources : IModel<LevelResources>
    {
        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own TextureResourceId)
        [RuleNotNull]
        [JsonProperty(Names.Textures)]
        public Dictionary<TextureResourceId, TextureResource> Textures { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own FontResourceId)
        [RuleNotNull]
        [JsonProperty(Names.Fonts)]
        public Dictionary<FontResourceId, FontResource> Fonts { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own AudioResourceId)
        [RuleNotNull]
        [JsonProperty(Names.Audios)]
        public Dictionary<AudioResourceId, AudioResource> Audios { get; set; }


        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ColliderId)
        [RuleNotNull]
        [JsonProperty(Names.Shapes)]
        public Dictionary<ColliderId, CompositeCollider> CompositeShapes { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ThemeId)
        [JsonProperty(Names.Themes)]
        public Dictionary<ThemeId, ThemeData> Themes { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ThemeId)
        [JsonProperty(Names.Effects)]
        public Dictionary<EffectId, EffectData> Effects { get; set; }

        
        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own PrefabId)
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPrefabs)]
        [JsonProperty(Names.Prefabs)]
        public Dictionary<PrefabId, Prefab> Prefabs { get; set; }
        
        public LevelResources()
        {
            Textures = new Dictionary<TextureResourceId, TextureResource>();
            Fonts = new Dictionary<FontResourceId, FontResource>();
            Audios = new Dictionary<AudioResourceId, AudioResource>();

            CompositeShapes = new Dictionary<ColliderId, CompositeCollider>();
            Themes = new Dictionary<ThemeId, ThemeData>();
            Effects = new Dictionary<EffectId, EffectData>();
            
            Prefabs = new Dictionary<PrefabId, Prefab>();
        }
        public LevelResources(Dictionary<TextureResourceId, TextureResource> textures,
            Dictionary<FontResourceId, FontResource> fonts,
            Dictionary<AudioResourceId, AudioResource> audios,
            Dictionary<ColliderId, CompositeCollider> compositeShapes,
            Dictionary<ThemeId, ThemeData> themes,
            Dictionary<EffectId, EffectData> effects,
            Dictionary<PrefabId, Prefab> prefabs)
        {
            Textures = textures;
            Fonts = fonts;
            Audios = audios;
            CompositeShapes = compositeShapes;
            Themes = themes;
            Effects = effects;
            Prefabs = prefabs;
        }
        public void Reset()
        {
            Textures.Clear();
            Fonts.Clear();
            Audios.Clear();
            CompositeShapes.Clear();
            Themes.Clear();
            Effects.Clear();
            Prefabs.Clear();
        }

        public object Clone() => Copy();
        public LevelResources Copy() => new(Textures.CopyDictionary(), Fonts.CopyDictionary(), Audios.CopyDictionary(),
            CompositeShapes.CopyDictionary(), Themes.CopyDictionary(), Effects.CopyDictionary(), Prefabs.CopyDictionary());

        public override bool Equals(object obj) => obj is LevelResources value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Textures.GetDictionaryHashCode(),
            Fonts.GetDictionaryHashCode(), Audios.GetDictionaryHashCode(), CompositeShapes.GetDictionaryHashCode(),
            Themes.GetDictionaryHashCode(), Effects.GetDictionaryHashCode(), Prefabs.GetDictionaryHashCode());

        public bool Equals(LevelResources other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Textures.DictionaryEquals(other.Textures)
                          && Fonts.DictionaryEquals(other.Fonts)
                          && Audios.DictionaryEquals(other.Audios)
                          && CompositeShapes.DictionaryEquals(other.CompositeShapes)
                          && Themes.DictionaryEquals(other.Themes)
                          && Effects.DictionaryEquals(other.Effects)
                          && Prefabs.DictionaryEquals(other.Prefabs);
            return result;
        }
    }
}