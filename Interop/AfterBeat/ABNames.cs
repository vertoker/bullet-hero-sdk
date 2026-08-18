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
    public static class ABNames
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

        // The editor's drawn notes - undocumented, and present in quantity on a real level. Not
        // gameplay, but authored by hand all the same, which is why they are read rather than left
        // to the extension data: an author is told they do not cross instead of finding out.
        public const string Annotations = "annotations";

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

        public const string AnnotationId = "id";
        public const string AnnotationMarker = "m";
        public const string AnnotationTime = "t";
        public const string AnnotationPoints = "p";
        public const string AnnotationColor = "c";

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

        // The editor's custom polygon, five numbers, documented nowhere - found by counting the keys
        // real levels carry that no model here reads. Thousands of objects in an ordinary level use
        // it, and every one of them used to import as a Square.
        public const string ObjectCustomShape = "csp";
        public const string ObjectText = "text";
        public const string ObjectDepth = "d";

        // Absent from the wiki's own object table and from every level written before 2026; found
        // by counting keys real levels carry that no model here read. It is one enum, not the two
        // checkboxes the editor's own panel shows - see ABRenderLayer.
        public const string ObjectRenderLayer = "rl";
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

        /// <summary> Where the object sits in the editor's own ordering. Bookkeeping, like the rest
        /// of this block - named here only so it stops reading as a key nobody has seen. </summary>
        public const string ObjectEditorTimelineOrder = "to";

        public const string ColorFlagRed = "r";
        public const string ColorFlagGreen = "g";
        public const string ColorFlagBlue = "b";

        public const string TrackKeyframes = "k";

        public const string KeyframeTime = "t";
        public const string KeyframeEase = "ct";
        public const string KeyframeRandomType = "r";
        public const string KeyframeRandomValues = "er";
        public const string KeyframeValues = "ev";

        // A real .vgd writes a level-global keyframe's STRING payload under its own key rather than
        // inside "ev" - the theme track is the only one that has one, and it carries nothing else.
        // Reading it out of "ev" (which the wiki's description implies) finds an empty array, so
        // every theme change in the level resolves to no theme at all and the level plays untinted.
        public const string KeyframeValuesStrings = "evs";

        public const string VectorX = "x";
        public const string VectorY = "y";

        #endregion

        #region .vgd prefab placements

        public const string PlacementId = "id";
        public const string PlacementPrefabId = "pid";
        public const string PlacementEditor = "ed";
        public const string PlacementTracks = "e";

        // Where a placement sits on the timeline. An object spells its start "st"; a placement uses
        // "t", the same key a marker or a checkpoint does, and the wiki's tree omits it entirely.
        // Without it every placement in a level starts at zero, which is the whole prefab library
        // playing in the first second.
        public const string PlacementTime = "t";

        // Three keys the format's description does not have and the source game does
        // (DataManager.GameData.PrefabObject), all absent from levels written before they existed -
        // which is why every one of them defaults to "this placement behaves as it always did".
        public const string PlacementParentId = "parid";
        public const string PlacementRepeatCount = "r";
        public const string PlacementRepeatOffset = "ro";

        #endregion

        #region .vgp

        public const string PrefabId = "id";
        public const string PrefabName = "n";
        public const string PrefabDescription = "description";
        public const string PrefabPreview = "preview";
        public const string PrefabType = "type";
        public const string PrefabOffset = "o";
        public const string PrefabObjectsList = "objs";

        /// <summary> Which of a template's own objects it is anchored by. </summary>
        public const string PrefabMainObjectId = "mid";

        /// <summary> A template's own prefab PLACEMENTS - the format's nesting, and the reason a
        /// template is a scope rather than a flat object list. </summary>
        public const string PrefabPlacementsList = "pobjs";

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
