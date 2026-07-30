using System;
using System.Collections.Generic;
using BH.SDK.Models;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Data;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Enum.Settings;
using BH.SDK.Models.Enum.Text;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Events;
using BH.SDK.Models.Game;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Meta;
using BH.SDK.Models.Objects;
using BH.SDK.Models.PostProcessing;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Models.Values;
using BH.SDK.Serialization;
using BH.SDK.Versions.V0_0;
using Newtonsoft.Json.Linq;

namespace BH.SDK.Tests
{
    // Mock/fixture construction for the test suite (moved out of SerializationTests so every test
    // file can share it - see ValidatorTests for the other consumer). "Valid" fixtures
    // (CreateTest*/CreateValid*) are meant to touch as much of each aggregate's field surface as
    // possible, so round-trip/serialization tests exercise real data instead of constructor
    // defaults - but they must stay rule-valid, since ValidatorTests asserts zero RuleAnalyzer
    // issues against several of them (CreateTestLevel, CreateTestLevelMeta, CreateValidTestSettings).
    // "Invalid" fixtures (CreateInvalid*) are deliberately left minimal and untouched here - each
    // encodes one specific, deliberately-chosen rule violation that TestFixerXxx (ValidatorTests)
    // asserts is both detected and auto-fixable by RuleFixer; broadening them risks introducing an
    // issue RuleFixer can't resolve, which isn't worth the coverage gain for a fixture whose whole
    // point is to be minimal and wrong in one specific way.
    public static class MockData
    {
        #region Actual Version
        
        public static EffectObject CreateTestEffect()
        {
            var effect = new EffectObject
            {
                ObjectId = new ObjectId(10),
                ParentObjectId = new ObjectId(5),
                Name = "TestEffect",
                Visible = false,
                StartFrame = 5,
                EndFrame = 300,
                Layer = 3,
                EffectId = EffectId.NewGuid(),
            };
            effect.Positions.Add(new PosKey());
            effect.Rotations.Add(new AngleKey());
            effect.Scales.Add(new ScaKey());
            effect.Sizes.Add(new ScaKey());
            effect.AnchorsMin.Add(new AlignmentKey());
            effect.AnchorsMax.Add(new AlignmentKey());
            effect.Pivots.Add(new AlignmentKey());
            return effect;
        }
        public static EffectData CreateTestEffectData()
        {
            var effect = new EffectData
            {
                HasStopLocalFrame = true,
                StopLocalFrame = 20,
                Core = new EffectObjectCore
                {
                    Render = false,
                    Loop = false,
                    ParticleCount = 1200,
                    LifetimeBounds = new Vector2Value(1f, 5f),
                    TextureResourceId = TextureResourceId.Circle,
                    ParticlePivot = new Alignment(new Vector2Value(0.25f, 0.75f)),
                },
                Forces = new EffectObjectForces
                {
                    StartGravityMin = new FloatValue(-1f),
                    StartGravityMax = new FloatValue(1f),
                    StartVelocityMin = new Vector2Value(-2f, -2f),
                    StartVelocityMax = new Vector2Value(2f, 2f),
                    StartAngularVelocityMin = new FloatValue(-10f),
                    StartAngularVelocityMax = new FloatValue(10f),
                    LinearVelocity = new Vector2Value(0.5f, 0.5f),
                    OrbitalVelocity = new Vector3Value(1f, 2f, 3f),
                    OrbitalCenterOffset = new Vector3Value(0.1f, 0.2f, 0.3f),
                    VelocitySpeed = new FloatValue(2f),
                    LinearForce = new Vector2Value(0.1f, -0.1f),
                },
                Angle = new EffectAngleCurvesBySpeed
                {
                    Curve = new CurveValue(new List<CurveKeyframeValue>
                    {
                        new(), new()
                    }, CurveWrapMode.Default, CurveWrapMode.Default),
                    SpeedRange = new Vector2Circle(0f, 1f, 2f),
                },
                Color = new EffectColorGradientRandom
                {
                    Gradient = new GradientValue(new List<GradientColorKeyValue>
                    {
                        new()
                    }, new List<GradientAlphaKeyValue>
                    {
                        new()
                    }, GradientInterpolationMode.PerceptualBlend, GradientColorSpace.Linear)
                },
                Scale = new EffectScaleCurvesBySpeed
                {
                    CurveX = new CurveValue(),
                    CurveY = new CurveValue(),
                    SpeedRange = new Vector2Value(),
                },
                Shape = new EffectShapeCircle
                {
                    Arc = new FloatValue(6.29f),
                    Radius = new FloatValue(1f),
                    Spread = new EffectShapeSpreadLoop(1f, 2f),
                    Thickness = new FloatValue(1f),
                },
            };
            return effect;
        }

