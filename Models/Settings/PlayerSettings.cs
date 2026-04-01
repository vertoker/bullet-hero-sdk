using System;
using BHSDK.Models.Interfaces.SaveData;

namespace BHSDK.Models.Settings
{
    public class PlayerSettings : IPlayerSettings
    {
        public Version GetVersion() => new(1, 0);
        
        // TODO several audio settings (game, UI, player)
        // TODO client graphics
    }
}