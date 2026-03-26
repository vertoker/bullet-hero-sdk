using System;
using BHSDK.Models.Interfaces.SaveData;

namespace BHSDK.Models.Settings
{
    public class PlayerSettings : IPlayerSettings
    {
        public Version GetVersion() => new(1, 0);
        
        // TODO client graphics
    }
}