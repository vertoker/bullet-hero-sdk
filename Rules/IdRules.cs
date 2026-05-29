namespace BH.SDK.Rules
{
    /// <summary> All rules and values/limits about all types of id in whole BH </summary>
    public static class IdRules
    {
        // ------------------------------------------------------------------------------------------------------------
        // ObjectId (FrameIndex)
        // ------------------------------------------------------------------------------------------------------------
        
        // Introducing to objectId. There are 2 types of identifiers
        
        // 1. ObjectId - stable identifier for saving.
        // Unique only inside each object scope. Can be referred (as pid) only in scope.
        // When PrefabObjects converts to regular Objects, they change all ids for each hierarchy level

        public const int NullObjectId = 0;
        public const int MinObjectId = 1;
        public const int MinUserObjectIdCounter = 1;
        
        // 2. InstanceIndex - runtime temporary index (not id, exactly direct indexation).
        // Changes every runtime and using for rendering instances in frame context only
        
        public const int InvalidInstanceIndex = -1;
        public const int MinInstanceIndex = 0;
        public const int CameraInstanceIndex = 0;
        
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
        
        public const int MinParentObjectId = -2;
        public const int MaxInframeObjectIdCounter = -3;
        
        // Public game objects (stable ids)
        
        // -1 => camera game object, exists only in player runtime (for ObjectId - error),
        // can be used as a parent with unique transform
        // Size is not regular (1f, 1f), but is
        // aspect >= 1f ? (10f * aspect, 10f) : (10f, 10f / aspect)
        // TODO update this data when add option to level like "expandToMinSide"
        public const int CameraObjectId = -1;
        
        // -2 => local player game object, can be used as a regular parent for user objects
        // In multiplayer works each localPlayer individually (no effect for )
        // Recommend: try not to use colliders for user objects, which parented from localPlayer
        public const int LocalPlayerObjectId = -2;
        
        // Private game objects (unstable ids)
        
        // (-4 - -3) - screen black borders, execute data from IScreenLimit, cover unwanted parts of level.
        // Exists as a children of camera
        public const int LeftScreenBorderObjectId = -3;
        public const int RightScreenBorderObjectId = -4;
        
        // TODO add selection for editor
        
        // ------------------------------------------------------------------------------------------------------------
        // ResourceId (TypedResourceId)
        // ------------------------------------------------------------------------------------------------------------
        
        // Introduction into resourceId. There are 3 types of identifiers
        
        // 1. TypedResourceId (audioResourceId, textureResourceId...) - stable identifier for saving.
        // Unique for each resource type (audio, textures...), what each number meaning
        // (0 - int.MaxValue) => game-space resources, defaults for whole game, stored in game, each resource has permanent id
        // (int.MinValue - -1) => user-space resources, unique for each level, stored externally: level folder, url...
        
        // 2. ResourceId - runtime temporary identifier.
        // Exists in (0, int.MaxValue), can't be negative, 0 is undefined (means null)
        // Changes every runtime, uses not only for level player
        
        public const int MinResourceIdCounter = 1;
        public const int NullResourceId = 0;
        // Special number for internal usings
        public const int InvalidResourceId = int.MinValue;
        
        // user-space resources by default must be uninitialized and also invalid
        public const int UninitializedUserTypedResourceId = 0;
        public const int MaxUserTypedResourceId = -1;
        
        // ------------------------------------------------------------------------------------------------------------
        // ColliderId
        // ------------------------------------------------------------------------------------------------------------
        
        // id for user-defined colliders, allowed only negative (started with -1, 0 is uninitialized)
        // this works like resource, but integrated in model. Works only with TextureObject
        
        public const int MaxColliderId = 0;
        public const int NullColliderId = 0;
    }
}