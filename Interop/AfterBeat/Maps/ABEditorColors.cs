using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat's editor chrome has a colour list of its own, separate from every theme a level
    // carries: EditorManager.GridColors, seven entries, and a marker stores an INDEX into it
    // (MarkerEditor draws GridColors[Mathf.Clamp(marker.color, 0, Count - 1)]). It is not a theme
    // palette and must not be routed through ABColorMap - a marker coloured 3 means this list's
    // fourth colour, not a level theme's fourth object colour, and the two are unrelated.
    //
    // The values are Inspector-authored, so the decompiled source declares the field and never its
    // contents; they were read out of the shipped game's own data (Afterbeat_Data/level6, the
    // editor scene, the MonoBehaviour whose script resolves to EditorManager). Three of the seven
    // match colours recovered independently for AB-DEFAULT-THEMES.md, which is the cross-check that
    // the byte-level decode landed where it claimed to.
    //
    // Markers used to import as flat white, which lost a real authoring signal: one workshop level
    // colours 43 markers across six of these seven.

    /// <summary> Afterbeat's editor-chrome colours - the palette a marker's colour index points
    /// into. </summary>
    public static class ABEditorColors
    {
        /// <summary> <c>EditorManager.GridColors</c>, in its own order. </summary>
        public static readonly Color4Value[] GridColors =
        {
            new(1f, 1f, 1f, 1f),
            new(0.501961f, 0.501961f, 0.501961f, 1f),
            new(0f, 0f, 0f, 1f),
            new(0.937255f, 0.278431f, 0.407843f, 1f),
            new(0f, 0.682353f, 0.937255f, 1f),
            new(0f, 0.937255f, 0.054902f, 1f),
            new(0.254902f, 0.262745f, 0.419608f, 1f),
        };

        /// <summary> The colour a marker's index means. Out-of-range clamps, exactly as the source
        /// game's own draw does, so a hand-edited index behaves the same on both sides. </summary>
        public static Color4Value Import(int index)
        {
            if (index < 0) index = 0;
            if (index >= GridColors.Length) index = GridColors.Length - 1;
            return GridColors[index].Copy();
        }

        /// <summary> And back - the nearest entry, since this format stores a colour and Afterbeat
        /// stores a choice among seven. Squared distance over RGB; alpha is 1 throughout the list,
        /// so weighing it would only ever penalize a translucent marker uniformly. </summary>
        public static int Export(Color4Value color)
        {
            if (color == null) return 0;

            var best = 0;
            var bestDistance = float.MaxValue;

            for (var i = 0; i < GridColors.Length; i++)
            {
                var entry = GridColors[i];
                var dr = entry.R - color.R;
                var dg = entry.G - color.G;
                var db = entry.B - color.B;
                var distance = dr * dr + dg * dg + db * db;

                if (distance >= bestDistance) continue;
                bestDistance = distance;
                best = i;
            }

            return best;
        }
    }
}