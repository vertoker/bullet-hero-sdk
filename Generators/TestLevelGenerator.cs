using System;
using System.Collections.Generic;
using BH.SDK.Models;
using BH.SDK.Models.Audio;
using BH.SDK.Models.AudioEffects;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Meta;
using BH.SDK.Models.Enum.Resources;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Meta;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Resources;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Models.Values;
using BH.SDK.Utils;

namespace BH.SDK.Generators
{
    public class TestLevelGenerator : BaseLevelGenerator<TestLevelGenerator.InputParameters>
    {
        public override Level GenerateLevel(InputParameters parameters)
        {
            var settings = new LevelSettings(45, 4500, new ScreenLimitFixed(new ScreenAspect(1, 1)), 0);
            
            // TODO add object for ObjectId.LocalPlayer

            var textureObject1 = new TextureObject
            {
                ObjectId = new ObjectId(1), ParentObjectId = ObjectId.Camera, StartFrame = 30, EndFrame = 120,
                TextureResourceId = new TextureResourceId(27), ColliderId = ColliderId.Null, Name = "TexObj1",
            };
            textureObject1.Positions.Add(new PosKey(new Vector2Value(0, 0), 0));
            textureObject1.Positions.Add(new PosKey(new Vector2Value(1, 1), 90));
            textureObject1.AnchorsMin.Add(AlignmentKey.GetLeftMiddle(0));
            textureObject1.AnchorsMax.Add(AlignmentKey.GetLeftMiddle(0));
            textureObject1.AnchorsMin.Add(AlignmentKey.GetCenterMiddle(90));
            textureObject1.AnchorsMax.Add(AlignmentKey.GetCenterMiddle(90));
            textureObject1.Pivots.Add(AlignmentKey.GetLeftMiddle(0));
            textureObject1.Colors.Add(new Color4X4Key(new ColorValue(1f, 1f, 0f, 1f), 0));
            textureObject1.Colors.Add(new Color4X4Key(new ColorValue(1f, 0f, 0f, 1f), 90));
            
            var textureObject2 = new TextureObject
            {
                ObjectId = new ObjectId(2), ParentObjectId = new ObjectId(1), StartFrame = 60, EndFrame = 90,
                TextureResourceId = TextureResourceId.Square, ColliderId = ColliderId.Square, Name = "TexObj2",
            };
            textureObject2.Positions.Add(new PosKey(new Vector2Value(-1, 0), 0));
            textureObject2.Positions.Add(new PosKey(new Vector2Value(1, 0), 30));
            textureObject2.Scales.Add(new ScaKey(new Vector2Value(0.5f, 0.5f), 15));
            textureObject1.Pivots.Add(AlignmentKey.GetLeftBottom(0));
            
            var textureObject3 = new TextureObject
            {
                ObjectId = new ObjectId(3), ParentObjectId = ObjectId.Null, StartFrame = 60, EndFrame = 90,
                TextureResourceId = TextureResourceId.Square, ColliderId = ColliderId.Square,
            };
            textureObject2.Positions.Add(new PosKey(new Vector2Value(1, 0), 0));
            textureObject2.Positions.Add(new PosKey(new Vector2Value(1, 0), 30));
            textureObject2.Scales.Add(new ScaKey(new Vector2Value(0.5f, 0.5f), 15));
            textureObject1.Pivots.Add(AlignmentKey.GetLeftBottom(0));

            var effectObject1 = new EffectObject
            {
                ObjectId = new ObjectId(4), ParentObjectId = ObjectId.Null, StartFrame = 60, EndFrame = 660,
                HasStopLocalFrame = true, StopLocalFrame = 300,
                Core = new EffectObjectCore
                {
                    Loop = true,
                    ParticleCount = 25,
                    LifetimeBounds = new Vector2Value(3f, 15f),
                    ParticlePivot = Alignment.Equilateral3,
                    TextureResourceId = TextureResourceId.Circle,
                },
                Forces = new EffectObjectForces
                {
                    StartGravityMin = new FloatValue(0f),
                    StartGravityMax = new FloatValue(0.12f),
                    StartVelocityMin = new Vector2Value(-0.3f, 0f),
                    StartVelocityMax = new Vector2Value(-3f, 0f),
                    StartAngularVelocityMin = new FloatValue(500f),
                    StartAngularVelocityMax = new FloatValue(1000f),
                    OrbitalVelocity = new Vector3Value(1f, 1f, 0f),
                },
                EffectColor = new EffectColorGradientOverLife(),
                EffectShape = new EffectShapeCircle(new FloatValue(1f), new FloatValue(0f), new FloatValue(BHSDKMath.PI2),
                    new EffectShapeSpreadLoop(0f, 1f)),
                EffectScale = new EffectScaleValue(new Vector2Value(5f, 5f)),
            };
            effectObject1.Positions.Add(new PosKey(new Vector2Value(2f, 0f), 0));
            effectObject1.Positions.Add(new PosKey(new Vector2Value(3f, 2f), 600));
            ((EffectColorGradientOverLife)effectObject1.EffectColor).Gradient.ColorKeys[0].ColorHDR = ColorValue.red;
            ((EffectColorGradientOverLife)effectObject1.EffectColor).Gradient.ColorKeys[1].ColorHDR = ColorValue.blue;

            var allText = "\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz" +
                          "{|}~ ¡¢£¤¥¦§¨©ª«¬-®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóô" +
                          "õö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌō" +
                          "ŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƒǼǽǾǿȘșȚțȷʼˆˇ˘˙˚˛˜˝̀́̂̃̄ЀЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕ" +
                          "ЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюяѐёђѓєѕіїјљњћќѝўџѢѣѪѫѲѳѴѵҐґҒғҔҕҖ" +
                          "җҘҙҚқҜҝҠҡҢңҤҥҪҫҮүҰұҶҷҸҹҺһӀӁӂӋӌӏӐӑӒӓӔӕӖӗӘәӜӝӞӟӢӣӤӥӦӧӨөӮӯӰӱӲӳӴӵӶӷӸӹԚԛԜԝ" +
                          "Ԥԥְֱֲֳִ־ׇׁׂאבגדהוזחטיךכלםמןנסעףפץצקרשתײ׳״ẀẁẂẃẄẅỲỳ–—‘’‚“”„†‡•…‰‹›⁄⁰⁴⁵⁶⁷⁸⁹⁽⁾₀₁₂₃₄₅₆₇₈₉₍₎" +
                          "₪€₮₴₸₹₽№™℮⅓⅔⅛⅜⅝⅞∂∆∏∑−∕√∞∫≈≠≤≥◊ﬁﬂשׁשׂשּׁשּׂאַאָאּבּגּדּהּוּזּטּיּךּכּלּמּנּסּףּפּצּקּרּשּתּוֹ";
            // var coolText = "cool text";
            
            var textObject1 = new TextObject
            {
                ObjectId = new ObjectId(5), ParentObjectId = ObjectId.Null, StartFrame = 90, EndFrame = 120,
                FontResourceId = FontResourceId.Default, // -1
                Text = new StringValue(allText),
            };
            textObject1.Sizes.Add(new ScaKey(new Vector2Value(10f, 2f), 0));
            textObject1.Colors.Add(new Color4Key(ColorValue.white, 0));
            textObject1.Colors.Add(new Color4Key(ColorValue.red, 30));
            textObject1.FontSizes.Add(new FloatKey(new FloatValue(2f), 0));
            textObject1.FontSizes.Add(new FloatKey(new FloatValue(0.3f), 30));
            
            var track1 = new LevelTrack
            {
                AudioId = new AudioId(1), AudioResourceId = new AudioResourceId(-2),
                StartFrame = 0, EndFrame = 10000, OffsetLocalTime = 0f,
                Effects = new LevelTrackEffects
                {
                    Volumes = new List<FloatKey>
                    {
                        new(new FloatValue(1f), 0),
                    },
                    Active = true,
                    PitchShifter = new AudioPitchShifter
                    {
                        Pitch = 0.75f,
                    },
                },
            };
            var track2 = new LevelTrack
            {
                AudioId = new AudioId(2), AudioResourceId = new AudioResourceId(-3),
                StartFrame = 500, EndFrame = 1000, OffsetLocalTime = 0f,
                Effects = new LevelTrackEffects
                {
                    Volumes = new List<FloatKey>
                    {
                        new(new FloatValue(1f), 500),
                    },
                    Active = false,
                },
            };
            var track3 = new LevelTrack
            {
                AudioId = new AudioId(3), AudioResourceId = new AudioResourceId(-4),
                StartFrame = 580, EndFrame = 800, OffsetLocalTime = 0f,
                Effects = new LevelTrackEffects
                {
                    Volumes = new List<FloatKey>
                    {
                        new(new FloatValue(1f), 550),
                    },
                    Active = false,
                },
            };
            var track4 = new LevelTrack
            {
                AudioId = new AudioId(5), AudioResourceId = new AudioResourceId(-1),
                StartFrame = 200, EndFrame = 700, OffsetLocalTime = 0f,
                Effects = new LevelTrackEffects
                {
                    Volumes = new List<FloatKey>
                    {
                        new(new FloatValue(1f), 550),
                    },
                    Active = false,
                },
            };

            var textureResource = new TextureResource(new TextureResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl,
                    "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png"),
            });
            /*var fontResource = new FontResource(new FontResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.LevelPath, "RubikStorm-Regular.ttf"),
            });*/
            var audioResource = new AudioResource(new AudioResourceId(-1), new List<ResourceKey>
            {
                new(ResourceUriType.DirectUrl,
                    "https://upload.wikimedia.org/wikipedia/commons/7/7a/%22six-seven%22.ogg"),
            });
            var audioResource2 = new AudioResource(new AudioResourceId(-2), new List<ResourceKey>
            {
                new(ResourceUriType.LevelPath, "Spider Dance.mp3"),
            });
            var audioResource3 = new AudioResource(new AudioResourceId(-3), new List<ResourceKey>
            {
                new(ResourceUriType.LevelPath, "amogus.wav"),
            });
            var audioResource4 = new AudioResource(new AudioResourceId(-4), new List<ResourceKey>
            {
                new(ResourceUriType.LevelPath, "sugoma.wav"),
            });

