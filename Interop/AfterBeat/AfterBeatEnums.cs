namespace BH.SDK.Interop.AfterBeat
{
    // One file rather than one per type, unlike Models/Enums/: these are a transcription of another
    // project's format, not this project's vocabulary. They are read together, they change together
    // if that format changes, and splitting them into twenty files would suggest each is a decision
    // somebody here made.
    //
    // Numbering is the FORMAT's, never renumbered for tidiness - the gaps are real. Object types
    // start at 4, and Randomization Type has no 2 (the wiki's own table skips it), so a value of 2
    // read from a file is genuinely unknown rather than a member nobody bothered to write down.

    /// <summary> Afterbeat object type - .vgd objects[].ot. </summary>
    public enum AfterBeatObjectType
    {
        Hit = 4,
        NoHit = 5,
        Empty = 6,
    }

    /// <summary> How an object decides when it dies - .vgd objects[].ak_t. The meaning of ak_o
    /// changes with it, which is why the two are never read apart. </summary>
    public enum AfterBeatAutokillType
    {
        /// <summary> Dies on its last keyframe. </summary>
        LastKeyframe = 1,

        /// <summary> Dies ak_o seconds after its last keyframe. </summary>
        LastKeyframeOffset = 2,

        /// <summary> Lives ak_o seconds from its own start. </summary>
        FixedTime = 3,

        /// <summary> Dies at absolute song time ak_o. </summary>
        SongTime = 4,
    }

    /// <summary> Per-object colour gradient - .vgd objects[].gt. </summary>
    public enum AfterBeatGradientType
    {
        None = 0,
        Linear = 1,
        InvertedLinear = 2,
        Radial = 3,
        InvertedRadial = 4,
    }

    /// <summary> How a keyframe randomizes its value - .vgd objects[].e[].k[].r. Value 2 is absent
    /// from the format's own table. </summary>
    public enum AfterBeatRandomType
    {
        None = 0,
        Linear = 1,
        Toggle = 3,
        Relative = 4,
    }

    /// <summary> Main shape family - .vgd objects[].s, paired with .so. </summary>
    public enum AfterBeatShape
    {
        Square = 0,
        Circle = 1,
        Triangle = 2,
        Arrow = 3,
        Text = 4,
        Hexagon = 5,
    }

    /// <summary> Which of the fourteen fixed .vgd events[] arrays an index is. The order IS the
    /// format - an array's meaning is its position, it carries no name of its own. </summary>
    public enum AfterBeatEventTrack
    {
        CameraPosition = 0,
        CameraZoom = 1,
        CameraRotation = 2,
        CameraShake = 3,
        Theme = 4,
        Chromatic = 5,
        Bloom = 6,
        Vignette = 7,
        LensDistortion = 8,
        Grain = 9,
        Gradient = 10,
        Glitch = 11,
        Hue = 12,
        PlayerForce = 13,
    }

    /// <summary> Screen-gradient blend mode - .vgd events[10].ev[4]. </summary>
    public enum AfterBeatGradientOverlayMode
    {
        Linear = 0,
        Additive = 1,
        Multiply = 2,
        Screen = 3,
    }

    /// <summary> What activates a trigger - .vgd triggers[].event_trigger. </summary>
    public enum AfterBeatTriggerActivator
    {
        Time = 0,
        PlayerHit = 1,
        PlayerDeath = 2,
        LevelStart = 3,
        LevelRestart = 4,
        LevelRewind = 5,
    }

    /// <summary> What a trigger does - .vgd triggers[].event_type. </summary>
    public enum AfterBeatTriggerEvent
    {
        VnInk = 0,
        VnTimeline = 1,
        PlayerBubble = 2,
        PlayerLocation = 3,
        PlayerDash = 4,
        PlayerMoveX = 5,
        PlayerMoveY = 6,
        BackgroundSpin = 7,
        BackgroundMove = 8,
        PlayerDashDirection = 9,
        TimeGoTo = 10,
        TimeSpeed = 11,
    }

    /// <summary> Prefab category - .vgp type. Note the legacy .lsp numbering is different; nothing
    /// here reads .lsp. </summary>
    public enum AfterBeatPrefabType
    {
        Character = 0,
        CharacterParts = 1,
        Props = 2,
        Bullets = 3,
        Pulses = 4,
        Bombs = 5,
        Spinners = 6,
        Beams = 7,
        Static = 8,
        Misc1 = 9,
        Misc2 = 10,
        Misc3 = 11,
    }

    /// <summary> Level difficulty - .vgm song.difficulty. </summary>
    public enum AfterBeatDifficulty
    {
        Basic = 0,
        Moderate = 1,
        Advanced = 2,
        Expert = 3,
        Master = 4,
    }

    /// <summary> Which service .vgm artist.link points at; the link value is a fragment, not a URL. </summary>
    public enum AfterBeatLinkType
    {
        Spotify = 0,
        Soundcloud = 1,
        Bandcamp = 2,
        YoutubeMusic = 3,
        Newgrounds = 4,
    }

    /// <summary> Which game the song came from - .vgm references.game.id. </summary>
    public enum AfterBeatGameReference
    {
        None = 0,
        Custom = 1,
        Adofai = 2,
        ThousandXResist = 3,
        WindowKill = 4,
    }

    /// <summary> Steam Workshop visibility - .vgm beatmap.visibility. </summary>
    public enum AfterBeatVisibility
    {
        Public = 0,
        Friends = 1,
        Private = 2,
    }

    /// <summary> Camera jiggle preference - .vgm song.cam_jiggle. </summary>
    public enum AfterBeatCamJiggle
    {
        PlayerSelected = 0,
        ForceJiggle = 1,
        ForceNoJiggle = 2,
    }

    /// <summary> Default play mode of the level in the editor - .vgd editor.general.test_mode. </summary>
    public enum AfterBeatTestMode
    {
        Zen = 0,
        Normal = 1,
    }

    /// <summary> Selection outline style in the editor - .vgd editor.general.outline_mode. </summary>
    public enum AfterBeatOutlineMode
    {
        Standard = 0,
        Reduced = 1,
        None = 2,
    }
}
