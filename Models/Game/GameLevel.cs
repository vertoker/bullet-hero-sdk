using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Objects;
using BHSDK.Models.Other;
using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Game
{
    public class GameLevel : IObjectScope
    {
        [JsonProperty(ModelNames.Event)]
        public GameEvents Events { get; set; }
        
        [JsonProperty(ModelNames.Camera + ModelNames.Event)]
        public CameraEvents CameraEvents { get; set; }
        
        [JsonProperty(ModelNames.PostProcessing + ModelNames.Event)]
        public PostProcessingEvents PostProcessingEvents { get; set; }
        
        [JsonProperty(ModelNames.Player + ModelNames.Event)]
        public PlayerEvents PlayerEvents { get; set; }
        
        
        [JsonProperty(ModelNames.Object)]
        public List<Object> Objects { get; set; }
        
        [JsonProperty(ModelNames.ParentObject)]
        public List<PrefabObject> PrefabObjects { get; set; }
        
        
        [JsonProperty(ModelNames.Prefab)]
        public List<Prefab> Prefabs { get; set; }
        
        [JsonProperty(ModelNames.Theme)]
        public List<Theme> Themes { get; set; }
        
        [JsonProperty(ModelNames.Texture)]
        public List<LevelTexture> Textures { get; set; }
        
        public GameLevel()
        {
            Events = new GameEvents();
            CameraEvents = new CameraEvents();
            PostProcessingEvents = new PostProcessingEvents();
            PlayerEvents = new PlayerEvents();
            
            Objects = new List<Object>();
            PrefabObjects = new List<PrefabObject>();
            
            Prefabs = new List<Prefab>();
            Themes = new List<Theme>();
            Textures = new List<LevelTexture>();
        }
        public GameLevel(GameEvents events, CameraEvents cameraEvents, PostProcessingEvents postProcessingEvents,
            PlayerEvents playerEvents, List<Object> objects, List<PrefabObject> prefabObjects,
            List<Prefab> prefabs, List<Theme> themes, List<LevelTexture> textures)
        {
            Events = events;
            CameraEvents = cameraEvents;
            PostProcessingEvents = postProcessingEvents;
            PlayerEvents = playerEvents;
            
            Objects = objects;
            PrefabObjects = prefabObjects;
            
            Prefabs = prefabs;
            Themes = themes;
            Textures = textures;
        }
    }
}