using System;
using System.Runtime.CompilerServices;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Models.Primitives
{
    [Serializable]
    public struct ObjectId : IEquatable<ObjectId>, IPrimitiveInt
    {
        public int value;
        int IPrimitiveInt.Value => value;

        public ObjectId(int value)
        {
            this.value = value;
        }
        public void Reset()
        {
            value = NullValue;
        }
        
        // ObjectId - stable identifier for saving.
        // Unique only inside each object scope. Can be referred (as pid) only in scope.
        // When PrefabObjects converts to regular Objects, they change all ids for each hierarchy level
        
        // What certain objectId's meaning
        // (1 - int.MaxValue) => user-space objects, valid for both ObjectId and ParentObjectId
        // 0 => undefined (for ObjectId) or null (for ParentObjectId), exists as a fallback value
        // All negative numbers (int.MinValue - -1) reserved for game-space objects
        
        // There are 3 types of objectIds: user objects, public game object and private game objects
        // user objects - objects created by users in levels, exists in this
        
        // public game objects - predefined game objects, stable and has permanent public objectId,
        // can be used by user objects (mostly as a parent), also they can use user game objects
        
        // private game objects - predefined game object, unstable and has changeable private objectId,
        // can not be used by user objects (throw error if you try), but they also can use user game objects
        
        public const int NullValue = 0;
        public const int MinLevelValue = 1;
        public const int MinLevelParentValue = -2;
        public const int MaxInframeValue = -3;
        
        public static readonly ObjectId Null = new(NullValue);
        public static readonly ObjectId MinLevel = new(MinLevelValue);
        public static readonly ObjectId MinLevelParent = new(MinLevelParentValue);
        public static readonly ObjectId MaxInframe = new(MaxInframeValue);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNotNull() => value != NullValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid() => value >= MinLevelValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidParent() => value >= MinLevelParentValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInframe() => value <= MaxInframeValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNull(int value) => value != NullValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(int value) => value >= MinLevelValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidParent(int value) => value >= MinLevelParentValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInframe(int value) => value <= MaxInframeValue;
        
        // Public game objects (stable ids)
        
        // -1 => camera game object, exists only in player runtime (for ObjectId - error),
        // can be used as a parent with unique transform
        // Size is not regular (1f, 1f), but is "aspect >= 1f ? (10f * aspect, 10f) : (10f, 10f / aspect)"
        public static readonly ObjectId Camera = new(-1);
        
        // -2 => local player game object, can be used as a regular parent for user objects
        // In multiplayer each localPlayer works individually (no effect on other players)
        // Recommend: try not to use colliders for user objects parented from localPlayer
        public static readonly ObjectId LocalPlayer = new(-2);
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ObjectId a, ObjectId b) => a.value == b.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ObjectId a, ObjectId b) => a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ObjectId other) => value == other.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is ObjectId other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{nameof(ObjectId)}={value}";
    }
}