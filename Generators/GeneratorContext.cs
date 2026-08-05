using System;
using System.Collections.Generic;
using BH.SDK.Models;
using BH.SDK.Models.Audio;
using BH.SDK.Models.Game;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Resources;
using BH.SDK.Models.SettingGroups;

namespace BH.SDK.Generators
{
    // The one rule of this class: a generator mutates the model THROUGH it and nowhere else. Every
    // mutating method here records what it did in the change log, which is the whole undo story
    // (see GeneratorChangeLog's header for why a journal rather than a Level copy).
    //
    // Scope and Counter are carried as a pair and neither is ever inferred from the other. A Prefab
    // implements both IObjectScope and IObjectIdCounter on one class, but at level scope they are
    // split across two: Level.Game is the scope, Level.Settings is the counter. Code that assumes
    // symmetry works in Prefab Mode and breaks at level scope, or the reverse - see the SDK's
    // CLAUDE.md, "the split every consumer must get right".

    /// <summary>
    /// The sandbox a Content/Modifier generator runs in: what it may read, what it may write, and
    /// the journal of everything it wrote.
    /// </summary>
    public sealed class GeneratorContext
    {
        /// <summary> Where created objects land - Level.Game normally, a Prefab's own template while
        /// Prefab Mode is active. </summary>
        public IObjectScope Scope { get; }

        /// <summary> Whoever owns Scope's ObjectId namespace - Level.Settings for a level,
        /// the Prefab itself (the same instance as Scope) for a template. </summary>
        public IObjectIdCounter Counter { get; }

        /// <summary> Timeline shape of the hosting level. Present even in Prefab Mode: a template's
        /// content is still authored against the level's framerate. </summary>
        public LevelSettings Settings { get; }

        /// <summary> The hosting level's resources - present in Prefab Mode too, since a template
        /// may reference textures, themes and colliders like anything else. </summary>
        public LevelResources Resources { get; }

        /// <summary> Level-global event tracks. NULL while a Prefab template is the active scope -
        /// declare GeneratorRequirements.LevelScope if you touch this. </summary>
        public GameLevel Game { get; }

        /// <summary> Scheduled audio. NULL in Prefab Mode, same reason as Game. </summary>
        public AudioLevel Audio { get; }

        /// <summary> First frame the run should write to. </summary>
        public int StartFrame { get; }

        /// <summary> Last frame the run should write to. </summary>
        public int EndFrame { get; }

        /// <summary> Parent every created object should attach to. ObjectId.Null means scope root. </summary>
        public ObjectId Parent { get; }

        /// <summary> Base layer for created objects. Parent-relative, like Layer everywhere else. </summary>
        public int Layer { get; }

        /// <summary> Seed for GeneratorRandom - same seed, same output, always. </summary>
        public uint Seed { get; }

        /// <summary> What the host currently has selected. Empty unless the generator declared
        /// GeneratorRequirements.Selection. </summary>
        public IReadOnlyList<ObjectId> Selection { get; }

        /// <summary> Read-only view of the target scope. Writing goes through Create/Edit/Delete -
        /// this is for looking around (finding a free layer, measuring existing content). </summary>
        public IReadOnlyDictionary<ObjectId, RectObject> Objects => Scope.Objects;

        /// <summary> Everything this run has changed so far. </summary>
        public GeneratorChangeLog Log { get; } = new();

        /// <summary> Level-scope run. Objects land in level.Game, ids come from level.Settings. </summary>
        public GeneratorContext(Level level, int startFrame, int endFrame,
            ObjectId parent = default, int layer = 0, uint seed = 0, IReadOnlyList<ObjectId> selection = null)
            : this(level.Game, level.Settings, level.Settings, level.Resources, level.Game, level.Audio,
                startFrame, endFrame, parent, layer, seed, selection)
        {
        }

        /// <summary> Prefab-template run: Game/Audio stay null, so a LevelScope generator can't be
        /// handed one of these by accident. </summary>
        public GeneratorContext(IObjectScope scope, IObjectIdCounter counter, LevelSettings settings,
            LevelResources resources, int startFrame, int endFrame,
            ObjectId parent = default, int layer = 0, uint seed = 0, IReadOnlyList<ObjectId> selection = null)
            : this(scope, counter, settings, resources, null, null,
                startFrame, endFrame, parent, layer, seed, selection)
        {
        }

