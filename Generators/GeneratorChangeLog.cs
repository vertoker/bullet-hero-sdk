using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Generators
{
    // Why a journal and not a Level copy. A generator run has to be undoable, and the obvious
    // implementation - snapshot the whole Level before running, restore it on undo - costs memory
    // proportional to the LEVEL, on every run, forever. A level anywhere near LevelRules.MaxObjects
    // (262 144) would make each generator click a multi-megabyte copy, and the undo stack holds many
    // of them. A journal costs memory proportional to what actually CHANGED, which for the typical
    // "spawn 32 bullets" run is 32 objects rather than the entire level.
    //
    // The consequence every generator author must internalise: mutating the model outside
    // GeneratorContext compiles fine, runs fine, and silently breaks undo. There is no way for this
    // layer to detect it. That is the single most dangerous mistake available in this system.

    /// <summary>
    /// Everything one generator run changed, in the order it changed it, as a replayable journal.
    /// Revert() walks it backwards, Reapply() forwards; both are exact, so an undo/redo pair leaves
    /// the model where it started.
    /// </summary>
    public sealed class GeneratorChangeLog
    {
        private readonly List<IGeneratorChange> _changes = new();

        public int Count => _changes.Count;

        internal void Add(IGeneratorChange change) => _changes.Add(change);

        /// <summary> Whether this log already carries a before-copy for that object, so a second
        /// Edit() of the same object doesn't overwrite the ORIGINAL state with an already-modified
        /// one. </summary>
        internal bool HasEdit(ObjectId id)
        {
            foreach (var change in _changes)
                if (change is ObjectEdited edited && edited.Id.Equals(id))
                    return true;
            return false;
        }

        /// <summary> Ids the run created, in creation order. Read off the journal rather than
        /// tracked separately, so it can't disagree with what was actually created. </summary>
        public ObjectId[] GetCreatedIds()
        {
            var ids = new List<ObjectId>();
            foreach (var change in _changes)
                if (change is ObjectCreated created)
                    ids.Add(created.Id);
            return ids.ToArray();
        }

        public void Revert()
        {
            for (var i = _changes.Count - 1; i >= 0; i--)
                _changes[i].Revert();
        }

        public void Reapply()
        {
            for (var i = 0; i < _changes.Count; i++)
                _changes[i].Reapply();
        }

        public override string ToString() => $"{_changes.Count} change(s)";
    }

    internal interface IGeneratorChange
    {
        void Revert();
        void Reapply();
    }

    // Id counters are deliberately NOT part of the journal. LevelSettings.ObjectIdCounter only ever
    // grows and ids of deleted objects are never reused (see its own doc comment), so rolling one
    // back on undo would let a later creation hand out an id a stale reference still points at.
    // Reapply re-inserts the SAME instance under the SAME id instead of minting a fresh one, so redo
    // stays exact without the counter having to move at all.

    internal sealed class ObjectCreated : IGeneratorChange
    {
        private readonly IObjectScope _scope;
        private readonly RectObject _instance;

        public ObjectId Id { get; }

        public ObjectCreated(IObjectScope scope, ObjectId id, RectObject instance)
        {
            _scope = scope;
            Id = id;
            _instance = instance;
        }

        public void Revert() => _scope.Objects.Remove(Id);
        public void Reapply() => _scope.Objects[Id] = _instance;
    }

    // Whole-object granularity, not per-field: an Edit hands the generator the live instance and it
    // may rewrite any part of it, including entire keyframe tracks, so a before-copy of the object
    // is the only snapshot that can describe every outcome. The after-copy is taken lazily, on the
    // first Revert, because it doesn't exist yet at Edit time.

    internal sealed class ObjectEdited : IGeneratorChange
    {
        private readonly IObjectScope _scope;
        private readonly RectObject _before;
        private RectObject _after;

        public ObjectId Id { get; }

        public ObjectEdited(IObjectScope scope, ObjectId id, RectObject before)
        {
            _scope = scope;
            Id = id;
            _before = before;
        }

        public void Revert()
        {
            if (_scope.Objects.TryGetValue(Id, out var current))
                _after = current.Copy();
            _scope.Objects[Id] = _before.Copy();
        }

        public void Reapply()
        {
            if (_after != null) _scope.Objects[Id] = _after.Copy();
        }
    }

    internal sealed class ObjectDeleted : IGeneratorChange
    {
        private readonly IObjectScope _scope;
        private readonly ObjectId _id;
        private readonly RectObject _instance;

        public ObjectDeleted(IObjectScope scope, ObjectId id, RectObject instance)
        {
            _scope = scope;
            _id = id;
            _instance = instance;
        }

        public void Revert() => _scope.Objects[_id] = _instance;
        public void Reapply() => _scope.Objects.Remove(_id);
    }

    internal sealed class ResourceAdded<TId, TResource> : IGeneratorChange
    {
        private readonly Dictionary<TId, TResource> _target;
        private readonly TId _id;
        private readonly TResource _resource;

        public ResourceAdded(Dictionary<TId, TResource> target, TId id, TResource resource)
        {
            _target = target;
            _id = id;
            _resource = resource;
        }

        public void Revert() => _target.Remove(_id);
        public void Reapply() => _target[_id] = _resource;
    }

    // The catch-all for a plain field that is neither an object, a resource nor a keyframe - the
    // level's own Framerate/FrameLength. It holds the writer rather than the owning object, so one
    // change type covers every such field without the journal knowing any of them by name.

    internal sealed class ValueChanged<T> : IGeneratorChange
    {
        private readonly Action<T> _write;
        private readonly T _before;
        private readonly T _after;

        public ValueChanged(Action<T> write, T before, T after)
        {
            _write = write;
            _before = before;
            _after = after;
        }

        public void Revert() => _write(_before);
        public void Reapply() => _write(_after);
    }

    internal sealed class ResourceRemoved<TId, TResource> : IGeneratorChange
    {
        private readonly Dictionary<TId, TResource> _target;
        private readonly TId _id;
        private readonly TResource _resource;

        public ResourceRemoved(Dictionary<TId, TResource> target, TId id, TResource resource)
        {
            _target = target;
            _id = id;
            _resource = resource;
        }

        public void Revert() => _target[_id] = _resource;
        public void Reapply() => _target.Remove(_id);
    }

    // Level-global tracks (GameEvents/CameraEvents/PostProcessingEvents/PlayerEvents) are plain
    // List<TKey> with no owning object to snapshot, which is why they get their own change types
    // instead of riding along on ObjectEdited. Index is preserved on both sides: these lists carry
    // no enforced sort order (see the SDK's Keyframes section), so re-inserting at the end after an
    // undo would reorder a track the author never touched.

    internal sealed class LevelKeyAdded<TKey> : IGeneratorChange
    {
        private readonly List<TKey> _track;
        private readonly int _index;
        private readonly TKey _key;

        public LevelKeyAdded(List<TKey> track, int index, TKey key)
        {
            _track = track;
            _index = index;
            _key = key;
        }

        public void Revert() => _track.RemoveAt(_index);
        public void Reapply() => _track.Insert(_index, _key);
    }

    internal sealed class LevelKeyRemoved<TKey> : IGeneratorChange
    {
        private readonly List<TKey> _track;
        private readonly int _index;
        private readonly TKey _key;

        public LevelKeyRemoved(List<TKey> track, int index, TKey key)
        {
            _track = track;
            _index = index;
            _key = key;
        }

        public void Revert() => _track.Insert(_index, _key);
        public void Reapply() => _track.RemoveAt(_index);
    }
}
