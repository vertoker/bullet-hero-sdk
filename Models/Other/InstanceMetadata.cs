using System;
using System.Collections.Generic;
using System.Reflection;

namespace BHSDK.Models.Other
{
    public static class InstanceMetadata
    {
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _propertyMaps = new();
        
        public static PropertyInfo GetProperty(Type type, string path)
        {
            // transform.position.x
            var parts = path.Split('.');
            var currentType = type;
            PropertyInfo result = null;
            
            foreach (var part in parts)
            {
                if (!_propertyMaps.TryGetValue(currentType, out var typeMap))
                {
                    typeMap = new Dictionary<string, PropertyInfo>();
                    foreach (var prop in currentType.GetProperties())
                    {
                        typeMap[prop.Name] = prop;
                    }
                    _propertyMaps[currentType] = typeMap;
                }
                
                if (!typeMap.TryGetValue(part, out result))
                    throw new ArgumentException($"Property {part} not found on {currentType}");
                
                currentType = result.PropertyType;
            }
            
            return result;
        }
    }
}