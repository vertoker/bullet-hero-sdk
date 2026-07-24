# Model Versioning System — Design Notes

Design captured from a conversation with the author. Nothing here has been implemented yet —
this file is the spec to implement against later, exactly as agreed, without re-deriving the
reasoning from scratch. When implementing, follow this document step by step; if something here
turns out to be wrong once real code is written, fix the code AND update this file to match.

## Original requirements (verbatim intent)

1. Must work with any SaveData model that has explicit versions.
2. Must be possible to load any historical version directly into a class representing that old
   version, then run it forward through converters/migrators to newer versions.
3. Serialization must not be locked to Newtonsoft Json — binary serialization is planned, and
   possibly XML later. Version resolution and migration must be format-agnostic.
4. Must support integrating third-party level formats (Project Arrhythmya first, others later).
5. Must not depend on Unity directly — the SDK is meant to become a plain .NET DLL with only
   NuGet dependencies eventually.
6. Maximum stability. Everything is changed only through code — no data/JSON-driven migration
   configs, no stringly-typed patch scripts.

## Problems found in the current implementation (baseline, before this redesign)

- `Models/Interfaces/SaveData/ISaveData.cs`, `IData.cs` — `IData.GetVersion()` is an **instance**
  method, so a version can only ever be declared by the one class that currently exists for a
  kind. There is no way to keep an old version's class around as a distinct compilable type under
  this model — which is the root blocker for requirement 2.
- `Serialization/CompatibilityService.cs` — `Convert<TValue>(...)` methods never actually migrate;
  every branch just casts the same instance (`// var fromVersion = data.GetVersion();` is
  commented out everywhere). `GetXxxType(Version)` methods only ever accept the current version
  and throw `NotSupportedException` for anything else — loading an old save currently just fails.
  The class's own top comment already flags this: *"TODO refactor this class, add MORE type
  checks, add at least 1 real converter"*, and *"TODO add new attribute [DataVersion(1,0,0,0)] and
  direct collect to dictionary"* — this doc formalizes exactly that suggestion.
- Versioning only exists at the top-level `SaveData<T>` layer. Nested/polymorphic models
  (`GameEvents`, `RectObject`/`EffectObject` variants, keyframe types) have no independent version
  — `TODO.md` already names this as unresolved.
- Version-aware deserialization logic is baked directly into Newtonsoft `JsonConverter`s
  (`JsonConverterData<T>`, `VersionConverter`), so adding BSON/XML serialization would require
  duplicating the whole version-resolution branching per format.
- `CompatibilityService.Convert<TValue>` is hand-written per data kind (one overload per
  interface) — adding a new SaveData kind or a new version means editing existing methods by
  hand; nothing is enforced by the compiler.
- One file leaks a `UnityEngine` reference: `Models/Resources/External/Pixel.cs` (only one found
  across `Models`/`Serialization`) — worth keeping in mind for requirement 5, not urgent.

## Chosen architecture

### Aggregates / domains — the core idea

Version numbers are not attached to every class. They are attached only to a small number of
**aggregate roots** — types that represent a whole subtree that should be migrated as one unit.
Most of the 100+ model classes never carry a version at all; they simply travel inside whichever
aggregate contains them.

Two tiers of aggregate roots:

1. **Forced tier — already exists today.** The six `IData` implementers (`Level`, `LevelMeta`,
   `UserSettings`, `Prefab`, `EffectObject`, `Theme`) are aggregate roots for a reason that has
   nothing to do with versioning: they are independently persistable/importable/exportable units
   (e.g. `Theme`/`Prefab` are both embedded inside `Level.Resources` — see
   `Models/Resources/LevelResources.cs:39,43` — *and* independently serializable via
   `SerializeData<TValue>`/`DeserializeData<TValue>`, matching the "level is a folder of files"
   model and future modding/sharing use cases). This tier is **not optional** and was never a
   choice made for versioning purposes — it's a pre-existing architectural fact this system must
   respect, not something to avoid or flatten away.
2. **Optional / lazy tier — introduced by us, only when needed.** Internal substructures such as
   `GameLevel`, `GameEvents`, etc. do *not* need their own version/envelope by default. Introduce
   a new envelope boundary at this tier only the first time a concrete change requires isolating
   part of a subtree from its siblings (e.g. `Marker` needs to change without forcing
   `RectObject`/`Objects` to be touched). Do not pre-emptively design a full boundary hierarchy.
   Exception: splitting `Level` into its four existing top-level sections (`Settings`, `Game`,
   `Audio`, `Resources`) as independent envelopes is close to free (they're already separate
   properties) and should be done from the start, since it caps the blast radius of *any* future
   change to "at most one of these four sections" instead of all of `Level`.

