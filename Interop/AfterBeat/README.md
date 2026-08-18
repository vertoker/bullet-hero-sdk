# Afterbeat interop

Two-way conversion between this format and **Afterbeat** (formerly *Project Arrhythmia*, by Vitamin
Games): its level (`.vgd`), metadata (`.vgm`), theme (`.vgt`) and prefab (`.vgp`) documents.

`AfterBeatInterop` is the entry point — one method per thing a host converts. Everything here takes
and returns **text**, never a path: this library reads no files, and where a document came from is
the host's business.

## Where the format description came from

These models were transcribed from the official Afterbeat wiki. Every page below was read at
transcription time (August 2026); nothing here was reverse-engineered from a binary or a save file.

| Page | What it covers |
|---|---|
| [VGD format](https://afterbeat.wiki.gg/wiki/VGD_format) | the level document: editor block, triggers, parallax, checkpoints, objects, prefab placements, prefabs, themes, markers, and the fourteen `events` arrays |
| [Object Data](https://afterbeat.wiki.gg/wiki/Object_Data) | one object's fields and its four keyframe tracks; the shape, object-type, autokill, gradient and randomization tables |
| [VGP format](https://afterbeat.wiki.gg/wiki/VGP_format) | the prefab document and its type table |
| [VGT format](https://afterbeat.wiki.gg/wiki/VGT_format) | the theme document and its 34 colours |
| [VGM format](https://afterbeat.wiki.gg/wiki/VGM_format) | the metadata document; difficulty, artist link and game-reference tables |
| [Level folder](https://afterbeat.wiki.gg/wiki/Level_folder) | what a level folder holds — **a stub at the time of writing**, so the file names below were recovered from the changelog pages instead |

Two caveats the wiki states about itself, both load-bearing here:

- **`Object Data` is marked `{{Proofread}}`** ("mostly copied directly") and its shape table is
  behind the game: the editor has custom polygon shapes (sides, roundness, thickness, slices,
  inverted) whose JSON keys are undocumented. This is the direct reason every model in `Models/`
  derives from `AfterBeatNode` and carries `[JsonExtensionData]` — unknown keys survive a round trip
  instead of being silently deleted on the first export.
- **`Level folder` is a stub.** File names (`level.vgd`, `level.jpg`, the song as `.ogg`/`.mp3`/
  `.wav`) come from the version-history pages rather than from a specification, so a host should
  probe for them rather than assume them.

The format carries **no version field** at all. Compatibility is expressed per key as "optional,
defaults to X", which is why every property in `Models/` is initialised to the documented default
rather than to the CLR's.

## Layout

```
AfterBeatNames.cs          every JSON key, deliberately separate from Models/Names.cs
AfterBeatEnums.cs          every enum table, with the format's own numbering and its gaps
AfterBeatOptions.cs        the choices a conversion cannot make on its own
AfterBeatSerialization.cs  its own JsonSerializerSettings - see below
AfterBeatInterop.cs        the entry point
Models/                    the wire models, one file per document
Maps/                      Time, Ease, Shape, Theme, Colour, Value(random), Id
Import/                    Afterbeat -> this format
Export/                    this format -> Afterbeat
```

`Generators/Interop/AfterBeatLevelGenerator.cs` wraps the import as `gen_level_afterbeat`, so the
editor gets a form, an estimate and a preset system for free. The export is **not** a generator: a
generator produces content, an export consumes a level.

## Three decisions worth knowing before changing anything here

**These documents do not go through `SerializationService`.** That service wraps every
`[DataVersion]` aggregate in a `{"version", "value"}` envelope and installs two dozen converters
implementing *this* format's polymorphism. Both would corrupt a foreign document. `Interop/` is also
not `Versions/`: that machinery upgrades this format's own domains between generations, and a
foreign format is not a generation of it.

**Unknown keys are preserved** (`[JsonExtensionData]` on `AfterBeatNode`) — see the caveat above.

**Ids are derived, never freshly generated.** Afterbeat names themes and prefabs with arbitrary
strings; `AfterBeatIdMap` hashes those into stable Guids, so re-importing a level, or importing a
`.vgt` and then a `.vgd` that references it, produce the same id both times.

## What crosses, and what does not

Everything lost or approximated goes into an `InteropReport` — aggregated by cause, with a count and
the first place it happened. A silent lossy import is the failure mode the whole class exists to
prevent.

> The summary below is the short version. The **full mapping standard** — every enum table, every
> slot index, every conversion law, and the complete ledger of what is skipped in each direction —
> lives in the consuming project at `docs/issues/AFTERBEAT_ISSUE.md`. Read that before changing any
> number in `Maps/`.

**Exact, both ways:** themes (Afterbeat's 34 colours are exactly the Project Arrhythmia slot layout
`ThemeData` already uses, minus alpha, which `.vgt` has no channel for), markers, checkpoints
including their respawn position, object hierarchy, and the object/prefab structure.

**Converted, with a rule worth remembering:**

| Thing | There | Here |
|---|---|---|
| time | seconds | frames at the level's framerate |
| object lifetime | start + an autokill rule | a half-open `FrameSpan` |
| keyframe time | relative to the object's start | the same — **not** rebased |
| rotation | degrees, each key relative to the previous | radians, absolute |
| draw order | absolute depth 0–60, smaller in front | parent-relative `Layer`, higher in front |
| object colour | theme slot + separate opacity | `ThemeRef` at full opacity, a literal below it |
| shapes | 25 `(shape, option)` pairs | 78 presets, plus synthesized geometry for the seven combinations no preset covers |
| parallax | a background subsystem | ordinary collider-less objects with the loop baked into keyframes |

**Not imported:** triggers, the screen-gradient and hue event tracks, depth of field, per-axis
parent inheritance and parent time offsets, prefab preview images and lead times.
**Player force is reported as deferred rather than dropped** — `PlayerEvents.Velocities` and
`VelocityPoints` already exist in the model, commented out, so that one is waiting on work rather
than on a decision.

**Not exported:** particle effects, audio (an Afterbeat level is one song file in a folder — no
track list, offsets, speeds or effects), level-authored geometry, anchors, per-corner colours,
per-character text effects, random values, beat segments past the first, checkpoint spaces other
than World, several post-processing effects, per-instance prefab overrides, and — worth naming
separately — **licensing, age rating and attribution**, which `.vgm` has no field for at all.

## Testing

`Tests/Interop/AfterBeat/` covers each map on its own plus a round trip. `AfterBeatCorpusTests`
additionally runs every real level found under the folder named by the `BH_AFTERBEAT_CORPUS`
environment variable; with the variable unset, or the folder empty, it passes having checked nothing
and says so. No level files live in this repository — this is somebody else's user content.
