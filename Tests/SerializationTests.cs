using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Enum.Settings;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Events;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Meta;
using BH.SDK.Models.Objects;
using BH.SDK.Models.PostProcessing;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Serialization;
using BH.SDK.Validations;
using BH.SDK.Versions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BH.SDK.Tests
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

            var json = serializationService.SerializeData(effect);
            Cat.Meow($"Effect - <color=green>{json}</color>");

            var effect2 = serializationService.DeserializeData<EffectObject>(json);
            Assert.IsTrue(effect.Equals(effect2));
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

        // IDataSerializer (VERSION-UPDATE.md, "Format-agnosticism") is generic per [DataVersion]
        // domain, not per concrete type - exercised here against two unrelated domains (Level and
        // Theme) to prove it isn't hardcoded to either one.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestDataSerializerRoundTrip()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);
            var dataSerializer = serializationService.DataSerializer;

            var level = CreateTestLevel();
            var levelAttribute = level.GetType().GetCustomAttribute<DataVersionAttribute>();
            var levelBytes = dataSerializer.SerializeEnvelope(levelAttribute.Domain, levelAttribute.Version, level);
            var (levelVersion, levelPayload) = dataSerializer.DeserializeEnvelope(levelBytes, typeof(Level));
            Assert.AreEqual(levelAttribute.Version, levelVersion);
            Assert.IsTrue(level.Equals((Level)levelPayload));

            var theme = CreateTestTheme();
            var themeAttribute = theme.GetType().GetCustomAttribute<DataVersionAttribute>();
            var themeBytes = dataSerializer.SerializeEnvelope(themeAttribute.Domain, themeAttribute.Version, theme);
            var (themeVersion, themePayload) = dataSerializer.DeserializeEnvelope(themeBytes, typeof(Theme));
            Assert.AreEqual(themeAttribute.Version, themeVersion);
            Assert.IsTrue(theme.Equals((Theme)themePayload));
        }

        // Exercises the full recursive migration chain against a hand-written v0.0 fixture
        // (Versions/V0_0) - Level -> LevelSettings/GameLevel/LevelResources (each independently
        // versioned, auto-upgraded by VersionedEnvelopeConverter) -> GameEvents (nested one level
        // deeper inside GameLevel) -> Audio (intentionally NOT independently versioned at v0.0,
        // migrated by hand inside LevelV0_0ToV1_0 instead). See VERSION-UPDATE.md.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestLevelV0_0Migration()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            const string json = @"{
                ""version"": ""0.0"",
                ""value"": {
                    ""test_settings"": { ""version"": ""0.0"", ""value"": { ""test_fps"": 61 } },
                    ""test_game"": {
                        ""version"": ""0.0"",
                        ""value"": {
                            ""test_game_events"": { ""version"": ""0.0"", ""value"": {} },
                            ""test_objects"": []
                        }
                    },
                    ""test_audio"": {},
                    ""test_resources"": { ""version"": ""0.0"", ""value"": { ""test_resources"": {} } }
                }
            }";

            var level = serializationService.DeserializeData<Level>(json);

            Assert.AreEqual(61, level.Settings.Framerate);
            Assert.AreEqual(610, level.Settings.FrameLength);
            Assert.IsNotNull(level.Game);
            Assert.IsNotNull(level.Game.Events);
            Assert.IsNotNull(level.Game.CameraEvents);
            Assert.IsNotNull(level.Game.PostProcessingEvents);
            Assert.IsNotNull(level.Game.PlayerEvents);
            Assert.AreEqual(0, level.Game.Objects.Count);
            Assert.AreEqual(0, level.Game.PrefabObjects.Count);
            Assert.IsNotNull(level.Audio);
            Assert.IsNotNull(level.Resources);

            // Migration-correctness oracle (see VERSION-UPDATE.md, Rule system section): a migrator's
            // output must never violate a RuleGroup.Error rule against the current-shape model, even
            // though Warning/Advice issues are allowed (e.g. a sparse fixture missing recommended data).
            var validator = new RuleAnalyzer();
            var issues = validator.Analyze(level, new RuleAnalyzerSettings());
            var errors = issues.Where(issue => issue.Rule.Group == RuleGroup.Error).ToList();
            Assert.IsEmpty(errors, string.Join("\n", errors.Select(issue => issue.ToString())));
        }

        public static Level CreateTestLevel()
        {
            var level = new Level();

            level.Settings.Framerate = 61;
            
            level.Game.Events.ScreenLimits.Add(new ScreenLimitKey(new ScreenLimitBounds(), 0));
            level.Game.Events.Backgrounds.Add(new Color3Key());
            level.Game.Events.Checkpoints.Add(new Checkpoint());
            level.Game.Events.Markers.Add(new Marker());
            level.Game.Events.Themes.Add(new ThemeKeyframe());
            level.Game.CameraEvents.Positions.Add(new PosKey());
            level.Game.CameraEvents.Rotations.Add(new AngleKey());
            level.Game.CameraEvents.Shakes.Add(new ShakeKey());
            level.Game.CameraEvents.Zooms.Add(new ZoomKey());

            level.Game.PostProcessingEvents.Blooms.Add(new BloomKey());
            level.Game.PostProcessingEvents.Chromatics.Add(new ChromaticAberrationKey());
            level.Game.PostProcessingEvents.Vignettes.Add(new VignetteKey());
            level.Game.PostProcessingEvents.Lenses.Add(new LensDistortionKey());
            level.Game.PostProcessingEvents.Grains.Add(new FilmGrainKey());
            level.Game.PostProcessingEvents.MotionBlurs.Add(new MotionBlurKey());
            level.Game.PostProcessingEvents.ColorCurveses.Add(new ColorCurvesKey());
            level.Game.PostProcessingEvents.LiftGammaGains.Add(new LiftGammaGainKey());
            level.Game.PostProcessingEvents.ShadowsMidtonesHighlightses.Add(new ShadowsMidtonesHighlightsKey());
            level.Game.PostProcessingEvents.WhiteBalances.Add(new WhiteBalanceKey());
            level.Game.PostProcessingEvents.AnalogGlitches.Add(new AnalogGlitchKey());
            level.Game.PostProcessingEvents.DigitalGlitches.Add(new DigitalGlitchKey());

            level.Game.PlayerEvents.Visibles.Add(new BoolKey());
            level.Game.PlayerEvents.Collisions.Add(new BoolKey());

            var textureObject = new TextureObject()
            {
                ObjectId = new ObjectId(1),
            };
            textureObject.Positions.Add(new PosKey());
            textureObject.Rotations.Add(new AngleKey());
            textureObject.Scales.Add(new ScaKey());
            textureObject.Sizes.Add(new ScaKey());
            textureObject.AnchorsMin.Add(new AlignmentKey());
            textureObject.AnchorsMax.Add(new AlignmentKey());
            textureObject.Pivots.Add(new AlignmentKey());
            textureObject.Colors.Add(new Color4X4Key());
            level.Game.Objects.Add(new ObjectId(1), textureObject);

            var textObject = new TextObject()
            {
                ObjectId = new ObjectId(2),
            };
            textObject.Colors.Add(new Color4Key());
            level.Game.Objects.Add(new ObjectId(2), textObject);

            var effectObject = new EffectObject()
            {
                ObjectId = new ObjectId(3),
            };
            level.Game.Objects.Add(new ObjectId(3), effectObject);

            var prefab = new Prefab()
            {
                PrefabId = PrefabId.NewGuid(),
            };
            prefab.Objects.Add(new ObjectId(1), new TextureObject()
            {
                ObjectId = new ObjectId(1),
            });
            prefab.Objects.Add(new ObjectId(2), new TextObject()
            {
                ObjectId = new ObjectId(2),
            });
            prefab.Objects.Add(new ObjectId(3), new EffectObject()
            {
                ObjectId = new ObjectId(3),
            });
            level.Resources.Prefabs.Add(prefab.PrefabId, prefab);

            var prefabObject = new PrefabObject { PrefabId = PrefabId.NewGuid() };
            level.Game.PrefabObjects.Add(prefabObject);

            level.Resources.Themes.Add(new ThemeId(1), new Theme(new ThemeId(1)));

            level.Resources.Textures.Add(new TextureResourceId(-1), new TextureResource(new TextureResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png")
            }));
            level.Resources.Fonts.Add(new FontResourceId(-1), new FontResource(new FontResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://github.com/google/fonts/raw/refs/heads/main/ofl/dekko/Dekko-Regular.ttf"),
            }));
            level.Resources.Audios.Add(new AudioResourceId(-1), new AudioResource(new AudioResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/7/7a/%22six-seven%22.ogg"),
            }));

            var trackEffects = new LevelTrackEffects();
            var track = new LevelTrack(new AudioId(1), new AudioResourceId(0), 0, 10,
                0f, 0, "", trackEffects);
            level.Audio.Tracks.Add(track.AudioId, track);

            return level;
        }

        public static Level CreateInvalidTestLevel()
        {
            var level = new Level();

            level.Settings.Framerate = -15;
            
            level.Game.Events.Backgrounds.Add(new Color3Key());
            level.Game.Events.Checkpoints.Add(new Checkpoint());
            level.Game.Events.Markers.Add(new Marker());
            level.Game.Events.Themes.Add(new ThemeKeyframe());
            level.Game.CameraEvents.Positions.Add(new PosKey());
            level.Game.CameraEvents.Rotations.Add(new AngleKey());
            level.Game.CameraEvents.Shakes.Add(new ShakeKey());
            level.Game.CameraEvents.Zooms.Add(new ZoomKey());

            level.Game.PostProcessingEvents.Blooms.Add(new BloomKey());
            level.Game.PostProcessingEvents.Chromatics.Add(new ChromaticAberrationKey());
            level.Game.PostProcessingEvents.Vignettes.Add(new VignetteKey());
            level.Game.PostProcessingEvents.Lenses.Add(new LensDistortionKey());
            level.Game.PostProcessingEvents.Grains.Add(new FilmGrainKey());
            level.Game.PostProcessingEvents.MotionBlurs.Add(new MotionBlurKey());
            level.Game.PostProcessingEvents.ColorCurveses.Add(new ColorCurvesKey());
            level.Game.PostProcessingEvents.LiftGammaGains.Add(new LiftGammaGainKey());
            level.Game.PostProcessingEvents.ShadowsMidtonesHighlightses.Add(new ShadowsMidtonesHighlightsKey());
            level.Game.PostProcessingEvents.WhiteBalances.Add(new WhiteBalanceKey());
            level.Game.PostProcessingEvents.AnalogGlitches.Add(new AnalogGlitchKey());
            level.Game.PostProcessingEvents.DigitalGlitches.Add(new DigitalGlitchKey());

            level.Game.PlayerEvents.Visibles.Add(new BoolKey());
            level.Game.PlayerEvents.Collisions.Add(new BoolKey());

            var textureObject = new TextureObject()
            {
                ObjectId = new ObjectId(1),
            };
            textureObject.Positions.Add(new PosKey());
            textureObject.Rotations.Add(new AngleKey());
            textureObject.Scales.Add(new ScaKey());
            textureObject.Sizes.Add(new ScaKey());
            textureObject.AnchorsMin.Add(new AlignmentKey());
            textureObject.AnchorsMax.Add(new AlignmentKey());
            textureObject.Pivots.Add(new AlignmentKey());
            textureObject.Colors.Add(new Color4X4Key());
            level.Game.Objects.Add(new ObjectId(1), textureObject);

            var textObject = new TextObject()
            {
                ObjectId = new ObjectId(2),
            };
            textObject.Colors.Add(new Color4Key());
            level.Game.Objects.Add(new ObjectId(2), textObject);

            var effectObject = new EffectObject()
            {
                ObjectId = new ObjectId(3),
            };
            level.Game.Objects.Add(new ObjectId(3), effectObject);

            var prefab = new Prefab();
            prefab.Objects.Add(new ObjectId(4), new TextureObject() { ObjectId = new ObjectId(4), });
            prefab.Objects.Add(new ObjectId(5), new TextObject() { ObjectId = new ObjectId(5), });
            prefab.Objects.Add(new ObjectId(6), new EffectObject() { ObjectId = new ObjectId(6), });
            level.Resources.Prefabs.Add(prefab.PrefabId, prefab);

            var prefabObject = new PrefabObject();
            level.Game.PrefabObjects.Add(prefabObject);

            level.Resources.Themes.Add(new ThemeId(1), new Theme(new ThemeId(1)));

            level.Resources.Textures.Add(new TextureResourceId(0), new TextureResource(new TextureResourceId(0), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png")
            }));
            level.Resources.Fonts.Add(new FontResourceId(0), new FontResource(new FontResourceId(0), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://github.com/google/fonts/raw/refs/heads/main/ofl/dekko/Dekko-Regular.ttf"),
            }));
            level.Resources.Audios.Add(new AudioResourceId(0), new AudioResource(new AudioResourceId(0), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/7/7a/%22six-seven%22.ogg"),
            }));

            var trackEffects = new LevelTrackEffects();
            var track = new LevelTrack(new AudioId(1), new AudioResourceId(0), 0, 1000,
                0f, 0, "track", trackEffects);
            level.Audio.Tracks.Add(track.AudioId, track);

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
            meta.LevelId = LevelId.NewGuid();
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
                    ResourceId = new TypedResourceId(-1),
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
                    ResourceId = new TypedResourceId(-1),
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

            var json = serializationService.SerializeData(prefab);
            Cat.Meow($"Prefab - <color=green>{json}</color>");

            var prefab2 = serializationService.DeserializeData<Prefab>(json);
            Assert.IsTrue(prefab.Equals(prefab2));
        }

        public static Prefab CreateTestPrefab()
        {
            var prefab = new Prefab();
            prefab.Objects.Add(new ObjectId(1), new TextureObject() { ObjectId = new ObjectId(1), });
            prefab.Objects.Add(new ObjectId(2), new TextObject() { ObjectId = new ObjectId(2), });
            prefab.Objects.Add(new ObjectId(3), new EffectObject() { ObjectId = new ObjectId(3), });
            
            var prefabObject = new PrefabObject
            {
                PrefabId = PrefabId.NewId(),
            };
            var modification = new Modification
            {
                ObjectId = new ObjectId(123),
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

            var json = serializationService.SerializeData(theme);
            Cat.Meow($"Theme - <color=green>{json}</color>");

            var theme2 = serializationService.DeserializeData<Theme>(json);
            Assert.IsTrue(theme.Equals(theme2));
        }

        public static Theme CreateTestTheme()
        {
            var theme = new Theme
            {
                Matrix =
                {
                    [1] = Color4Value.red,
                    [2] = Color4Value.green,
                    [3] = Color4Value.blue,
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