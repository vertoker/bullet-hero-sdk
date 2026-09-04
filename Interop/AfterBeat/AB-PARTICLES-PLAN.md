# Afterbeat particles — integration plan

Afterbeat's particle emitter (`ot = 7`, `ABEnums.cs:53`) is the one object type this converter reads
and then throws the interesting half of away. Today `ABObjectImporter.CreateTarget`
(`Import/ABObjectImporter.cs:402-419`) builds an ordinary `ShapeObject` for it and `IsHit`
(`Import/ABObjectImporter.cs:436-440`) reports `object_type_particles` as approximated — "imported as
their own shape, drawn once and never hitting the player, and emit nothing".

**That description is wrong about the source game, not just incomplete.** An Afterbeat emitter draws
no shape of its own at all: `ObjectManager.InitVisual` spawns `particlePrefab` and returns
(`ObjectManager.cs:618-680`), so the object's `(shape, shapeOption)` is resolved into the *particle
renderer's mesh* (`ObjectManager.cs:905-953`), never into a standalone quad. The current import
therefore draws a static shape the level never drew, in place of a stream of small ones. Fixing that
is the actual motive here; the emission is what makes it look right.

## Where the parameters actually live

**Not in `csp`.** `BeatmapObject` declares `ParticleSpawnRatePerSecondValueIndex` … 
`ParticleStartSpeedValueIndex` (`BeatmapObject.cs:411-425`) directly under the `csp` field
(`BeatmapObject.cs:408-409`), which reads as if indices 4-11 continue the custom-shape array. They do
not. Every accessor goes through `GetParticleSettingValue`, which reads
`events[0].keyframes[0].GetVal(index, default)` (`BeatmapObject.cs:896-903`) — **the first position
keyframe's own value array**. `csp` stays the five-float shape half (`ObjectManager.cs:872-902`,
`GetCustomParam(0..3)`), and the corpus agrees: every `csp` present is length 5, and no `ot = 7`
object carries one.

`Models/VgdObject.cs`'s `CustomShape` model is therefore already correct and needs no change; what is
missing is a reader over `e[0].k[0].ev[4..11]`.

## 1. The source model, parameter by parameter

| ev | Accessor | Default | Clamp | What it drives in Unity |
|---|---|---|---|---|
| 4 | `GetParticleSpawnRatePerSecond` `BeatmapObject.cs:812` | `0` | `max(0, x)` | `EmissionModule.rateOverTime` `ObjectManager.cs:1863-1864` |
| 5 | `GetParticleSpawnRatePerUnit` `BeatmapObject.cs:817` | `0` | `max(0, x)` | `emission.rateOverDistance` `ObjectManager.cs:1865` |
| 6 | `GetParticleWorldSpace` `BeatmapObject.cs:822` | `1` (true) | `>= 0.5` | `main.simulationSpace` Local/World `ObjectManager.cs:1090`, plus `renderer.alignment` + `main.scalingMode` `ObjectManager.cs:1155-1164` |
| 7 | `GetParticleDespawnOnEnd` `BeatmapObject.cs:827` | `0` (false) | `>= 0.5` | no module — object lifetime: `length = logicalLength + particleMaxLifetime` `ObjectManager.cs:1403`, plus a `0.02` buffer `ObjectManager.cs:460-463` |
| 8 | `GetParticleEmitterShapeType` `BeatmapObject.cs:832` | `0` Rectangle | `round(x) != 1 → Rectangle` | `shape.shapeType` Box/Circle `ObjectManager.cs:1127-1143` |
| 9 | `GetParticleEmitterArc` `BeatmapObject.cs:841` | `360` | `[0, 360]` | `shape.arc` `ObjectManager.cs:1131-1137` — **circle only** |
| 10 | `GetParticleEmitterRadiusThickness` `BeatmapObject.cs:846` | `1` | `[0, 1]` | `shape.radiusThickness` `ObjectManager.cs:1138` — **circle only** |
| 11 | `GetParticleStartSpeed` `BeatmapObject.cs:851` | `1` | `max(0, x)` | `main.startSpeed` `ObjectManager.cs:1855` |

