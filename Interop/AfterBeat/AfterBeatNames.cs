namespace BH.SDK.Interop.AfterBeat
{
    // Deliberately NOT Models/Names.cs, and the separation is load-bearing in both directions:
    // renaming one of this format's own keys must never be possible by editing Bullet Hero's wire
    // format, and vice versa. It is the same reason Versions/VX_Y/ snapshots carry frozen literals
    // instead of sharing the live constants.
    //
    // Several keys collide across documents and the collisions are real, not a transcription
    // mistake: "o" is an origin vector on an object and a float lead time on a prefab, "s" is an
    // int shape id on an object and a nested shape object on a parallax object, "d" is a render
    // depth on an object and a description string on a marker, "t" is a time on a keyframe and a
    // transform object on a parallax object. They are safe because no two of them ever share a
    // class - which is exactly why they are named here by MEANING rather than by spelling.

    /// <summary>
    /// Every JSON key of the Afterbeat (Project Arrhythmia) formats - .vgd, .vgm, .vgt, .vgp.
    /// Sources are listed in this folder's README.md.
    /// </summary>
    public static class AfterBeatNames
    {
        #region .vgd root

        public const string Editor = "editor";
        public const string Triggers = "triggers";
        public const string EditorPrefabSpawn = "editor_prefab_spawn";
        public const string ParallaxSettings = "parallax_settings";
        public const string Checkpoints = "checkpoints";
        public const string Objects = "objects";
        public const string PrefabObjects = "prefab_objects";
        public const string Prefabs = "prefabs";
        public const string Themes = "themes";
        public const string Markers = "markers";
        public const string Events = "events";

        #endregion

        #region .vgd editor block

        public const string EditorGeneral = "general";
        public const string EditorComplexity = "complexity";
        public const string EditorTheme = "theme";
        public const string EditorTestMode = "test_mode";
        public const string EditorTextSelectObjects = "text_select_objects";
        public const string EditorTextSelectBackgrounds = "text_select_backgrounds";
        public const string EditorOutlineMode = "outline_mode";
        public const string EditorCollapseLength = "collapse_length";

        public const string EditorBpm = "bpm";
        public const string EditorBpmSnap = "snap";
        public const string EditorBpmSnapObjects = "objects";
        public const string EditorBpmSnapCheckpoints = "checkpoints";
        public const string EditorBpmValue = "bpm_value";
        public const string EditorBpmOffset = "bpm_offset";

        // Documented as identical to bpm_value and "possibly unused". Carried so a round trip does
        // not quietly drop half of what the file said about its own tempo.
        public const string EditorBpmValueDuplicate = "BPMValue";

        public const string EditorGrid = "grid";
        public const string EditorGridScale = "scale";
        public const string EditorGridThickness = "thickness";
        public const string EditorGridOpacity = "opacity";
        public const string EditorGridColor = "color";

        public const string EditorPreview = "preview";
        public const string EditorPreviewCamZoomOffset = "cam_zoom_offset";
        public const string EditorPreviewCamZoomOffsetColor = "cam_zoom_offset_color";

        public const string EditorAutosave = "autosave";
        public const string EditorAutosaveMax = "as_max";
        public const string EditorAutosaveInterval = "as_interval";

        #endregion

        #region .vgd triggers and prefab spawn slots

        public const string TriggerActivator = "event_trigger";
        public const string TriggerTime = "event_trigger_time";
        public const string TriggerRetrigger = "event_retrigger";
        public const string TriggerEvent = "event_type";
        public const string TriggerData = "event_data";

        public const string SpawnExpanded = "expanded";
        public const string SpawnActive = "active";
        public const string SpawnPrefab = "prefab";
        public const string SpawnKeycodes = "keycodes";

        #endregion

        #region .vgd parallax

        public const string ParallaxLayers = "l";
        public const string ParallaxMainLayer = "ml";
        public const string ParallaxDofActive = "dof_active";
        public const string ParallaxDofValue = "dof_value";

        public const string ParallaxLayerDepth = "d";
        public const string ParallaxLayerColor = "c";
        public const string ParallaxLayerObjects = "o";

        public const string ParallaxObjectShape = "s";
        public const string ParallaxObjectColor = "c";
        public const string ParallaxObjectTransform = "t";
        public const string ParallaxObjectAnimation = "an";

        public const string ParallaxTransformPosition = "p";
        public const string ParallaxTransformScale = "s";
        public const string ParallaxTransformRotation = "r";

        public const string ParallaxAnimationLength = "l";
        public const string ParallaxAnimationDelay = "ld";
        public const string ParallaxAnimationLoopPosition = "ap";
        public const string ParallaxAnimationLoopScale = "as";
        public const string ParallaxAnimationLoopRotation = "ar";

        #endregion

        #region .vgd checkpoints and markers

        // Capitalized in the format, unlike every other id key. Not a transcription mistake.
        public const string CheckpointId = "ID";
        public const string CheckpointName = "n";
        public const string CheckpointTime = "t";
        public const string CheckpointPosition = "p";

        public const string MarkerId = "ID";
        public const string MarkerName = "n";
        public const string MarkerDescription = "d";
        public const string MarkerColor = "c";
        public const string MarkerTime = "t";

        #endregion

        #region Object Data (shared by .vgd objects and .vgp objs)

        public const string ObjectId = "id";
        public const string ObjectPrefabId = "pre_id";
        public const string ObjectPrefabInstanceId = "pre_iid";
        public const string ObjectName = "n";
        public const string ObjectType = "ot";
        public const string ObjectStartTime = "st";
        public const string ObjectAutokillType = "ak_t";
        public const string ObjectAutokillOffset = "ak_o";
        public const string ObjectGradientType = "gt";
        public const string ObjectGradientRotation = "gr";
        public const string ObjectGradientScale = "gs";
        public const string ObjectShape = "s";
        public const string ObjectShapeOption = "so";
        public const string ObjectText = "text";
        public const string ObjectDepth = "d";
        public const string ObjectParentId = "p_id";
        public const string ObjectParentType = "p_t";
        public const string ObjectParentOffsets = "p_o";
        public const string ObjectEditor = "ed";
        public const string ObjectOrigin = "o";
        public const string ObjectTracks = "e";

        public const string ObjectEditorLocked = "lk";
        public const string ObjectEditorCollapsed = "co";
        public const string ObjectEditorTextColor = "tc";
        public const string ObjectEditorBackgroundColor = "bgc";
        public const string ObjectEditorBin = "b";
        public const string ObjectEditorLayer = "l";

        public const string ColorFlagRed = "r";
        public const string ColorFlagGreen = "g";
        public const string ColorFlagBlue = "b";

        public const string TrackKeyframes = "k";

        public const string KeyframeTime = "t";
        public const string KeyframeEase = "ct";
        public const string KeyframeRandomType = "r";
        public const string KeyframeRandomValues = "er";
        public const string KeyframeValues = "ev";

        public const string VectorX = "x";
        public const string VectorY = "y";

        #endregion

        #region .vgd prefab placements

        public const string PlacementId = "id";
        public const string PlacementPrefabId = "pid";
        public const string PlacementEditor = "ed";
        public const string PlacementTracks = "e";

        #endregion

        #region .vgp

        public const string PrefabId = "id";
        public const string PrefabName = "n";
        public const string PrefabDescription = "description";
        public const string PrefabPreview = "preview";
        public const string PrefabType = "type";
        public const string PrefabOffset = "o";
        public const string PrefabObjectsList = "objs";

        #endregion

        #region .vgt

        public const string ThemeId = "id";
        public const string ThemeName = "name";
        public const string ThemeBackground = "base_bg";
        public const string ThemeGui = "base_gui";
        public const string ThemeGuiAccent = "base_gui_accent";
        public const string ThemePlayers = "pla";
        public const string ThemeObjects = "obj";
        public const string ThemeEffects = "fx";
        public const string ThemeParallax = "bg";

        #endregion

        #region .vgm

        public const string MetaBeatmap = "beatmap";
        public const string MetaDateEdited = "date_edited";
        public const string MetaGameVersion = "game_version";
        public const string MetaWorkshopId = "workshop_id";
        public const string MetaVisibility = "visibility";
        public const string MetaChangelog = "changelog";

        public const string MetaCreator = "creator";
        public const string MetaSteamName = "steam_name";
        public const string MetaSteamId = "steam_id";

        public const string MetaSong = "song";
        public const string MetaSongTitle = "title";
        public const string MetaSongDescription = "description";
        public const string MetaSongDifficulty = "difficulty";
        public const string MetaSongBpm = "bpm";
        public const string MetaSongTime = "time";
        public const string MetaSongPreviewStart = "preview_start";
        public const string MetaSongPreviewLength = "preview_length";
        public const string MetaSongCamJiggle = "cam_jiggle";

        public const string MetaArtist = "artist";
        public const string MetaArtistName = "name";
        public const string MetaArtistLinkType = "link_type";
        public const string MetaArtistLink = "link";

        public const string MetaReferences = "references";
        public const string MetaReferenceGame = "game";
        public const string MetaReferenceGameId = "id";
        public const string MetaReferenceGameCustom = "custom";

        #endregion
    }
}
