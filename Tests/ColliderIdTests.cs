using System;
using BH.SDK.Models.Interfaces.Primitives;
using BH.SDK.Models.Primitives;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    // ColliderId is a Guid-backed id struct, deliberately structured like ThemeId/EffectId (see
    // those types' own doc comments) - this test suite exercises the shared shape (Null/IsEnabled/
    // NewGuid/string constructor/equality) so a future change to one is easy to cross-check against
    // the others.
    public class ColliderIdTests
    {
        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void Null_IsNotEnabled()
        {
            Assert.IsFalse(ColliderId.Null.IsEnabled());
            Assert.AreEqual(Guid.Empty, ColliderId.Null.value);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void NewGuid_IsEnabled_AndDistinct()
        {
            var a = ColliderId.NewGuid();
            var b = ColliderId.NewGuid();

            Assert.IsTrue(a.IsEnabled());
            Assert.IsTrue(b.IsEnabled());
            Assert.AreNotEqual(a, b);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void StringConstructor_RoundTripsGuidConstructor()
        {
            var guid = Guid.NewGuid();
            var fromGuid = new ColliderId(guid);
            var fromString = new ColliderId(guid.ToString());

            Assert.AreEqual(fromGuid, fromString);
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void Equality_OperatorsAndEquatable_AgreeWithValue()
        {
            var guid = Guid.NewGuid();
            var a = new ColliderId(guid);
            var b = new ColliderId(guid);
            var c = ColliderId.NewGuid();

            Assert.IsTrue(a == b);
            Assert.IsTrue(a.Equals(b));
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());

            Assert.IsTrue(a != c);
            Assert.IsFalse(a.Equals(c));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        public void IPrimitiveGuid_ExposesSameValue()
        {
            var colliderId = ColliderId.NewGuid();
            IPrimitiveGuid primitive = colliderId;

            Assert.AreEqual(colliderId.value, primitive.Value);
        }
    }
}