`ParticleEmitterShapeType { Rectangle = 0, Circle = 1 }` (`BeatmapObject.cs:54-58`). The editor panel
is `EditorElement_ObjectPanel.cs:1420-1530` — its increment steps (speed `0.1`, arc `1`/`15`, radius
thickness `0.1`) are the only hint the format gives about intended granularity.

Two more, not in the array: `emission.SetBursts(empty)` — **no burst mode at all**
(`ObjectManager.cs:1866`) — and `main.loop = false` with `main.duration` set to the object's own
logical length (`ObjectManager.cs:1851-1853`).

### The keyframe tracks are re-purposed, and this is the whole feature

An emitter's four tracks do two jobs at once. Values 0/1 keep their ordinary meaning and animate the
**emitter**; values 2/3 are a second, hidden channel describing **one particle over its own life**.
The bridge is `ConfigureParticleLifetimeFromTimeline` (`ObjectManager.cs:1828-1900`).

- **Lifetime.** `T = ResolveParticleTimelineLength` = the largest keyframe time across tracks 0-3,
  floored at `0.01` (`ObjectManager.cs:1634-1651`), assigned to `main.startLifetime`
  (`ObjectManager.cs:1848`). Every particle lives exactly as long as the object's own animation, with
  no spread.
- **Track 0, position.** Values 0/1 move the emitter (`DOLocalMove`, `ObjectManager.cs:1430`/`1434`).
  Values 2/3 are built into curves with `velocityDerivative: true` and fed to
  `velocityOverLifetime.x/y`, space Local (`ObjectManager.cs:1867-1868`, `1872-1880`) — i.e. the
  authored numbers are a *position* over particle life, and the graph consumes its derivative.
- **Track 1, scale.** Values 0/1 drive `shape.scale` — **the emitter volume, not the visual**
  (`ObjectManager.cs:1459-1481`; the non-particle branch at `1484`/`1488` is what an ordinary object
  gets instead). Values 2/3, default `1`, feed `sizeOverLifetime.x/y` with `separateAxes`
  (`ObjectManager.cs:1869-1870`, `1881-1889`).
- **Track 2, rotation.** Value 0 rotates the emitter (`ObjectManager.cs:1514`/`1519`). Value 2 of the
  **first** keyframe is `main.startRotation`, degrees to radians (`ObjectManager.cs:1839`/`1854`),
  and the same value's curve derivative feeds `rotationOverLifetime.z`
  (`ObjectManager.cs:1871`, `1890-1898`).
- **Track 3, colour.** Theme slot + opacity are resampled into a `Gradient` and pushed to
  `colorOverLifetime` (`GameManager.cs:2761-2800`, called from `GameManager.cs:2660-2663`, cached by
  `ComputeParticleColorEventsSignature` `GameManager.cs:2734-2759`). The object's colour timeline is
  a **per-particle gradient**, not the emitter's colour over time.
- **Easing.** `BuildParticleTimelineCurve` (`ObjectManager.cs:1653-1826`) samples an eased segment at
  16 points (`PARTICLE_EASE_SAMPLES`, `ObjectManager.cs:118`); `Instant` segments are dropped
  entirely on the velocity form (`ObjectManager.cs:1717-1720`) and collapsed to a step otherwise.

### Everything else the dump settles

- **A circle emitter is an ELLIPSE, and its arc starts at +X going counter-clockwise.**
  `shape.radius` is never assigned (`ObjectManager.cs:1127-1143`), so it keeps the particle
  prefab's own value and `shape.scale` multiplies it per axis — their editor's own gizmo restates
  it verbatim (`EditorEmptyRendering.cs:423-425`, `radiusX = radius * scale.x`), and the same gizmo
  walks the arc as `cos/sin` from angle zero (`:444-449`), i.e. from +X counter-clockwise. That is
  the convention this format now matches: `EffectShapeCircle.Aspect` carries the second semi-axis,
  and the shipped VFX graphs were rotated so their own arc starts at +X too (it used to start at
  +Y and sweep clockwise, which turned an authored dome into a sideways half-disc).
