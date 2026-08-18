using System;
using System.Security.Cryptography;
using System.Text;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat identifies themes and prefabs by arbitrary STRINGS; this format identifies them by
    // Guid. A fresh Guid per import would work exactly once - re-importing the same level, or
    // importing a theme file and then a level that references it, would produce two different ids
    // for the same thing and every reference between them would dangle.
    //
    // So the Guid is DERIVED from the string, deterministically: the same source id always yields
    // the same id here, in this run and in any other, on any machine. That is the same shape as a
    // name-based UUID, and it is what makes "import the .vgt, then import the .vgd that uses it"
    // resolve.
    //
    // SHA-256 truncated to 16 bytes, not MD5: nothing here is a security claim, but a hash with
    // published collisions is a bad default to leave in a file format's neighbourhood. The
    // namespace tag keeps a theme called "0" and a prefab called "0" apart.

    /// <summary> Afterbeat's string ids, mapped onto this format's Guid ids, deterministically. </summary>
    public static class AfterBeatIdMap
    {
        private const string ThemeTag = "afterbeat.theme";
        private const string PrefabTag = "afterbeat.prefab";
        private const string ShapeTag = "afterbeat.shape";

        /// <summary> A stable Guid for a (namespace, id) pair. An empty id yields
        /// <see cref="Guid.Empty"/>, which every id type here reads as Null. </summary>
        public static Guid ToGuid(string tag, string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId)) return Guid.Empty;

            var payload = Encoding.UTF8.GetBytes(tag + "\0" + sourceId);
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(payload);

            var bytes = new byte[16];
            Array.Copy(hash, bytes, 16);
            var guid = new Guid(bytes);

            // Astronomically unlikely, but a Null id means "unset" everywhere in this format and
            // must never be produced by accident.
            return guid == Guid.Empty ? new Guid(1, 0, 0, bytes) : guid;
        }

        public static ThemeId ToThemeId(string sourceId) => new(ToGuid(ThemeTag, sourceId));
        public static PrefabId ToPrefabId(string sourceId) => new(ToGuid(PrefabTag, sourceId));

        /// <summary> For a shape this converter has to synthesize; a shape that maps onto a built-in
        /// preset keeps that preset's own id instead. </summary>
        public static ShapeId ToShapeId(string sourceId) => new(ToGuid(ShapeTag, sourceId));
    }
}