### When a version bump / new frozen snapshot is actually needed

Only when old JSON, if deserialized directly into the current class, would be **semantically
wrong** — not merely different:
- a field was renamed or removed and its meaning moved elsewhere,
- a unit/range/meaning changed (e.g. degrees → radians),
- a collection's shape changed (`Dictionary` → `List`, a field became polymorphic, etc.),
- a new field's correct default depends on the old data rather than being a constant.

Adding a new field with a harmless default (e.g. `bool IsLocked = false`) needs **no** version
bump — Newtonsoft already leaves it at the default, and that's correct. Don't over-version.

### Chain-freezing mechanics (the part initially undersold, corrected mid-conversation)

Changing a nested leaf class forces freezing that leaf **and every direct ancestor class up to,
but not including, the nearest independently-versioned envelope boundary** — this is unavoidable
in C#, because a container class's property has a concrete static type
(`GameEvents.Markers : List<Marker>` must become `List<MarkerV1>` in the frozen ancestor). This
*is* a chain reaction, but it is bounded to one aggregate, not to the whole `Level` tree — provided
envelope boundaries actually exist at the levels you want insulated. The deeper you push envelope
splitting down the tree ahead of time, the shorter each future freeze chain becomes; the tradeoff
is upfront design/plumbing cost, which is why the lazy-introduction rule above exists.

Worked example that was used to pin this down (Marker changes, then later Checkpoint changes,
both living inside `GameEvents` inside `GameLevel`, with no separate `GameEvents` envelope yet):

- Marker changes: freeze `MarkerV1`, `GameEventsV1` (its `Markers` field must point at
  `List<MarkerV1>`), `GameLevelV1` (its `Events` field must point at `GameEventsV1`). `Level`
  itself is **not** touched or frozen, because `Level.Game` is deserialized through its own nested
  version envelope, not inlined — so `Level`'s own version stays put regardless of how many times
  `GameLevel` changes internally.
- Checkpoint changes later: freeze `CheckpointV1` (first freeze of Checkpoint specifically — the
  suffix number is a per-class local counter, *not* tied to the domain's overall version number),
  `GameEventsV2` (second freeze of GameEvents, now pointing at `List<CheckpointV1>` while
  `Markers` already points at the current `Marker`), `GameLevelV2`. Again, `Level` untouched.
- If `Marker`/`Checkpoint` turn out to change often enough that re-freezing `GameEvents`/
  `GameLevel` every time becomes wasteful, *that* is the point to split `GameEvents` off into its
  own independent envelope (with its own `[DataVersion]`), which will shrink future chains to just
  the leaf + `GameEventsVN`.
- Frozen snapshot classes are **never edited again** once created. If the same leaf changes a
  second time later, create a new snapshot next to the old one (`EffectAngleValueV1`,
  `EffectAngleValueV2`, ...), never overwrite.
- Frozen snapshots don't need the `IModel<T>` boilerplate (`Copy`/`Clone`/`Equals`/`GetHashCode`/
  `Reset`) that live classes have — they're transient deserialization targets, not domain
  objects. Plain properties + `[JsonProperty]` is enough.
- **Gotcha to respect**: `[JsonProperty]` on a frozen snapshot must use the literal string value
  (e.g. `"angle"`), never the shared `Names.Xxx` constant (`Models/Names.cs`) — that constant
  reflects *current* naming and may be renamed/removed later, which would silently break
  compilation of an old, supposedly-frozen snapshot.
- Only aggregate-root types need a `[DataVersion]` attribute + a `VersionedTypeRegistry` entry.
  Deeper snapshot classes that exist purely as another snapshot's implementation detail
  (`GameEventsV1` referencing `MarkerV1`) are wired by plain C# reference from that ancestor's own
  source file — no reflection/registry lookup needed for them.

### Building blocks to implement

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class DataVersionAttribute : Attribute
{
    public string Domain { get; }
    public int Major { get; }
    public int Minor { get; }
    public DataVersionAttribute(string domain, int major, int minor) { ... }
    // also needs a way to express "exclude these converters when building this domain's
    // default/value serializer" — see EffectObject gotcha below. Extend this attribute with an
    // ExcludedConverterTypes array, or add a small separate per-domain config; finalize whichever
    // is less awkward once actually writing this code.
}

