using Newtonsoft.Json;

namespace BHSDK.Serialization
{
    [System.Serializable]
    public class SerializationSettings
    {
        public Formatting formatting;
        public TypeNameHandling typeNameHandling;
        public MemberSerialization memberSerialization;

        public SerializationSettings()
        {
            formatting = Formatting.None;
            typeNameHandling = TypeNameHandling.None;
            memberSerialization = MemberSerialization.OptIn;
        }
        public SerializationSettings(Formatting formatting = Formatting.None,
            TypeNameHandling typeNameHandling = TypeNameHandling.None,
            MemberSerialization memberSerialization = MemberSerialization.OptIn)
        {
            this.formatting = formatting;
            this.typeNameHandling = typeNameHandling;
            this.memberSerialization = memberSerialization;
        }
    }
}