        public static Level CreateTestLevel()
        {
            var level = new Level();
            var themeId = ThemeId.NewGuid();

            level.Settings.Framerate = 61;
            level.Settings.ObjectIdCounter = 4;
            level.Settings.AudioIdCounter = 2;

            level.Game.Events.ScreenLimits.Add(new ScreenLimitKey(new ScreenLimitBounds(), 0));
            level.Game.Events.Backgrounds.Add(new Color3Key());
            level.Game.Events.Checkpoints.Add(new Checkpoint
            {
                Name = "Start",
                Active = true,
                Color4 = Color4Value.green,
            });
            level.Game.Events.Markers.Add(new Marker
            {
                Name = "Marker1",
                Description = "First marker",
                Color4 = new Color4Value(1f, 1f, 0f, 1f),
            });
            level.Game.Events.Themes.Add(new ThemeKeyframe(themeId, 0));
            level.Game.CameraEvents.Positions.Add(new PosKey());
            level.Game.CameraEvents.Rotations.Add(new AngleKey());
            level.Game.CameraEvents.Shakes.Add(new ShakeKey());
            level.Game.CameraEvents.Zooms.Add(new ZoomKey());
            level.Game.CameraEvents.Pivots.Add(new AlignmentKey());

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
            level.Game.PlayerEvents.Controls.Add(new BoolKey());
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
            textureObject.UVs.Add(new UVKey());
            level.Game.Objects.Add(new ObjectId(1), textureObject);

            var textObject = new TextObject()
            {
                ObjectId = new ObjectId(2),
                Text = new StringValue("Hello"),
                FontResourceId = new FontResourceId(-1),
                WordWrap = false,
                HorizontalAlignment = TextObjectHorizontalAlignment.Left,
                VerticalAlignment = TextObjectVerticalAlignment.Top,
            };
            textObject.Colors.Add(new Color4Key());
            textObject.FontSizes.Add(new FloatKey());
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
            var innerPrefabObject = new PrefabObject { PrefabId = PrefabId.NewGuid() };
            innerPrefabObject.ObjectIds.Add(new ObjectIdModification
            {
                PrevObjectId = new ObjectId(1),
                NextObjectId = new ObjectId(2),
            });
            innerPrefabObject.Modifications.Add(new Modification
            {
                ObjectId = new ObjectId(1),
                Path = "core.render",
                Value = false,
            });
            prefab.PrefabObjects.Add(innerPrefabObject);
            level.Resources.Prefabs.Add(prefab.PrefabId, prefab);

            var prefabObject = new PrefabObject { PrefabId = PrefabId.NewGuid() };
            prefabObject.ObjectIds.Add(new ObjectIdModification
            {
                PrevObjectId = new ObjectId(1),
                NextObjectId = new ObjectId(3),
            });
            prefabObject.Modifications.Add(new Modification
            {
                ObjectId = new ObjectId(2),
                Path = "text",
                Value = "overridden",
            });
            level.Game.PrefabObjects.Add(prefabObject);

            level.Resources.Themes.Add(themeId, new ThemeData(themeId));

            var textureResource = new TextureResource(new TextureResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png")
            })
            {
                TextureResourceUV = new Vector4Value(2f, 2f, 0.1f, 0.1f),
            };
            level.Resources.Textures.Add(textureResource.TextureResourceId, textureResource);
            level.Resources.Fonts.Add(new FontResourceId(-1), new FontResource(new FontResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://github.com/google/fonts/raw/refs/heads/main/ofl/dekko/Dekko-Regular.ttf"),
            }));
            level.Resources.Audios.Add(new AudioResourceId(-1), new AudioResource(new AudioResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl, "https://upload.wikimedia.org/wikipedia/commons/7/7a/%22six-seven%22.ogg"),
            }));
            var customColliderId = ColliderId.NewGuid();
            level.Resources.CompositeShapes.Add(customColliderId, new CompositeCollider(
                customColliderId, "CustomCollider", new List<TriangleCollider> { new() }));

            var trackEffects = new LevelTrackEffects
            {
                Active = true,
            };
            trackEffects.Volumes.Add(new FloatKey());
            trackEffects.StereoPans.Add(new FloatKey());
            trackEffects.Lowpass.CutoffFreq = 8000f;
            trackEffects.Lowpass.MixLevel = -10f;
            trackEffects.Highpass.CutoffFreq = 200f;
            trackEffects.Echo.Delay = 250f;
            trackEffects.Echo.Decay = 0.6f;
            trackEffects.Reverb.DecayTime = 2f;
            trackEffects.Chorus.Rate = 1.2f;
            trackEffects.PitchShifter.Pitch = 1.1f;
            trackEffects.Distortion.Level = 0.3f;
            trackEffects.Flange.Depth = 0.5f;
            trackEffects.Compressor.Threshold = -20f;
            trackEffects.Normalize.MaximumAmp = 15f;
            trackEffects.ParamEQ.CenterFreq = 3000f;

            var track = new LevelTrack(new AudioId(1), new AudioResourceId(-1), 0, 10,
                0f, 0, "track", trackEffects);
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

            var invalidLevelThemeId = ThemeId.NewGuid();
            level.Resources.Themes.Add(invalidLevelThemeId, new ThemeData(invalidLevelThemeId));

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

        public static LevelMeta CreateTestLevelMeta()
        {
            var meta = new LevelMeta();
            meta.LevelId = LevelId.NewGuid();
            meta.LevelVersion = new Version(1, 0);
            meta.LevelName = new StringValue("cool level");
            meta.LevelDescription = new StringValue("cool description");
            meta.LevelLogo = new ResourceKey(ResourceUriType.DirectUrl, "https://example.com/logo.png");
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
            meta.ResourcesMeta[0].ResourceSources.Add(new StringValue("https://mirror1.example.com/spider-dance.ogg"));
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

        public static Prefab CreateTestPrefab()
        {
            var prefab = new Prefab
            {
                PrefabId = PrefabId.NewGuid(),
            };
            prefab.Objects.Add(new ObjectId(1), new TextureObject() { ObjectId = new ObjectId(1), });
            prefab.Objects.Add(new ObjectId(2), new TextObject() { ObjectId = new ObjectId(2), });
            prefab.Objects.Add(new ObjectId(3), new EffectObject() { ObjectId = new ObjectId(3), });

            var prefabObject = new PrefabObject
            {
                PrefabId = PrefabId.NewId(),
            };
            prefabObject.ObjectIds.Add(new ObjectIdModification
            {
                PrevObjectId = new ObjectId(1),
                NextObjectId = new ObjectId(2),
            });
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

        public static ThemeData CreateTestTheme()
        {
            var theme = new ThemeData(ThemeId.NewGuid(), "TestTheme");
            for (var i = 0; i < theme.Matrix.Length; i++)
                theme.Matrix[i] = new Color4Value(i / 63f, 1f - i / 63f, 0.5f, 1f);
            theme.Matrix[1] = Color4Value.red;
            theme.Matrix[2] = Color4Value.green;
            theme.Matrix[3] = Color4Value.blue;
            return theme;
        }

        public static CompositeCollider CreateTestCompositeCollider()
        {
            return new CompositeCollider(ColliderId.NewGuid(), "TestCollider", new List<TriangleCollider>
            {
                new(-0.5f, -0.5f, 0.5f, -0.5f, 0.5f, 0.5f),
                new(-0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f),
            });
        }

        public static UserSettings CreateValidTestSettings()
        {
            var settings = new UserSettings
            {
                General =
                {
                    ResourceParallelLoadCount = 4,
                    ResourceWebTimeout = 10f,
                },
                Controls =
                {
                    ClassicControlsType = ClassicControlsType.Mouse,
                },
                Audio =
                {
                    Volume = 0.8f,
                    Game = 0.6f,
                    UI = 0.4f,
                },
                Graphics =
                {
                    FramerateTarget = FramerateTarget.Fixed,
                    FixedFramerate = 120,
                    Audio =
                    {
                        Render = false,
                        RenderEffects = false,
                        MaxDiffTime = 0.15f,
                        UseScrub = false,
                        ScrubTime = 0.2f,
                    },
                    Effects =
                    {
                        Render = false,
                        FramerateTarget = FramerateTarget.ScreenHz,
                        FixedFramerate = 90,
                        MaxScrubTime = 0.3f,
                    },
                    PostProcessing =
                    {
                        RenderBloom = false,
                        RenderChroma = false,
                        RenderVignette = false,
                        RenderLens = false,
                        RenderGrain = false,
                        RenderMotionBlur = false,
                        RenderColorCurves = false,
                        RenderLiftGammaGain = false,
                        RenderShadowsMidtonesHighlights = false,
                        RenderWhiteBalance = false,
                        RenderAnalogGlitch = false,
                        RenderDigitalGlitch = false,
                    },
                },
                GameEditor =
                {
                    Autosave = false,
                    AutosaveRate = 30f,
                    MaxAutosaveFiles = 10,
                    CameraMinSize = 0.5f,
                    CameraMaxSize = 50f,
                },
            };
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

        #endregion

        #region Version v0.0
        
        // Mock data for the Versions/V0_0 migration-test generation (see VERSION-UPDATE.md, "a real
        // (test) generation exists", and TestLevelV0_0Migration in SerializationTests.cs). Built from
        // the actual LevelSettingsV0_0/GameEventsV0_0/GameLevelV0_0/LevelResourcesV0_0 snapshot
        // classes and serialized through the real SerializationService rather than a hand-typed JSON
        // literal, so the fixture stays honest against those classes' own [JsonProperty] names (e.g.
        // "test_fps") instead of silently drifting out of sync with a copy-pasted string if they ever
        // change.
        public static LevelSettingsV0_0 CreateTestLevelSettingsV0_0() => new() { Framerate = 61 };

        public static GameEventsV0_0 CreateTestGameEventsV0_0() => new();

        public static GameLevelV0_0 CreateTestGameLevelV0_0()
        {
            var gameLevel = new GameLevelV0_0
            {
                GameEvents = new GameEvents(),
                Objects = new Dictionary<ObjectId, RectObject>(),
            };
            var textureObject = new TextureObject { ObjectId = new ObjectId(1) };
            gameLevel.Objects.Add(textureObject.ObjectId, textureObject);
            return gameLevel;
        }

        public static LevelResourcesV0_0 CreateTestLevelResourcesV0_0() => new()
        {
            Resources = new Dictionary<int, object>(),
        };

        public static AudioLevelV0_0 CreateTestAudioLevelV0_0() => new();

        public static LevelV0_0 CreateTestLevelV0_0() => new()
        {
            Settings = new LevelSettings(),
            Game = new GameLevel(),
            Audio = CreateTestAudioLevelV0_0(),
            Resources = new LevelResources(),
        };

        // GameEvents/LevelSettings/LevelResources are independently-versioned domains (see
        // VERSION-UPDATE.md) - VersionedEnvelopeConverter always writes a nested envelope using the
        // runtime instance's OWN [DataVersion] attribute, so serializing LevelV0_0/GameLevelV0_0 as a
        // whole tags every nested envelope with the domain's CURRENT version (1.0), never 0.0 - there
        // is no way to make a LevelV0_0/GameLevelV0_0 field actually hold a LevelSettingsV0_0/
        // GameEventsV0_0 instance, since their declared property types are the CURRENT classes by
        // design (see the "gotcha" in VERSION-UPDATE.md). So each independently-versioned fragment is
        // serialized standalone from its own VX_Y snapshot type instead (which DOES carry the old
        // [DataVersion]) and spliced into the current-shape envelope produced from the outer object -
        // a genuine v0.0 tag can only ever come from a real VX_Y instance's own attribute.
        public static string CreateTestLevelV0_0Json(SerializationService serializationService)
        {
            var settingsFragment = JObject.Parse(serializationService.SerializeData(CreateTestLevelSettingsV0_0()));
            var gameEventsFragment = JObject.Parse(serializationService.SerializeData(CreateTestGameEventsV0_0()));
            var resourcesFragment = JObject.Parse(serializationService.SerializeData(CreateTestLevelResourcesV0_0()));

            var gameFragment = JObject.Parse(serializationService.SerializeData(CreateTestGameLevelV0_0()));
            gameFragment[Names.Value]![NamesV0_0.GameEvents] = gameEventsFragment;

            var levelJson = JObject.Parse(serializationService.SerializeData(CreateTestLevelV0_0()));
            levelJson[Names.Value]![NamesV0_0.Settings] = settingsFragment;
            levelJson[Names.Value]![NamesV0_0.Game] = gameFragment;
            levelJson[Names.Value]![NamesV0_0.Resources] = resourcesFragment;

            return levelJson.ToString();
        }
        
        #endregion
    }
}