- **Parenting is ordinary.** The prefab is spawned under `gameObjRef.ParentChainEnd`
  (`ObjectManager.cs:620`), so an emitter inherits its parent chain like any other object.
- **Colour is inherited**, through `objRef.rend` — which for the particle prefab *is* the
  `ParticleSystemRenderer` (`ObjectHelpers.cs:118-122`, `333-344`) — and then specifically through
  `ApplyThemeColorToParticleObjectRef` (`GameManager.cs:2660-2663`).
- **Determinism exists there too**: `ApplyDeterministicParticleSeed` (`ObjectManager.cs:1910-1932`)
  hashes the object id with the level seed into `ParticleSystem.randomSeed`.
- **Render bands apply**: Background layer and `AbovePlayer` sorting order are handled in the
  particle branch itself (`ObjectManager.cs:656-676`).
- **Material**: `Material_ParticleOverride`, else the shape's own prefab material, else
  `Material_Gradient` when `gt != 0`, else `Material_Default` (`ObjectManager.cs:955-983`), forced
  through `EnsureParticleCompatibleMaterial`/`FindBestParticleShader` (`ObjectManager.cs:985-1010`).
  No mesh resolved → billboard (`ObjectManager.cs:1146-1154`).

## 2. Measured usage — read this before deciding the feature is worth it

Two workshop levels plus their autosaves. Every autosave matches its `level.vgd` exactly, so the
numbers below are per level, not per file.

| Level | Objects | `ot` histogram | `csp` present | `ot = 7` |
|---|---|---|---|---|
| `3378679400` (2024-11) | 423 | `0`: 267, `5`: 91, `6`: 65 | 0 | **0** |
| `3781241588` (2026-08) | 1577 | `0`: 772, `5`: 600, `6`: 134, `7`: **71** | 541 (all length 5) | **71** |

**The older level has none at all.** That is consistent with the feature being recent — it is not
evidence that authors avoid it. In the level that does use it, 71 emitters are 4.5% of the object
count and are plainly foreground content (names: `Emitter` ×61, plus `RainEmitter`, `FloorEmitter`,
`EmitterLihgtblue`, …). None sits in a prefab, none is parented, nothing is parented *to* one, and
none carries a gradient (`gt`). All 71 use autokill type `4` (SongTime), depths 9-59, `rl = 1`
(AbovePlayer) on 26 of them.

Parameter values actually authored, out of 71:

```
ev[4]  spawn/sec        71 present   2 … 500   (100 ×11, 200 ×7, 300 ×7, 400 ×4, 500 ×2)
ev[5]  spawn/unit       69 present   0 in every single one
ev[6]  worldSpace       69 present   false ×60, true ×9   (2 absent -> default true)
ev[7]  despawnOnEnd     65 present   false ×38, true ×27  (6 absent -> default false)
ev[8]  emitterShape     40 present   Rectangle ×27, Circle ×13  (31 absent -> Rectangle)
ev[9]  arc              13 present   180 ×8, 360 ×3, 0 ×2
ev[10] radiusThickness  13 present   1 ×10, 0.9 ×2, 0.1 ×1
ev[11] startSpeed        2 present   3 ×2  (69 use the default 1)
```

Particle lifetime `T`: min `0.04 s`, median `0.84 s`, max `9.3 s`.
Hidden channels in use: velocity (track 0, vals 2/3) on **59/71**; size-over-life (track 1, vals 2/3)
on **71/71**; rotation-over-life (track 2, val 2) on **17/71**.
Keyframe counts: position 1-4, scale 1-4, rotation always 1, colour 1-4 — 63/71 animate position,
19/71 animate scale, 62/71 animate colour.
Distinct definitions: **47** by emitter parameters + particle mesh, **64** by the full set including
lifetime, size curve and colour gradient. Dedup saves roughly a tenth; determinism is the real reason
to key them (below), not the saving.

