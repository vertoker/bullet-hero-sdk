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

        /// <summary> VgdLevel.Editor. </summary>
        public const string Editor = "editor";

        /// <summary> VgdLevel.Triggers. </summary>
        public const string Triggers = "triggers";

        /// <summary> VgdLevel.PrefabSpawnSlots. </summary>
        public const string EditorPrefabSpawn = "editor_prefab_spawn";

        /// <summary> VgdLevel.Parallax. </summary>
        public const string ParallaxSettings = "parallax_settings";

        /// <summary> VgdLevel.Checkpoints. </summary>
        public const string Checkpoints = "checkpoints";

        /// <summary> VgdLevel.Objects. </summary>
        public const string Objects = "objects";

        /// <summary> VgdLevel.PrefabPlacements. </summary>
        public const string PrefabObjects = "prefab_objects";

        /// <summary> VgdLevel.Prefabs. </summary>
        public const string Prefabs = "prefabs";

        /// <summary> VgdLevel.Themes. </summary>
        public const string Themes = "themes";

        /// <summary> VgdLevel.Markers. </summary>
        public const string Markers = "markers";

        // The editor's drawn notes - undocumented, and present in quantity on a real level. Not
        // gameplay, but authored by hand all the same, which is why they are read rather than left
        // to the extension data: an author is told they do not cross instead of finding out.

        /// <summary> VgdLevel.Annotations. </summary>
        public const string Annotations = "annotations";

        /// <summary> VgdLevel.Events. </summary>
        public const string Events = "events";

        #endregion

        #region .vgd editor block

        /// <summary> VgdEditor.General. </summary>
        public const string EditorGeneral = "general";

        /// <summary> VgdEditorGeneral.Complexity. </summary>
        public const string EditorComplexity = "complexity";

        /// <summary> VgdEditorGeneral.Theme. </summary>
        public const string EditorTheme = "theme";

        /// <summary> VgdEditorGeneral.TestMode. </summary>
        public const string EditorTestMode = "test_mode";

        /// <summary> VgdEditorGeneral.TextSelectObjects. </summary>
        public const string EditorTextSelectObjects = "text_select_objects";

        /// <summary> VgdEditorGeneral.TextSelectBackgrounds. </summary>
        public const string EditorTextSelectBackgrounds = "text_select_backgrounds";

        /// <summary> VgdEditorGeneral.OutlineMode. </summary>
        public const string EditorOutlineMode = "outline_mode";

        /// <summary> VgdEditorGeneral.CollapseLength. </summary>
        public const string EditorCollapseLength = "collapse_length";

        /// <summary> VgdEditor.Bpm. </summary>
        public const string EditorBpm = "bpm";

        /// <summary> VgdEditorBpm.Snap. </summary>
        public const string EditorBpmSnap = "snap";

        /// <summary> VgdEditorBpmSnap.Objects. </summary>
        public const string EditorBpmSnapObjects = "objects";

        /// <summary> VgdEditorBpmSnap.Checkpoints. </summary>
        public const string EditorBpmSnapCheckpoints = "checkpoints";

        /// <summary> VgdEditorBpm.Value. </summary>
        public const string EditorBpmValue = "bpm_value";

        /// <summary> VgdEditorBpm.Offset. </summary>
        public const string EditorBpmOffset = "bpm_offset";

        // Documented as identical to bpm_value and "possibly unused". Carried so a round trip does
        // not quietly drop half of what the file said about its own tempo.

        /// <summary> VgdEditorBpm.ValueDuplicate. </summary>
        public const string EditorBpmValueDuplicate = "BPMValue";

        /// <summary> VgdEditor.Grid. </summary>
        public const string EditorGrid = "grid";

        /// <summary> VgdEditorGrid.Scale. </summary>
        public const string EditorGridScale = "scale";

        /// <summary> VgdEditorGrid.Thickness. </summary>
        public const string EditorGridThickness = "thickness";

        /// <summary> VgdEditorGrid.Opacity. </summary>
        public const string EditorGridOpacity = "opacity";

        /// <summary> VgdEditorGrid.Color. </summary>
        public const string EditorGridColor = "color";

        /// <summary> VgdEditor.Preview. </summary>
        public const string EditorPreview = "preview";

        /// <summary> VgdEditorPreview.CameraZoomOffset. </summary>
        public const string EditorPreviewCamZoomOffset = "cam_zoom_offset";

        /// <summary> VgdEditorPreview.CameraZoomOffsetColor. </summary>
        public const string EditorPreviewCamZoomOffsetColor = "cam_zoom_offset_color";

        /// <summary> VgdEditor.Autosave. </summary>
        public const string EditorAutosave = "autosave";

        /// <summary> VgdEditorAutosave.Max. </summary>
        public const string EditorAutosaveMax = "as_max";

        /// <summary> VgdEditorAutosave.Interval. </summary>
        public const string EditorAutosaveInterval = "as_interval";

        #endregion

        #region .vgd triggers and prefab spawn slots

        /// <summary> VgdTrigger.Activator. </summary>
        public const string TriggerActivator = "event_trigger";

        /// <summary> VgdTrigger.TimeRange. </summary>
        public const string TriggerTime = "event_trigger_time";

        /// <summary> VgdTrigger.Retrigger. </summary>
        public const string TriggerRetrigger = "event_retrigger";

        /// <summary> VgdTrigger.Event. </summary>
        public const string TriggerEvent = "event_type";

        /// <summary> VgdTrigger.Data. </summary>
        public const string TriggerData = "event_data";

        /// <summary> VgdPrefabSpawnSlot.Expanded. </summary>
        public const string SpawnExpanded = "expanded";

        /// <summary> VgdPrefabSpawnSlot.Active. </summary>
        public const string SpawnActive = "active";

        /// <summary> VgdPrefabSpawnSlot.PrefabId. </summary>
        public const string SpawnPrefab = "prefab";

        /// <summary> VgdPrefabSpawnSlot.Keycodes. </summary>
        public const string SpawnKeycodes = "keycodes";

        #endregion

        #region .vgd parallax

        /// <summary> VgdParallaxSettings.Layers. </summary>
        public const string ParallaxLayers = "l";

        /// <summary> VgdParallaxSettings.MainLayer. </summary>
        public const string ParallaxMainLayer = "ml";

        /// <summary> VgdParallaxSettings.DepthOfFieldActive. </summary>
        public const string ParallaxDofActive = "dof_active";

        /// <summary> VgdParallaxSettings.DepthOfFieldValue. </summary>
        public const string ParallaxDofValue = "dof_value";

        /// <summary> VgdParallaxLayer.Depth. </summary>
        public const string ParallaxLayerDepth = "d";

        /// <summary> VgdParallaxLayer.Color. </summary>
        public const string ParallaxLayerColor = "c";

        /// <summary> VgdParallaxLayer.Objects. </summary>
        public const string ParallaxLayerObjects = "o";

        /// <summary> VgdParallaxObject.Shape. </summary>
        public const string ParallaxObjectShape = "s";

        // A parallax object's shape node carries its own text under "t", NOT under the "text" a
        // gameplay object uses (ParallaxObject.ShapeData in the source game). Its own constant, so
        // nothing is tempted to reuse ObjectText or one of the several other things called "t".

        /// <summary> VgdParallaxShape.Text. </summary>
        public const string ParallaxShapeText = "t";

        /// <summary> VgdParallaxObject.Color. </summary>
        public const string ParallaxObjectColor = "c";

        /// <summary> VgdParallaxObject.Transform. </summary>
        public const string ParallaxObjectTransform = "t";

        /// <summary> VgdParallaxObject.Animation. </summary>
        public const string ParallaxObjectAnimation = "an";

        /// <summary> VgdParallaxAnimation.Position, VgdParallaxTransform.Position. </summary>
        public const string ParallaxTransformPosition = "p";

        /// <summary> VgdParallaxAnimation.Scale, VgdParallaxTransform.Scale. </summary>
        public const string ParallaxTransformScale = "s";

        /// <summary> VgdParallaxAnimation.Rotation, VgdParallaxTransform.Rotation. </summary>
        public const string ParallaxTransformRotation = "r";

        /// <summary> VgdParallaxAnimation.Length. </summary>
        public const string ParallaxAnimationLength = "l";

        /// <summary> VgdParallaxAnimation.Delay. </summary>
        public const string ParallaxAnimationDelay = "ld";

        /// <summary> VgdParallaxAnimation.LoopPosition. </summary>
        public const string ParallaxAnimationLoopPosition = "ap";

        /// <summary> VgdParallaxAnimation.LoopScale. </summary>
        public const string ParallaxAnimationLoopScale = "as";

        /// <summary> VgdParallaxAnimation.LoopRotation. </summary>
        public const string ParallaxAnimationLoopRotation = "ar";

        #endregion

        #region .vgd checkpoints and markers

        // Capitalized in the format, unlike every other id key. Not a transcription mistake.

        /// <summary> VgdCheckpoint.Id. </summary>
        public const string CheckpointId = "ID";

        /// <summary> VgdCheckpoint.Name. </summary>
        public const string CheckpointName = "n";

        /// <summary> VgdCheckpoint.Time. </summary>
        public const string CheckpointTime = "t";

        /// <summary> VgdCheckpoint.Position. </summary>
        public const string CheckpointPosition = "p";

        /// <summary> VgdMarker.Id. </summary>
        public const string MarkerId = "ID";

        /// <summary> VgdMarker.Name. </summary>
        public const string MarkerName = "n";

        /// <summary> VgdMarker.Description. </summary>
        public const string MarkerDescription = "d";

        /// <summary> VgdMarker.Color. </summary>
        public const string MarkerColor = "c";

        /// <summary> VgdMarker.Time. </summary>
        public const string MarkerTime = "t";

        /// <summary> VgdMarker.Duration. </summary>
        public const string MarkerDuration = "dur";

        /// <summary> VgdAnnotation.Id. </summary>
        public const string AnnotationId = "id";

        /// <summary> VgdAnnotation.MarkerId. </summary>
        public const string AnnotationMarker = "m";

        /// <summary> VgdAnnotation.Time. </summary>
        public const string AnnotationTime = "t";

        /// <summary> VgdAnnotation.Points. </summary>
        public const string AnnotationPoints = "p";

        /// <summary> VgdAnnotation.Color. </summary>
        public const string AnnotationColor = "c";

        #endregion

        #region Object Data (shared by .vgd objects and .vgp objs)

        /// <summary> TrackIndex.Id, VgdParallaxObject.Id. </summary>
        public const string ObjectId = "id";

        /// <summary> TrackIndex.SourcePrefabId. </summary>
        public const string ObjectPrefabId = "pre_id";

        /// <summary> TrackIndex.SourcePlacementId. </summary>
        public const string ObjectPrefabInstanceId = "pre_iid";

        /// <summary> TrackIndex.Name. </summary>
        public const string ObjectName = "n";

        /// <summary> TrackIndex.ObjectType. </summary>
        public const string ObjectType = "ot";

        /// <summary> TrackIndex.StartTime. </summary>
        public const string ObjectStartTime = "st";

        /// <summary> TrackIndex.AutokillType. </summary>
        public const string ObjectAutokillType = "ak_t";

        /// <summary> TrackIndex.AutokillOffset. </summary>
        public const string ObjectAutokillOffset = "ak_o";

        /// <summary> TrackIndex.GradientType. </summary>
        public const string ObjectGradientType = "gt";

        /// <summary> TrackIndex.GradientRotation. </summary>
        public const string ObjectGradientRotation = "gr";

        /// <summary> TrackIndex.GradientScale. </summary>
        public const string ObjectGradientScale = "gs";

        /// <summary> TrackIndex.Shape, VgdParallaxShape.Shape. </summary>
        public const string ObjectShape = "s";

        /// <summary> TrackIndex.ShapeOption, VgdParallaxShape.ShapeOption. </summary>
        public const string ObjectShapeOption = "so";

        // The editor's custom polygon, five numbers, documented nowhere - found by counting the keys
        // real levels carry that no model here reads. Thousands of objects in an ordinary level use
        // it, and every one of them used to import as a Square.

        /// <summary> TrackIndex.CustomShape. </summary>
        public const string ObjectCustomShape = "csp";

        /// <summary> CustomShapeIndex.Text. </summary>
        public const string ObjectText = "text";

        /// <summary> CustomShapeIndex.Depth. </summary>
        public const string ObjectDepth = "d";

        // Absent from the wiki's own object table and from every level written before 2026; found
        // by counting keys real levels carry that no model here read. It is one enum, not the two
        // checkboxes the editor's own panel shows - see ABRenderLayer.

        /// <summary> CustomShapeIndex.RenderLayer. </summary>
        public const string ObjectRenderLayer = "rl";

        /// <summary> CustomShapeIndex.ParentId. </summary>
        public const string ObjectParentId = "p_id";

        /// <summary> CustomShapeIndex.ParentType. </summary>
        public const string ObjectParentType = "p_t";

        /// <summary> CustomShapeIndex.ParentOffsets. </summary>
        public const string ObjectParentOffsets = "p_o";

        /// <summary> CustomShapeIndex.Editor. </summary>
        public const string ObjectEditor = "ed";

        /// <summary> CustomShapeIndex.Origin. </summary>
        public const string ObjectOrigin = "o";

        /// <summary> CustomShapeIndex.Tracks. </summary>
        public const string ObjectTracks = "e";

        /// <summary> VgdObjectEditor.Locked. </summary>
        public const string ObjectEditorLocked = "lk";

        /// <summary> VgdObjectEditor.Collapsed. </summary>
        public const string ObjectEditorCollapsed = "co";

        /// <summary> VgdObjectEditor.TextColor. </summary>
        public const string ObjectEditorTextColor = "tc";

        /// <summary> VgdObjectEditor.BackgroundColor. </summary>
        public const string ObjectEditorBackgroundColor = "bgc";

        /// <summary> VgdObjectEditor.Bin. </summary>
        public const string ObjectEditorBin = "b";

        /// <summary> VgdObjectEditor.Layer. </summary>
        public const string ObjectEditorLayer = "l";

        /// <summary> Where the object sits in the editor's own ordering. Bookkeeping, like the rest
        /// of this block - named here only so it stops reading as a key nobody has seen. </summary>
        public const string ObjectEditorTimelineOrder = "to";

        /// <summary> VgdColorFlags.Red. </summary>
        public const string ColorFlagRed = "r";

        /// <summary> VgdColorFlags.Green. </summary>
        public const string ColorFlagGreen = "g";

        /// <summary> VgdColorFlags.Blue. </summary>
        public const string ColorFlagBlue = "b";

        /// <summary> VgdTrack.Keyframes. </summary>
        public const string TrackKeyframes = "k";

        /// <summary> VgdEventKeyframe.Time, VgdKeyframe.Time. </summary>
        public const string KeyframeTime = "t";

        /// <summary> VgdEventKeyframe.Ease, VgdKeyframe.Ease. </summary>
        public const string KeyframeEase = "ct";

        /// <summary> VgdKeyframe.RandomType. </summary>
        public const string KeyframeRandomType = "r";

        /// <summary> VgdKeyframe.RandomValues. </summary>
        public const string KeyframeRandomValues = "er";

        /// <summary> VgdEventKeyframe.Values, VgdKeyframe.Values, VgdPlacementValue.Values. </summary>
        public const string KeyframeValues = "ev";

        // A real .vgd writes a level-global keyframe's STRING payload under its own key rather than
        // inside "ev" - the theme track is the only one that has one, and it carries nothing else.
        // Reading it out of "ev" (which the wiki's description implies) finds an empty array, so
        // every theme change in the level resolves to no theme at all and the level plays untinted.

        /// <summary> VgdEventKeyframe.Strings. </summary>
        public const string KeyframeValuesStrings = "evs";

        /// <summary> VgdVector2.X. </summary>
        public const string VectorX = "x";

        /// <summary> VgdVector2.Y. </summary>
        public const string VectorY = "y";

        #endregion

        #region .vgd prefab placements

        /// <summary> TrackIndex.Id. </summary>
        public const string PlacementId = "id";

        /// <summary> TrackIndex.PrefabId. </summary>
        public const string PlacementPrefabId = "pid";

        /// <summary> TrackIndex.Editor. </summary>
        public const string PlacementEditor = "ed";

        /// <summary> TrackIndex.Tracks. </summary>
        public const string PlacementTracks = "e";

        // Where a placement sits on the timeline. An object spells its start "st"; a placement uses
        // "t", the same key a marker or a checkpoint does, and the wiki's tree omits it entirely.
        // Without it every placement in a level starts at zero, which is the whole prefab library
        // playing in the first second.

        /// <summary> TrackIndex.StartTime. </summary>
        public const string PlacementTime = "t";

        // Three keys the format's description does not have and the source game does
        // (DataManager.GameData.PrefabObject), all absent from levels written before they existed -
        // which is why every one of them defaults to "this placement behaves as it always did".

        /// <summary> TrackIndex.ParentId. </summary>
        public const string PlacementParentId = "parid";

        /// <summary> TrackIndex.RepeatCount. </summary>
        public const string PlacementRepeatCount = "r";

        /// <summary> TrackIndex.RepeatOffset. </summary>
        public const string PlacementRepeatOffset = "ro";

        #endregion

        #region .vgp

        /// <summary> VgpPrefab.Id. </summary>
        public const string PrefabId = "id";

        /// <summary> VgpPrefab.Name. </summary>
        public const string PrefabName = "n";

        /// <summary> VgpPrefab.Description. </summary>
        public const string PrefabDescription = "description";

        /// <summary> VgpPrefab.Preview. </summary>
        public const string PrefabPreview = "preview";

        /// <summary> VgpPrefab.Type. </summary>
        public const string PrefabType = "type";

        /// <summary> VgpPrefab.Offset. </summary>
        public const string PrefabOffset = "o";

        /// <summary> VgpPrefab.Objects. </summary>
        public const string PrefabObjectsList = "objs";

        /// <summary> Which of a template's own objects it is anchored by. </summary>
        public const string PrefabMainObjectId = "mid";

        /// <summary> A template's own prefab PLACEMENTS - the format's nesting, and the reason a
        /// template is a scope rather than a flat object list. </summary>
        public const string PrefabPlacementsList = "pobjs";

        #endregion

        #region .vgt

        /// <summary> VgtTheme.Id. </summary>
        public const string ThemeId = "id";

        /// <summary> VgtTheme.Name. </summary>
        public const string ThemeName = "name";

        /// <summary> VgtTheme.Background. </summary>
        public const string ThemeBackground = "base_bg";

        /// <summary> VgtTheme.Gui. </summary>
        public const string ThemeGui = "base_gui";

        /// <summary> VgtTheme.GuiAccent. </summary>
        public const string ThemeGuiAccent = "base_gui_accent";

        /// <summary> VgtTheme.Players. </summary>
        public const string ThemePlayers = "pla";

        /// <summary> VgtTheme.Objects. </summary>
        public const string ThemeObjects = "obj";

        /// <summary> VgtTheme.Effects. </summary>
        public const string ThemeEffects = "fx";

        /// <summary> VgtTheme.Parallax. </summary>
        public const string ThemeParallax = "bg";

        #endregion

        #region .vgm

        /// <summary> VgmMeta.Beatmap. </summary>
        public const string MetaBeatmap = "beatmap";

        /// <summary> VgmBeatmap.DateEdited. </summary>
        public const string MetaDateEdited = "date_edited";

        /// <summary> VgmBeatmap.GameVersion. </summary>
        public const string MetaGameVersion = "game_version";

        /// <summary> VgmBeatmap.WorkshopId. </summary>
        public const string MetaWorkshopId = "workshop_id";

        /// <summary> VgmBeatmap.Visibility. </summary>
        public const string MetaVisibility = "visibility";

        /// <summary> VgmBeatmap.Changelog. </summary>
        public const string MetaChangelog = "changelog";

        /// <summary> VgmMeta.Creator. </summary>
        public const string MetaCreator = "creator";

        /// <summary> VgmCreator.SteamName. </summary>
        public const string MetaSteamName = "steam_name";

        /// <summary> VgmCreator.SteamId. </summary>
        public const string MetaSteamId = "steam_id";

        /// <summary> VgmMeta.Song. </summary>
        public const string MetaSong = "song";

        /// <summary> VgmSong.Title. </summary>
        public const string MetaSongTitle = "title";

        /// <summary> VgmSong.Description. </summary>
        public const string MetaSongDescription = "description";

        /// <summary> VgmSong.Difficulty. </summary>
        public const string MetaSongDifficulty = "difficulty";

        /// <summary> VgmSong.Bpm. </summary>
        public const string MetaSongBpm = "bpm";

        /// <summary> VgmSong.Time. </summary>
        public const string MetaSongTime = "time";

        /// <summary> VgmSong.PreviewStart. </summary>
        public const string MetaSongPreviewStart = "preview_start";

        /// <summary> VgmSong.PreviewLength. </summary>
        public const string MetaSongPreviewLength = "preview_length";

        /// <summary> VgmSong.CamJiggle. </summary>
        public const string MetaSongCamJiggle = "cam_jiggle";

        /// <summary> VgmMeta.Artist. </summary>
        public const string MetaArtist = "artist";

        /// <summary> VgmArtist.Name. </summary>
        public const string MetaArtistName = "name";

        /// <summary> VgmArtist.LinkType. </summary>
        public const string MetaArtistLinkType = "link_type";

        /// <summary> VgmArtist.Link. </summary>
        public const string MetaArtistLink = "link";

        /// <summary> VgmMeta.References. </summary>
        public const string MetaReferences = "references";

        /// <summary> VgmReferences.Game. </summary>
        public const string MetaReferenceGame = "game";

        /// <summary> VgmGameReference.Id. </summary>
        public const string MetaReferenceGameId = "id";

        /// <summary> VgmGameReference.Custom. </summary>
        public const string MetaReferenceGameCustom = "custom";

        #endregion
    }
}