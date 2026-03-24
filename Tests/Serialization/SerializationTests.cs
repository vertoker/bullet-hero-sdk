using System.IO;
using BHSDK.Models;
using BHSDK.Models.Components;
using BHSDK.Models.Events;
using BHSDK.Models.NoGame;
using BHSDK.Models.Objects;
using BHSDK.Models.PostProcessing;
using BHSDK.Models.Values;
using BHSDK.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace BHSDK.Tests.Serialization
{
    public class SerializationTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void TestSerialization()
        {
            var settings = new SerializationSettings(Formatting.Indented);
            var serializationService = new SerializationService(settings);

            var level = CreateTestLevel();

            var levelData = new LevelData(level);
            var textWriter = new StringWriter();
            serializationService.Serializer.Serialize(textWriter, levelData);
            var json = textWriter.ToString();
            Debug.Log($"<color=green>{json}</color>");

            var reader = new JsonTextReader(new StringReader(json));
            levelData = serializationService.Serializer.Deserialize<LevelData>(reader);
        }

        private static Level CreateTestLevel()
        {
            var level = new Level();
            level.Meta.Authors.Add(new Author());
            level.Track.Sources.Add(new LevelTrackSource());

            level.Game.ScreenLimit = new ScreenLimitBounds();
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

            level.Game.PlayerEvents.Clr.Add(new Clr());
            level.Game.PlayerEvents.Collisions.Add(new Bln());
            level.Game.PlayerEvents.Velocities.Add(new Velocity());
            level.Game.PlayerEvents.VelocityPoints.Add(new VelocityPoint());

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

            return level;
        }
    }
}