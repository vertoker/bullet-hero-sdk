using System;
using BHSDK.Models.Interfaces;
using BHSDK.Models.V1;

namespace BHSDK.Services
{
    public class CompatibilityService
    {
        public CompatibilityService()
        {
            
        }
        
        public ILevel UpdateModel(object model)
        {
            if (model is not ILevelCompatibilityModel)
                throw new ArgumentException("Provided model is not ILevelCompatibilityModel, use valid object for conversion");
            
            // Versions here
            
            // if (model is LevelV1 modelV1) model = Convert(modelV1); // uncomment to start a pattern
            
            return (ILevel)model;
        }
        
        // private LevelV2 Convert(LevelV1 model) // uncomment to start a pattern
    }
}