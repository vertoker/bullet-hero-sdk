using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace BHSDK.Services
{
    public class ModificationService
    {
        private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _propertyMaps = new();

        public void Add(Type type)
        {
            if (_propertyMaps.TryGetValue(type, out var typeMap)) return;
            typeMap = new Dictionary<string, PropertyInfo>();
            _propertyMaps[type] = typeMap;
                
            foreach (var prop in type.GetProperties())
            {
                var attribute = prop.GetCustomAttribute<JsonPropertyAttribute>();
                if (attribute?.PropertyName == null) continue;
                    
                typeMap[attribute.PropertyName] = prop;
            }
        }
        public void Remove(Type type)
        {
            _propertyMaps.Remove(type);
        }
        
        [CanBeNull]
        public object GetValue(object obj, string path)
        {
            if (obj == null) return null;
            if (string.IsNullOrEmpty(path)) return null;
            
            using var enumerator = new Enumerator(path); // transform.position.x

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid()) continue;
                
                if (!_propertyMaps.TryGetValue(obj.GetType(), out var typeMap))
                    return null;
                
                var part = enumerator.Current.GetSubstring(path);
                if (!typeMap.TryGetValue(part, out var propertyInfo))
                    return null;
                
                obj = propertyInfo.GetValue(obj);
                if (enumerator.Current.HasIndex() && obj is Array array)
                {
                    var index = enumerator.Current.Index;
                    if (index < 0 || index >= array.Length) return null;
                    obj = array.GetValue(index);
                }
            }
            
            return obj;
        }
        public bool SetValue(object obj, object value, string path)
        {
            if (obj == null) return false;
            if (string.IsNullOrEmpty(path)) return false;
            
            using var enumerator = new Enumerator(path); // transform.position.x
            PathPart pathPart = default;
            PropertyInfo propertyInfo = null;

            while (enumerator.MoveNext())
            {
                if (propertyInfo != null)
                {
                    obj = propertyInfo.GetValue(obj);
                    if (pathPart.HasIndex() && obj is Array array)
                    {
                        var index = pathPart.Index;
                        if (index < 0 || index >= array.Length) return false;
                        obj = array.GetValue(index);
                    }
                }
                
                pathPart = enumerator.Current;
                if (!pathPart.IsValid()) continue;
                
                if (!_propertyMaps.TryGetValue(obj.GetType(), out var typeMap))
                    return false;
                
                var part = pathPart.GetSubstring(path);
                if (!typeMap.TryGetValue(part, out propertyInfo))
                    return false;
            }
            
            if (propertyInfo != null && pathPart.IsValid()) // SetValue
            {
                if (pathPart.HasIndex() && propertyInfo.GetValue(obj) is Array array)
                {
                    var index = pathPart.Index;
                    if (index < 0 || index >= array.Length) return false;
                    array.SetValue(value, index);
                    return true;
                }
                
                propertyInfo.SetValue(obj, value);
                return true;
            }
            return false;
        }

        public struct Enumerator : IEnumerator<PathPart>
        {
            private readonly string _expression;
            private int _currentIndex;
            
            public PathPart Current { get; private set; }
            object IEnumerator.Current => Current;

            public Enumerator(string expression)
            {
                _expression = expression;
                _currentIndex = 0;
                Current = default;
            }

            public bool MoveNext()
            {
                var startIndex = _currentIndex;
                var startIndexParenthesis = _currentIndex;
                var endIndexParenthesis = _currentIndex;
                
                var flagStartParenthesis = false;
                var flagEndParenthesis = false;
                
                while (_currentIndex < _expression.Length)
                {
                    var currentChar = _expression[_currentIndex];
                    switch (currentChar)
                    {
                        case ' ':
                            throw new ArgumentException("Unsupported character");
                        
                        case '.':
                            if (flagStartParenthesis != flagEndParenthesis)
                                throw new ArgumentException("Parenthesis is not properly closed");
                            if (startIndex == _currentIndex)
                                throw new ArgumentException("Empty content between points");

                            if (flagStartParenthesis)
                            {
                                var parameterLength = endIndexParenthesis - startIndexParenthesis;
                                if (parameterLength < 1) throw new ArgumentException("No any parameter in parenthesis");
                                
                                var span = _expression.AsSpan().Slice(startIndexParenthesis, parameterLength);
                                var parameter = int.Parse(span);
                                
                                var length = startIndexParenthesis - startIndex - 1;
                                Current = new PathPart(startIndex, length, parameter);
                            }
                            else
                            {
                                var length = _currentIndex - startIndex;
                                Current = new PathPart(startIndex, length);
                            }
                            
                            _currentIndex++;
                            if (_currentIndex == _expression.Length)
                                throw new ArgumentException("Inappropriate end of expression with '.'");
                            return true;

                        case '[':
                            if (flagStartParenthesis)
                                throw new ArgumentException("End parenthesis is already opened");
                            
                            flagStartParenthesis = true;
                            _currentIndex++;
                            startIndexParenthesis = _currentIndex; // already +1
                            break;
                        
                        case ']':
                            if (flagEndParenthesis)
                                throw new ArgumentException("End parenthesis is already closed");
                            
                            flagEndParenthesis = true;
                            endIndexParenthesis = _currentIndex;
                            _currentIndex++;
                            break;
                        
                        default:
                            _currentIndex++;
                            break;
                    }
                }

                if (startIndex < _currentIndex)
                {
                    if (flagStartParenthesis != flagEndParenthesis)
                        throw new Exception("Parenthesis is not properly closed");

                    if (flagStartParenthesis)
                    {
                        var parameterLength = endIndexParenthesis - startIndexParenthesis;
                        if (parameterLength < 1) throw new Exception("No any parameter in parenthesis");
                                
                        var span = _expression.AsSpan().Slice(startIndexParenthesis, parameterLength);
                        var parameter = int.Parse(span);
                                
                        var length = startIndexParenthesis - startIndex - 1;
                        Current = new PathPart(startIndex, length, parameter);
                    }
                    else
                    {
                        var length = _currentIndex - startIndex;
                        Current = new PathPart(startIndex, length);
                    }
                            
                    _currentIndex++;
                    return true;
                }
                Current = default;
                return false;
            }
            public void Reset()
            {
                _currentIndex = 0;
            }
            public void Dispose()
            {
                _currentIndex = 0;
            }
        }
        
        public readonly struct PathPart
        {
            public readonly int StartIndex;
            public readonly int Length;
            public readonly int Index;

            public PathPart(int startIndex, int length, int index = -1)
            {
                StartIndex = startIndex;
                Length = length;
                Index = index;
            }

            public bool HasIndex() => Index >= 0;
            public bool IsValid() => Length > 0;
            
            public string GetSubstring(string expression)
            {
                if (Length == 0) return string.Empty;
                return expression.Substring(StartIndex, Length);
            }
        }
    }
}