            var prefabGuid1 = new Guid("aaaa0001-0000-0000-0000-000000000000");
            var prefabGuid2 = new Guid("aaaa0002-0000-0000-0000-000000000000");
            
            var prefab1 = new Prefab
            {
                PrefabGuid = prefabGuid1,
                Objects = new Dictionary<ObjectId, RectObject>()
                {
                    {
                        new ObjectId(1), new TextureObject
                        {
                            ObjectId = new ObjectId(1), ParentObjectId = ObjectId.Null, TextureResourceId = new TextureResourceId(78),
                            ColliderId = new ColliderId(78),
                            StartFrame = 150, EndFrame = 210, Name = "Prefab 1",
                        }
                    },
                    {
                        new ObjectId(2), new TextureObject
                        {
                            ObjectId = new ObjectId(2), ParentObjectId = new ObjectId(1), TextureResourceId = new TextureResourceId(79), ColliderId = new ColliderId(79),
                            StartFrame = 150, EndFrame = 210, Name = "Prefab 2",
                            Positions = new List<PosKey>
                            {
                                new(new Vector2Value(2f, 2f), 150)
                            }
                        }
                    }
                }
            };
            var prefab2 = new Prefab
            {
                PrefabGuid = prefabGuid2,
                PrefabObjects = new List<PrefabObject>
                {
                    new()
                    {
                        PrefabGuid = prefabGuid1,
                        ObjectIds = new List<ObjectIdModification>
                        {
                            new(new ObjectId(1), new ObjectId(11)),
                            new(new ObjectId(2), new ObjectId(12)),
                        },
                        Modifications = new List<Modification>(),
                    }
                }
            };
            