**Verdict.** Emitters are absent from one real level and load-bearing in the other. Worth doing, and
worth doing in the smallest useful increment first — the emission rate and the particle mesh alone
already replace a wrong static shape with something recognisable.

## 3. The target model

`EffectObject` (`Models/Objects/EffectObject.cs`) is a bare `RectObject` carrying an `EffectId`;
the payload is an `EffectData` (`Models/Data/EffectData.cs`) in `Level.Resources.Effects`
(`Models/Resources/LevelResources.cs:65-68`). Its halves: `Core` (`Models/Effects/
EffectObjectCore.cs`), `Forces` (`Models/Effects/EffectObjectForces.cs`), and the four independently
polymorphic sub-shapes `Shape`/`Angle`/`Scale`/`Color`.

Two runtime facts decide most of the mapping, and both were measured rather than assumed:

- **`Core.ParticleCount` is a spawn RATE, not a capacity.** The exposed `ParticleCount` parameter
  (`Art/VisualEffects/UniversalVFX_Local.vfx:3097`) has one output slot
  (`UniversalVFX_Local.vfx:3124-3159`) linked to **both** `VFXSpawnerConstantRate.Rate`
  (`UniversalVFX_Local.vfx:2106`, `2146`) and `VFXSpawnerBurst`'s count
  (`UniversalVFX_Local.vfx:2780`, `2788`); `Core.Loop` gates which spawner runs
  (`GamePlayer/Models/Effects/EffectTimeData.cs`, the comment above `GetReplayWindow`). System
  capacity is a fixed `1024` (`UniversalVFX_Local.vfx`, and it is `ParticleCount_Max`). So with `Loop = true`,
  `ParticleCount` is **particles per second** — an exact counterpart for `ev[4]`.
- **Simulation space is not authorable.** Two graphs exist (`UniversalVFX_Local.vfx`,
  `UniversalVFX_World.vfx`) and `EffectProperties.Core_IsLocal` exists
  (`GamePlayer/Models/Effects/EffectProperties.cs:66`), but `EffectObjectCore` has no `IsLocal`
  property — only the comment "For user-space it's always Local"
  (`Models/Effects/EffectObjectCore.cs:32`). `ev[6]` cannot cross.

## 4. Field-by-field mapping

### Source → target

```
ev[4]  spawnRatePerSecond   => Core.ParticleCount            (uint, clamp EffectRules.Core.ParticleCount_Max = 1024)
       (no source)          => Core.Loop = true              constant-rate spawner
T                           => Core.LifetimeBounds = (T, T)  seconds; AB has no lifetime spread
ev[7]  despawnOnEnd = false => FrameSpan extended by ToFrame(T)
                             + HasStopLocalFrame = true, StopLocalFrame = original span duration
ev[7]  despawnOnEnd = true  => span unchanged, HasStopLocalFrame = false
ev[8]  Rectangle            => EffectShapeRectangle.Size = scale track key0 (val0, val1)
ev[8]  Circle               => EffectShapeCircle
ev[9]    arc (deg)          =>   .Arc = arc * pi/180        (EffectRules.Shape.Arc_Max = 2pi)
ev[10]   radiusThickness    =>   .Thickness                 exact, both [0, 1]
       scale track key0     =>   .Radius = val0, .Aspect = val1 / val0   (the two semi-axes)
(shape, shapeOption)        => Core.ParticleShapeId          via ABShapeMap.Import (Maps/ABShapeMap.cs:295)
track1 vals 2/3 over time   => EffectScaleCurvesOverLife.CurveX / .CurveY
track2 val 2 over time      => EffectAngleCurvesOverLife.Curve    degrees => radians
track3 slot+opacity         => EffectColorGradientOverLife.Gradient
track0 vals 2/3 at t = 0    => Forces.StartVelocityMin = StartVelocityMax   (approximated, see below)
track0 vals 0/1             => EffectObject.Positions        unchanged, the emitter still moves
track2 val 0                => EffectObject.Rotations        unchanged, the emitter still rotates
```

