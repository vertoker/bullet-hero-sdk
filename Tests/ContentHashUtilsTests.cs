using System.IO;
using System.Text;
using BH.SDK.Utils;
using NUnit.Framework;

namespace BH.SDK.Tests
{
    /// <summary>
    /// ContentHashUtils and ByteSizeUtils: the digest a takedown is answered by, and the way a size
    /// limit is explained to whoever ran into it.
    /// </summary>
    public class ContentHashUtilsTests
    {
        // Published SHA-256 vectors, not values this implementation produced. A digest that only
        // agrees with itself is worthless - the whole point is that a moderator can reproduce it
        // with sha256sum, or a server written in another language can match against it.
        private const string EmptyHash =
            "sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        private const string AbcHash =
            "sha256:ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad";

        #region Hashing

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TestKnownVectors()
        {
            Assert.AreEqual(EmptyHash, ContentHashUtils.Sha256(new byte[0]));
            Assert.AreEqual(AbcHash, ContentHashUtils.Sha256(Encoding.ASCII.GetBytes("abc")));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.Easy)]
        public void TestStreamMatchesBuffer()
        {
            var data = Encoding.ASCII.GetBytes("abc");
            using var stream = new MemoryStream(data);

            Assert.AreEqual(AbcHash, ContentHashUtils.Sha256(stream));
        }

        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TestDifferentBytesDifferentHash()
        {
            var first = ContentHashUtils.Sha256(Encoding.ASCII.GetBytes("track-a"));
            var second = ContentHashUtils.Sha256(Encoding.ASCII.GetBytes("track-b"));

            Assert.AreNotEqual(first, second);
            Assert.IsTrue(ContentHashUtils.IsSha256(first));
        }

        #endregion

        #region Validation

        [TestCase("sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", true)]
        [TestCase("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", false)]
        [TestCase("sha256:tooshort", false)]
        [TestCase("md5:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TestIsSha256(string value, bool expected)
        {
            Assert.AreEqual(expected, ContentHashUtils.IsSha256(value));
        }

        // A value can arrive from a tool that prints uppercase hex, and two spellings of the same
        // digest must not read as two different works.
        [Test]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TestMatchesIgnoresCase()
        {
            Assert.IsTrue(ContentHashUtils.Matches(AbcHash, AbcHash.ToUpperInvariant()));
            Assert.IsFalse(ContentHashUtils.Matches(AbcHash, EmptyHash));
            Assert.IsFalse(ContentHashUtils.Matches(AbcHash, null));
            Assert.IsFalse(ContentHashUtils.Matches(null, null));
        }

        #endregion

        #region Sizes

        [TestCase(0L, "0 B")]
        [TestCase(-5L, "0 B")]
        [TestCase(512L, "512 B")]
        [TestCase(1024L, "1 KB")]
        [TestCase(1536L, "1.5 KB")]
        [TestCase(1048576L, "1 MB")]
        [TestCase(52428800L, "50 MB")]
        [TestCase(1073741824L, "1 GB")]
        [Author(Metadata.Author.Vertoker)]
        [Category(Metadata.Category.Self)]
        [Category(Metadata.Category.VeryEasy)]
        public void TestFormatSize(long bytes, string expected)
        {
            Assert.AreEqual(expected, ByteSizeUtils.Format(bytes));
        }

        #endregion
    }
}
