using System;

namespace BH.SDK.Models.Enum.Meta
{
    // The first [Flags] enum in the format. Every other enum here is a single choice validated by
    // RuleEnumValid, which cannot cover this one (Enum.IsDefined rejects legitimate combinations) -
    // hence RuleEnumFlagsValid, written for exactly this type.
    //
    // FlashingLights and LoudAudio are not "maturity" and do not raise an age rating. They sit in the
    // same set because they have the same single consumer - the warning line shown before a level
    // starts - and a rhythm/bullet-hell game is precisely where a photosensitive player needs that
    // warning to exist at all.

    /// <summary> What a level or resource contains, beyond the bare age number. </summary>
    [Flags]
    public enum ContentDescriptor : ushort
    {
        None = 0,

        Violence = 1 << 0,
        Blood = 1 << 1,
        Language = 1 << 2,
        SexualContent = 1 << 3,
        Substances = 1 << 4,
        Gambling = 1 << 5,
        Horror = 1 << 6,

        /// <summary> Strobing or rapidly flashing visuals - a photosensitivity warning. </summary>
        FlashingLights = 1 << 7,

        /// <summary> Sudden or sustained loud audio. </summary>
        LoudAudio = 1 << 8,
    }
}