Curve time is normalised `[0, 1]` on both sides — `BuildParticleTimelineCurve` divides by `T`
(`ObjectManager.cs:1667`, `1686-1687`) and `CurveValue` is authored over the same range — so the
conversion is a straight rewrite plus an ease translation through `Maps/ABEaseMap.cs`.

### Target fields with no source counterpart (leave at their defaults)

`Core.Render` (`true`), `Core.TextureResourceId` (`Null` — an Afterbeat particle carries no image),
`Core.ParticlePivot` (`0.5, 0.5`), `Forces.StartGravityMin`/`Max`, `Forces.StartAngularVelocityMin`/
`Max`, `Forces.OrbitalVelocity`, `Forces.OrbitalCenterOffset`, `Forces.VelocitySpeed`,
`Forces.LinearForce`, `Forces.LinearVelocity`, and every `*BySpeed` / `Random*` variant of
`Angle`/`Scale`/`Color`. Afterbeat drives none of them.

### Source parameters with no target (report as lost)

Named individually, because "particle details" is exactly the kind of summary this converter's report
exists to avoid:

1. **`ev[5]` spawn rate per unit** (`emission.rateOverDistance`). We have no distance-based emission
   at all. Costs nothing today — every one of the 69 corpus objects carrying it has it at `0` — so
   the report line should fire only on a non-zero value.
2. **`ev[6]` world space.** Not authorable (above). 9/71 use it. A world-space emitter leaves its
   particles behind as it travels; ours drags them along. Visible, and unfixable in the format.
3. **`ev[11]` start speed.** `main.startSpeed` pushes a particle along the emitter shape's normal:
   radially outward for Circle, along `+Z` for Box — which in a 2D scene is invisible, and is very
   likely why 69/71 leave it at the default and put the real motion in the velocity channel instead.
   No radial-outward force exists here. 2/71 affected.
4. **The velocity curve** (track 0, values 2/3). Only its value at `t = 0` crosses, into
   `Forces.StartVelocityMin`/`Max`; the shape of the curve after that is flattened. 59/71 affected —
   this is the largest single approximation in the whole mapping, and the one to name first in the
   report.
5. **An animated emitter volume** (track 1, values 0/1 past the first keyframe). `EffectShape*`
   fields are `IFloat`/`IVector2` values, not keyframe tracks; only the first keyframe crosses.
   19/71 affected.
6. **Theme-referenced particle colour.** `GradientValue` holds literal colours and has no `ThemeRef`
   variant, so the gradient is resolved once against `ABImportContext.ReferenceTheme` — exactly the
   fallback `Maps/ABColorMap.cs` already applies elsewhere. A theme change stops recolouring the
   particles.
7. **The gradient material** (`gt != 0` on an emitter, `ObjectManager.cs:978-981`, `1096-1110`).
   0/71 in the corpus, but legal source data.
8. **The billboard fallback** when no mesh resolves (`ObjectManager.cs:1146-1154`). Our
   `Core.ParticleShapeId` is always a real shape; `Null` draws nothing rather than a quad.
9. **`PARTICLE_LOGICAL_END_BUFFER`** — the `0.02 s` grace added to a non-despawning emitter's length
   (`ObjectManager.cs:47`, `460-463`). Sub-frame at 60 fps; drops silently and correctly.
10. ~~**Emitter-shape `Circle` vs `Box` scale semantics.**~~ **Closed — nothing is lost.** Unity's
    `ShapeModule.scale` on a Circle scales it non-uniformly, and `EffectShapeCircle` grew an
    `Aspect` to say the same thing: the horizontal extent crosses as `Radius` and the vertical one
    as a ratio of it. The source game never assigns `shape.radius` either, so the authored scale IS
    the pair of semi-axes and nothing is halved on the way — see `Maps/ABParticleMap.cs`'s own note.
    All 13 corpus circles were affected before this; the emitters that draw the cloud tops in
    `weathergirl` are `14x3`, `15x3`, `16x5`, `18x6.7`, and collapsing them to a circle of the
    larger axis is what made a flat dome read as a round arc.

