using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryCompositeColliderResourcesConverter : DictionaryAsListConverter<ColliderId, CompositeCollider>
    {
        protected override ColliderId GetKey(CompositeCollider value) => value.ColliderId;
    }
}