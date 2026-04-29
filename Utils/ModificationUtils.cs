using System;
using System.Collections.Generic;
using System.Reflection;
using BHSDK.Models.Effects;
using BHSDK.Models.Objects;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;
using UnityEngine;
using Object = BHSDK.Models.Objects.Object;

namespace BHSDK.Utils
{
    public static class ModificationUtils
    {
        private static readonly Dictionary<(Type type, string name),
            (PropertyCategory category, PropertyInfo info)> Data = new(32);
        private static readonly Dictionary<Type, PropertyInfo> ListIndexers = new(16);
        
        private static readonly List<PathSegment> SegmentBuffer = new(4);
        private static readonly object[] ParamsCache = { 0 };
        private static readonly HashSet<Type> ProcessedTypes = new();
        
        static ModificationUtils()
        {
            Debug.Log($"Init ModificationUtils");
            
            // Objects
            
            AddPropertiesRecursive<Object, TextureObject, EffectObject, TextObject>();
            
            // Effects
            
            AddPropertiesRecursive<EffectShapePoint, EffectShapeCircle, EffectShapeRectangle,
                EffectShapeLine, EffectShapeCone, EffectShapeTorus>();
            AddPropertiesRecursive<EffectShapeSpreadRandom, EffectShapeSpreadLoop,
                EffectShapeSpreadPingPong, EffectShapeSpreadSine>();
            AddPropertiesRecursive<EffectAngleValue, EffectAngleCurvesOverLife,
                EffectAngleCurvesBySpeed, EffectAngleRandomUniform, EffectAngleRandomPerComponent>();
            AddPropertiesRecursive<EffectScaleValue, EffectScaleCurvesOverLife,
                EffectScaleCurvesBySpeed, EffectScaleRandomUniform, EffectScaleRandomPerComponent>();
            AddPropertiesRecursive<EffectColorValue, EffectColorGradientOverLife,
                EffectColorGradientBySpeed, EffectColorRandomUniform, EffectColorRandomPerComponent>();
            
            // Values
            
            AddPropertiesRecursive<Vector2Value, Vector2Circle, Vector2Rect, Vector2RectStep>();
            AddPropertiesRecursive<Vector3Value, Vector3Circle, Vector3Rect, Vector3RectStep>();
            AddPropertiesRecursive<Vector4Value, Vector4Circle, Vector4Rect, Vector4RectStep>();
            
            AddPropertiesRecursive<ColorValue, ColorThemeRef, ColorMinMax>();
            AddPropertiesRecursive<FloatValue, FloatMinMax, FloatMinMaxStep>();
            AddPropertiesRecursive<IntValue, IntMinMax, IntMinMaxStep>();
            AddPropertiesRecursive<StringValue, StringLocalized>();
            AddPropertiesRecursive<ScreenLimitNone, ScreenLimitFixed, ScreenLimitBounds>();
        }

