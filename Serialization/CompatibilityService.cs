using System;
using BHSDK.Models;

namespace BHSDK.Serialization
{
    public class CompatibilityService
    {
        public Type GetLevelType(Version version)
        {
            return typeof(Level);
        }
    }
}