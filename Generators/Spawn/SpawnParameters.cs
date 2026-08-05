using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;

namespace BH.SDK.Generators.Spawn
{
    // Every spawning generator needs the same four "what does one of these look like" fields, so
    // they live on a shared base class rather than being copy-pasted (and inevitably renamed) into
    // each parameters class. Reflection over a derived parameters class returns inherited public
    // fields too, so a form picks these up with no extra work.
    //
    // The one thing inheritance does NOT solve is ordering: Hints.Order must list every field,
    // inherited ones included, so FieldOrder below exists to be appended to each generator's own
    // Order call instead of spelling the four names out again per generator.

    /// <summary>
    /// The object template shared by every generator that spawns objects: what is drawn, how big,
    /// what colour, and what (if anything) it collides with.
    /// </summary>
    public class SpawnParameters
    {
        /// <summary> Image every spawned object draws. </summary>
        public TextureResourceId Texture = TextureResourceId.Square;

        /// <summary> Size of one object. Polymorphic on purpose - RandomRect here is what makes a
        /// field of bullets look hand-placed instead of stamped. </summary>
        public IVector2 Size = new Vector2Value(1f, 1f);

        /// <summary> Tint of one object; themeable, so a generated pattern follows the level's
        /// palette instead of freezing a literal colour. </summary>
        public IColor4 Color = new Color4Value(1f, 1f, 1f, 1f);

        /// <summary> Collision shape, or Null for pure decoration. </summary>
        public ColliderId Collider = ColliderId.Null;

        /// <summary> The inherited fields, in the order a form should show them - append to each
        /// generator's own Hints.Order so it never has to name them one by one. </summary>
        public static readonly string[] FieldOrder =
        {
            nameof(Texture), nameof(Size), nameof(Color), nameof(Collider),
        };
    }
}
