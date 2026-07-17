using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.Base
{
    // Implemented by converters that resolve a concrete implementation of a polymorphic type and need
    // a private "default" JsonSerializer to populate that concrete type's own members - one that carries
    // every other registered converter (so nested custom types, e.g. an IVector2 field inside a Color4Key,
    // still work) but leaves out whichever converter(s) would otherwise try to re-wrap the concrete type's
    // plain member JSON as another [type, value] array.
    // SerializationService.GetConverters wires this up automatically for every converter in its list that
    // implements this interface, so adding a new one is the only step required - no manual bookkeeping.
    public interface IRequiresDefaultSerializer
    {
        IEnumerable<JsonConverter> GetExcludedConverters(IReadOnlyList<JsonConverter> allConverters);
        void SetDefaultSerializer(JsonSerializer serializer);
    }
}
