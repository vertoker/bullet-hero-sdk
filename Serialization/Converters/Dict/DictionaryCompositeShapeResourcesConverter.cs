using BH.SDK.Models.Data;
using BH.SDK.Models.Primitives;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryCompositeShapeResourcesConverter : DictionaryAsListConverter<ShapeId, CompositeShape>
    {
        protected override ShapeId GetKey(CompositeShape value) => value.ShapeId;
    }
}
