using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace BHSDK.Utils
{
    public static class ModificationStatic2
    {
        private static readonly ConcurrentDictionary<(Type rootType, string path), PathAccessor[]> _chainCache = new();
        private static readonly ConcurrentDictionary<Type, Dictionary<string, MemberInfo>> _typeMemberCache = new();

        public static void SetValue(object root, string path, object value)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var chain = _chainCache.GetOrAdd((root.GetType(), path), BuildChain);
            ApplyChain(root, value, chain);
        }

        private static void ApplyChain(object root, object value, PathAccessor[] chain)
        {
            object target = root;
            for (int i = 0; i < chain.Length - 1; i++)
            {
                target = chain[i].GetValue(target);
                if (target == null)
                    throw new NullReferenceException($"Intermediate target at step {i} is null.");
            }

            chain[^1].SetValue(target, value);
        }

        private static PathAccessor[] BuildChain((Type rootType, string path) key)
        {
            var segments = ParsePath(key.path);
            var chain = new List<PathAccessor>();
            Type currentType = key.rootType;

            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                var member = FindMemberByJsonProperty(currentType, seg.Name);
                Type memberType = GetMemberType(member);

                var memberAccessor = new MemberAccessor(member);
                chain.Add(memberAccessor);

                if (seg.Index >= 0)
                {
                    var indexedAccessor = CreateIndexAccessor(memberType, seg.Index);
                    chain.Add(indexedAccessor);
                    currentType = indexedAccessor.GetValueType(memberType);
                }
                else
                {
                    currentType = memberType;
                }
            }

            return chain.ToArray();
        }

        private static MemberInfo FindMemberByJsonProperty(Type type, string jsonName)
        {
            var map = _typeMemberCache.GetOrAdd(type, t =>
            {
                var dict = new Dictionary<string, MemberInfo>(StringComparer.Ordinal);
                foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var attr = field.GetCustomAttribute<JsonPropertyAttribute>();
                    dict[attr?.PropertyName ?? field.Name] = field;
                }

                foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                     BindingFlags.Instance))
                {
                    var attr = prop.GetCustomAttribute<JsonPropertyAttribute>();
                    dict[attr?.PropertyName ?? prop.Name] = prop;
                }

                return dict;
            });

            if (map.TryGetValue(jsonName, out var member))
                return member;

            throw new MissingMemberException($"Type '{type}' has no member with [JsonProperty] name '{jsonName}'.");
        }

        private static Type GetMemberType(MemberInfo member)
        {
            return member switch
            {
                FieldInfo fi => fi.FieldType,
                PropertyInfo pi => pi.PropertyType,
                _ => throw new NotSupportedException($"Unsupported member type: {member.MemberType}")
            };
        }

        private static PathAccessor CreateIndexAccessor(Type containerType, int index)
        {
            if (containerType.IsArray)
                return new ArrayIndexAccessor(index);
            else if (containerType.IsGenericType && containerType.GetGenericTypeDefinition() == typeof(List<>))
                return new ListIndexAccessor(containerType, index);
            else
                throw new NotSupportedException($"Indexer not supported for type {containerType}");
        }

        private struct PathSegment
        {
            public string Name;
            public int Index;
        }

        private static List<PathSegment> ParsePath(string path)
        {
            var segments = new List<PathSegment>();
            int i = 0;
            while (i < path.Length)
            {
                int start = i;
                while (i < path.Length && path[i] != '.' && path[i] != '[') i++;
                string name = path[start..i];
                if (name.Length == 0) throw new FormatException($"Empty name at {i} in '{path}'");

                int index = -1;
                if (i < path.Length && path[i] == '[')
                {
                    i++;
                    int idxStart = i;
                    while (i < path.Length && path[i] != ']') i++;
                    if (i == path.Length) throw new FormatException("Missing ']'");
                    index = int.Parse(path[idxStart..i]);
                    i++; // пропускаем ']'
                }

                segments.Add(new PathSegment { Name = name, Index = index });

                if (i < path.Length && path[i] == '.')
                    i++;
            }

            return segments;
        }

        internal abstract class PathAccessor
        {
            public abstract object GetValue(object target);
            public abstract void SetValue(object target, object value);
            public abstract Type GetValueType(Type containerType); // тип после применения
        }

        internal class MemberAccessor : PathAccessor
        {
            private readonly MemberInfo _member;

            public MemberAccessor(MemberInfo member) => _member = member;

            public override object GetValue(object target)
            {
                return _member switch
                {
                    FieldInfo fi => fi.GetValue(target),
                    PropertyInfo pi => pi.GetValue(target),
                    _ => throw new InvalidOperationException()
                };
            }

            public override void SetValue(object target, object value)
            {
                switch (_member)
                {
                    case FieldInfo fi: fi.SetValue(target, value); break;
                    case PropertyInfo pi: pi.SetValue(target, value); break;
                }
            }

            public override Type GetValueType(Type containerType) => GetMemberType(_member);
        }

        internal class ArrayIndexAccessor : PathAccessor
        {
            private readonly int _index;

            public ArrayIndexAccessor(int index) => _index = index;

            public override object GetValue(object target) =>
                ((Array)target).GetValue(_index);

            public override void SetValue(object target, object value) =>
                ((Array)target).SetValue(value, _index);

            public override Type GetValueType(Type containerType) =>
                containerType.GetElementType();
        }

        internal class ListIndexAccessor : PathAccessor
        {
            private readonly PropertyInfo _indexer;
            private readonly int _index;

            public ListIndexAccessor(Type listType, int index)
            {
                _index = index;
                // у List<T> индексатор называется "Item"
                _indexer = listType.GetProperty("Item", BindingFlags.Instance | BindingFlags.Public)
                           ?? throw new InvalidOperationException($"No indexer on {listType}");
            }

            public override object GetValue(object target) =>
                _indexer.GetValue(target, new object[] { _index });

            public override void SetValue(object target, object value) =>
                _indexer.SetValue(target, value, new object[] { _index });

            public override Type GetValueType(Type containerType) =>
                _indexer.PropertyType;
        }
    }
}