using System.Collections.Generic;
using System.IO;
using BHSDK.Models;
using BHSDK.Models.Audio;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Events;
using BHSDK.Models.Keyframes;
using BHSDK.Models.NoGame;
using BHSDK.Models.Objects;
using BHSDK.Models.PostProcessing;
using BHSDK.Models.Resources;
using BHSDK.Models.SaveData;
using BHSDK.Models.Settings;
using BHSDK.Models.Values;
using BHSDK.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace BHSDK.Tests
{
    public class SerializationTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestEffectSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var effect = CreateTestEffect();

            var data = new EffectData(effect);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Debug.Log($"Effect - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<EffectData>(reader);
        }

        private static EffectObject CreateTestEffect()
        {
            var effect = new EffectObject
            {
                Name = "TestEffect",
                Core = new EffectObjectCore
                {
                    Loop = false,
                    ParticleCount = 1200,
                },
                EffectAngle = new EffectAngleCurvesBySpeed
                {
                    Curve = new CurveValue(new List<CurveKeyframeValue>
                    {
                        new(), new()
                    }, CurveWrapMode.Default, CurveWrapMode.Default),
                    SpeedRange = new Vector2Circle(0f, 1f, 2f),
                },
                EffectColor = new EffectColorGradientRandom
                {
                    Gradient = new GradientValue(new List<GradientColorKeyValue>
                    {
                        new()
                    }, new List<GradientAlphaKeyValue>
                    {
                        new()
                    }, GradientInterpolationMode.PerceptualBlend, GradientColorSpace.Linear)
                },
                EffectScale = new EffectScaleCurvesBySpeed
                {
                    CurveX = new CurveValue(),
                    CurveY = new CurveValue(),
                    SpeedRange = new Vector2Value(),
                },
                EffectShape = new EffectShapeCircle
                {
                    Arc = new FloatValue(6.29f),
                    Radius = new FloatValue(1f),
                    Spread = new EffectShapeSpreadLoop(1f, 2f),
                    Thickness = new FloatValue(1f),
                },
            };
            return effect;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestLevelSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var level = CreateTestLevel();

            var data = new LevelData(level);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Debug.Log($"Level - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<LevelData>(reader);
        }
        
        private static Level CreateTestLevel()
        {
            var level = new Level();

            level.Settings.Framerate = 61;
            level.Settings.ScreenLimit = new ScreenLimitBounds();
            
            level.Meta.Authors.Add(new Author());
            
            level.Game.Events.Backgrounds.Add(new Clr());
            level.Game.Events.Checkpoints.Add(new Checkpoint());
            level.Game.Events.Markers.Add(new Marker());
            level.Game.Events.Themes.Add(new ThemeKeyframe());
            level.Game.CameraEvents.Pos.Add(new Pos());
            level.Game.CameraEvents.Rot.Add(new Rot());
            level.Game.CameraEvents.Shake.Add(new Shake());
            level.Game.CameraEvents.Zoom.Add(new Zoom());

            level.Game.PostProcessingEvents.Bloom.Add(new Bloom());
            level.Game.PostProcessingEvents.Chroma.Add(new ChromaticAberration());
            level.Game.PostProcessingEvents.Vignette.Add(new Vignette());
            level.Game.PostProcessingEvents.Lens.Add(new LensDistortion());
            level.Game.PostProcessingEvents.Grain.Add(new FilmGrain());
            level.Game.PostProcessingEvents.MotionBlur.Add(new MotionBlur());
            level.Game.PostProcessingEvents.ColorCurves.Add(new ColorCurves());
            level.Game.PostProcessingEvents.LiftGammaGain.Add(new LiftGammaGain());
            level.Game.PostProcessingEvents.ShadowsMidtonesHighlights.Add(new ShadowsMidtonesHighlights());
            level.Game.PostProcessingEvents.WhiteBalance.Add(new WhiteBalance());
            level.Game.PostProcessingEvents.AnalogGlitch.Add(new AnalogGlitch());
            level.Game.PostProcessingEvents.DigitalGlitch.Add(new DigitalGlitch());

            level.Game.PlayerEvents.Visibles.Add(new BoolKey());
            level.Game.PlayerEvents.Collisions.Add(new BoolKey());

            var textureObject = new TextureObject();
            textureObject.Pos.Add(new Pos());
            textureObject.Rot.Add(new Rot());
            textureObject.Sca.Add(new Sca());
            textureObject.Clr.Add(new Clr());
            level.Game.Objects.Add(textureObject);

            var textObject = new TextObject();
            textObject.Clr.Add(new Clr());
            level.Game.Objects.Add(textObject);

            var effectObject = new EffectObject();
            level.Game.Objects.Add(effectObject);

            var prefab = new Prefab();
            prefab.Objects.Add(new TextureObject());
            prefab.Objects.Add(new TextObject());
            prefab.Objects.Add(new EffectObject());
            level.Game.Prefabs.Add(prefab);
            
            var prefabObject = new PrefabObject { PrefabIndex = 0 };
            level.Game.PrefabObjects.Add(prefabObject);
            
            level.Game.Themes.Add(new Theme());
            
            level.Resources.Textures.Add(new TextureResource(-1, new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png")
            }));
            level.Resources.Fonts.Add(new FontResource(-1, new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://github.com/google/fonts/raw/refs/heads/main/ofl/dekko/Dekko-Regular.ttf"),
            }));
            level.Resources.Audios.Add(new AudioResource(-1, new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/7/7a/%22six-seven%22.ogg"),
            }));

            var trackEffects = new LevelTrackEffects();
            var track = new LevelTrack(1, "cool song", "vertoker", 0, 1000, 0f,
                0, trackEffects);
            level.Audio.Tracks.Add(track);

            return level;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestPrefabSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var prefab = CreateTestPrefab();

            var data = new PrefabData(prefab);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Debug.Log($"Prefab - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<PrefabData>(reader);
        }

        private static Prefab CreateTestPrefab()
        {
            var prefab = new Prefab();
            prefab.Objects.Add(new TextureObject());
            prefab.Objects.Add(new TextObject());
            prefab.Objects.Add(new EffectObject());
            
            var prefabObject = new PrefabObject
            {
                PrefabIndex = 0
            };
            var modification = new Modification
            {
                ObjectId = 123,
                Path = "id",
                Value = 321
            };
            prefabObject.Modifications.Add(modification);
            prefab.PrefabObjects.Add(prefabObject);
            
            return prefab;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestThemeSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var theme = CreateTestTheme();

            var data = new ThemeData(theme);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Debug.Log($"Theme - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<ThemeData>(reader);
        }

        private static Theme CreateTestTheme()
        {
            var theme = new Theme
            {
                Matrix =
                {
                    [1] = new ColorValue(Color.red),
                    [2] = new ColorValue(Color.green),
                    [3] = new ColorValue(Color.blue)
                }
            };
            return theme;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestPlayerSettingsSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var playerSettings = CreateTestPlayerSettings();

            var data = new PlayerSettingsData(playerSettings);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Debug.Log($"PlayerSettings - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<PlayerSettingsData>(reader);
        }

        private PlayerSettings CreateTestPlayerSettings()
        {
            var settings = new PlayerSettings();
            return settings;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestEditorSettingsSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var editorSettings = CreateTestEditorSettings();

            var data = new EditorSettingsData(editorSettings);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Debug.Log($"EditorSettings - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<EditorSettingsData>(reader);
        }

        private EditorSettings CreateTestEditorSettings()
        {
            var settings = new EditorSettings();
            return settings;
        }
    }
}