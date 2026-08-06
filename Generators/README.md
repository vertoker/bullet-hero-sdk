# Generators

A generator is a piece of authoring automation shipped with the format: it produces level content
from a handful of parameters, instead of the author placing every object by hand. Radial bullet
patterns, a grid of blocks, quantizing every keyframe to a beat grid, turning an image into
objects — all the same mechanism.

The design goal is that **adding a generator is adding one class**. No host writes UI for it, no
registry is edited, no switch gains a case. Everything a host needs — how to list it, what to call
it, what form to show, what it will cost, how to undo it — comes off the contract below.

The full design document lives in the consuming project at
`docs/superpowers/specs/2026-08-05-sdk-generators-design.md`.

## Three kinds

| Kind | Produces | Entry point |
|---|---|---|
| `Level` | a whole new `Level` + `LevelMeta` | `ILevelGenerator.Create(parameters)` |
| `Content` | new objects/resources in the active scope | `IScopeGenerator.Run(context, parameters)` |
| `Modifier` | edits to objects that already exist | `IScopeGenerator.Run(context, parameters)` |

Content and Modifier share one entry point on purpose: they differ in intent and in
`GeneratorRequirements`, not in mechanism.

## Writing one

```csharp
public class RadialGenerator : BaseContentGenerator<RadialGenerator.Parameters>
{
    public override string NameKey => "gen_radial";

    public override GeneratorHints Hints => HintsValue;

    private static readonly GeneratorHints HintsValue = new GeneratorHints.Builder()
        .Section(GeneratorSections.Main, SpawnParameters.MainFields)
        .Section(GeneratorSections.Main, nameof(Parameters.Count), nameof(Parameters.Radius))
        .Section(GeneratorSections.Additional, SpawnParameters.AdditionalFields)
        .Section(GeneratorSections.Additional, nameof(Parameters.CenterX), nameof(Parameters.CenterY))
        .Range(nameof(Parameters.Count), 1, 512)
        .Unit(nameof(Parameters.Radius), "px")
        .Build();

    protected override void Generate(GeneratorContext context, Parameters parameters)
    {
        for (var i = 0; i < parameters.Count; i++)
        {
            var obj = context.Create<TextureObject>();
            obj.ParentObjectId = context.Parent;
            obj.StartFrame = context.StartFrame;
            obj.EndFrame = context.EndFrame;
            obj.Layer = context.Layer;
            // ... place it on the circle
        }
    }

    protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
        => new(parameters.Count);

    public class Parameters
    {
        public int Count = 16;
        public float Radius = 100f;
    }
}
```

`GeneratorRegistry` finds it by reflection; nothing else needs to change.

## Rules that are not optional

- **Mutate only through `GeneratorContext`.** It records every change in a `GeneratorChangeLog`,
  which is the entire undo story. Touching the model directly compiles, runs, and silently breaks
  undo — there is no way for the SDK to detect it.
- **Every field must be listed, and listed under a section.** `Type.GetFields()` order is
  unspecified by the CLI, so a form built without `Order`/`Section` can reshuffle itself on a
  recompile. `Section(key, fields)` is `Order` plus a header - use it, and keep plain `Order` for
  the rare field that belongs to no group. Sections are shown in the order they are first declared,
  `Main` (the resources and the few numbers that decide how much of what appears) before
  `Additional` (placement, timing, easing, per-mode switches). A spawning generator splices
  `SpawnParameters.MainFields`/`AdditionalFields` into its own two calls.
- **A parameters class must not shadow an inherited field.** Everything is keyed by field NAME, so
  two fields called `Texture` mean one form row bound at random and one hint hitting both.
- **Every number needs a `Range`.** A host clamps writes against `Hints.Ranges` and has nothing to
  clamp against without one, so an unbounded field becomes a level the format rejects. Enforced by a
  test, and it covers `IFloat`/`IVector2`/`IVector3` fields too, not just `int`/`float`.
- **Parameters are public mutable fields** with a parameterless constructor. That is what a form
  binds to and what a preset serializes.
- **`Estimate` must match what `Run` produces.** A host shows the estimate before running and
  refuses when it would blow past `LevelRules.MaxObjects`; a drifting estimate is worse than none.
- **Declare `GeneratorRequirements.LevelScope`** if you touch `context.Game` or `context.Audio` —
  both are null while a `Prefab` template is the active scope, and a host disables the generator
  there instead of running it into a null.
- **Randomness comes from `context.CreateRandom()`**, not `System.Random`. Same seed, same level,
  on every runtime.
- **Parent what you create to `context.Parent`** — every generator already does, and that one line
  is what makes host-side grouping work: with grouping on, `Parent` is a container object the
  context creates once, on first use, so a whole run can be moved as one thing. A generator that
  parents to `ObjectId.Null` by hand opts itself out of a feature it never had to implement.
  **Layer splitting** (one layer per created object, stepping up from `context.Layer`) needs nothing
  from a generator at all: `BaseScopeGenerator.Run` applies it over the journal after `Generate`,
  precisely so it also wins over generators that write `Layer` themselves.

## Folders

