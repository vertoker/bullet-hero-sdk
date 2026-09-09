using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BH.SDK.Interop.AfterBeat;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BH.SDK.Tests.Interop
{
    // TWO KEYS OF ONE CLASS SHARING A NAME IS A SILENT READ OF THE WRONG FIELD, and this format is
    // where that is most likely to happen: `ABNames` holds 176 constants over 136 distinct values,
    // because another project's format reuses one-letter keys across its documents. Twenty of those
    // values are shared by two or more constants, and every one of those sharings is legitimate -
    // `"t"` alone is a time on a keyframe, a transform on a parallax object, a text on a parallax
    // shape and a time on four other things. The file's own header says so.
    //
    // WHICH IS EXACTLY WHY THE UNIQUENESS CHECK BELONGS ON THE CLASS RATHER THAN ON THE TABLE. A
    // collision across two documents is the format; a collision inside ONE document is a defect,
    // and Newtonsoft does not report it - it writes both and reads back whichever it hit last, so a
    // field simply stops round-tripping. Nothing in the import would throw, and the level would
    // arrive missing one property.
    //
    // The sweep is over the reflected types rather than over a list, so a model added to the format
    // is covered the day it is written. The constants themselves are checked for the one thing a
    // key may never be regardless of which document it belongs to: empty, or padded.

    /// <summary> The Afterbeat interop models: no two properties of one class share a JSON key, and
    /// no key in ABNames is blank or padded. </summary>
    public class AfterBeatKeyTests
    {
        private const string ModelsNamespace = "BH.SDK.Interop.AfterBeat.Models";

        private static IEnumerable<Type> ModelTypes() => typeof(ABNames).Assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => type.Namespace == ModelsNamespace)
            .OrderBy(type => type.Name, StringComparer.Ordinal);

        private static IEnumerable<(PropertyInfo Property, string Key)> KeysOf(Type type) => type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => (property, property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName))
            .Where(pair => pair.PropertyName != null)
            .Select(pair => (pair.property, pair.PropertyName));

        private static string Report(List<string> failures)
        {
            var builder = new StringBuilder();
            builder.Append(failures.Count).Append(" key problem(s):");
            foreach (var failure in failures) builder.Append('\n').Append("  ").Append(failure);
            return builder.ToString();
        }

        // THE ONE THIS FILE EXISTS FOR.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void NoModel_HasTwoPropertiesUnderOneKey()
        {
            var failures = new List<string>();
            var checkedTypes = 0;
            var checkedKeys = 0;

            foreach (var type in ModelTypes())
            {
                var seen = new Dictionary<string, string>();
                var any = false;

                foreach (var (property, key) in KeysOf(type))
                {
                    any = true;
                    checkedKeys++;

                    if (seen.TryGetValue(key, out var owner))
                        failures.Add($"{type.Name}: '{property.Name}' and '{owner}' both write \"{key}\"");
                    else seen[key] = property.Name;
                }

                if (any) checkedTypes++;
            }

            Assert.GreaterOrEqual(checkedTypes, 5, "the namespace filter matched almost no models");
            Assert.Greater(checkedKeys, 50, "the attribute filter matched almost no keys");
            Assert.IsEmpty(failures, Report(failures));
        }

        // A key is written into a file and matched by string equality, so a stray space or an empty
        // name is a property that can never be read back - and neither shows up in a diff.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void EveryConstant_IsANonEmptyUnpaddedKey()
        {
            var failures = new List<string>();
            var checkedConstants = 0;

            foreach (var field in typeof(ABNames).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!field.IsLiteral || field.FieldType != typeof(string)) continue;

                checkedConstants++;
                var value = (string)field.GetRawConstantValue();

                if (string.IsNullOrEmpty(value)) failures.Add($"{field.Name} is empty");
                else if (value != value.Trim()) failures.Add($"{field.Name} is \"{value}\", padded");
            }

            Assert.GreaterOrEqual(checkedConstants, 150, "the reflection filter matched almost nothing");
            Assert.IsEmpty(failures, Report(failures));
        }

        // The collisions ACROSS documents are the format and must not be "fixed" - so they are
        // stated here rather than left to be rediscovered as a bug. What this pins is that they stay
        // collisions of NAME only: every constant sharing a value still has a distinct member name,
        // which is what lets the models refer to the right one.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void SharedKeys_AreSharedByNameOnly()
        {
            var byValue = new Dictionary<string, List<string>>();

            foreach (var field in typeof(ABNames).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!field.IsLiteral || field.FieldType != typeof(string)) continue;

                var value = (string)field.GetRawConstantValue();
                if (!byValue.TryGetValue(value, out var names)) byValue[value] = names = new List<string>();
                names.Add(field.Name);
            }

            var shared = byValue.Where(pair => pair.Value.Count > 1).ToList();

            Assert.IsNotEmpty(shared,
                "no key is shared any more - if the format really stopped reusing them, this test " +
                "and ABNames' own header both need updating");

            foreach (var (value, names) in shared)
            {
                Assert.AreEqual(names.Count, names.Distinct(StringComparer.Ordinal).Count(),
                    $"\"{value}\" is declared twice under one member name");
            }
        }
    }
}