        private GeneratorContext(IObjectScope scope, IObjectIdCounter counter, LevelSettings settings,
            LevelResources resources, GameLevel game, AudioLevel audio, int startFrame, int endFrame,
            ObjectId parent, int layer, uint seed, IReadOnlyList<ObjectId> selection)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Counter = counter ?? throw new ArgumentNullException(nameof(counter));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Resources = resources ?? throw new ArgumentNullException(nameof(resources));
            Game = game;
            Audio = audio;

            StartFrame = startFrame;
            EndFrame = endFrame;
            Parent = parent;
            Layer = layer;
            Seed = seed;
            Selection = selection ?? Array.Empty<ObjectId>();
        }

        /// <summary> A fresh RNG on this context's seed. Take one per generator run, not per object,
        /// or every object gets the same number. </summary>
        public GeneratorRandom CreateRandom() => new(Seed);

        // Deliberately does NOT set StartFrame/EndFrame/Layer/ParentObjectId - the generator does,
        // from this context's own inputs, because "spread objects across the range" and "put them
        // all on the same frame" are both legitimate and only the generator knows which it means.
        // It also can't use LevelUtils.SetObjectId: that overload takes LevelSettings specifically,
        // which is the wrong counter inside a Prefab template.

        /// <summary> Create a new object in the target scope with a freshly minted id. </summary>
        public T Create<T>() where T : RectObject, new()
        {
            var obj = new T { ObjectId = Counter.GetNextObjectId() };
            Scope.Objects.Add(obj.ObjectId, obj);
            Log.Add(new ObjectCreated(Scope, obj.ObjectId, obj));
            return obj;
        }

        /// <summary> Take an existing object for modification, snapshotting it first. The returned
        /// instance is the live one - mutate it freely, including its keyframe tracks. </summary>
        public T Edit<T>(ObjectId id) where T : RectObject
        {
            if (!Scope.Objects.TryGetValue(id, out var obj))
                throw new KeyNotFoundException($"Object {id.value} is not in the target scope");
            if (obj is not T typed)
                throw new InvalidCastException($"Object {id.value} is {obj.GetType().Name}, not {typeof(T).Name}");

            // A second Edit of the same object must not overwrite the ORIGINAL before-copy with an
            // already-modified one, or undo restores a half-generated state.
            if (!Log.HasEdit(id)) Log.Add(new ObjectEdited(Scope, id, obj.Copy()));
            return typed;
        }

        /// <summary> Same as Edit&lt;T&gt; when the concrete subtype doesn't matter. </summary>
        public RectObject Edit(ObjectId id) => Edit<RectObject>(id);

        /// <summary> Remove an object from the target scope. Children are NOT removed - reparenting
        /// or cascading is a content decision the generator makes explicitly. </summary>
        public void Delete(ObjectId id)
        {
            if (!Scope.Objects.TryGetValue(id, out var obj)) return;
            Scope.Objects.Remove(id);
            Log.Add(new ObjectDeleted(Scope, id, obj));
        }

        /// <summary> Add a level resource to one of LevelResources' dictionaries. </summary>
        public void AddResource<TId, TResource>(Dictionary<TId, TResource> target, TId id, TResource resource)
        {
            target[id] = resource;
            Log.Add(new ResourceAdded<TId, TResource>(target, id, resource));
        }

        // Object-owned keyframe tracks never come through here - they belong to an object, so
        // Create/Edit already snapshots them. These two are only for the level-global tracks in
        // GameEvents/CameraEvents/PostProcessingEvents/PlayerEvents, which have no owning object.
        //
        // Neither sorts nor deduplicates. Tracks carry no enforced sort order, and Frame uniqueness
        // ([RuleCollectionUnique]) is a per-generator decision - mod_quantize_keyframes alone has
        // three different answers for what to do about a collision.

        /// <summary> Append a keyframe to a level-global track. </summary>
        public void AddLevelKey<TKey>(List<TKey> track, TKey key)
        {
            track.Add(key);
            Log.Add(new LevelKeyAdded<TKey>(track, track.Count - 1, key));
        }

        /// <summary> Remove every matching keyframe from a level-global track. This is what keeps a
        /// destructive generator undoable: the removed keys live on in the journal, so undo puts
        /// them back at the exact indices they were removed from. </summary>
        public int RemoveLevelKeys<TKey>(List<TKey> track, Predicate<TKey> match)
        {
            var removed = 0;
            for (var i = track.Count - 1; i >= 0; i--)
            {
                if (!match(track[i])) continue;
                Log.Add(new LevelKeyRemoved<TKey>(track, i, track[i]));
                track.RemoveAt(i);
                removed++;
            }
            return removed;
        }
    }
}
