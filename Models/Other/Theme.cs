using System;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Other
{
    public class Theme : ITheme
    {
        public const int Count = 64;
        
        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(ModelNames.Name)]
        public string Name { get; set; }
        
        [JsonProperty(ModelNames.Matrix)]
        public ColorValue[] Matrix { get; set; }
        
        // Theme - is map of colors, level can refer to color via index
        // Theme is a predefined array in runtime
        // Now it's 64 or 8x8 grid. If you see PA and this game, what indexes means (starts with 1)
        // 1 - fallback color, if index is not founded
        // 2 - GUI (PA)
        // 3 - Background (PA)
        // 4-7 - Players (PA)
        // 8 - Tail (PA)
        // 9-16 - free space
        // 17-25 - objects (PA)
        // 26-32 - free
        // 33-41 - parallax (PA)
        // 42-48 - free
        // 49-57 - effects (PA)
        // 58-64 - free
        
        public Theme()
        {
            Name = string.Empty;
            Matrix = new ColorValue[Count];
            Array.Fill(Matrix, new ColorValue(Color.white));
        }
        public Theme(string name, ColorValue[] matrix)
        {
            Name = name;
            Matrix = matrix;
        }
    }
}