## 5. Structural questions

**Does the emitter become an `EffectObject` beside the shape object, or replace it?**
**Replace.** The source draws no standalone shape (`ObjectManager.cs:618-680` returns before the
ordinary visual path), so keeping one would keep a bug. `CreateTarget`
(`Import/ABObjectImporter.cs:402-419`) grows a branch above the `ShapeObject` one, returning an
`EffectObject`. `ColliderId` disappears with it — `IsHit` already answers `false` for Particles
(`Import/ABObjectImporter.cs:436-440`), and an `EffectObject` has no collider field at all.

**What happens to the object's own shape?** It becomes `Core.ParticleShapeId`, through the same
`ABShapeMap.Import` call `CreateTarget` already makes (`Maps/ABShapeMap.cs:295`). That is the reason
the particle half needs no new geometry machinery: a synthesized shape lands in
`Level.Resources.CompositeShapes` exactly as it does for an ordinary object, and is then referenced
by an effect instead of by a `ShapeObject`.

**How does the emitter's lifetime map onto a `FrameSpan`?**
`ABTimeMap.ResolveSpan` (`Maps/ABTimeMap.cs:139`) stays the source of the span. Then:

- `despawnOnEnd = true` → span unchanged, no stop frame. The slot is released at the span's end and
  the graph is cleared, which is what Afterbeat does.
- `despawnOnEnd = false` → the span is **extended** by `ABTimeMap.ToFrame(T, framerate)` and
  `HasStopLocalFrame` / `StopLocalFrame` are set to the original duration. Emission stops where the
  object used to end, the particles already alive finish their life inside the extension. That is a
  faithful reproduction of `ObjectManager.cs:1403`, not an approximation — worth saying explicitly,
  because `EffectData.StopLocalFrame` is documented as local to the object's own start
  (`Models/Data/EffectData.cs:39-47`), which is exactly the frame of reference this needs.

Watch the interaction with parenting: `RectObject`'s child-inside-parent convention is resolved on
read, never stored (root `CLAUDE.md`, "Frames, spans and the timeline"), so an extended emitter under
a shorter parent is legal authored data and simply plays less. Nothing to enforce, nothing to report.

**Does it need a new `EffectData` per distinct parameter set, keyed like `ABShapeMap`?**
Yes, and for `ABIdMap`'s stated reason rather than for the dedup (`Maps/ABIdMap.cs:8-20`): a fresh
`Guid` per import means re-importing the same level produces different ids and every reference
between runs dangles. Add `ABIdMap.ToEffectId(string sourceId)` beside `ToShapeId`
(`Maps/ABIdMap.cs:53`) with its own `"afterbeat.effect"` tag, and feed it a **canonical signature
string** built from the eight parameters + `T` + the mesh's `ShapeId` + the serialized curves and
gradient, formatted invariantly. Measured, that collapses 71 emitters to 64 resources (47 if only the
parameters and mesh are keyed).

The alternative — one `EffectData` per emitter, keyed by the source object id — is also
deterministic and simpler, and should be recorded as rejected rather than unconsidered: it writes 71
near-identical resources, and an author who later retunes one shared effect has to retune all of
them. Names come from the source object (`n`), disambiguated by an index, since `Emitter` appears 61
times.

## 6. Export direction

`ABObjectExporter.Export` drops effect objects and says so
(`Export/ABObjectExporter.cs:58-63`, code `"effects"`). That line is accurate today and should stay
until the import side is finished — a one-way conversion is honest, a lossy round trip is not.

