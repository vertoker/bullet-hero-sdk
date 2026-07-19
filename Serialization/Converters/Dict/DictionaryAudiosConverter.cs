using BH.SDK.Models.Audio;
using BH.SDK.Models.Primitives;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.Dict
{
    public class DictionaryAudiosConverter : DictionaryAsListConverter<AudioId, LevelTrack>
    {
        protected override AudioId GetKey(LevelTrack value) => value.AudioId;
    }
}