- root — the contract (`IGenerator`, the bases, `GeneratorContext`, `GeneratorChangeLog`,
  `GeneratorHints`, `GeneratorRegistry`, `GeneratorRandom`).
- `Spawn/` — `SpawnParameters` (the shared object template: texture/size/colour/collider) and
  `BaseSpawnGenerator<T>`, which handles minting/parenting/framing each object so a concrete
  generator is only placement math.
- `External/` — the input interfaces a generator implements to say "this parameter comes from the
  host": `IAudioFileInput`, `IWaveformInput`, `IBeatFramesInput`, `IPixelTextureInput`.
- `Modifiers/` — `ObjectTrackMask` + `ObjectTracks` (enumerate an object's ten keyframe tracks
  generically) and the modifiers themselves.
- `Geometry/`, `Bullets/`, `Audio/`, `Textures/`, `Utility/` — the concrete generators.

## Roster

**Level**: `gen_level_empty`.

**Geometry** (static shapes): `gen_grid`, `gen_radial`, `gen_spiral`, `gen_polygon`,
`gen_fractal` (Koch / Sierpinski / Tree, depth-capped because these grow by a constant factor per
level).

**Bullets** (animated, keys baked at author time): `gen_bullet_wave`, `gen_bullet_spiral`,
`gen_bullet_laser_sweep` (a collider-less warning beam plus the firing beam — `ColliderId` is a
static field and cannot animate from harmless to lethal), `gen_bullet_rain` (seeded scatter),
`gen_bullet_homing` (pursuit curve baked into position keys, capped by `LevelRules.MaxObjectKeys`).

**Audio / image** (all `ExternalAnalysis`, see below): `gen_level_audio_file` (a level built around
a song: clip resource, track, timeline length), `gen_audio_waveform`, `gen_beat_flash` (camera only,
the one `LevelScope` generator), `gen_texture_objects` (image → objects, run-merged).

**Modifiers** (edit the selection, create nothing): `mod_quantize_keyframes` (snap keys to a BPM or
frame-step grid, Nearest/Floor/Ceil, per-track mask — a key whose grid line is already taken stays
put rather than overwriting the key that got there first), `mod_stagger` (delay each object one step
further, ordered by selection/layer/x/y/distance; bounds and keyframes shift independently).

**Utility**: `gen_capacity_hint` — recompute `LevelSettings.LimitHints` (peak simultaneous objects
per type) on demand, so the number is visible while deciding whether a section is too heavy.

The roster is complete; new generators are additive from here.

## ExternalAnalysis — parameters the host fills in

The SDK has no audio decoder, no FFT and no image loader, and is not getting any: it is a format
library, and those would drag a platform dependency into it. A generator that needs such data
declares `GeneratorRequirements.ExternalAnalysis` and implements one of the `External/` interfaces;
the host checks for the interface, obtains the data however it can, and writes it back before `Run`.

Matching by interface rather than by field name is deliberate — renaming a field then becomes a
compile error instead of a silent breakage across an assembly boundary.

A generator handed nothing must produce nothing (`gen_audio_waveform` with no peaks creates no bars)
rather than invent a plausible shape, which would look like success.

What the Unity editor supplies today (`Services.GameEditor`'s `GeneratorExternalInputsService`):
peaks from the audio timeline's own `AudioWaveformCache`, beat frames from the level's **markers**
(there is no beat detector — markers are the beat grid this project actually has), pixels from a
registered texture, and an audio path from the native file picker.

## Three things a spawning generator must get right

- **A keyframe's `Frame` is LOCAL to its object** — the runtime reads it back as
  `obj.StartFrame + Frame`. `BaseSpawnGenerator`'s `AddPosition`/`AddRotation`/`AddSize`/`AddColor`
  take an ABSOLUTE frame and convert; anything writing `obj.Positions.Add` by hand must do the same,
  and a modifier snapping to a level-wide grid has to convert the other way first
  (`mod_quantize_keyframes`). Getting this wrong is invisible in every other check: the objects
  appear, in the right place, with the right lifetime — and then never move, because their keys sit
  past their own death. A sweep test asserts every created key lands inside
  `[0, EndFrame - StartFrame]`.
- **Placement math is degrees, storage is RADIANS.** Every hand-authored rotation in a real level is
  a multiple of π (a full turn is 6.2831855), and the Unity project converts to degrees only at its
  inspector boundary. `AddRotation` takes degrees and converts; writing an `AngleKey` by hand without
  converting stores 45 *radians* — the object spins at ~2578°, which is what "the rotation values are
  5k–15k" looked like.
- **A staggered generator must not spawn past its window.** `CanSpawn` is the check (strictly before
  `EndFrame`, since a bullet needs a frame to travel in); without it the overflow clamps onto the
  last frame as one-frame ghosts flashing after the pattern is over, and `Estimate` has to apply the
  same check so it counts what the run actually creates.
- **A lifetime clamped to a single frame gets ONE key per track.** `BaseSpawnGenerator.CanAnimate`
  is the check; two keys on the same frame violate `[RuleCollectionUnique]`, and `Estimate` has to
  apply the same clamp or it drifts from reality exactly where the window truncates the pattern.