public interface IMigration<in TFrom, out TTo> { TTo Migrate(TFrom from); }
// + a non-generic IMigration (FromType/ToType/MigrateUntyped) so a registry can hold them
// polymorphically and chain them without knowing every TFrom/TTo pair at compile time.

public static class VersionedTypeRegistry
{
    // scans the assembly once (static ctor, like today's CompatibilityService), collects:
    //  - (domain, major, minor) -> concrete Type, from [DataVersion] on every candidate class
    //  - domain -> ordered list of IMigration steps, from every IMigration implementer found
    public static Type Resolve(string domain, int major, int minor);
    public static object UpgradeToLatest(string domain, object instance, int fromMajor, int fromMinor);
}
```

Replace `CompatibilityService` entirely with this. The one thing worth keeping from it is *not*
code but a fact: its static-constructor list of the six top-level interfaces
(`Add(typeof(ILevel))`, etc.) already enumerates exactly the "forced tier" domains — that
enumeration becomes part of `VersionedTypeRegistry`'s own bootstrap, just re-expressed as domain
name strings instead of marker interfaces.

### Serialization: one generic converter instead of six

```csharp
public class VersionedEnvelopeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) =>
        objectType.GetCustomAttribute<DataVersionAttribute>() != null;

    // WriteJson: { "version": "<major>.<minor>", "value": <serialize instance normally> }
    // ReadJson: read version tag -> VersionedTypeRegistry.Resolve(domain, version) -> concrete
    //           type -> deserialize "value" into it -> VersionedTypeRegistry.UpgradeToLatest(...)
}
```

- One instance of this converter replaces `JsonConverterData<T>` **and all six**
  `LevelDataConverter`/`ThemeDataConverter`/`PrefabDataConverter`/`UserSettingsDataConverter`/
  `EffectDataConverter`/`LevelMetaDataConverter`.
- This is *not* a special case for "aggregated" vs "non-aggregated" models — there is no such
  distinction in code. `CanConvert` is gated purely on the `[DataVersion]` attribute being
  present. A "non-aggregated" model (`Theme`, `UserSettings`) is just the degenerate case where
  the attribute appears once, at the top, and nothing inside it is independently versioned. An
  "aggregated" model (`Level`) is the *same* mechanism applied recursively — `Level` carries
  `[DataVersion]`, and `GameLevel` (nested inside it) *also* carries its own — the recursion falls
  out naturally from Newtonsoft's normal nested `Serialize` calls hitting `CanConvert` again.
  Types without the attribute (`Marker`, `Checkpoint`, `RectObject` variants, ...) are untouched,
  serialized exactly as they are today, no envelope, no registry lookup, zero overhead.
- Wire format for the envelope itself is unchanged (`{"version": ..., "value": ...}`, same
  `Names.Version`/`Names.Value` constants) — existing saved level files stay loadable without a
  "meta-migration" of the envelope shape itself.
- `VersionConverter` (`Serialization/Converters/VersionConverter.cs`, `System.Version` ↔ string)
  is unrelated to the removed machinery and stays as-is; the new converter still needs it to
  read/write the `"version"` field.

**Known gotcha to preserve**: `EffectObject : RectObject` (see
`Serialization/Converters/Data/EffectDataConverter.cs`), and `ObjectConverter`
(`Serialization/Converters/CustomTypes/ObjectConverter.cs`) treats any `RectObject`-derived type
polymorphically (`[type, value]` wrapping) when it's a value inside the `Objects` dictionary. But
when `EffectObject` is serialized as its own standalone top-level SaveData kind, it must **not**
go through `ObjectConverter`'s wrapping — it needs a private "default" serializer that excludes
`ObjectConverter`, exactly like today's `IRequiresDefaultSerializer`/`OverrideValueSerializer`
pattern (`Serialization/Converters/Base/IRequiresDefaultSerializer.cs`). The new
`VersionedEnvelopeConverter` needs an equivalent, per-domain "excluded converters" configuration
(see the attribute sketch above) so it can build the right nested serializer specifically for the
`Effect` domain while every other domain uses the plain default. This is an open detail to finalize
when actually writing the code — don't lose it.

### Format-agnosticism (requirement 3)

```csharp
public interface IDataSerializer
{
    byte[] SerializeEnvelope(string domain, Version version, object payload);
    (Version version, object rawPayload) DeserializeEnvelope(byte[] data, Type payloadType);
}
```

Only the envelope read/write step is format-specific (JSON via `JObject`, future BSON via
`BsonDataWriter`/`BsonDataReader`, future XML via `XDocument`). `VersionedTypeRegistry`'s
resolve-by-version and migration-chain logic is 100% shared across all of them — no duplication
per format, unlike the current Newtonsoft-only design.

### External integration (requirement 4, Project Arrhythmya first)

Modeled as a separate concept from the internal version-migration chain — e.g.
`IExternalLevelImporter`, living outside `Versions/`. Two options depending on how close PA's
schema is to some real historical BH generation:
- If close enough, target an old frozen BH snapshot type and let the *existing* migration chain
  carry it forward to latest — avoids re-writing mapping logic every time BH's format evolves
  further after the importer was written.
- Otherwise, write a direct one-shot adapter straight to the current model.
No exploration of PA's actual format has happened yet in this conversation — this is a design
placeholder, not a decision.

### Rule system (`Rules/`, `Validations/RuleAnalyzer.cs`) — no architecture changes needed

- `RuleAnalyzer` only ever validates the current/latest object graph (already hardcoded to
  `typeof(Level)` at construction, `Validations/RuleAnalyzer.cs:25`, and only ever called with a
  fully-formed object in `Analyze(object obj, ...)`). Frozen historical snapshot classes must
  **never** carry `[RuleContainer]`/`[RuleNotNull]`/etc — they're transient deserialization
  targets, never passed to `Analyze`.
- Use `RuleAnalyzer` as a **migration correctness test oracle**: every migrator's test should run
  its output through `RuleAnalyzer.Analyze(...)` and assert zero `RuleGroup.Error` issues. This is
  a testing discipline to add, not a runtime pipeline change.
- `BaseRuleAttribute` already has a `Fix()` auto-repair mechanism
  (`Rules/Attributes/BaseRuleAttribute.cs:29`). Consider running it as an optional repair pass
  immediately after migration, for cases where a newly stricter rule (e.g. a brand new
  `[RuleNotNull]`) can't be reliably satisfied by a hand-written migrator default. Division of
  responsibility: the migrator supplies a domain-reasonable default; `Fix()` mops up edge cases
  the migrator's author didn't anticipate.
- Side note, unrelated to versioning but found while investigating this: `RuleAnalyzer` is
  hardcoded to only ever cache/validate `typeof(Level)` today — `Theme`/`Prefab`/`UserSettings`/
  `EffectObject`/`LevelMeta` aren't wired into any rule cache currently. Only relevant if/when one
  of those needs independent rule validation; `RuleAnalyzer` would need to accept a root type
  parameter instead of the hardcoded one. Not blocking for this versioning work.

## Why nested/aggregated versioning over one flat version number

The author's first instinct was a single version number at the top of the file (like today's
`SaveData<T>.Version`) — recorded here because it's a legitimate, more common approach, not a
strawman. Comparison:

- **Single flat version**: simplest mental model, one linear migration chain. But to stay fully
  typed (requirement 6 — no stringly-typed JObject patch scripts), *every* release requires
  freezing a full copy of the *entire* `Level` tree (100+ classes) even if only one leaf changed —
  exactly the "100 classes × 6 versions" explosion the author was worried about at the start of
  this design conversation. The only way to avoid that explosion under a single flat version is
  to fall back to weakly-typed tree-patching migrations, which conflicts with requirement 6.
- **Nested/aggregated version** (chosen): blast radius of any change is contained to the aggregate
  it lives in; stays fully typed and compiler-checked throughout. This is a known, real pattern
  for composite documents, not an invented one — Unity's own `.unity`/`.prefab` YAML gives each
  *component* its own `serializedVersion`, independent of the rest of the scene file; Kubernetes
  gives each resource its own `apiVersion` inside one YAML; NuGet/npm version each package
  independently within one project. Chosen specifically because it's the only option among those
  considered that satisfies both "don't duplicate the whole tree per release" and "stay typed/
  code-only," which are both requirements the author explicitly set.

## What gets removed entirely

- `Models/Interfaces/SaveData/IData.cs`, `ISaveData.cs` — fully superseded by `[DataVersion]` +
  `VersionedTypeRegistry` (which also solves the version→type direction `IData.GetVersion()`
  could never solve, since that needs an instance that doesn't exist yet at read time).
- `Models/Interfaces/SaveData/ILevel.cs`, `ILevelMeta.cs`, `IUserSettings.cs`, `IPrefab.cs`,
  `IEffect.cs`, `ITheme.cs` — confirmed empty marker interfaces (`public interface ILevel : IData
  { }`, etc.), confirmed **zero** usages anywhere in `Assets/Code` (checked via repo-wide grep).
  Existed only so `CompatibilityService` had something to put in its list; the domain is now a
  plain string on `[DataVersion]` instead.
- `Models/SaveData.cs` (`SaveData<T>` class) — the new converter builds/reads the same
  `{version, value}` shape directly via `JObject`, no intermediate wrapper object needed.
- `Serialization/CompatibilityService.cs` — replaced by `VersionedTypeRegistry` (see above; the
  file's own TODO comments effectively describe this replacement already).
- `Serialization/Converters/Base/JsonConverterData.cs` and all six of
  `Serialization/Converters/Data/{Level,LevelMeta,Prefab,Theme,UserSettings,Effect}DataConverter.cs`
  — replaced by the single `VersionedEnvelopeConverter`.
- `Serialization/DataConverters/IDataConverter.cs` — dead/unused scaffolding referencing `IData`,
  no implementers found anywhere; delete alongside the rest.

## What stays unchanged

- `Serialization/Converters/VersionConverter.cs` (`System.Version` ↔ string).
- `Models/Names.cs` — `Names.Version`/`Names.Value` constants, since the envelope wire format is
  unchanged.
- Everything under `Rules/`, `Validations/` — no architecture change, see Rule system section
  above.
- `Serialization/Converters/Base/IRequiresDefaultSerializer.cs` — concept stays, generalized to
  work per-domain from inside `VersionedEnvelopeConverter` instead of per hand-written converter
  class (see `EffectObject` gotcha above).
- The six domain model classes (`Level`, `LevelMeta`, `UserSettings`, `Prefab`, `EffectObject`,
  `Theme`) keep existing where they are; they only lose `: ILevel`/`: IData`/etc. from their
  interface list and gain `[DataVersion("Level", 1, 0)]` etc. Their existing
  `public static readonly Version Version = new(1, 0);` / `GetVersion()` members should be removed
  too, to avoid two parallel sources of truth for the same number (the attribute becomes the only
  source of truth for both directions — write path and read/resolve path).

## Tests to update

`Tests/SerializationTests.cs` currently constructs `new SaveData<T>(...)` directly in three tests
(`TestEffectSerialization`, `TestPrefabSerialization`, `TestThemeSerialization`) and serializes
that wrapper manually via `serializationService.Serializer.Serialize(...)`. These need to move to
whatever the new top-level API looks like (presumably still `SerializeData<TValue>`/
`DeserializeData<TValue>`, now driven purely by the `[DataVersion]` attribute on `TValue` rather
than requiring a `SaveData<TValue>` wrapper). `TestLevelSerialization`, `TestLevelMetaSerialization`,
`TestPlayerSettingsSerialization` already use `SerializeData`/`DeserializeData` and should need
little to no change.

Add, per aggregate that ever gets a real historical version: a round-trip test per historical
generation (old fixture JSON → migrate → assert equals expected current-shape value → assert
`RuleAnalyzer.Analyze` reports zero `RuleGroup.Error`).

## Housekeeping once implemented

- `TODO.md`'s serialization bullet *"definitely sort out model versioning (CompatibilityService's
  per-type Version -> Type mapping only covers the top-level SaveData<T> types right now -
  nested/polymorphic model versioning across breaking model changes is unresolved)"* should be
  removed/updated to reflect this is done.
- `Versions/README.md`'s placeholder note ("Here will be ALL models for versions... All
  **migrators** will use JObject, otherwise if too many changes - it will use temp models") should
  be replaced with a real description once this lands — note it currently suggests a JObject-based
  approach for migrators, which this design deliberately moved away from in favor of fully typed
  snapshot classes, per requirement 6.

## Not yet decided / left for implementation time

- Exact shape of the "excluded converters per domain" mechanism (extend `DataVersionAttribute`
  with an array, vs. a separate small per-domain config table).
- Exact wiring point in `SerializationService.GetConverters` for the new single converter plus
  its per-domain default-serializer construction (today's `IRequiresDefaultSerializer` wiring
  loop is the closest existing analogue — see `Serialization/SerializationService.cs:102-116`).
- Whether/when to actually split `GameEvents` off `GameLevel` as its own envelope — deferred until
  a real change demands it, per the lazy-introduction rule above.
- Project Arrhythmya's actual schema has not been examined yet — the external-importer section
  above is a placeholder shape, not an analyzed plan.

## Q&A recap (brief answers to every question asked in the design conversation)

**Q: What are the architectural problems with the current versioning code, and what are the
options for a proper implementation?**
A: See "Problems found" and "Chosen architecture" above — current code never actually migrates
(just casts), only supports the current version, versions only exist at the top level, and
version logic is baked into Newtonsoft converters. Chose typed POCO snapshots + attribute-driven
migrator registry + format-agnostic envelope, over a weakly-typed JObject-patch alternative that
was rejected for conflicting with the stability requirement.

**Q: Should anything be done to the Rule system for versioning?**
A: No architecture change. Historical snapshots never carry Rule attributes (they're never
validated — `RuleAnalyzer` only ever sees post-migration, current-shape objects). Reuse
`RuleAnalyzer` as a migration-correctness test oracle, and consider `BaseRuleAttribute.Fix()` as
an optional post-migration repair pass for edge cases a migrator's default can't anticipate.

**Q: What do "aggregate"/"domain" and "aggregation" mean, in simple terms?**
A: An aggregate/domain is a boundary you draw around a group of classes so they share one version
number instead of each class having its own. Aggregation is the act of drawing such boundaries.
Analogy used: a level is already "a folder of files" (per the project's own framing); an aggregate
is like giving one subfolder its own revision number, independent of the folder's own number.

**Q: If rules only validate the latest version, how can they be used to test "all versions"?**
A: They never validate old data directly. Each historical test fixture is migrated to the current
shape first; `RuleAnalyzer.Analyze` is then run once against that final current-shape result. Many
input fixtures, but always the same validated output type.

**Q: If `Marker` changes to a new shape, is a `LevelV1` snapshot actually required to load an old
file that really is `Level` version 1?**
A: No — not unless `Level`'s own shape (which top-level sections it has) changed. `Level.Game` is
deserialized through its own nested version envelope, so `GameLevel` (the nearest existing
envelope containing `Marker`) gets frozen (`GameLevelV1`, plus `GameEventsV1`, `MarkerV1`), while
`Level` itself is untouched and its version tag still resolves to the current `Level` class.

**Q: If `Checkpoint` changes next, does that force reusing/updating the old frozen models — i.e.
is a chain reaction really unavoidable?**
A: Partially yes, but it's bounded. Freezing `CheckpointV1` also forces a *new* freeze of
`GameEventsV2`/`GameLevelV2` (their field types must point at the right concrete snapshot types),
but never touches `Level`. The chain is contained to one aggregate (`GameLevel`), not the whole
tree — and can be shortened further later by splitting `GameEvents` into its own envelope once
`Marker`/`Checkpoint` changes prove frequent enough to justify it.

**Q: Compare a single flat version number against several nested versions, and justify why nested
is better.**
A: See "Why nested/aggregated versioning over one flat version number" above. Single flat version
either explodes into full-tree snapshot duplication per release (if kept typed) or forces
weakly-typed JObject patching (if not) — nested/aggregated avoids both while staying typed, with
real precedent in Unity's own per-component `serializedVersion`, Kubernetes `apiVersion`, and
per-package versions in NuGet/npm.

**Q: How should serializers behave when non-aggregated and aggregated models exist side by side?**
A: There's no special case in code for this distinction at all — one generic
`VersionedEnvelopeConverter`, gated purely on the presence of `[DataVersion]` on the runtime type,
handles both uniformly. A non-aggregated model is just the case where the attribute appears once;
an aggregated model is the same mechanism recursing into a nested type that also carries the
attribute.

**Q: Will `ISaveData`/`IData` end up removed — is that the right call — and what else would be
removed?**
A: Yes, correct call. See "What gets removed entirely" above for the full list: `IData`,
`ISaveData`, the six empty marker interfaces (`ILevel`, etc. — confirmed unused anywhere outside
the SDK), `SaveData<T>`, `CompatibilityService`, `JsonConverterData<T>` + its six subclasses, and
the dead `IDataConverter.cs`. `VersionConverter`, `Names.Version`/`Names.Value`, and the Rule
system are unaffected — see "What stays unchanged".
