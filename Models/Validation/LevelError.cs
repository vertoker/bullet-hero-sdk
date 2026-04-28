using System;
using BHSDK.Models.Enum;

namespace BHSDK.Models.Validation
{
    public struct LevelError
    {
        public Type Type;
        public string Name;
        public LevelErrorType ErrorType;
    }
}