using System;
using System.Collections.Generic;
using System.IO;
using BHSDK.Models;
using BHSDK.Models.Audio;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum.Meta;
using BHSDK.Models.Enum.Resources;
using BHSDK.Models.Enum.Settings;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Events;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Meta;
using BHSDK.Models.Objects;
using BHSDK.Models.PostProcessing;
using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using BHSDK.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

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

            var data = new SaveData<EffectObject>(effect);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Cat.Meow($"Effect - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<SaveData<EffectObject>>(reader);
        }

        public static EffectObject CreateTestEffect()
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
            var json = serializationService.SerializeData(level);
            Cat.Meow($"Level - <color=green>{json}</color>");

            var level2 = serializationService.DeserializeData<Level>(json);
            Assert.IsTrue(level.Equals(level2));
        }
        
        public static Level CreateTestLevel()
        {
            var level = new Level();

            level.Settings.Framerate = 61;
            level.Settings.ScreenLimit = new ScreenLimitBounds();
            
            level.Game.Events.Backgrounds.Add(new Clr());
            level.Game.Events.Checkpoints.Add(new Checkpoint());
            level.Game.Events.Markers.Add(new Marker());
            level.Game.Events.Themes.Add(new ThemeKeyframe());
            level.Game.CameraEvents.Positions.Add(new Pos());
            level.Game.CameraEvents.Rotations.Add(new Rot());
            level.Game.CameraEvents.Shakes.Add(new Shake());
            level.Game.CameraEvents.Zooms.Add(new Zoom());

            level.Game.PostProcessingEvents.Blooms.Add(new Bloom());
            level.Game.PostProcessingEvents.Chromas.Add(new ChromaticAberration());
            level.Game.PostProcessingEvents.Vignettes.Add(new Vignette());
            level.Game.PostProcessingEvents.Lenses.Add(new LensDistortion());
            level.Game.PostProcessingEvents.Grains.Add(new FilmGrain());
            level.Game.PostProcessingEvents.MotionBlurs.Add(new MotionBlur());
            level.Game.PostProcessingEvents.ColorCurveses.Add(new ColorCurves());
            level.Game.PostProcessingEvents.LiftGammaGains.Add(new LiftGammaGain());
            level.Game.PostProcessingEvents.ShadowsMidtonesHighlightses.Add(new ShadowsMidtonesHighlights());
            level.Game.PostProcessingEvents.WhiteBalances.Add(new WhiteBalance());
            level.Game.PostProcessingEvents.AnalogGlitches.Add(new AnalogGlitch());
            level.Game.PostProcessingEvents.DigitalGlitches.Add(new DigitalGlitch());

            level.Game.PlayerEvents.Visibles.Add(new BoolKey());
            level.Game.PlayerEvents.Collisions.Add(new BoolKey());

            var textureObject = new TextureObject()
            {
                ObjectId = 1,
            };
            textureObject.Positions.Add(new Pos());
            textureObject.Rotations.Add(new Rot());
            textureObject.Scales.Add(new Sca());
            textureObject.Colors.Add(new Clr());
            level.Game.Objects.Add(textureObject);

            var textObject = new TextObject()
            {
                ObjectId = 2,
            };
            textObject.Colors.Add(new Clr());
            level.Game.Objects.Add(textObject);

            var effectObject = new EffectObject()
            {
                ObjectId = 3,
            };
            level.Game.Objects.Add(effectObject);

            var prefab = new Prefab()
            {
                PrefabGuid = Guid.NewGuid(),
            };
            prefab.Objects.Add(new TextureObject()
            {
                ObjectId = 1,
            });
            prefab.Objects.Add(new TextObject()
            {
                ObjectId = 2,
            });
            prefab.Objects.Add(new EffectObject()
            {
                ObjectId = 3,
            });
            level.Game.Prefabs.Add(prefab);
            
            var prefabObject = new PrefabObject { PrefabGuid = Guid.NewGuid() };
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
            var track = new LevelTrack(1, 0, 10, 0f, 0, trackEffects);
            level.Audio.Tracks.Add(track);

            return level;
        }
        
        public static Level CreateInvalidTestLevel()
        {
            var level = new Level();

            level.Settings.Framerate = -15;
            level.Settings.ScreenLimit = null;
            
            level.Game.Events.Backgrounds.Add(new Clr());
            level.Game.Events.Checkpoints.Add(new Checkpoint());
            level.Game.Events.Markers.Add(new Marker());
            level.Game.Events.Themes.Add(new ThemeKeyframe());
            level.Game.CameraEvents.Positions.Add(new Pos());
            level.Game.CameraEvents.Rotations.Add(new Rot());
            level.Game.CameraEvents.Shakes.Add(new Shake());
            level.Game.CameraEvents.Zooms.Add(new Zoom());

            level.Game.PostProcessingEvents.Blooms.Add(new Bloom());
            level.Game.PostProcessingEvents.Chromas.Add(new ChromaticAberration());
            level.Game.PostProcessingEvents.Vignettes.Add(new Vignette());
            level.Game.PostProcessingEvents.Lenses.Add(new LensDistortion());
            level.Game.PostProcessingEvents.Grains.Add(new FilmGrain());
            level.Game.PostProcessingEvents.MotionBlurs.Add(new MotionBlur());
            level.Game.PostProcessingEvents.ColorCurveses.Add(new ColorCurves());
            level.Game.PostProcessingEvents.LiftGammaGains.Add(new LiftGammaGain());
            level.Game.PostProcessingEvents.ShadowsMidtonesHighlightses.Add(new ShadowsMidtonesHighlights());
            level.Game.PostProcessingEvents.WhiteBalances.Add(new WhiteBalance());
            level.Game.PostProcessingEvents.AnalogGlitches.Add(new AnalogGlitch());
            level.Game.PostProcessingEvents.DigitalGlitches.Add(new DigitalGlitch());

            level.Game.PlayerEvents.Visibles.Add(new BoolKey());
            level.Game.PlayerEvents.Collisions.Add(new BoolKey());

            var textureObject = new TextureObject();
            textureObject.Positions.Add(new Pos());
            textureObject.Rotations.Add(new Rot());
            textureObject.Scales.Add(new Sca());
            textureObject.Colors.Add(new Clr());
            level.Game.Objects.Add(textureObject);

            var textObject = new TextObject();
            textObject.Colors.Add(new Clr());
            level.Game.Objects.Add(textObject);

            var effectObject = new EffectObject();
            level.Game.Objects.Add(effectObject);

            var prefab = new Prefab();
            prefab.Objects.Add(new TextureObject());
            prefab.Objects.Add(new TextObject());
            prefab.Objects.Add(new EffectObject());
            level.Game.Prefabs.Add(prefab);
            
            var prefabObject = new PrefabObject();
            level.Game.PrefabObjects.Add(prefabObject);
            
            level.Game.Themes.Add(new Theme());
            
            level.Resources.Textures.Add(new TextureResource(0, new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png")
            }));
            level.Resources.Fonts.Add(new FontResource(0, new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://github.com/google/fonts/raw/refs/heads/main/ofl/dekko/Dekko-Regular.ttf"),
            }));
            level.Resources.Audios.Add(new AudioResource(0, new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/7/7a/%22six-seven%22.ogg"),
            }));

            var trackEffects = new LevelTrackEffects();
            var track = new LevelTrack(1, 0, 1000, 0f, 0, trackEffects);
            level.Audio.Tracks.Add(track);

            return level;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestLevelMetaSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var levelMeta = CreateTestLevelMeta();
            var json = serializationService.SerializeData(levelMeta);
            Cat.Meow($"LevelMeta - <color=green>{json}</color>");

            var levelMeta2 = serializationService.DeserializeData<LevelMeta>(json);
            Assert.IsTrue(levelMeta.Equals(levelMeta2));
        }

        public static LevelMeta CreateTestLevelMeta()
        {
            var meta = new LevelMeta();
            meta.LevelGuid = Guid.NewGuid();
            meta.LevelVersion = new Version(1, 0);
            meta.LevelName = new StringValue("cool level");
            meta.LevelDescription = new StringValue("cool description");
            meta.LevelLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0);
            meta.LevelAuthors = new List<Author>
            {
                new(new StringValue("vertoker"), "vertoker.com"),
            };
            meta.ResourcesMeta = new List<ResourceMeta>
            {
                new()
                {
                    ResourceType = ResourceType.Audio,
                    ResourceId = -1,
                    ResourceTitle = new StringValue("Spider Dance"),
                    ResourceDescription = new StringValue("Cool boss track from Undertale"),
                    ResourceUrl = "https://www.youtube.com/watch?v=NH-GAwLAO30",
                    ResourceLicense = new TypicalLicense(TypicalLicenseType.Proprietary),
                    ResourceAuthors = new List<Author>
                    {
                        new(new StringValue("Toby Fox"), "https://x.com/tobyfox"),
                    },
                },
            };
            return meta;
        }

        public static LevelMeta CreateInvalidTestLevelMeta()
        {
            var meta = new LevelMeta();
            meta.LevelVersion = null;
            meta.LevelLicense = null;
            meta.ResourcesMeta = new List<ResourceMeta>
            {
                new()
                {
                    ResourceType = ResourceType.Audio,
                    ResourceId = -1,
                    ResourceTitle = null,
                    ResourceDescription = null,
                    ResourceUrl = null,
                    ResourceLicense = null,
                    ResourceAuthors = new List<Author>
                    {
                        new(new StringValue("Toby Fox"), "https://x.com/tobyfox"),
                    },
                },
            };
            return meta;
        }
        
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestPrefabSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var prefab = CreateTestPrefab();

            var data = new SaveData<Prefab>(prefab);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Cat.Meow($"Prefab - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<SaveData<Prefab>>(reader);
        }

        public static Prefab CreateTestPrefab()
        {
            var prefab = new Prefab();
            prefab.Objects.Add(new TextureObject());
            prefab.Objects.Add(new TextObject());
            prefab.Objects.Add(new EffectObject());
            
            var prefabObject = new PrefabObject
            {
                PrefabGuid = Guid.NewGuid(),
            };
            var modification = new Modification
            {
                ObjectId = 123,
                Path = "sf",
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

            var data = new SaveData<Theme>(theme);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, data);
            var json = textWriter.ToString();
            Cat.Meow($"Theme - <color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            data = serializationService.Serializer.Deserialize<SaveData<Theme>>(reader);
        }

        public static Theme CreateTestTheme()
        {
            var theme = new Theme
            {
                Matrix =
                {
                    [1] = ColorValue.red,
                    [2] = ColorValue.green,
                    [3] = ColorValue.blue,
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

            var testSettings = CreateValidTestSettings();

            var json = serializationService.SerializeData(testSettings);
            Cat.Meow($"Settings - <color=green>{json}</color>");

            var testSettings2 = serializationService.DeserializeData<UserSettings>(json);
            Assert.IsTrue(testSettings.Equals(testSettings2));
        }

        public static UserSettings CreateValidTestSettings()
        {
            var settings = new UserSettings();
            return settings;
        }
        public static UserSettings CreateInvalidTestSettings()
        {
            var settings = new UserSettings
            {
                General =
                {
                    ResourceParallelLoadCount = -1,
                    ResourceWebTimeout = -1f
                },
                Controls =
                {
                    ClassicControlsType = ClassicControlsType.Mouse
                },
                Audio =
                {
                    Game = 1.5f,
                    UI = -1f
                },
                Graphics =
                {
                    FixedFramerate = 1000,
                    Effects =
                    {
                        FixedFramerate = -1
                    },
                    PostProcessing =
                    {
                        RenderColorCurves = false
                    }
                },
                GameEditor =
                {
                    CameraMinSize = -23f,
                    CameraMaxSize = 23f
                }
            };
            return settings;
        }
    }
}