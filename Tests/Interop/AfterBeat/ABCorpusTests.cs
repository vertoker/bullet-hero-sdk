using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Objects;
using BH.SDK.Validations;
using BH.SDK.Validations.Graph;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop.AfterBeat
{
    // Runs the converter against REAL Afterbeat content, which is the only thing that can find what
    // a hand-written fixture cannot: keys the wiki does not document, shapes newer than its tables,
    // and the ordinary weirdness of a level somebody actually made.
    //
    // No such file lives in this repository, and that is deliberate rather than an oversight - it
    // is other people's user content, and this is a public MIT library. The corpus lives outside,
    // named by an environment variable the author sets once. With the variable unset, or the folder
    // empty, this fixture PASSES having checked nothing and says so in the log.
    //
    // One [Test] with a loop rather than a [TestCaseSource], on purpose: NUnit reports a test case
    // source that yields nothing as a non-runnable test, which is a red suite for a corpus that is
    // legitimately absent. [Explicit]/[Ignore] are not used anywhere in this project for the same
    // family of reason - a test that never runs rots silently.
    public class ABCorpusTests
    {
        /// <summary> Where the author keeps real Afterbeat content. Expected to point at the
        /// "afterbeat" folder beside the game's own saves. </summary>
        public const string CorpusVariable = "BH_AFTERBEAT_CORPUS";

        /// <summary> The framerate the corpus is imported at. High enough that keyframes a level
        /// authored close together do not collapse onto one frame. </summary>
        private const int Framerate = 60;

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Extreme)]
        public void Corpus_EveryLevel_ImportsValidatesAndExports()
        {
            var files = Collect("*.vgd");
            if (files.Count == 0) return;

            var reports = new StringBuilder[files.Count];
            var lines = new string[files.Count];

            InParallel(files, () => new ValidationFacade(), (index, file, facade) =>
            {
                var report = reports[index] = new StringBuilder();

                var json = ReadOrSkip(file, report);
                if (json == null) return;

                // ONE PARSE PER DOCUMENT, where this used to take two: CountEmitters read the same
                // text through Newtonsoft a second time to count what the importer's own input
                // already holds, and on this corpus that second read cost more than every check
                // here put together.
                if (!ABSerialization.TryDeserialize<VgdLevel>(json, out var source, out var error))
                {
                    report.AppendLine($"{file}: could not be imported - {error}");
                    return;
                }

                var result = ABLevelImporter.Import(source, null, new ABOptions(Framerate));

                if (result.Report.HasFailure || result.Level == null)
                {
                    report.AppendLine($"{file}: could not be imported - {result.Report}");
                    return;
                }

                // The model this produced has to be one the format itself accepts; an import that
                // writes an illegal level is worse than one that refuses. Errors only - an advice
                // or a warning is the format telling the author something, not the converter
                // having produced something illegal.
                var validation = facade.Validate(result.Level);
                if (validation.HasErrors)
                    report.AppendLine($"{file}: imported into a level that fails validation - " +
                                      $"{validation}{Environment.NewLine}{Breakdown(validation)}");

                // Every emitter has to have become a real effect placement pointing at a resource
                // that is actually there. A dangling EffectId draws nothing and says nothing, which
                // is precisely the silent failure the derived id exists to prevent.
                var emitters = CountEmitters(source);
                var placements = result.Level.Game.Objects.Values.OfType<EffectObject>().ToArray();

                if (placements.Length != emitters)
                    report.AppendLine($"{file}: {emitters} emitter(s) in the document, " +
                                      $"{placements.Length} effect placement(s) imported");

                foreach (var placement in placements)
                    if (!result.Level.Resources.Effects.ContainsKey(placement.EffectId))
                        report.AppendLine($"{file}: effect placement {placement.ObjectId} " +
                                          "points at a resource that is not in the level");

                var exported = ABLevelExporter.Export(result.Level, result.Meta);
                if (exported.Level == null)
                    report.AppendLine($"{file}: imported but could not be exported back");

                lines[index] = $"{Path.GetFileName(file)}: " +
                               $"{result.Level.Game.Objects.Count} object(s), {result.Report}";
            });

            var failures = new StringBuilder();
            for (var index = 0; index < files.Count; index++)
            {
                if (lines[index] != null) TestContext.WriteLine(lines[index]);
                if (reports[index] != null) failures.Append(reports[index]);
            }

            Assert.IsEmpty(failures.ToString());
        }

        /// <summary> Emitters in the raw document, counted off the source rather than off what the
        /// importer produced - the point is to compare the two. </summary>
        private static int CountEmitters(VgdLevel source)
        {
            if (source?.Objects == null) return 0;

            var count = 0;
            foreach (var candidate in source.Objects)
                if (ABParticleMap.IsEmitter(candidate))
                    count++;

            return count;
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Corpus_EveryTheme_RoundTrips()
        {
            var files = Collect("*.vgt");
            if (files.Count == 0) return;

            var failures = new StringBuilder();

            foreach (var file in files)
            {
                var json = ReadOrSkip(file, failures);
                if (json == null) continue;

                var report = new InteropReport();
                var theme = ABInterop.ImportTheme(json, report);

                if (theme == null || report.HasFailure)
                {
                    failures.AppendLine($"{file}: could not be imported - {report}");
                    continue;
                }

                if (string.IsNullOrEmpty(ABInterop.ExportTheme(theme)))
                    failures.AppendLine($"{file}: imported but exported to nothing");
            }

            Assert.IsEmpty(failures.ToString());
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Hard)]
        public void Corpus_EveryPrefab_RoundTrips()
        {
            var files = Collect("*.vgp");
            if (files.Count == 0) return;

            var failures = new StringBuilder();

            foreach (var file in files)
            {
                var json = ReadOrSkip(file, failures);
                if (json == null) continue;

                var report = new InteropReport();
                var prefab = ABInterop.ImportPrefab(json, new ABOptions(Framerate), report);

                if (prefab == null || report.HasFailure)
                {
                    failures.AppendLine($"{file}: could not be imported - {report}");
                    continue;
                }

                if (string.IsNullOrEmpty(ABInterop.ExportPrefab(prefab)))
                    failures.AppendLine($"{file}: imported but exported to nothing");
            }

            Assert.IsEmpty(failures.ToString());
        }

        // A count of findings names nothing an author or a converter can act on. What matters is
        // WHICH rule, and one example of where - the same finding usually repeats across a thousand
        // objects, and the first one is the one to go and look at.
        private static string Breakdown(ValidationReport report)
        {
            var counts = new Dictionary<string, int>();
            var examples = new Dictionary<string, string>();

            foreach (var issue in report.RuleIssues ?? new List<RuleIssue>())
                Tally(counts, examples, issue.Rule?.GetType().Name ?? "rule",
                    $"{issue.GetPath()} = {Describe(issue)}");

            foreach (var issue in report.GraphIssues ?? new List<GraphIssue>())
                Tally(counts, examples, $"graph:{issue.Rule}", issue.ToString());

            var keys = new List<string>(counts.Keys);
            keys.Sort((a, b) => counts[b].CompareTo(counts[a]));

            var text = new StringBuilder();
            foreach (var key in keys)
                text.AppendLine($"    {key} x{counts[key]}   e.g. {examples[key]}");
            return text.ToString();
        }

        // The value is what turns a finding into a fix: "out of range" says nothing, "-1.5 where the
        // range starts at 0" says the source format allows a negative size and this one does not.
        private static string Describe(RuleIssue issue)
        {
            try
            {
                var value = issue.GetValue();
                return value?.ToString() ?? "null";
            }
            catch (Exception exception)
            {
                return $"<unreadable: {exception.GetType().Name}>";
            }
        }

        private static void Tally(IDictionary<string, int> counts, IDictionary<string, string> examples,
            string key, string example)
        {
            counts.TryGetValue(key, out var count);
            counts[key] = count + 1;
            if (!examples.ContainsKey(key)) examples[key] = example ?? string.Empty;
        }

        // The other half of what a corpus is for, and the half that pays off fastest: every node
        // keeps what it did not recognise (ABNode.Unknown), and nothing ever looked inside
        // it. Counting those keys is what revealed the theme track's own payload key, a placement's
        // start time and the editor's custom polygon - three silent, level-breaking conversions that
        // every hand-written fixture in this folder passed straight over.
        //
        // It asserts nothing and cannot fail: an unknown key is not a defect, it is the format being
        // larger than this transcription of it. The output is the finding.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Extreme)]
        public void Corpus_UnknownKeys_AreListed()
        {
            var files = Collect("*.vgd");
            if (files.Count == 0) return;

            var read = new bool[files.Count];
            var counts = new Dictionary<string, int>[files.Count];
            var samples = new Dictionary<string, string>[files.Count];

            InParallel(files, (index, file) =>
            {
                var json = ReadOrSkip(file, new StringBuilder());
                if (json == null) return;

                if (!ABSerialization.TryDeserialize<VgdLevel>(json, out var level, out _))
                    return;

                // Each worker tallies its OWN document and the parsed model dies with the call.
                // Holding every document to walk them afterwards is the one thing the memory here
                // cannot take - this corpus is tens of megabytes of JSON and parses to far more.
                read[index] = true;
                counts[index] = new Dictionary<string, int>();
                samples[index] = new Dictionary<string, string>();

                CollectUnknown(level, counts[index], samples[index]);
            });

            var totals = new Dictionary<string, int>();
            var examples = new Dictionary<string, string>();
            var documents = 0;

            // Merged in FILE ORDER, so which occurrence becomes a key's example - and where a tie
            // in the sort below lands - is the same on every run however the workers finished.
            for (var index = 0; index < files.Count; index++)
            {
                if (!read[index]) continue;
                documents++;

                foreach (var pair in counts[index])
                {
                    totals.TryGetValue(pair.Key, out var count);
                    totals[pair.Key] = count + pair.Value;
                }

                foreach (var pair in samples[index])
                    if (!examples.ContainsKey(pair.Key)) examples[pair.Key] = pair.Value;
            }

            if (totals.Count == 0)
            {
                TestContext.WriteLine($"Every key of {documents} document(s) is one this build reads.");
                return;
            }

            var lines = new List<string>(totals.Keys);
            lines.Sort((a, b) => totals[b].CompareTo(totals[a]));

            TestContext.WriteLine($"Keys no model here reads, across {documents} document(s):");
            foreach (var key in lines)
                TestContext.WriteLine($"  {key} x{totals[key]}   e.g. {examples[key]}");
        }

        // Walks the wire models rather than the raw JSON, so a key is only "unknown" where the model
        // that OWNS it has no property for it - which is what separates a key nobody has seen from
        // one this build reads perfectly well somewhere else. That distinction is the whole point:
        // a placement's own start time was spelled the same as a marker's, and reading the two as
        // one key would have hidden it.
        private static void CollectUnknown(VgdLevel level,
            IDictionary<string, int> counts, IDictionary<string, string> samples)
        {
            Note(level, "level", counts, samples);
            Note(level.Editor, "editor", counts, samples);
            Note(level.Parallax, "parallax_settings", counts, samples);

            foreach (var checkpoint in level.Checkpoints) Note(checkpoint, "checkpoints[]", counts, samples);
            foreach (var marker in level.Markers) Note(marker, "markers[]", counts, samples);
            foreach (var annotation in level.Annotations) Note(annotation, "annotations[]", counts, samples);
            foreach (var trigger in level.Triggers) Note(trigger, "triggers[]", counts, samples);

            foreach (var theme in level.Themes) Note(theme, "themes[]", counts, samples);
            foreach (var obj in level.Objects) NoteObject(obj, "objects[]", counts, samples);

            foreach (var placement in level.PrefabPlacements)
            {
                Note(placement, "prefab_objects[]", counts, samples);
                Note(placement.Editor, "prefab_objects[].ed", counts, samples);
            }

            foreach (var prefab in level.Prefabs)
            {
                Note(prefab, "prefabs[]", counts, samples);
                foreach (var obj in prefab.Objects) NoteObject(obj, "prefabs[].objs[]", counts, samples);
            }

            if (level.Events == null) return;
            foreach (var track in level.Events)
            foreach (var key in track ?? new List<VgdEventKeyframe>())
                Note(key, "events[][]", counts, samples);
        }

        private static void NoteObject(VgdObject source, string path,
            IDictionary<string, int> counts, IDictionary<string, string> samples)
        {
            if (source == null) return;

            Note(source, path, counts, samples);
            Note(source.Editor, $"{path}.ed", counts, samples);

            if (source.Tracks == null) return;
            foreach (var track in source.Tracks)
            {
                Note(track, $"{path}.e[]", counts, samples);
                if (track?.Keyframes == null) continue;
                foreach (var keyframe in track.Keyframes) Note(keyframe, $"{path}.e[].k[]", counts, samples);
            }
        }

        private static void Note(ABNode node, string path,
            IDictionary<string, int> counts, IDictionary<string, string> samples)
        {
            if (node?.Unknown == null) return;

            foreach (var pair in node.Unknown)
            {
                var key = $"{path}.{pair.Key}";
                counts.TryGetValue(key, out var count);
                counts[key] = count + 1;

                if (samples.ContainsKey(key)) continue;
                var text = pair.Value?.ToString(Newtonsoft.Json.Formatting.None) ?? "null";
                samples[key] = text.Length > 80 ? text.Substring(0, 80) + "…" : text;
            }
        }

        #region Parallelism

        // EVERY DOCUMENT IS INDEPENDENT OF EVERY OTHER, and reading them one at a time left every
        // core but one idle for the better part of a minute - the corpus is a couple of dozen
        // files and tens of megabytes of JSON, and the parse is most of what this fixture does.
        //
        // WHAT BOUNDS THE DEGREE IS MEMORY, NOT CORES. The largest thing here is a parsed .vgd plus
        // the Level it imports into, and a handful of those at once is already hundreds of
        // megabytes; a machine with more cores must not turn that into gigabytes.
        //
        // Every result is written into an array indexed by FILE, never appended to a shared buffer,
        // which is what makes the report read identically on every run without a single lock.
        private const int MaxParallelFiles = 4;

        private static ParallelOptions Options => new()
        {
            MaxDegreeOfParallelism = Math.Min(MaxParallelFiles, Environment.ProcessorCount),
        };

        private static void InParallel(IReadOnlyList<string> files, Action<int, string> body)
            => Parallel.For(0, files.Count, Options, index => body(index, files[index]));

        /// <summary> As above, with one <typeparamref name="TState"/> per WORKER rather than one
        /// per file: a ValidationFacade warms a cache over the whole assembly in its constructor,
        /// which is worth paying a few times and not once per document. </summary>
        private static void InParallel<TState>(IReadOnlyList<string> files,
            Func<TState> create, Action<int, string, TState> body)
            => Parallel.For(0, files.Count, Options, create,
                (index, _, state) =>
                {
                    body(index, files[index], state);
                    return state;
                },
                _ => { });

        #endregion

        #region Corpus discovery

        // Every reason to find nothing is legitimate and none of them is a failure: the variable is
        // unset on a machine that has no corpus, and the folder is empty until the author drops
        // something into it.
        private static List<string> Collect(string pattern)
        {
            var files = new List<string>();
            var root = Environment.GetEnvironmentVariable(CorpusVariable);

            if (string.IsNullOrWhiteSpace(root))
            {
                TestContext.WriteLine(
                    $"{CorpusVariable} is not set - no Afterbeat corpus to check. " +
                    "Point it at the 'afterbeat' folder beside the game's saves to run this for real.");
                return files;
            }

            if (!Directory.Exists(root))
            {
                TestContext.WriteLine($"{CorpusVariable} points at '{root}', which does not exist.");
                return files;
            }

            files.AddRange(Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories));
            TestContext.WriteLine($"Found {files.Count} {pattern} file(s) under '{root}'.");
            return files;
        }

        private static string ReadOrSkip(string file, StringBuilder failures)
        {
            try
            {
                return File.ReadAllText(file);
            }
            catch (Exception exception)
            {
                failures.AppendLine($"{file}: could not be read - {exception.Message}");
                return null;
            }
        }

        #endregion
    }
}
