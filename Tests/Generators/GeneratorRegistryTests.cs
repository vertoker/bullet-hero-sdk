using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BH.SDK.Generators;
using NUnit.Framework;

namespace BH.SDK.Tests.Generators
{
    // These are the whole-system invariants: every generator the SDK ships is discoverable, named
    // uniquely, and describes its own form completely. They cost nothing per generator and catch
    // the two mistakes that would otherwise only surface in a host's UI - a duplicate key silently
    // shadowing a generator, and a field the form renders in an unpredictable position.
    public class GeneratorRegistryTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Registry_FindsShippedGenerators()
        {
            Assert.IsNotEmpty(GeneratorRegistry.All);
            Assert.IsTrue(GeneratorRegistry.TryGet("gen_level_empty", out var empty));
            Assert.IsInstanceOf<EmptyLevelGenerator>(empty);
            Assert.AreEqual(GeneratorKind.Level, empty.Kind);
        }

        // Test fixtures live in this assembly precisely so they stay out of a host's list.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void Registry_ScansOnlyTheSdkAssembly()
        {
            Assert.IsFalse(GeneratorRegistry.TryGet("gen_test_spawn", out _));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void NameKeys_AreUnique()
        {
            var keys = GeneratorRegistry.All.Select(generator => generator.NameKey).ToList();
            CollectionAssert.AllItemsAreUnique(keys);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void NameKeys_FollowTheKindPrefixConvention()
        {
            foreach (var generator in GeneratorRegistry.All)
            {
                var expected = generator.Kind switch
                {
                    GeneratorKind.Level => "gen_level_",
                    GeneratorKind.Modifier => "mod_",
                    _ => "gen_",
                };
                Assert.IsTrue(generator.NameKey.StartsWith(expected),
                    $"{generator.GetType().Name}: '{generator.NameKey}' should start with '{expected}'");
            }
        }

        // Type.GetFields() order is unspecified by the CLI, so an unlisted field is a form whose
        // layout can change on a recompile - see GeneratorHints' header.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void HintsOrder_CoversEveryParameterField()
        {
            foreach (var generator in GeneratorRegistry.All)
                AssertOrderCoversFields(generator);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void HintsOrder_CoversEveryParameterField_ForTestFixtures()
        {
            IGenerator[] fixtures =
            {
                new SpawnTestGenerator(), new RenameTestModifier(),
                new CameraFlashTestGenerator(), new ScatterTestGenerator(),
            };
            foreach (var generator in fixtures)
                AssertOrderCoversFields(generator);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void CreateDefaultParameters_ReturnsParametersType_AndAFreshInstanceEachTime()
        {
            foreach (var generator in GeneratorRegistry.All)
            {
                var first = generator.CreateDefaultParameters();
                var second = generator.CreateDefaultParameters();

                Assert.IsInstanceOf(generator.ParametersType, first);
                Assert.AreNotSame(first, second,
                    $"{generator.NameKey} must not hand out one shared parameters instance");
            }
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void Hints_AreNeverNull()
        {
            foreach (var generator in GeneratorRegistry.All)
                Assert.IsNotNull(generator.Hints, generator.NameKey);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void OfKind_FiltersByKind()
        {
            foreach (var generator in GeneratorRegistry.OfKind(GeneratorKind.Level))
            {
                Assert.AreEqual(GeneratorKind.Level, generator.Kind);
                Assert.IsInstanceOf<ILevelGenerator>(generator);
            }
            foreach (var generator in GeneratorRegistry.OfKind(GeneratorKind.Content))
                Assert.IsInstanceOf<IScopeGenerator>(generator);
        }

        private static void AssertOrderCoversFields(IGenerator generator)
        {
            var fields = generator.ParametersType
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Select(field => field.Name);
            var ordered = new HashSet<string>(generator.Hints.Order);

            foreach (var field in fields)
            {
                Assert.IsTrue(ordered.Contains(field),
                    $"{generator.NameKey}: field '{field}' is missing from Hints.Order");
            }
        }
    }
}
