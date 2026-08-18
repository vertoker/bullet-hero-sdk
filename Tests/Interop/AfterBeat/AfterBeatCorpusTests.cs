using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BH.SDK.Interop;
using BH.SDK.Interop.AfterBeat;
using BH.SDK.Interop.AfterBeat.Export;
using BH.SDK.Interop.AfterBeat.Import;
using BH.SDK.Validations;
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
    public class AfterBeatCorpusTests
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

            var failures = new StringBuilder();

            foreach (var file in files)
            {
                var json = ReadOrSkip(file, failures);
                if (json == null) continue;

                var result = AfterBeatLevelImporter.ImportJson(json, null, new AfterBeatOptions(Framerate));

                if (result.Report.HasFailure || result.Level == null)
                {
                    failures.AppendLine($"{file}: could not be imported - {result.Report}");
                    continue;
                }

                // The model this produced has to be one the format itself accepts; an import that
                // writes an illegal level is worse than one that refuses. Errors only - an advice
                // or a warning is the format telling the author something, not the converter
                // having produced something illegal.
                var validation = new ValidationFacade().Validate(result.Level);
                if (validation.HasErrors)
                    failures.AppendLine($"{file}: imported into a level that fails validation - {validation}");

                var exported = AfterBeatLevelExporter.Export(result.Level, result.Meta);
                if (exported.Level == null)
                    failures.AppendLine($"{file}: imported but could not be exported back");

                TestContext.WriteLine($"{Path.GetFileName(file)}: " +
                                      $"{result.Level.Game.Objects.Count} object(s), {result.Report}");
            }

            Assert.IsEmpty(failures.ToString());
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
                var theme = AfterBeatInterop.ImportTheme(json, report);

                if (theme == null || report.HasFailure)
                {
                    failures.AppendLine($"{file}: could not be imported - {report}");
                    continue;
                }

                if (string.IsNullOrEmpty(AfterBeatInterop.ExportTheme(theme)))
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
                var prefab = AfterBeatInterop.ImportPrefab(json, new AfterBeatOptions(Framerate), report);

                if (prefab == null || report.HasFailure)
                {
                    failures.AppendLine($"{file}: could not be imported - {report}");
                    continue;
                }

                if (string.IsNullOrEmpty(AfterBeatInterop.ExportPrefab(prefab)))
                    failures.AppendLine($"{file}: imported but exported to nothing");
            }

            Assert.IsEmpty(failures.ToString());
        }

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
