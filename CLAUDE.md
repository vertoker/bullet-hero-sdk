# CLAUDE.md — bullet-hero-sdk

This is `bullet-hero-sdk` (root namespace `BH.SDK`), a standalone C# library defining the level/save
file format for the game Bullet Hero. It's vendored into the main Unity project as a git submodule at
`Assets/Plugins/BulletHeroSDK` (own repo: `vertoker/bullet-hero-sdk`, MIT license) — see the main
project's root `CLAUDE.md` (Rule 4, "Open SDK") for *why* it's split out this way (player-data
longevity, external interop with other rhythm games, third-party modding tools). **This file is
scoped to the SDK repo itself.** Prefer editing here directly over patching a vendored copy; the
Unity project's own `CLAUDE.md`s document the *consumer* side (`Core`'s "SDK model vs runtime state"
split, `GameEditor`'s `TargetScope`/Prefab Mode).

The main `BulletHeroSDK` asmdef is deliberately Unity-independent: `noEngineReferences: true`,
depends only on Newtonsoft.Json/Newtonsoft.Json.Bson (NuGet). Everything that genuinely needs
`UnityEngine` lives in separate, opt-in asmdefs (`UnityExtensions/`, `UnityIntegration/`) so the core
model/serialization/rules code can be reused standalone (e.g. by external tooling, a future
companion website) without dragging Unity along.

## Mental model

A level on disk is **three independent files**, each its own serialization root:
- `level.json`/`.bson` — a `Level` (`Models/Level.cs`): `Settings`/`Game`/`Audio`/`Resources`.
- `metadata.json`/`.bson` — a `LevelMeta` (`Models/LevelMeta.cs`): name/description/authors/license/
  per-resource UGC metadata. **Not a field of `Level`** — a wholly separate aggregate, easy to
  assume otherwise since both describe "the same level."
- `settings.json` — a `UserSettings` (device-wide player options, not per-level).

Every serialization root is a class carrying `[DataVersion(domain, major, minor)]` — see
"Serialization & versioning" below; everything else is a plain nested model versioned only as part
of its containing envelope. `Models/FileNames.cs` names the on-disk files; `PathUtils.FindDataFile`
resolves the actual extension (json vs bson chosen per-level at creation).

Almost every "value-shaped" field in the model tree is **polymorphic**: a keyframeable float,
vector, or color can be a literal `Value`, `RandomMinMax[Step]`, `RandomCircle`/`RandomRect[Step]`,
or (for colors) a `ThemeRef` — see "The polymorphic Value system" below. This is the SDK-side half
of what the Unity project's `Core/CLAUDE.md` calls "two parallel data shapes": the SDK model keeps
values tagged with their *kind*; Unity's runtime `*KeyState`/`*State` structs keep that same tag
alive so `GamePlayer`'s jobs can re-roll randomness every frame instead of freezing it once at load.

## Layer map (folders)

