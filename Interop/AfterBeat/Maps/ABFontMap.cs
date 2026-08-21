using System;
using System.Collections.Generic;
using BH.SDK.Models.Primitives.Resources;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat has no font FIELD at all - a text object's typeface is set INLINE, with
    // TextMeshPro's <font> tag written into the string itself, and TMP resolves it by name as
    // Resources.Load(TMP_Settings.defaultFontAssetPath + name). This format instead carries one
    // FontResourceId per TextObject, so the tag cannot cross as a tag: it is removed on import
    // (nothing here parses it, and an unparsed tag draws as its own characters) and what it asked
    // for is written onto the object instead.
    //
    // THE TEN NAMES BELOW ARE THE GAME'S OWN Resources FOLDER, read out of its data files rather
    // than out of the wiki: the paths under "fonts & materials/" in Afterbeat_Data/globalgamemanagers
    // are exactly what Resources.Load can answer, so they are the whole vocabulary a <font> tag has.
    // A name outside them loads nothing, TMP leaves the typeface it was already drawing in alone,
    // and Selector.Push does the same - an unknown name is not a change of typeface, it is a tag
    // that did nothing over there either. (The material variants sitting in that same folder -
    // "liberationsans sdf - outline" and friends - are <material> tags, a different vocabulary that
    // is not read here.) Old levels write "<font=LiberationSans SDF>" instead, which the source
    // game rewrites on load (DataManager.UpdateBeatmap's fonts gate, versions <= 23.10.4); a .vgd
    // read here never went through that gate, hence Normalize strips the "SDF" suffix and the
    // quotes rather than trusting the migrated spelling.
    //
    // The right-hand side is this project's own font presets (Assets/Settings/PresetsFonts/*.asset,
    // FontResourceScriptable) and the ids ARE that contract: nothing in the SDK names a
    // game-shipped font, so the two lists have to stay in lockstep by hand.
    //
    // Pairing is by ROLE, never by metrics - nothing here is metric-compatible with anything there,
    // so the question is only which of ours carries the same intent:
    //
    //   LiberationSans          -> NotoSans      the source's default onto this format's default
    //   Roboto-Bold             -> Roboto        the same typeface, one weight lighter
    //   Inconsolata             -> JetBrainsMono monospace onto the only monospace here
    //   MajorMonoDisplay        -> JetBrainsMono monospace again; the display cut has no counterpart
    //   Anton                   -> Oi            the heaviest display face on each side
    //   Oswald Bold             -> AdventPro     condensed, and deliberately NOT Oi: Anton and
    //                                            Oswald Bold are near twins, and collapsing both
    //                                            onto one face loses a distinction the level made
    //   Bangers                 -> ComicRelief   comic lettering
    //   Poorstory               -> ComicRelief   informal/handwritten; its Korean coverage is lost
    //                                            either way, no font shipped here has it
    //   Electronic Highway Sign -> PressStart2P  pixel/LED grid
    //   hellovetica             -> Tiny5         low-resolution bitmap grotesque
    //
    // RubikStorm and Play are on no right-hand side: Afterbeat ships nothing distressed and nothing
    // technical to point at them. They still export - see ToName, where every preset needs an answer.

    /// <summary> Typeface names as Afterbeat's <c>&lt;font&gt;</c> tag spells them, mapped onto this
    /// game's own font presets. </summary>
    public static class ABFontMap
    {
        // Mirror of Assets/Settings/PresetsFonts/*.asset - see the header's lockstep note

        private const int NotoSansId = 1;
        private const int RobotoId = 2;
        private const int JetBrainsMonoId = 3;
        private const int PlayId = 4;
        private const int ComicReliefId = 5;
        private const int OiId = 6;
        private const int PressStart2PId = 7;
        private const int Tiny5Id = 8;
        private const int AdventProId = 9;
        private const int RubikStormId = 10;

        /// <summary> The suffix a TextMeshPro font asset carries in its own name, which older levels
        /// write into the tag and newer ones do not. </summary>
        private const string SdfSuffix = " sdf";

        private static readonly Dictionary<string, FontResourceId> ToFont = new(StringComparer.Ordinal)
        {
            { "liberationsans", new FontResourceId(NotoSansId) },
            { "roboto-bold", new FontResourceId(RobotoId) },
            { "inconsolata", new FontResourceId(JetBrainsMonoId) },
            { "majormonodisplay", new FontResourceId(JetBrainsMonoId) },
            { "anton", new FontResourceId(OiId) },
            { "oswald bold", new FontResourceId(AdventProId) },
            { "bangers", new FontResourceId(ComicReliefId) },
            { "poorstory", new FontResourceId(ComicReliefId) },
            { "electronic highway sign", new FontResourceId(PressStart2PId) },
            { "hellovetica", new FontResourceId(Tiny5Id) },
        };

        // The way back is the same pairing read the other way, and it cannot be the same table:
        // two source names land on one preset here (Inconsolata and MajorMonoDisplay, Bangers and
        // Poorstory), so each preset needs one CANONICAL name chosen for it. Two presets Afterbeat
        // inspired nothing in are paired by role like everything else - Play onto LiberationSans as
        // the neutral grotesque, RubikStorm onto Anton as the heaviest thing there - because the
        // alternative is exporting them as the default anyway, only silently.
        //
        // The names carry the "SDF" suffix exactly where the game's own asset does (its Resources
        // paths are the authority; four of the ten have no suffix at all), so the tag written here
        // is what its Resources.Load answers to.

        private static readonly Dictionary<int, string> ToName = new()
        {
            { NotoSansId, "LiberationSans" },
            { RobotoId, "Roboto-Bold SDF" },
            { JetBrainsMonoId, "Inconsolata" },
            { PlayId, "LiberationSans" },
            { ComicReliefId, "Bangers SDF" },
            { OiId, "Anton SDF" },
            { PressStart2PId, "Electronic Highway Sign SDF" },
            { Tiny5Id, "hellovetica" },
            { AdventProId, "Oswald Bold SDF" },
            { RubikStormId, "Anton SDF" },
        };

        /// <summary> The name Afterbeat knows the nearest typeface to this one by, false for a font
        /// the level ships itself and for any preset nothing over there resembles. </summary>
        public static bool TryExport(FontResourceId fontResourceId, out string name)
        {
            name = null;
            return fontResourceId.IsGameDefined() && ToName.TryGetValue(fontResourceId.value, out name);
        }

        /// <summary> Resolves one <c>&lt;font&gt;</c> value, false when Afterbeat itself would have
        /// found nothing under that name. </summary>
        public static bool TryResolve(string name, out FontResourceId fontResourceId)
        {
            fontResourceId = FontResourceId.Default;

            var normalized = Normalize(name);
            return !string.IsNullOrEmpty(normalized) && ToFont.TryGetValue(normalized, out fontResourceId);
        }

        /// <summary> Folds a tag's value into the spelling the table is keyed by - unquoted,
        /// lower case, without the font asset's own "SDF" suffix. </summary>
        public static string Normalize(string name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;

            var value = name.Trim().Trim('"', '\'').Trim().ToLowerInvariant();
            if (value.EndsWith(SdfSuffix, StringComparison.Ordinal))
                value = value[..^SdfSuffix.Length].TrimEnd();

            return value;
        }

        // A text can name several typefaces and this format holds one, so the object is given the
        // one covering MOST of its characters rather than the first one written - a colour-sized
        // <font> run around one word must not repaint the whole block. Ties go to whichever was
        // seen first, which is what keeps an ordinary "one tag, one font" string obvious.
        //
        // The stack is TMP's own: <font> nests and </font> returns to what was in force under it.

        /// <summary> Accumulates which typeface covers how much of one string while its tags are
        /// scanned, so the object can be given the one that covers most of it. </summary>
        public sealed class Selector
        {
            private readonly List<FontResourceId> _stack = new();
            private readonly List<Coverage> _coverage = new();

            /// <summary> Whether any tag named a typeface this map knows - the ones it does not are
            /// no loss, since they drew in the current typeface over there too. </summary>
            public bool Recognized { get; private set; }

            private FontResourceId Current => _stack.Count > 0 ? _stack[^1] : FontResourceId.Default;

            /// <summary> Opens a <c>&lt;font&gt;</c> run. </summary>
            public void Push(string name)
            {
                if (TryResolve(name, out var fontResourceId)) Recognized = true;
                else fontResourceId = Current;

                _stack.Add(fontResourceId);
            }

            /// <summary> Closes the innermost <c>&lt;font&gt;</c> run. </summary>
            public void Pop()
            {
                if (_stack.Count > 0) _stack.RemoveAt(_stack.Count - 1);
            }

            /// <summary> Charges a run of drawn characters to whichever typeface is in force. </summary>
            public void Count(int characters)
            {
                if (characters <= 0) return;

                var current = Current;
                for (var index = 0; index < _coverage.Count; index++)
                {
                    if (_coverage[index].FontResourceId != current) continue;
                    _coverage[index] = new Coverage(current, _coverage[index].Characters + characters);
                    return;
                }

                _coverage.Add(new Coverage(current, characters));
            }

            /// <summary> The typeface the object is given, reporting through <paramref name="mixed"/>
            /// whether the string used more than one. </summary>
            public FontResourceId Resolve(out bool mixed)
            {
                var winner = FontResourceId.Default;
                var best = 0;
                var used = 0;

                foreach (var coverage in _coverage)
                {
                    if (coverage.Characters <= 0) continue;
                    used++;

                    if (coverage.Characters <= best) continue;
                    best = coverage.Characters;
                    winner = coverage.FontResourceId;
                }

                mixed = used > 1;
                return winner;
            }

            private readonly struct Coverage
            {
                public readonly FontResourceId FontResourceId;
                public readonly int Characters;

                public Coverage(FontResourceId fontResourceId, int characters)
                {
                    FontResourceId = fontResourceId;
                    Characters = characters;
                }
            }
        }
    }
}