If it is ever built, only this subset can cross, and the report has to name the rest:
`Core.ParticleCount` → `ev[4]`; `Core.LifetimeBounds` → the object's own timeline length, **and only
when `X == Y`** (a spread has no representation); `EffectShapeRectangle`/`EffectShapeCircle` →
`ev[8..10]` plus the scale track's first keyframe; `Core.ParticleShapeId` → `(shape, shapeOption)`
through `ABShapeMap.Export` (`Maps/ABShapeMap.cs:452`); `EffectScaleCurvesOverLife` → track 1 values
2/3 sampled; `EffectAngleCurvesOverLife` → track 2 value 2; `EffectColorGradientOverLife` → track 3
theme slots via a nearest-slot search, which `Maps/ABColorMap.cs` already does in that direction.

Everything else has no representation whatsoever and must be reported by name, not as a category:
`EffectShapePoint`/`Line`/`Cone`/`Torus`, every `IEffectShapeSpread`, every `Random*` and `*BySpeed`
variant of `Angle`/`Scale`/`Color`, the whole `Forces` group, `Core.TextureResourceId`,
`Core.ParticlePivot`, `Core.Render = false`, and `Core.Loop = false` (Afterbeat has no burst — 
`ObjectManager.cs:1866` clears the burst list unconditionally).

Until then the existing `Dropped("effects", …)` message should be the only thing that changes: it
currently claims "Afterbeat has no particle effects", which will be false the moment the import
lands. Reword to say the export does not write them, not that the target format lacks them.

## 7. Staged work plan

Each stage is independently shippable and independently visible. Tests carry the SDK's three
attributes without exception (`CLAUDE.md`, "Testing"): `[Author(Metadata.Author.Vertoker)]`,
`[Category(Metadata.Category.Self)]` = `"BH.SDK"`, and exactly one difficulty.

**Stage 1 — read the parameters.** A new `Maps/ABParticleMap.cs` holding the eight indices, their
defaults and their clamps, plus a `TryRead(VgdObject) -> ABParticleSettings?` over `e[0].k[0].ev`.
Nothing in the importer changes yet. This is the piece the rest is wrong without, and it is worth
landing alone because the index-vs-`csp` mistake is the easiest one in this whole area to make.
*Tests*: defaults when the array is short, each clamp at both ends, `round(x) != 1 → Rectangle`,
`>= 0.5` truthiness on `ev[6]`/`ev[7]` — `VeryEasy`, one fixture built from the real object shape.

**Stage 2 — the minimum useful emitter.** `CreateTarget` returns an `EffectObject`; one `EffectData`
per canonical signature via a new `ABIdMap.ToEffectId`; `Core.ParticleCount` from `ev[4]`,
`Core.Loop = true`, `Core.LifetimeBounds = (T, T)`, `Core.ParticleShapeId` from `ABShapeMap.Import`,
`Shape` = Rectangle or Circle from `ev[8..10]` + the scale track's first keyframe. Colour, curves and
the velocity channel stay at defaults. The `object_type_particles` report line changes from
"emit nothing" to naming what was approximated.
*Tests*: an `ot = 7` object imports as `EffectObject` with a matching `EffectData` in
`Resources.Effects`; two identical emitters share one `EffectId` and two different ones do not;
re-importing the same document twice produces the same `EffectId` (the `ABIdMap` contract);
Rectangle vs Circle selection and the degree→radian arc. `Normal`, plus one `Hard` re-import
determinism sweep.

**Stage 3 — lifetime and the span.** `despawnOnEnd` handling: span extension by `ToFrame(T)` plus
`HasStopLocalFrame`/`StopLocalFrame`.
*Tests*: both branches of `ev[7]`; the extended span's `StopLocalFrame` equals the unextended
duration; an emitter whose extension runs past the level's own `FrameDuration` still validates (a
root object is not bounded by the level). `Normal`.