            var prefabObject1 = new PrefabObject
            {
                PrefabGuid = prefabGuid1,
                ObjectIds = new List<ObjectIdModification>
                {
                    new(new ObjectId(1), new ObjectId(21)),
                    new(new ObjectId(2), new ObjectId(22)),
                },
                Modifications = new List<Modification>(),
            };
            var prefabObject2 = new PrefabObject
            {
                PrefabGuid = prefabGuid2,
                ObjectIds = new List<ObjectIdModification>
                {
                    new(new ObjectId(11), new ObjectId(23)),
                    new(new ObjectId(12), new ObjectId(24)),
                },
                Modifications = new List<Modification>(),
            };
            
            var level = new Level
            {
                Settings = settings
            };

            level.Game.Objects.Add(textureObject2.ObjectId, textureObject2);
            level.Game.Objects.Add(textureObject3.ObjectId, textureObject3);
            level.Game.Objects.Add(effectObject1.ObjectId, effectObject1);
            level.Game.Objects.Add(textureObject1.ObjectId, textureObject1);
            level.Game.Objects.Add(textObject1.ObjectId, textObject1);
            
            level.Game.Prefabs.Add(prefab1);
            level.Game.Prefabs.Add(prefab2);
            level.Game.PrefabObjects.Add(prefabObject1);
            level.Game.PrefabObjects.Add(prefabObject2);
            
            level.Game.CameraEvents.Zooms.Add(new ZoomKey(new FloatValue(1f), 0));
            level.Game.CameraEvents.Zooms.Add(new ZoomKey(new FloatValue(0.5f), 90));
            level.Game.CameraEvents.Shakes.Add(new ShakeKey(0f, 1f, 0));
            level.Game.CameraEvents.Shakes.Add(new ShakeKey(1f, 100f, 120));
            level.Game.CameraEvents.Shakes.Add(new ShakeKey(0f, 1f, 180));
            
            level.Audio.Tracks.Add(track1);
            level.Audio.Tracks.Add(track2);
            level.Audio.Tracks.Add(track3);
            level.Audio.Tracks.Add(track4);
            
