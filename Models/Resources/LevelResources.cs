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
    /// <summary>
    /// Everything a level owns beyond its objects: seven dictionaries of user-defined resources.
    /// Only user-defined (negative-id) entries ever appear here - anything shipped with the game
    /// lives in the game's own registries and is referenced by id without being copied in.
    /// </summary>
    [RuleContainer]
    [DataVersion(DataDomains.LevelResources, 1, 0)]
    public class LevelResources : IModel<LevelResources>
    {
        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own TextureResourceId)

        /// <summary> Images the level ships with. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Textures)]
        public Dictionary<TextureResourceId, TextureResource> Textures { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own FontResourceId)

        /// <summary> Typefaces the level ships with. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Fonts)]
        public Dictionary<FontResourceId, FontResource> Fonts { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own AudioResourceId)

        /// <summary> Clips the level ships with, including the song itself. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Audios)]
        public Dictionary<AudioResourceId, AudioResource> Audios { get; set; }


        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ColliderId)

        /// <summary> Custom collision shapes, beyond the built-in library. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Shapes)]
        public Dictionary<ColliderId, CompositeCollider> CompositeShapes { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ThemeId)

        /// <summary> Color palettes the level switches between over time. </summary>
        [JsonProperty(Names.Themes)]
        public Dictionary<ThemeId, ThemeData> Themes { get; set; }

        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ThemeId)

        /// <summary> Particle-system definitions, shared by every EffectObject that points at one. </summary>
        [JsonProperty(Names.Effects)]
        public Dictionary<EffectId, EffectData> Effects { get; set; }


        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own PrefabId)

        /// <summary> Reusable object templates. Unlike the other six, these hold level content rather
        /// than external assets - a prefab is authored here, not fetched. </summary>
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