- **Models/** — the level format itself (see "Object model" / "Value system" below for the
  architecturally important parts). Subfolders: `Objects/` (scene-object hierarchy), `Values/`
  (polymorphic Value implementations), `Keyframes/` (keyframe wrapper types), `Effects/`+`Audio/`+
  `AudioEffects/`+`PostProcessing/` (per-domain payload data), `Game/` (`GameLevel` + level-global
  event tracks), `Events/` (one-shot markers/checkpoints, not animated tracks), `Data/` (`ThemeData`/
  `EffectData`/`CompositeCollider` — resource-level aggregate payloads), `Resources/` (level resource
  dictionaries + `TypedResourceId` family), `SettingGroups/` (⚠️ two unrelated aggregates share this
  folder — `LevelSettings`, per-level, vs. `UserSettings`' sub-groups, per-device — see below),
  `Meta/` (`Author`, `ResourceMeta` — consumed by `LevelMeta`, itself NOT in this folder),
  `Interfaces/`, `Enum/`, `Primitives/` (id structs). `Models/Names.cs` is the single source of truth
  for every `[JsonProperty]` name (short/abbreviated on purpose — see Serialization).
- **Serialization/** — `Serializers/` (`SerializationService`, the JSON/BSON entry point),
  `Converters/Base/` + `Converters/CustomTypes/` + `Converters/Dict/` (the polymorphism/id/dictionary
  JsonConverters — see "Serialization pipeline" below).
- **Versions/** — the model-versioning/migration system (`[DataVersion]`, `VersionedTypeRegistry`,
  `IMigration`) plus one subfolder per historical format generation (`V0_0/`). Has its own detailed
  `README.md` — read it first, this file only adds what it doesn't cover.
- **Rules/** — `public const` numeric/enum clamp tables (`FrameRules`, `ValueRules`, `LevelRules`,
  `AudioRules`, `EffectRules`, `PostProcessingRules`, `ResourceRules`, `TextRules`) plus
  `Rules/Attributes/` (declarative `[RuleXxx]` property attributes consumed by `Validations/`).
- **Validations/** — the reflection-based rule engine (`RuleAnalyzer`/`RuleFixer`) that walks
  `[RuleContainer]`-marked object graphs and checks/auto-fixes every `[RuleXxx]`-attributed property.
  **Opt-in tooling, not wired into save/load anywhere** — see "Rules & validation" below.
- **Utils/** — `BHSDKMath` (Unity-independent math, since the core assembly can't reference
  `UnityEngine.Mathf`), `DimensionalIndexer2` (flat-array↔2D-grid indexing), `LevelUtils`
  (`RectObject` id/parent/bounds setters), `ModelUtils` (deep-copy/equality/hash helpers for
  `List`/`Dictionary`/`Array` of `ICopyable<T>`), `ModificationUtils`/`TypeExtensions`.
- **Services/** — `SerializationService`-adjacent but SDK-root-level: `CryptographyService`
  (AES-256-CBC), `ModificationService` (reflection path-based get/set, see "Modification system"
  below), `TextFormatService` (`{variable}` string templating).
- **Generators/** — `BaseGenerator<T>`/`BaseLevelGenerator<T>` (partial-reload / full-reload level
  content generators) + concrete examples (`EmptyLevelGenerator`, `TestLevelGenerator`,
  `TextureToObjectsGenerator`). Has its own `README.md`.
- **Roslyn/** — a *separate* asmdef (`BulletHeroSDK.Roslyn`, Editor-only, `autoReferenced: false`),
  entirely `#if BHSDK_ROSLYN`-gated (that define is never set inside the Unity project, so this
  compiles to an empty assembly here — it's meant for a standalone analyzer-package build of the SDK
  repo). `RuleContainerAnalyzer.cs` — see "Rules & validation" below.
- **UnityExtensions/** — Unity-type conversion glue (`Pixel`↔`Color32`, `PixelTexture`↔`Texture2D`,
  `IFrameable` framerate resolution reading `Screen.currentResolution`). Own asmdef, unconditionally
  requires `UnityEngine` (unlike the core SDK).
- **UnityIntegration/** — `Cat.cs`, a tiny `Debug.Log`-style logging façade (`Meow`/`MeowWarn`/
  `MeowError`/...) gated by `#if BHSDK_UNITY` per call, falling back to `Console.WriteLine`. Own
  asmdef, distinct purpose from `UnityExtensions/` (logging, not data conversion) — don't conflate
  the two folders.
- **Tests/** — `BulletHeroSDK.Tests.asmdef`, NUnit. `MockData.cs` is the shared fixture factory (not
  a test itself) — read its header comment before writing new tests that need a `Level`/`Prefab`/etc.

## Object model (`Models/Objects/`)

`RectObject` is the base of every placeable scene object: `ObjectId`, `ParentObjectId`, `Name`,
`Visible`, `StartFrame`/`EndFrame`/`Layer`, plus the shared keyframe tracks
(`Positions`/`Rotations`/`Scales`/`Sizes`/`AnchorsMin`/`AnchorsMax`/`Pivots`). Empty keyframe lists
are valid (mirrors Unity project's `defaults.xxx` fallback convention). Subclasses, each overriding
`GetModelType() : ObjectType`: `TextureObject`, `EffectObject` (thin — just an `EffectId` pointing
into `Level.Resources.Effects`, the actual payload lives in `EffectData`), `TextObject`,
`PrefabObject` (see "Prefab system" below).

**Polymorphism mechanism** (applies throughout the whole model tree, not just objects — see "Value
system"): `ObjectType` (byte enum) is resolved by `Serialization/Converters/CustomTypes/
ObjectConverter.cs`, registered globally in `SerializationService`, **not** via a `[JsonConverter]`
attribute on `RectObject` itself and **not** via Newtonsoft `TypeNameHandling`/`$type`. Adding a new
`RectObject` subtype means extending `ObjectConverter.GetType`'s switch (throws
`ArgumentOutOfRangeException` otherwise) — there's no attribute-based auto-discovery.

## `IObjectScope` / `IObjectIdCounter` — the split every consumer must get right

`IObjectScope` (`Dictionary<ObjectId, RectObject> Objects`) and `IObjectIdCounter`
(`ObjectId GetNextObjectId()`) are two separate, narrow interfaces. **`Prefab` implements both on
one class** (its own `Objects` + its own int counter). **At level scope the two are split across
different classes**: `Level.Game` (`GameLevel`) is the `IObjectScope`, but `Level.Settings`
(`LevelSettings`) is the `IObjectIdCounter` — `GameLevel` does *not* implement `IObjectIdCounter`,
`LevelSettings` does *not* implement `IObjectScope`. Anything that needs "the scope+counter pair"
generically (the Unity project's `PrefabMaterializer`, `LevelEditorOperation.TargetScope`/
`TargetCounter`) must combine `Level.Game`+`Level.Settings` explicitly for the level case, unlike the
`Prefab` case where one object satisfies both. Don't assume symmetry here.

`ObjectId` has reserved negative constants beyond plain user ids: `Camera = -1` (player-runtime-only,
invalid as an actual `ObjectId`), `LocalPlayer = -2` (a valid parent target), `PrefabRoot = -3`
(only meaningful *inside* a `Prefab` template — same effect as an unset/`Null` `ParentObjectId`
there). `RuleParentObjectIdValidAttribute` accepts all three everywhere, even where semantically
meaningless (e.g. `PrefabRoot` at level scope) — a known leniency gap, not yet context-checked.

## Prefab system

`Prefab` (`Models/Objects/Prefab.cs`, a `Level.Resources.Prefabs` entry) is the *template*: its own
`Objects`/`ObjectIdCounter`. `PrefabObject` (a `RectObject` subclass) is the *placement*: `PrefabId`
(which template) + `Dictionary<ObjectId, ObjectId> ObjectIds` (template-inner id → this placement's
own materialized outer id). Placements — whether at level scope or nested inside another `Prefab`'s
own `Objects` — live in the **same** `Objects` dictionary as everything else, discriminated only by
`GetModelType() == ObjectType.PrefabObject`; there's no separate placement list.

**`PrefabObject` no longer has a `Modifications` field** (removed — see the Unity project's `git log`
"remove ObjectIdModification.cs, now uses dictionary"). The `Modification` class itself
(`Models/Objects/Modification.cs`: `ObjectId` + `string Path` + `object Value`) and the generic
`Services/ModificationService.cs` (reflection get/set by dotted/indexed path string, e.g.
`"transform.position[0]"`, resolved via each property's `[JsonProperty]` name) still exist as
reusable infrastructure, but nothing in `Models/Objects/` wires `Modification` into `PrefabObject`
anymore — don't assume it's load-bearing for prefab overrides today. `Modification.cs`'s own doc
comment lists its design limitations (only `RectObject`/`Prefab`, no deep-inheritance across nested
prefab scopes) if this gets revived.

## The polymorphic Value system

Every "authorable value" field goes through one of these interfaces (`Models/Interfaces/Values/`),
each `: IModel<TSelf>` plus a single discriminator method returning a `*Type` enum:

| Interface | discriminator enum | concrete variants |
|---|---|---|
| `IFloat` / `IInt` | `FloatType`/`IntType` | Value / RandomMinMax / RandomMinMaxStep |
| `IVector2/3/4` | `VectorType` (shared) | Value / RandomRect / RandomRectStep / RandomCircle |
| `IColor3` (RGB) / `IColor4` (RGBA) | `ColorType` (shared) | Value / ThemeRef / RandomMinMax |
| `IString` | `StringType` | Value / Localized (`List<StringLanguage>`) |
| `IScreenLimit` | `ScreenLimitType` | None / Fixed / Bounds |
| `ILicense` | `LicenseType` | NoSpecified / Typical / Custom |

**Discriminator mechanism — a 2-element JSON array, not Newtonsoft `$type`/`TypeNameHandling`.** Each
interface has its own `JsonConverter<TInterface, TType> : JsonConverterCustomType<T, TType>`
(`Serialization/Converters/CustomTypes/*.cs`) writing `[typeEnum, payload]`. The base class
(`Serialization/Converters/Base/JsonConverterCustomType.cs`) needs a *second*, private "default"
serializer (containing every other converter except itself) to deserialize the resolved concrete
type's own plain members without recursively re-wrapping them — see `IRequiresDefaultSerializer`
(`Serialization/Converters/Base/`, **not** under `Models/Interfaces` despite the SDK's own TODO.md
implying otherwise). `SerializationService.GetConverters` auto-wires this for any converter
implementing the interface — adding a new polymorphic-value converter needs no other bookkeeping.

Same 2-element-array mechanism backs several other polymorphic families beyond the table above:
`EffectAngleConverter`/`EffectColorConverter`/`EffectScaleConverter`/`EffectShapeConverter`/
`EffectShapeSpreadConverter` (effect emitter sub-shapes, see "Effects" below), `Color4X4KeyConverter`
(4-corner keyframe color — `Color4X4KeyType.Value/Horizontal/Vertical/BariCentrical`), and
`ObjectConverter` (`RectObject` hierarchy, see "Object model" above).

**Dictionaries with a self-describing value** (`ObjectId→RectObject`, `AudioId→LevelTrack`,
`ThemeId→ThemeData`, `EffectId→EffectData`, `PrefabId→Prefab`, resource-id dicts) serialize as a
**plain array with the key dropped** (`Serialization/Converters/Dict/DictionaryAsListConverter`,
key recovered from the value on read). Dictionaries where the key *can't* be derived from the value
(id→id remap tables) use `DictionaryAsPairListConverter` instead (array of `{K,V}` structs) — plain
Newtonsoft dictionary serialization needs a `TypeConverter` on the key type to use it as a JSON
property name, which value-type ids like `ObjectId` don't have; this sidesteps that entirely.

## Keyframes

`Keyframe` (`Models/Keyframes/Keyframe.cs`) is the shared base: `Frame` (int) + `Ease` (`EaseType`,
29-value enum, stored **per-keyframe**, not per-track). Concrete keyframe types wrap one payload
field each (`FloatKey`, `Vector2/3/4Key`, `Color3Key`, `Color4Key`, `AngleKey`, `ScaKey`,
`AlignmentKey`, `UVKey`, `ZoomKey`, `ShakeKey`, `ScreenLimitKey`, `VelocityPoint`, ...). **`BoolKey`
is the one outlier** — implements bare `IFrame` (no `Ease`), since a toggle has nothing to
interpolate. **4-corner texture color** is its own polymorphic keyframe family, separate from the
`IColor4` value system: `IColor4X4Key` → `Color4Key` (all 4 corners same) / `ColorHorizontalKey`
(Left/Right) / `ColorVerticalKey` (Bottom/Top) / `Color4X4Key` (all 4 independent — `BL`/`BR`/`TL`/
`TR`), resolved via `Color4X4KeyConverter`.

**No enforced sort order.** `[RuleCollectionUnique(nameof(XKey.Frame))]` validates Frame values are
*unique* within a track's `List<TKeyframe>` — it does not enforce them being sorted. Consumers must
sort/search themselves (Unity's `LevelPlayerMath.FindMatchIndexes` assumes sorted input at that
layer, converted once at load via `LevelStateBuilder`).

## Effects, Audio, PostProcessing, Theme

- **Effects**: `EffectObject.EffectId` points at an `EffectData` resource (`Models/Data/
  EffectData.cs`, `Level.Resources.Effects`) — the SDK-side analog of Unity's `EffectGameStatic`
  index table. `EffectData` groups: `Core` (render/loop/particle-count/lifetime/texture/pivot),
  `Forces` (gravity/velocity/orbital/linear-force), plus 4 independently-polymorphic sub-shapes:
  `IEffectAngle`/`IEffectColor`/`IEffectScale` (each: Value / CurvesOverLife / CurvesBySpeed /
  RandomUniform / RandomPerComponent — the Random pair is structurally identical, differs only in
  evaluation semantics) and `IEffectShape` (Point/Circle/Rectangle/Line/Cone/Torus, with
  Circle/Line/Cone/Torus additionally nesting an `IEffectShapeSpread`: Random/Loop/PingPong/Sine —
  `Sine` and `Point` carry zero fields, pure enum-selected behavior).
- **Audio**: `AudioLevel.Tracks : Dictionary<AudioId, LevelTrack>` (one flat dict per level;
  `AudioId` bans negative values — no game/user-defined split unlike other ids). `LevelTrackEffects`
  is a **flat class with one always-present field per DSP effect** (Lowpass/Highpass/Echo/Reverb/
  Chorus/PitchShifter/Distortion/Flange/Compressor/Normalize/ParamEQ), not a dictionary/flags/list.
  Each effect's "enabled" state is encoded as `MixLevel > -80dB` (`AudioRules.IsActiveMixLevel`) —
  **no explicit bool per effect**. `LevelTrackEffects.Active` (track-level, default `false`) *is* an
  explicit bool — don't confuse the two.
- **PostProcessing**: `GameLevel.PostProcessingEvents` — top-level `Active` (default `true`,
  opposite default from audio's `Active`) + 12 keyframe-track lists, one per URP effect (Bloom,
  ChromaticAberration, Vignette, LensDistortion, FilmGrain, MotionBlur, ColorCurves, LiftGammaGain,
  ShadowsMidtonesHighlights, WhiteBalance, AnalogGlitch, DigitalGlitch — matches Unity's
  `Core/Models/Groups/PostProcessingGroup.cs` field-for-field). `PostProcessingKeyframe` implements
  `IKeyframe` directly (not `: Keyframe`) and adds its own per-keyframe `Active` bool — every effect
  is independently toggleable per-keyframe *in addition to* the track-level switch. Several fields
  are commented `HEAVY, PHONES DON'T LIKE IT` (Bloom, MotionBlur, AnalogGlitch, DigitalGlitch).
- **Theme**: `ThemeData` (`Level.Resources.Themes`) holds a fixed `Color4Value[64] Matrix` (an
  "8×8 grid", index layout documented in-file, mirrors *Project Arrhythmya*'s convention).
  `ThemeKeyframe` (a real animated track, unlike `Marker`/`Checkpoint`) selects which `ThemeId` is
  **active** over time. `ColorType.ThemeRef` (`Color3ThemeRef`/`Color4ThemeRef`) stores only a raw
  `int ThemeColorIndex` (0-63) — **not** a `ThemeId` — indexing into whichever theme is currently
  active. Two-level indirection: `ThemeKeyframe` picks the theme, `ThemeRef` picks a slot within it.

## `Models/Game/`, `Models/Events/`: level-global vs. per-object

`GameLevel` (`Level.Game`) = `Objects` (the `IObjectScope`) + four `[DataVersion]`-tagged event
aggregates: `GameEvents` (Markers — editor-only annotation, no gameplay effect — Checkpoints,
ScreenLimits, Backgrounds [`Color3Key`, themeable], Themes [`ThemeKeyframe`]), `CameraEvents`
(Positions/Rotations/Pivots like a `RectObject`, but `Zooms` instead of 2-axis `Sizes` and an added
`Shakes` track — no Layer/Anchors/Sizes, the camera has no parent/isn't rendered), `PostProcessingEvents`
(above), `PlayerEvents` (`Visibles`/`Controls`/`Collisions`, each `List<BoolKey>`). `Checkpoint`/
`Marker` (`Models/Events/`) are flat one-shot lists, not animated keyframe tracks, despite living
alongside `ThemeKeyframe` (which *is* a real track) in the same `GameEvents` class.

## `Models/SettingGroups/` — one folder, two unrelated aggregates

`LevelSettings` (`Level.Settings`, per-level: `Framerate`, `FrameLength`, `ObjectIdCounter`,
`AudioIdCounter` — the `IObjectIdCounter` implementation) has nothing to do with the rest of this
folder (`GeneralSettings`/`ControlsSettings`/`AudioSettings`/`GraphicsSettings`/
`GameEditorSettings`), which are all sub-groups of `UserSettings` (per-device, `settings.json`).
The `UserSettings` sub-groups additionally implement `IMoveable<T>` (`Pull(source)` — an in-place
merge, distinct from `IModel<T>`'s `Copy`/`Reset`).

## `Models/Resources/`

`LevelResources` (`Level.Resources`): seven dictionaries — `Textures`/`Fonts`/`Audios` (by their
typed resource id), `CompositeShapes` (by `ColliderId`), `Themes`/`Effects`/`Prefabs` (by their own
Guid-based id). **Every dictionary here can only ever contain user-defined (negative-id) resources**
— each concrete `Resource` subtype's id property is rule-capped to the negative range; game-defined
resources are baked into the game/its own registries and never appear in a level's own `Resources`.
`TypedResourceId` (`Models/Primitives/Resources/`) is the shared convention: `0`=Null,
`[1,MaxInt]`=game-defined (permanent), `[MinInt,-1]`=user-defined (needs `Resource.Sources`, up to
`Resource.MaxSourcesCount`=4 fallback URIs per resource). `TextureResourceId`/`FontResourceId`/
`AudioResourceId`/`BytesResourceId`/`TextResourceId` are narrow per-category wrappers sharing this
range, freely convertible to/from the untyped `TypedResourceId`.

`Guid`-based ids (`ColliderId`/`EffectId`/`LevelId`/`PrefabId`/`ThemeId`) deliberately have **no**
positive/negative split — a `Guid` has no natural sign, so game-defined vs. user-defined is
determined by *which collection* an id is found in (game registry vs. level `Resources`), not by the
id's own value, unlike the int-based `TypedResourceId` family.

## `IModel<T>` pattern

`IModel<T> : ICopyable<T>, IEquatable<T>, IResetable` — every live domain model implements: `Copy()`
(new instance), `Equals(T)`/`Equals(object)`/`GetHashCode()` (hand-written, not generated — this is
exactly the kind of boilerplate a copy-paste mistake hides in, see "Conventions" below), `Reset()`
(back to defaults, in place, no allocation). Some additionally implement `IUpdatable<T>`
(`Update(src)` — in-place field copy from another instance, distinct from both `Copy` and `Reset`;
used by `RectObject`+subclasses, `EffectData`+its sub-groups). **Frozen historical snapshot classes
under `Versions/VX_Y/` deliberately skip all of this** — they're one-shot deserialization targets,
not domain objects; don't expect every `[JsonProperty]`-bearing class in this codebase to implement
`IModel<T>`.

## Serialization pipeline (`SerializationService`)

`SerializationService.SerializeData<T>`/`DeserializeData<T>` are the plain string-JSON entry points —
both throw `ArgumentException` if `T` has no `[DataVersion]` (only aggregate roots may go through
this API). `GetDataSerializer(SerializationType.Json/Bson)` returns an `IDataSerializer`
(`SerializeEnvelope`/`DeserializeEnvelope` operating on raw `byte[]` + `EnvelopeData` — version tag +
untyped payload) — `JsonDataSerializer`/`BsonDataSerializer` share all envelope logic in
`BaseNewtonsoftDataSerializer`, differing only in `JsonTextWriter/Reader` vs. `BsonDataWriter/Reader`.
`SerializationType` is `byte`-backed (`Json=0, Bson=1`); `SerializationTypeExtensions.ToFileExtension`/
`TryFromFileExtension` map `.json`/`.bson`.

Two `JsonSerializerSettings` are built: one with the full converter list, one bare ("`settingsDefault`",
the escape hatch every `IRequiresDefaultSerializer` converter needs — see "Value system" above).
`ObjectCreationHandling = Replace` is load-bearing (documented inline in `SerializationService.cs`):
without it, deserializing into a non-null nested object/list left by a parameterless constructor
(e.g. a default 2-key curve) *populates into* the existing instance instead of replacing it, breaking
round-trip equality. `ContractResolver` only forces `MemberSerialization` (`OptIn` by default) onto
every contract.

**Id/primitive wrapper structs** (`ObjectId`, `ThemeId`, `AudioId`, `PrefabId`, `ColliderId`,
`EffectId`, any `IPrimitiveGuid`/`IPrimitiveInt`/`IPrimitiveFloat`) serialize as a **bare scalar**, not
`{"Value": ...}`, via `PrimitiveGuidConverter`/`PrimitiveIntConverter`/`PrimitiveFloatConverter` — all
reconstruct via `Activator.CreateInstance(type, value)`, so every such wrapper needs a public
single-arg constructor. `PrimitiveGuidConverter` specifically handles Guid surfacing as a `string`
under JSON but an already-boxed `Guid` under BSON (BSON's native UUID subtype).

## Model versioning (`Versions/`)

**Read `Versions/README.md` first** — it documents the folder convention (generation-first, e.g.
`V0_0/` + `V0_0/Migrations/`) and the "nested envelope always resolves to the domain's *current*
type" rule in detail; this section only adds what it doesn't cover.

- `[DataVersion(domain, major, minor)]` marks an aggregate-root boundary that gets its own envelope
  and migrates as one unit. **Every live domain today is `(1, 0)`** — nothing has bumped yet. 15
  types currently carry it: `Level`, `LevelMeta`, `UserSettings`, `Prefab`, `EffectData`, `ThemeData`,
  `CompositeCollider` (SDK-repo "core" tier); `LevelSettings`, `GameLevel`, `AudioLevel`,
  `LevelResources` (nested under `Level`); `GameEvents`, `CameraEvents`, `PostProcessingEvents`,
  `PlayerEvents` (nested under `GameLevel`). `DataDomains.cs` is the `nameof()`-based constant list.
- `VersionedEnvelopeConverter` (`Serialization/Converters/`, not `Versions/`) writes/reads the
  `{"version": "major.minor", "value": ...}` wrapper, gated purely by `[DataVersion]` presence — a
  `_activeDomains` reentrancy guard lets member serialization fall through to plain fields while
  writing/reading that same domain's own payload, without special-casing "nested vs. top-level."
  On read, it always resolves + upgrades through `VersionedTypeRegistry` and returns the domain's
  **current-shape type**, never the historical snapshot type.
- `VersionedTypeRegistry` populates itself via a **one-time reflection scan in a static constructor**
  (same pattern as the Unity project's `ReflectionUtils.GetImplementations<T>()`), indexing both
  every `[DataVersion]` type and every `IMigration` implementation. `UpgradeToLatest` walks
  `IMigration` step by step from a deserialized instance's version to the domain's latest, throwing
  if a step is missing.
- **`V0_0` is a scaffold that exercises the machinery end-to-end, not real shipped format history** —
  its `Names` use placeholder JSON keys (`"test_settings"`, etc.) and its snapshot classes are
  structurally near-identical to current ones. There is no `V1_0` folder or class anywhere — "current"
  is just the live, un-suffixed model class carrying `[DataVersion(..., 1, 0)]` directly; migrator
  filenames like `LevelV0_0ToV1_0.cs` reference that live class by convention, not an actual file.
- Replaces an older `CompatibilityService`/`SaveData<T>`/`JsonConverterData<T>` design — those names
  are fully gone from the codebase (only survive in a comment explaining what replaced them); don't
  reintroduce or reference them as if live.
- **Dangling cross-reference**: both `Versions/README.md` and this SDK's `TODO.md` say "see
  `VERSION-UPDATE.md` at the SDK root" — that file does not exist. Likely renamed to
  `Versions/README.md` without updating the pointer, or lost; don't try to find it.
- Open per the SDK's own `TODO.md`: nested/optional aggregates below the current per-domain split,
  the first *real* migrator once a domain actually needs to bump past 1.0, Project Arrhythmya import.

## Rules & validation

`Rules/` classes are mostly pure `public const` numeric tables with zero `Models/` dependency
(`FrameRules`, `ValueRules`, `LevelRules`, `AudioRules`, `PostProcessingRules`, `ResourceRules`,
`TextRules`) — `EffectRules` is the one exception, constructing default `CurveValue`/`GradientValue`
model instances. `RuleGroup` (`None/Error/Warning/Advice`) is a severity enum that exists but is
**never actually set away from its `Error` default** by any current rule attribute.

`Rules/Attributes/` are declarative `[RuleXxx]` property attributes (`[AttributeUsage(Property)]`
only — never fields), all `: BaseRuleAttribute` (`IsValidType`/`IsValid`/`Fix`). `[RuleContainer]`
(a bare class-level marker) opts a type into the reflective walk — applied broadly across `Models/`
(156+ files), not just a handful of aggregate roots. `Rules/Attributes/Contextual/` need the root
`Level` as context (`RuleLevelFrameAttribute` checks against `Level.Settings.FrameLength`,
`RuleObjectIdValidAttribute`/`RuleParentObjectIdValidAttribute` check `ObjectId` validity/parent
rules) — both have an explicit `// TODO add complex check for parenting and ids uniqueness`, i.e.
**cross-object graph invariants (referential integrity, uniqueness, cycles) are not implemented
anywhere yet**, only single-property range/shape checks in isolation. `Rules/Attributes/Values/` are
typed against the polymorphic `IFloat`/`IVector2-4`/`IString`/`IPrimitiveInt`/`IPrimitiveGuid`
interfaces, switching per concrete variant to check/clamp.

`Validations/RuleAnalyzer` walks any `[RuleContainer]`-typed object graph (generic over the root, not
hardcoded to `Level`) and returns `List<RuleIssue>`; `RuleFixer` applies fixes **in reverse trace
order** deliberately (fixing may invalidate/shift deeper issues). `RuleIssue`/`RulePath` carry the
full trace from root to the failing property (including list index / dict key) so a fix knows exactly
where to write. **This whole system is opt-in library code — nothing in the SDK or the Unity project
currently calls `RuleAnalyzer`/`RuleFixer` outside the SDK's own tests.** If a consumer (e.g. the
in-game editor, before allowing a save) wants validation, it has to call this explicitly; there is no
automatic hook on load/save today.

`Roslyn/RuleContainerAnalyzer.cs` (separate `BulletHeroSDK.Roslyn` asmdef, `#if BHSDK_ROSLYN`-gated,
so inert inside the Unity project) enforces at compile time that every `[RuleContainer]` class is
non-static, non-abstract, and has a public parameterless constructor — because several `Fix*` paths
(`RuleNotNullAttribute`, the `RuleIPrimitiveXxx` family) call `Activator.CreateInstance` on property
*types* at runtime, and a violation here would otherwise only surface as a rare, hard-to-place
`MissingMethodException` deep inside an editor "auto-fix my level" flow.

## Conventions

- **`Names.cs` constants, never string literals, for `[JsonProperty]`.** Deliberately short/
  abbreviated (`"f"` for Frame, `"v"` for Value, `"t"` reused for both TypeShort and TimeShort —
  disambiguated only by never co-occurring on the same model) to keep the wire format compact.
  Historical `Versions/VX_Y/` snapshot classes intentionally use their own frozen literals/`NamesVX_Y`
  instead, so renaming a *current* key can never silently corrupt what an old snapshot deserializes.
- **New polymorphic value/effect variant** = new enum case + concrete class + a case in the matching
  `JsonConverterCustomType<T,TType>` subclass's `GetType`/`GetCustomType` switch. No attribute-based
  auto-discovery anywhere in this system (unlike `[RuleContainer]`'s reflection scan or
  `VersionedTypeRegistry`'s reflection scan) — every converter is a manual, explicit mapping.
  Same applies to `RectObject` subtypes via `ObjectConverter`.
- **New `[DataVersion]` aggregate** = add the attribute at `(1, 0)` if genuinely new, or bump + write
  a `VX_Y/` snapshot + `IMigration` pair if changing an existing domain's shape — see `Versions/
  README.md`'s folder-convention rules in full before doing this, several easy-to-miss subtleties
  (nested property must stay typed as the *current* class, snapshot classes skip `IModel<T>`, a
  domain with no independent envelope yet at some generation gets a snapshot with no `[DataVersion]`
  at all).
- **Level-wide/track-wide numeric collection caps live in `Rules/LevelRules.cs`/`AudioRules.cs`**
  (max markers/checkpoints/keys/prefabs/audio-layers/...) — check there before hardcoding a magic
  cap elsewhere; `Level.Objects` itself is deliberately uncapped (see the commented-out
  `MaxObjects` in `LevelRules.cs`).
- **`IModel<T>`'s `Equals(object obj) => obj is T value && Equals(value);` boilerplate is hand-written
  per class, not generated** — when adding a new `IModel<T>` type, double-check the `is T` matches
  the enclosing class exactly. This exact one-line pattern is the single easiest place to introduce a
  silent bug in this codebase: it looks identical across every model, so pasting it from a sibling
  class without updating the type name compiles fine and just makes `Equals(object)` always return
  `false` for real instances (the strongly-typed `Equals(T)` overload keeps working, so the bug only
  shows up wherever something compares through the non-generic `object.Equals`/`==` boxed path).

## Testing

`Tests/` (`BulletHeroSDK.Tests.asmdef`, NUnit): `MockData.cs` is the shared fixture builder (moved out
of `SerializationTests` specifically so every test file can reuse it) — `CreateTestXxx`/
`CreateValidXxx` factories deliberately touch as much field surface as possible while staying
rule-valid; `CreateInvalidXxx` factories are deliberately minimal, each encoding exactly one rule
violation `RuleFixer` must detect and fix. A `#region Version v0.0` half builds equivalents against
`V0_0` snapshot types for exercising the migration path, including a hand-spliced JSON envelope
builder (`CreateTestLevelV0_0Json`) — needed because `VersionedEnvelopeConverter` always tags a
*whole* current-shape object with the *current* version when serializing, so each historical fragment
has to be serialized standalone from its own real `VX_Y` type and spliced in by hand.

Per the SDK's own `TODO.md`: `SerializationService`'s stability across all keyframe/value/effect type
combinations (round-trip + real saved level files) is explicitly flagged as not yet fully verified,
especially after the `IRequiresDefaultSerializer` refactor — treat serialization-adjacent changes
here as higher-risk than they might look.
