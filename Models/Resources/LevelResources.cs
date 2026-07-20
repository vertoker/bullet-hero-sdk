using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Resources
{
    [RuleContainer]
    public class LevelResources : IModel<LevelResources>
    {
        [RuleNotNull]
        [JsonProperty(Names.Textures)]
        public Dictionary<TextureResourceId, TextureResource> Textures { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Fonts)]
        public Dictionary<FontResourceId, FontResource> Fonts { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Audios)]
        public Dictionary<AudioResourceId, AudioResource> Audios { get; set; }
        
        
        [RuleNotNull]
        [JsonProperty(Names.Shapes)]
        public Dictionary<ColliderId, CompositeCollider> CompositeShapes { get; set; }

        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxThemes)]
        [JsonProperty(Names.Themes)]
        public Dictionary<ThemeId, Theme> Themes { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(LevelRules.MaxPrefabs)]
        [JsonProperty(Names.Prefabs)]
        public Dictionary<PrefabId, Prefab> Prefabs { get; set; }
        
        public LevelResources()
        {
            Textures = new Dictionary<TextureResourceId, TextureResource>();
            Fonts = new Dictionary<FontResourceId, FontResource>();
            Audios = new Dictionary<AudioResourceId, AudioResource>();

            CompositeShapes = new Dictionary<ColliderId, CompositeCollider>();
            Themes = new Dictionary<ThemeId, Theme>();
            Prefabs = new Dictionary<PrefabId, Prefab>();
        }
        public LevelResources(Dictionary<TextureResourceId, TextureResource> textures,
            Dictionary<FontResourceId, FontResource> fonts,
            Dictionary<AudioResourceId, AudioResource> audios,
            Dictionary<ColliderId, CompositeCollider> compositeShapes,
            Dictionary<ThemeId, Theme> themes, Dictionary<PrefabId, Prefab> prefabs)
        {
            Textures = textures;
            Fonts = fonts;
            Audios = audios;
            CompositeShapes = compositeShapes;
            Themes = themes;
            Prefabs = prefabs;
        }
        public void Reset()
        {
            Textures.Clear();
            Fonts.Clear();
            Audios.Clear();
            CompositeShapes.Clear();
            Themes.Clear();
            Prefabs.Clear();
        }

        public object Clone() => Copy();
        public LevelResources Copy() => new(Textures.CopyDictionary(), Fonts.CopyDictionary(), Audios.CopyDictionary(),
            CompositeShapes.CopyDictionary(), Themes.CopyDictionary(), Prefabs.CopyDictionary());

        public override bool Equals(object obj) => obj is LevelResources value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Textures.GetDictionaryHashCode(),
            Fonts.GetDictionaryHashCode(), Audios.GetDictionaryHashCode(), CompositeShapes.GetDictionaryHashCode(),
            Prefabs.GetDictionaryHashCode(), Themes.GetDictionaryHashCode());

        public bool Equals(LevelResources other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Textures.DictionaryEquals(other.Textures)
                          && Fonts.DictionaryEquals(other.Fonts)
                          && Audios.DictionaryEquals(other.Audios)
                          && CompositeShapes.DictionaryEquals(other.CompositeShapes)
                          && Themes.DictionaryEquals(other.Themes)
                          && Prefabs.DictionaryEquals(other.Prefabs);
            return result;
        }
    }
}