        public static void Apply(this Object obj, Modification mod)
        {
            ParsePath(mod.Path, SegmentBuffer);
            if (SegmentBuffer.Count == 0) return;

            object currentObject = obj;
            var preLength = SegmentBuffer.Count - 1;
            for (var i = 0; i < preLength; i++)
            {
                if (currentObject == null)
                {
                    Debug.LogWarning($"CurrentObject is null, index={i}");
                    return;
                }
                
                var segment = SegmentBuffer[i];
                (Type type, string name) key = (currentObject.GetType(), segment.Name);
                if (!Data.TryGetValue(key, out var value))
                {
                    Debug.LogWarning($"Can't find data for key {key.type}/{key.name}");
                    return;
                }

                switch (value.category)
                {
                    case PropertyCategory.Value:
                    {
                        currentObject = value.info.GetValue(currentObject);
                        break;
                    }
                    case PropertyCategory.List:
                    {
                        currentObject = GetListValue(value.info, segment, currentObject);
                        break;
                    }
                    case PropertyCategory.Array:
                    {
                        currentObject = GetArrayValue(value.info, currentObject, segment);
                        break;
                    }
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            
            if (currentObject == null)
            {
                Debug.LogWarning($"CurrentObject is null, index={preLength}");
                return;
            }
            
            var lastSegment = SegmentBuffer[preLength];
            (Type type, string name) lastKey = (currentObject.GetType(), lastSegment.Name);
            if (!Data.TryGetValue(lastKey, out var lastValue))
            {
                Debug.LogWarning($"Can't find data for key {lastKey.type}/{lastKey.name}");
                return;
            }
            
            switch (lastValue.category)
            {
                case PropertyCategory.Value:
                {
                    if (!CheckTypeMatch(mod.Value.GetType(), lastValue.info.PropertyType)) return;
                    
                    lastValue.info.SetValue(currentObject, mod.Value);
                    break;
                }
                case PropertyCategory.List:
                {
                    var toType = lastValue.info.PropertyType.GetGenericArguments()[0];
                    if (!CheckTypeMatch(mod.Value.GetType(), toType)) return;
                    
                    SetListValue(lastValue.info, lastSegment, currentObject, mod.Value);
                    break;
                }
                case PropertyCategory.Array:
                {
                    var toType = lastValue.info.PropertyType.GetElementType();
                    if (!CheckTypeMatch(mod.Value.GetType(), toType)) return;
                    
                    SetArrayValue(lastValue.info, lastSegment, currentObject, mod.Value);
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static bool CheckTypeMatch(Type from, Type to)
        {
            if (from == to) return true;
            
            Debug.LogWarning($"Type mismatch, from={from}, to={to}");
            return false;
        }
        private static object GetListValue(PropertyInfo info, PathSegment segment, object currentObject)
        {
            ParamsCache[0] = segment.Index;
            var list = info.GetValue(currentObject);
            var indexer = ListIndexers[info.PropertyType];
            return indexer.GetValue(list, ParamsCache);
        }
        private static void SetListValue(PropertyInfo info, PathSegment segment, object currentObject, object newValue)
        {
            ParamsCache[0] = segment.Index;
            var list = info.GetValue(currentObject);
            var listIndexer = ListIndexers[info.PropertyType];
            listIndexer.SetValue(list, newValue, ParamsCache);
        }
        
        private static object GetArrayValue(PropertyInfo info, object currentObject, PathSegment segment)
        {
            var array = info.GetValue(currentObject);
            return ((Array)array).GetValue(segment.Index);
        }
        private static void SetArrayValue(PropertyInfo info, PathSegment segment, object currentObject, object newValue)
        {
            var array = info.GetValue(currentObject);
            ((Array)array).SetValue(newValue, segment.Index);
        }

        public static void AddProperties<T>()
            => AddProperties(typeof(T));
        public static void AddProperties<T1, T2>()
            => AddProperties(typeof(T1), typeof(T2));
        public static void AddProperties<T1, T2, T3>()
            => AddProperties(typeof(T1), typeof(T2), typeof(T3));
        public static void AddProperties<T1, T2, T3, T4>()
            => AddProperties(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        public static void AddProperties<T1, T2, T3, T4, T5>()
            => AddProperties(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        public static void AddProperties<T1, T2, T3, T4, T5, T6>()
            => AddProperties(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        public static void AddProperties<T1, T2, T3, T4, T5, T6, T7>()
            => AddProperties(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
        public static void AddProperties<T1, T2, T3, T4, T5, T6, T7, T8>()
            => AddProperties(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
        
        public static void AddPropertiesRecursive<T>()
            => AddPropertiesRecursive(typeof(T));
        public static void AddPropertiesRecursive<T1, T2>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2));
        public static void AddPropertiesRecursive<T1, T2, T3>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2), typeof(T3));
        public static void AddPropertiesRecursive<T1, T2, T3, T4>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        public static void AddPropertiesRecursive<T1, T2, T3, T4, T5>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        public static void AddPropertiesRecursive<T1, T2, T3, T4, T5, T6>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        public static void AddPropertiesRecursive<T1, T2, T3, T4, T5, T6, T7>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
        public static void AddPropertiesRecursive<T1, T2, T3, T4, T5, T6, T7, T8>()
            => AddPropertiesRecursive(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
        
        public static void AddProperties(params Type[] types)
        {
            foreach (var type in types)
                AddProperties(type);
        }
        public static void AddPropertiesRecursive(params Type[] types)
        {
            foreach (var type in types)
                AddPropertiesRecursive(type);
        }
        
        public static void AddProperties(Type type)
        {
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                RegisterProperty(type, property);
            }
        }
        public static void AddPropertiesRecursive(Type type)
        {
            if (!ProcessedTypes.Add(type)) return; // recursive protection
            
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var (propertyType, propertyCategory) = RegisterProperty(type, property);
                if (propertyType == null) continue;
                
                switch (propertyCategory)
                {
                    case PropertyCategory.Value:
                    {
                        AddPropertiesRecursive(propertyType);
                        break;
                    }
                    case PropertyCategory.List:
                    {
                        var elementType = propertyType.GetGenericArguments()[0];
                        AddPropertiesRecursive(elementType);
                        break;
                    }
                    case PropertyCategory.Array:
                    {
                        var elementType = propertyType.GetElementType();
                        AddPropertiesRecursive(elementType);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            ProcessedTypes.Remove(type);
        }
        // TODO add interfaces support into AddPropertiesRecursive (like IVector2, IFloat, IEffectShape)

        private static (Type, PropertyCategory) RegisterProperty(Type type, PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<JsonPropertyAttribute>();
            if (attr == null) return (null, PropertyCategory.Value);
            
            var propertyType = property.PropertyType;
            var propertyName = attr.PropertyName;
            var propertyCategory = GetCategory(propertyType);
            Data[(type, propertyName)] = (propertyCategory, property);

            if (propertyCategory == PropertyCategory.List)
            {
                if (!ListIndexers.ContainsKey(propertyType))
                {
                    var indexer = propertyType.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public)
                                  ?? throw new InvalidOperationException($"No indexer on {propertyType}");
                    ListIndexers.Add(propertyType, indexer);
                }
            }
            
            Debug.Log($"t: {type}, pt: {propertyType}, pn: {propertyName}");
            return (propertyType, propertyCategory);
        }

        private static PropertyCategory GetCategory(Type type)
        {
            if (type.IsArray)  return PropertyCategory.Array;
            if (type.IsList()) return PropertyCategory.List;
            return PropertyCategory.Value;
        }
        
        private enum PropertyCategory : byte
        {
            Value = 0,
            List = 1,
            Array = 2,
        }
        private struct PathSegment
        {
            public string Name;
            public int Index;
        }

        private static void ParsePath(string path, List<PathSegment> buffer)
        {
            buffer.Clear();
            var index = 0;
            while (index < path.Length)
            {
                var startIndex = index;
                while (index < path.Length && path[index] != '.' && path[index] != '[') index++;
                var name = path[startIndex..index];
                if (name.Length == 0)
                {
                    Debug.LogWarning($"FormatException: Empty name at {index} in '{path}'");
                    buffer.Clear();
                    return;
                }

                var segmentIndex = -1;
                if (index < path.Length && path[index] == '[')
                {
                    index++;
                    var segmentIndexStart = index;
                    while (index < path.Length && path[index] != ']') index++;
                    if (index == path.Length)
                    {
                        Debug.LogWarning($"FormatException: Missing ']'");
                        buffer.Clear();
                        return;
                    }
                    
                    segmentIndex = int.Parse(path[segmentIndexStart..index]);
                    index++;
                }

                buffer.Add(new PathSegment { Name = name, Index = segmentIndex });

                if (index < path.Length && path[index] == '.') index++;
            }
        }
    }
}