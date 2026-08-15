using Newtonsoft.Json;

namespace BH.SDK.Serialization
{
    // Formatting deliberately does NOT live here. It is a property of the SerializationType a caller
    // picks per save (SerializationTypeExtensions.ToFormatting), not of the one shared JsonSerializer
    // every save in the process goes through - held here, one screen's "write this readable" would
    // have re-indented every level file written afterwards.
    [System.Serializable]
    public class SerializationSettings
    {
        public TypeNameHandling typeNameHandling;
        public MemberSerialization memberSerialization;

        public SerializationSettings()
        {
            typeNameHandling = TypeNameHandling.None;
            memberSerialization = MemberSerialization.OptIn;
        }
        public SerializationSettings(TypeNameHandling typeNameHandling = TypeNameHandling.None,
            MemberSerialization memberSerialization = MemberSerialization.OptIn)
        {
            this.typeNameHandling = typeNameHandling;
            this.memberSerialization = memberSerialization;
        }
    }
}