**Stage 4 — the over-life curves.** `EffectScaleCurvesOverLife` from track 1 values 2/3,
`EffectAngleCurvesOverLife` from track 2 value 2 (degrees → radians),
`EffectColorGradientOverLife` from track 3 through `ABColorMap` + `ABImportContext.ReferenceTheme`.
Ease translation through `ABEaseMap`; an `Instant` segment becomes a step, matching
`ObjectManager.cs:1717-1720`.
*Tests*: normalised curve times against `T`; a single-keyframe track produces a flat curve at the
authored value, not an empty one; the gradient's stop count matches the colour track's keyframe
count; degrees→radians on the angle curve. `Hard` for the gradient case, `Normal` for the rest.

**Stage 5 — velocity, and the honest report.** `Forces.StartVelocityMin`/`Max` from the velocity
channel's value at `t = 0`, and one report line per lost item from §4 — each with its own code, each
firing only when the source actually used the thing (a zero `ev[5]` is not a loss).
*Tests*: one per report code, asserting it fires on a using document and stays silent on a default
one; `InteropReport.IsClean` on an emitter that uses nothing lost. `Normal`.

**Stage 6 — corpus and documentation.** Extend `ABCorpusTests` to assert every `ot = 7` object in the
corpus produces an `EffectObject` with a resolvable `EffectId`, and update this folder's `README.md`:
remove particle emission from "Not imported", add the named losses, and reword the export's
`"effects"` message. `Extreme` for the corpus sweep, which is where it already sits.

## 8. Open questions

- **Does an `EffectObject`'s own `Scale` reach the emitter shape, the particles, or both?**
  `RenderEffectTransformsJob` applies the whole global transform to the VFX GameObject
  (`Assets/Code/GamePlayer/Jobs/RenderEffectTransformsJob.cs`), and the graph is local-space, but
  whether that scales spawn positions, particle size, or both is not answerable from the asset
  without running it. It decides whether stage 2 should write the emitter volume into
  `EffectShape*.Size` (the plan's choice) or into the object's `Scales` — and the latter would
  additionally make an *animated* emitter volume crossable, closing loss #5. **Test this first in
  stage 2**; it is the only decision in the plan that a measurement can still overturn.
- **`Core.ParticleCount` as a rate is confirmed for the shipped graph, not for the format.**
  `EffectData`'s own summary calls it "how many particles the system may have alive at once"
  (`Models/Effects/EffectObjectCore.cs:34-35`), which contradicts what the graph does with it. One of
  the two is wrong and the discrepancy is worth resolving before a conversion law is written on top
  of it.
- **Afterbeat's own `ev[4]` scale.** `100` particles/second is the modal value and `500` the maximum
  observed, against our `ParticleCount_Default = 10` and `_Max = 1024`. Whether a direct copy is
  visually equivalent, or whether Afterbeat's rate is throttled somewhere the dump does not show, is
  undetermined.
- **What a `0` arc means.** `GetParticleEmitterArc` clamps to `[0, 360]` and the graph nudges an
  exact `0` or `360` off the boundary and back (`ObjectManager.cs:1131-1137`) — evidently working
  around a Unity quirk. Two corpus objects author `0`. Whether that is "no emission" or "a line" is
  undetermined; `EffectRules.Shape.Arc_Min` is `0` here too, so the value crosses either way.
- **The velocity channel's units.** The authored numbers are a position over particle life and the
  graph takes their derivative (`ObjectManager.cs:1867-1868`), with
  `velocityOverLifetime.x/y = MinMaxCurve(1f, curve)` — so the multiplier is `1` and the curve's own
  units are world-units per second. Whether that matches our `StartVelocity`'s units one-to-one is
  untested.
- **Nested emitters.** No corpus emitter is parented, is a parent, or sits inside a prefab, so the
  parent-chain and prefab paths are entirely unexercised. `ABPrefabImporter` will hit them the moment
  a level uses one.
- **`main.duration` and looping.** Afterbeat sets `loop = false` with `duration` = the object's own
  length (`ObjectManager.cs:1851-1853`) yet emits continuously through `rateOverTime`; this plan maps
  that to `Core.Loop = true` + `StopLocalFrame`. Whether the two agree at the boundary — the last
  frame of the span — is untested.
