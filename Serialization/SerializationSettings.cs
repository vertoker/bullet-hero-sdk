using Newtonsoft.Json;

namespace BHSDK.Serialization
{
    public class SerializationSettings
    {
        public Formatting Formatting { get; set; }
        public TypeNameHandling TypeNameHandling { get; set; }
        public MemberSerialization MemberSerialization { get; set; }

        public SerializationSettings()
        {
            Formatting = Formatting.None;
            TypeNameHandling = TypeNameHandling.None;
            MemberSerialization = MemberSerialization.OptIn;
        }
        public SerializationSettings(Formatting formatting = Formatting.None,
            TypeNameHandling typeNameHandling = TypeNameHandling.None,
            MemberSerialization memberSerialization = MemberSerialization.OptIn)
        {
            Formatting = formatting;
            TypeNameHandling = typeNameHandling;
            MemberSerialization = memberSerialization;
        }
    }
}