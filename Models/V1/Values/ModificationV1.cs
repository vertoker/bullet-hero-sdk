using BHSDK.Models.Interfaces.Values;

namespace BHSDK.Models.V1.Values
{
    public class ModificationV1 : IModification
    {
        public string PropertyPath { get; set; }
        public string Value { get; set; }

        public ModificationV1()
        {
            PropertyPath = string.Empty;
            Value = string.Empty;
        }
        public ModificationV1(string propertyPath, string value)
        {
            PropertyPath = propertyPath;
            Value = value;
        }
    }
}