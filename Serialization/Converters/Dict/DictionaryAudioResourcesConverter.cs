using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryAudioResourcesConverter : DictionaryAsListConverter<AudioResourceId, AudioResource>
    {
        protected override AudioResourceId GetKey(AudioResource value) => value.AudioResourceId;
    }
}