            level.Resources.Textures.Add(textureResource);
            // level.Resources.Fonts.Add(fontResource);
            level.Resources.Audios.Add(audioResource);
            level.Resources.Audios.Add(audioResource2);
            level.Resources.Audios.Add(audioResource3);
            level.Resources.Audios.Add(audioResource4);
            
            return level;
        }
        public override LevelMeta GenerateMeta(InputParameters parameters)
        {
            var author = new Author(new StringLocalized(new StringLanguage("en", "vertoker"),
                new StringLanguage("ru", "вертокер")), "vertoker.com");
            
            var textureResourceMeta = new ResourceMeta(ResourceType.Texture, -1, 
                new StringValue("some texture"), new StringValue(), 
                "https://commons.wikimedia.org/wiki/File:PNG_transparency_demonstration_1.png", 
                new TypicalLicense(TypicalLicenseType.CC_BY_SA_3_0),
                new List<IString>(),
                new List<Author>
                {
                    new(new StringValue("Daniel G."), "https://commons.wikimedia.org/wiki/User:Daniel_G."),
                    new(new StringValue("Ed g2s"), "https://commons.wikimedia.org/wiki/User:Ed_g2s"),
                    new(new StringValue("CyberShadow"), "https://commons.wikimedia.org/wiki/User:CyberShadow"),
                });
            /*var fontResourceMeta = new ResourceMeta(ResourceType.Font, -1, 
                new StringValue("Rubik Storm"), new StringValue(), 
                "https://fonts.google.com/specimen/Rubik+Storm",
                new TypicalLicense(TypicalLicenseType.SIL_OFL_1_1),
                new List<IString>(),
                new List<Author>
                {
                    new(new StringValue("NaN"), string.Empty),
                    new(new StringValue("Luke Prowse"), string.Empty),
                });*/
            var audioResourceMeta = new ResourceMeta(ResourceType.Audio, -1,
                new StringValue("six-seven"), new StringValue(),
                "https://commons.wikimedia.org/wiki/File:%22six-seven%22.ogg",
                new TypicalLicense(TypicalLicenseType.CC_BY_SA_4_0),
                new List<IString>(),
                new List<Author>
                {
                    new(new StringValue("WhatADrag07"), "https://commons.wikimedia.org/wiki/User:WhatADrag07"),
                });
            var audioResource2Meta = new ResourceMeta(ResourceType.Audio, -2,
                new StringValue("Spider Dance"), new StringValue(),
                "https://www.youtube.com/watch?v=NH-GAwLAO30",
                new TypicalLicense(TypicalLicenseType.Proprietary),
                new List<IString>(),
                new List<Author>
                {
                    new(new StringValue("Toby Fox"), "https://x.com/tobyfox"),
                });
            var audioResource3Meta = new ResourceMeta(ResourceType.Audio, -3, 
                new StringValue("amogus"), 
                new StringValue("Popular meme for "),
                "https://www.youtube.com/watch?v=_dCvuaJEn5A",
                new TypicalLicense(TypicalLicenseType.CC_BY_NC_SA_4_0),
                new List<IString>
                {
                    new StringValue("https://archive.org/details/amogus-audio-original"),
                },
                new List<Author>
                {
                    new(new StringValue("MenesSlavos"), string.Empty),
                });
            var audioResource4Meta = new ResourceMeta(ResourceType.Audio, -4, 
                new StringValue("sugoma"), 
                new StringValue(),
                "https://www.youtube.com/watch?v=nFLjYnEba9E",
                new TypicalLicense(TypicalLicenseType.YT),
                new List<IString>
                {
                    new StringValue("https://archive.org/details/amogus-audio-original"),
                },
                new List<Author>
                {
                    new(new StringValue("MenesSlavos"), string.Empty),
                });

            var meta = new LevelMeta
            {
                LevelGuid = Guid.NewGuid(),
                LevelName = parameters.LevelName.Copy(),
                LevelDescription = parameters.LevelDescription.Copy(),
                LevelVersion = new Version(1, 0),
                LevelLicense = new TypicalLicense(TypicalLicenseType.CC_BY_NC_4_0),
                LevelAuthors = new List<Author> { author },
                ResourcesMeta = new List<ResourceMeta>
                {
                    textureResourceMeta,
                    // fontResourceMeta,
                    audioResourceMeta,
                    audioResource2Meta,
                    audioResource3Meta,
                    audioResource4Meta,
                }
            };
            
            return meta;
        }

        public class InputParameters : BaseInputParameters
        {
            public InputParameters(IString levelName, IString levelDescription) : base(levelName, levelDescription)
            {
                
            }
        }
    }
}