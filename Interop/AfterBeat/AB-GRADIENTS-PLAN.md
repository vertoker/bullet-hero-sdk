# Afterbeat object gradients — integration plan

Afterbeat lets an object's colour be a two-colour gradient instead of a flat fill: `gt`/`gr`/`gs` on
the object, plus a second theme slot in every colour keyframe. Today this converter reads exactly one
of those five numbers. `ABObjectImporter.BuildShapeColor` (`Import/ABObjectImporter.cs:830-861`) turns
a linear gradient into a `ColorHorizontalKey` and reports its rotation and scale as having "no
equivalent here"; a radial one is dropped to its start colour. `gr` and `gs` are never read anywhere
in this folder, and `ABObjectExporter.ExportColors` (`Export/ABObjectExporter.cs:303-341`) never
writes them back, so they serialize at their defaults `0` and `1`.

**The claim that rotation and scale have no equivalent is true of the current model and is not a
reason to drop them.** A converted level is supposed to survive `Afterbeat → ours → Afterbeat`
unchanged; five numbers that go in and do not come out make that impossible for one object in every
eight. This plan is about storing them, and only secondarily about drawing them — those turn out to
be separable, which is what makes the work shippable in halves.

## 1. Measured usage — read this before deciding the feature is worth it

Two workshop levels plus their autosaves. Every autosave matches its own `level.vgd` exactly on every
count below, so the numbers are per level, not per file.

| Level | Objects | `gt != 0` | Share |
|---|---|---|---|
| `3378679400` (2024-11) | 423 | **1** | 0.24% |
| `3781241588` (2026-08) | 1577 | **212** | **13.4%** |

Split by type, `3781241588`:

| `gt` | Meaning | Count |
|---|---|---|
| 1 | Linear | 175 |
| 2 | Linear inverted | 11 |
| 3 | Radial | 16 |
| 4 | Radial inverted | 10 |

186 linear, **26 radial** — the radial ones are what the importer flattens away entirely today.

The older level's single gradient object carries neither `gr` nor `gs` (both at their defaults), which
is the case an importer must not mistake for "absent means zero" — `gs` defaults to `1`.

**Parameter values actually authored**, out of 212:

```
gr   68 distinct values, all integers, 0 … 360
     0 x47   90 x19   270 x17   180 x10   360 x9   202 x9   223 x8   314 x6
     the remaining ~60 values occur 1-4 times each (9, 34, 59, 84, 127, 199, 236, 275, 357, ...)
gs   40 distinct values, 0.25 … 8.32
     1 x91   0.25 x43   0.75 x11   0.92 x5   0.67 x4   1.17 x4   2.6 x4   5.21 x4
```

**116 distinct `(gt, gr, gs)` triples across 212 objects.** This is not a handful of presets being
reused; the author sweeps the parameter space, which is the strongest single argument against any
scheme that only preserves a few canonical cases.

Non-default parameters per type:

| `gt` | n | `gr != 0` | `gs != 1` |
|---|---|---|---|
| 1 | 175 | 147 | 98 |
| 2 | 11 | 11 | 10 |
| 3 | 16 | 6 | 7 |
| 4 | 10 | 1 | 6 |

Six radial objects carry a non-zero `gr` even though the editor hides the rotation control for radial
(`EditorElement_ShapeSettings.cs:316-317`) — almost certainly leftover values from when those objects
were linear. `SetupMaterial` pushes `_Angle` for radial anyway (`ObjectManager.cs:569-572`), so
whether it does anything is undetermined (see §9).

**Gradient objects are transformed objects.** 168/212 carry a non-zero rotation keyframe (146/175,
8/11, 10/16, 4/10 by type) and 24 of those animate it; 199/212 have a non-uniform scale, 44 animate
it. Four in five are rotated, which makes the object-space question in §2.5 the highest-risk unknown
in this whole area rather than a detail.

**The two colour slots**, over the 307 colour keyframes belonging to gradient objects:

```
213  three values, ev[0] != ev[2]   a real two-colour gradient
 71  three values, ev[0] == ev[2]   degenerate, renders flat
 23  two values only                end colour falls back to theme index 0 (DataManager.cs:2723-2730)
185  fully opaque (ev[1] >= 100)    both ends import as Color4ThemeRef (Maps/ABColorMap.cs:99-100)
```

52/212 objects animate colour across more than one keyframe (key-count histogram 1:160, 2:12, 3:37,
4:3). 137/212 are opaque on *every* key, so their colours currently stay theme-referenced end to end —
a property §4's rejected option would destroy.

Also measured, because it decides whether the exporter may write `ev[2]` unconditionally: **109
non-gradient objects carry a dead third colour value** (123 keyframes). The game ignores it
(`ObjectManager.cs:1524`), so writing it always is harmless.

**What survives a round trip today**, `3781241588`:

| Case | Objects | Preserved |
|---|---|---|
| linear, `gr` in {0, 180}, `gs >= 1` | ~43 | colours and axis |
| linear at `gr` 90/270 | 36 | colours; **axis wrong** — exported as horizontal |
| linear at any other `gr` | 107 | colours only |
| radial | 26 | start colour only |
| `gs != 1` (any type) | 121 | never |
| inverted (`gt` 2 or 4) | 21 | colours, possibly swapped; `_Forwards` gone |

**Verdict.** One real level uses the feature once and the other uses it on 13% of its objects — the
same shape as the particle emitters, and the same reading: recent, load-bearing where used. Worth
doing, and worth splitting so the round-trip half lands before the rendering half.

## 2. What the source game actually does

### 2.1 The three fields

`BeatmapObject.cs:427-437`:

```
[JsonProperty("gt")] [DefaultValue(0)] public byte  gradientType;
[JsonProperty("gr")] [DefaultValue(0)] public float gradientRotation;
[JsonProperty("gs")] [DefaultValue(1)] public float gradientScale = 1f;
```

- `gradientType` is a **byte with five values**, not the flag its name suggests.
- `gradientRotation` is a **float**. `Models/VgdObject.cs:62` types it `int` — see §3.
- `gradientScale` **defaults to 1**; an absent `gs` is 1, not 0. `Models/VgdObject.cs:65` already has
  this right.
- `ShouldSerializegradientRotation`/`ShouldSerializegradientScale` (`BeatmapObject.cs:913-921`) return
  `gradientType != 0`, so the game writes them only for gradient objects — but the 2024 corpus omits
  them even with `gt = 1`, so an importer must supply the defaults rather than assume presence.
- Helpers: `HasGradient => gradientType != 0` (`BeatmapObject.cs:545`), `HasLinearGradient => gt == 1
  || gt == 2` (`BeatmapObject.cs:548-556`).
- `ParallaxObject.cs:98-149` declares the same three fields for background objects with the same
  semantics. Out of scope for `ABObjectImporter`, but `Import/ABParallaxImporter.cs` inherits every
  conclusion in this document.

### 2.2 What the four types render

`ObjectManager.SetupMaterial` (`ObjectManager.cs:545-582`) is the whole of it:

```
case 0:      material = Material_Default
case 1, 2:   material = Material_Gradient,  _GRADIENTTYPE_LINEAR on,  _GRADIENTTYPE_RADIAL off
case 3, 4:   material = Material_Gradient,  _GRADIENTTYPE_RADIAL on,  _GRADIENTTYPE_LINEAR off
if gt > 0:   SetFloat("_Angle", gr);  SetFloat("_Scale", gs)
             SetInt("_Forwards", (gt == 2 || gt == 4) ? 1 : 0)
```

So the five types are two shader keywords times one `_Forwards` int, and **"inverted" is a shader
parameter, not a colour swap**. Whether `_Forwards = 1` is equivalent to exchanging the two colours is
**undetermined**: it would be for a symmetric, unsaturated ramp, but not necessarily once `_Scale`
saturates the ramp or for the radial form. The current importer's `InvertedLinear →
ColorHorizontalKey(end, start)` (`Import/ABObjectImporter.cs:847-849`) is therefore a guess, not a
proven identity.

The same three properties are pushed onto a particle renderer's material
(`ObjectManager.cs:1098-1108`), and `ResolveParticleRendererMaterial` falls back to
`Material_Gradient` when `gt != 0` (`ObjectManager.cs:955-982`) — the intersection with
`AB-PARTICLES-PLAN.md`'s loss #7.

### 2.3 Units and ranges, from the editor

`EditorElement_ShapeSettings.cs:324-340`:

- rotation: `CompLib.InitSliderWithResetAndInput(..., 0f, 360f, ..., "gradient rotation", "f0", 1f,
  1f, 360f)` — **degrees, range [0, 360]**, displayed with no decimals, step 1, reset value 360.
- scale: `(..., 0.25f, 10f, ..., "gradient scale", "f2", 0.1f, 0.1f, 1f)` — **range [0.25, 10]**, two
  decimals, step 0.1, reset value 1.
- the **rotation control is shown only for `HasLinearGradient`** (`:316-317`); the scale control is
  shown for every gradient type (`:318-319`).

The `"f0"` format is why every `gr` in the corpus is an integer. It is a display format, not a
constraint on the field, which is float.

### 2.4 The colour keyframes

`events[3]` values are `[colorIndex, opacity, gradientEndColorIndex]`. In `ObjectManager.cs:1523-1631`:

- `bool flag3 = beatmapObject.gradientType != 0;` (`:1524`) gates **every** read of `GetVal(2)` —
  confirmed: the end colour exists only when there is a gradient.
- `bool flag2` (`:1529-1536`) is true iff *some* keyframe on the track has a value at index 1; when
  false, `sequence.Opacity` is forced to 100 (`:1588`, `:1627`).
- The start pair (`LastColor`/`NewColor`) and the end pair (`LastColorExtra`/`NewColorExtra`) are
  driven by the **same** `ColorValue` t (`:1556-1620`) — the two ends interpolate in lockstep.
- Opacity does interpolate between keyframes (`num9 → num10`, `:1621-1626`).

`GameManager.cs:2634-2645` applies the result:

```
gt == 0 : _BaseColor  = fadeColor(Resolve(Last, New, t), Opacity / 100)
gt != 0 : _BaseColor  = Resolve(Last, New, t)
          _ExtraColor = Resolve(LastExtra, NewExtra, t)
          _Alpha      = Opacity / 100
```

(`ColorID = "_BaseColor"`, `EndColorID = "_ExtraColor"`, `AlphaColorID = "_Alpha"`,
`GameManager.cs:2523-2525`.)

**The end colour has no opacity of its own — one `_Alpha` covers both ends.** The existing comment at
`Maps/ABColorMap.cs:270-273` ("Afterbeat carries ONE opacity for the pair") is correct and must stay.

A keyframe missing its third value resolves to theme index 0: `GetVal(int, float _default = 0f)`
(`DataManager.cs:2723-2730`). `VgdKeyframe.GetValue` (`Models/VgdObject.cs:268-269`) returns `0f`
identically, so the converter already matches the game here.

### 2.5 The two things the dump cannot settle

**The ramp formula.** `AB_Dump` is 506 `.cs` files and no `.shader`/`.hlsl`/`.cginc`; the gradient
shader ships compiled inside the installed game. What `_Angle` is measured from, its handedness, and
whether `_Scale` is a ramp *length* or a ramp *frequency* are **undetermined**. The property names
and the `[0, 360]` / `[0.25, 10]` ranges are consistent with "direction in degrees" and "extent
multiplier", and nothing more can be claimed.

**Object space or world space.** **Undetermined.** The evidence points hard at object space — the
material is per-object on the object's own `MeshRenderer` (`ObjectManager.cs:554`, `562`,
`ApplyGradientMaterial` `:584-595`), and exposing a *gradient* rotation separately from the object's
own rotation track only makes sense if the gradient is anchored to the object — but with no shader
there is no proof. With 168/212 gradient objects rotated, getting this backwards would visibly break
four out of five of them.

Both are settled by one manual comparison in the source editor, described in §9.

## 3. Current defects, independent of any redesign

These are wrong today and would be worth fixing even if the design in §5 were rejected outright.

1. **`Models/VgdObject.cs:62` types `GradientRotation` as `int`**; `BeatmapObject.cs:433` is `float`.
   Nothing in the measured corpus is fractional (the editor slider is `"f0"`), so no level has been
   corrupted yet — but any level written by another tool, or by a future editor with a finer step,
   loses its fraction silently on import and again on export.
2. **Every linear gradient imports as `ColorHorizontalKey` regardless of `gr`**
   (`Import/ABObjectImporter.cs:840-850`). At `gr = 90` or `270` the ramp runs vertically, so
   **36 objects arrive with their axis rotated by 90 degrees**. `ColorVerticalKey` exists and is
   already handled on export (`Maps/ABColorMap.cs:248-253`); using it for the two vertical angles is a
   strict improvement available immediately.
3. **Radial gradients are dropped to a flat start colour** (`Import/ABObjectImporter.cs:852-857`) —
   26 objects. Correct as a *rendering* fallback (see §6.4), wrong as a *storage* decision, because
   the drop is what makes the round trip lossy.
4. **The exporter never writes `gr`/`gs`** and infers `gt = Linear` from "some keyframe had two
   different indices" (`Export/ABObjectExporter.cs:316-327`). Even a `ColorVerticalKey`, which it
   detects and reports (`Maps/ABColorMap.cs:248-253`), goes back out as a horizontal gradient.

## 4. The three options

### Option 1 — per-corner colours only

Bake the gradient into four corner colours at import (`Color4X4Key`), recover `(type, rotation,
scale)` from those corners at export.

**When a corner fill is pixel-exact.** Our shader interpolates the four corners bilinearly over the
object's local unit box (`Assets/Code/Shaders/Unlit2D_Opaque.shader:156`, `163-165`; transparent
variant `:180-182`). A bilinear field is `a + b*u + c*v + d*u*v`, and the corner samples of an
*affine* field satisfy `BL + TR = BR + TL`, i.e. `d = 0` — so bilinear reconstruction reproduces any
affine function exactly. A linear ramp is affine wherever its clamp is inactive, and the clamp is
inactive over the whole box iff

```
s >= |cos θ| + |sin θ|          (right-hand side ranges over [1, sqrt(2)])
```

Always true at `s >= sqrt(2)`; true at `s >= 1` for axis-aligned angles. Below that the real ramp has
saturated flat bands that no bilinear field has. Corpus: `gs == 1` on 78/186 linear objects, 43 of
which are also axis-aligned; **43 objects sit at `gs = 0.25`**, deeply saturated. A radial ramp is
rotationally symmetric and is not bilinear at any parameters.

**Why it fails as a round-trip mechanism — two independent reasons.**

1. **The map is not invertible.** Recovering `(angle, scale, start, end)` from four RGBA corners is
   underdetermined even in the easy case: for an unsaturated ramp there is a one-parameter family of
   `(scale, start, end)` giving identical corners — lengthen the ramp and pull the endpoint colours
   further apart, and nothing at the corners changes. For a saturated or radial ramp not even the
   direction survives. Four corner colours are exactly the information we already have; they are not
   enough to reconstruct what produced them.
2. **Theme references die.** Afterbeat stores a theme *index* per end, and the import keeps that as
   `Color4ThemeRef` whenever the keyframe is opaque (`Maps/ABColorMap.cs:99-100`) — that is 137/212
   gradient objects, opaque on every key. A rotated ramp's corner colours are intermediate *blends* of
   two theme colours, and no `Color4ThemeRef` expresses a blend, so all four corners would have to be
   baked to literals. The objects stop following a theme change, and the export degrades from an index
   passthrough into a nearest-colour search. This kills the option even where the geometry works.

The cleanly invertible subset is `gt` in {1, 2}, `gr` in {0, 180} (Horizontal) or {90, 270}
(Vertical), `gs >= 1`: **43 of 212 objects, 20%**. Worth implementing anyway as defect fix #2 above,
but it is a floor, not a design.

### Option 2 — carry the source parameters in an existing extension point

There is nothing legitimate to carry them in. Listed by how plausible each sounds, so nobody proposes
one of them again:

- **`RectObject`/`ShapeObject` have no free-form field.** `Models/Objects/RectObject.cs:32-114` is
  ObjectId / ParentObjectId / Name / Active / Span / Layer plus seven keyframe lists;
  `Models/Objects/ShapeObject.cs:37-69` adds ShapeId / ColliderId / ShaderType / TextureResourceId /
  Colors / UVs. No property bag, no tag set, nothing untyped.
- **A `Name` convention** (`"Ring #gt1gr90gs0.66"`) — an **abuse**. `Name` is author-visible in the
  hierarchy, author-editable, and validated by nothing. A level whose artwork survives only until
  somebody renames an object is not a mechanism.
- **`PrefabObject.Modifications`** — an **abuse and inapplicable**. Its key is a template-inner
  `ObjectId` plus a `[JsonProperty]` field *path*, re-applied over a fresh template copy after every
  materialize/resync. It can only address fields that already exist on the model, and only inside a
  prefab placement. It cannot hold a field the model does not have.
- **`LevelHints`** — the most tempting and the worst. `Models/Level.cs:49-58` states the membership
  test outright: hints are "the only aggregate here that holds nothing an author wrote" and "may be
  dropped without changing what the level is". Gradient parameters change what the level looks like,
  so a consumer legitimately dropping hints would silently delete the artwork.
- **A level-authored resource** keyed by `ObjectId` in `Level.Resources` breaks no stated invariant
  and would work mechanically — but it is a private side table only this converter understands. Our
  own editor would show, and let the author edit, a two-corner colour whose real appearance is decided
  somewhere else, and that shadow model would need its own resolution, validation and undo tracking in
  parallel with the real one. This is the thing the root `CLAUDE.md`'s "engine, not a one-off game"
  rule exists to prevent.

The only non-abusive form of option 2 is option 3 with worse ergonomics.

### Option 3 — a real format feature (recommended)

A fifth `IColor4X4Key` variant holding the two end colours plus the ramp's own shape. This is exactly
the case the SDK's documented recipe covers, and it separates the two halves of the problem: **the
round trip is a format concern, not a rendering one.** Once the parameters are stored, `Afterbeat →
ours → Afterbeat` is exact whether or not the renderer draws a real ramp — which is what lets stage 1
ship without touching a shader.

## 5. The target model

`IColor4X4Key` (`Models/Interfaces/Keyframes/IColor4X4Key.cs`) is a polymorphic keyframe family
discriminated by `Color4X4KeyType` (`Value = 0`, `Horizontal = 1`, `Vertical = 2`,
`BariCentrical = 3`) and resolved by `Serialization/Converters/CustomTypes/Color4X4KeyConverter.cs`.
The SDK's `CLAUDE.md` states the shape of adding one: "new enum case + concrete class + a case in the
matching `JsonConverterCustomType<T,TType>` subclass's `GetType`/`GetCustomType` switch. No
attribute-based auto-discovery anywhere in this system". Nothing else is registered.

```
Color4X4KeyType.Gradient = 4

ColorGradientKey : Keyframe, IColor4X4Key, IModel<ColorGradientKey>
    IColor4        Color4Start      // [RuleNotNull(typeof(Color4Value))]
    IColor4        Color4End        // [RuleNotNull(typeof(Color4Value))]
    GradientShape  Shape            // Linear = 0, Radial = 1
    bool           Inverted
    float          Angle            // degrees, CCW from +X, in the object's local unit box
    float          Scale            // ramp extent in box units
```

`Shape` plus `Inverted` rather than one four-valued enum, because that is literally what the source
stores — a keyword pair times `_Forwards` (`ObjectManager.cs:547-580`) — and it keeps `Inverted`
orthogonal to the shape instead of doubling every future variant.

Rule ranges deliberately **wider** than the source sliders (`gr` 0-360, `gs` 0.25-10): `Angle` in
`[0, 360]`, `Scale` in `[0.01, 100]`. An import must never clamp. `Angle` is **not** normalised
mod 360 — `gr = 360` occurs 9 times in the corpus and has to come back out as 360, not 0, for the
round trip to be byte-exact.

Per-keyframe rather than three new fields on `ShapeObject`, because the ramp's shape is meaningless
without the two colours it applies to, and this codebase's habit is to make illegal states
unrepresentable (`FrameSpan` is the standing example). The price is one reconciliation rule on export,
stated in §6.2.

## 6. Conversion

### 6.1 Import: Afterbeat → ours, exact

Per colour keyframe of an object with `gt != 0`; every keyframe of one object gets the same shape
parameters, since the source stores them per object.

```
Shape    = (gt == 1 || gt == 2) ? Linear : Radial
Inverted = (gt == 2 || gt == 4)
Angle    = gr                                       // absent -> 0
Scale    = gs                                       // absent -> 1
Start    = ABColorMap.Import(ev[0], opacity, ...)
End      = ABColorMap.Import(ev[2], opacity, ...)   // ev[2] absent -> 0, matching DataManager.cs:2723
```

`opacity` stays the single `OpacityOf(key)` applied to both ends, matching `_Alpha`
(`GameManager.cs:2643`). `gt == 0` keeps the current `Color4Key(start)` path, and a non-gradient
object's dead `ev[2]` stays ignored exactly as the game ignores it.

Both gradient report codes disappear: `gradient_linear` (`Import/ABObjectImporter.cs:842-844`) and
`gradient_radial` (`:854-856`). The only colour report left on this path is the existing
`color_opacity_literal`.

### 6.2 Export: ours → Afterbeat, exact for anything that came from Afterbeat

```
gt = Shape == Linear ? (Inverted ? 2 : 1) : (Inverted ? 4 : 3)
gr = Angle
gs = Scale
ev = [ Export(Start).Index, Export(Start).Opacity * 100, Export(End).Index ]
```

Two reconciliation rules, because `gt`/`gr`/`gs` are per-object there and per-keyframe here:

1. The shape parameters come from the **lowest-frame** gradient key on the track. If any other
   gradient key disagrees, report `Approximated("gradient_animated", ...)` — an animated ramp is
   something this format can express and Afterbeat cannot.
2. A track mixing gradient and non-gradient keys exports the non-gradient ones as
   `(index, opacity, index)` — a degenerate gradient whose ends agree, which renders flat under any
   ramp. Mixed tracks therefore stay visually lossless.

`ColorHorizontalKey` and `ColorVerticalKey` keep exporting their two colours, but must now also write
`gr = 0` / `gr = 90` and `gs = 1` explicitly instead of leaving the axis to the default. That alone
closes defect #2 in the export direction.

### 6.3 What is exact and what is not

Exact, in both directions, for every one of the 116 corpus triples: `gt`, `gr`, `gs`, both theme
indices, and the opacity — because they are stored rather than derived. The round trip does not depend
on the renderer agreeing with Afterbeat about what the numbers mean.

Approximate, and unchanged from today: a semi-transparent end resolves to a literal colour rather than
a theme reference (`Maps/ABColorMap.cs:102-107`) — 122 of 307 gradient colour keys; and a literal
colour exports through a nearest-colour search rather than an index passthrough.

### 6.4 Rendering, stage 1 — a corner fill that changes no pixels

`Color4X4KeyState` (`Assets/Code/Core/Models/KeyStates/Color4X4KeyState.cs:73-115`) already fans every
variant out into four corner colours, and everything downstream — `FrameMath.GetGlobalColor4X4`
(`Assets/Code/GamePlayer/Utils/FrameMath.cs:692-722`), the render jobs, both shaders, the shader
resolver — consumes only those four. The new case evaluates the ramp at the four corners of the local
unit box `uv` in `[0,1]^2`:

```
θ = radians(Angle),  d = (cos θ, sin θ),  s = Scale
Linear :  k(uv) = clamp(0.5 + dot(d, uv - 0.5) / s, 0, 1)
Radial :  k(uv) = clamp(2 * length(uv - 0.5) / s, 0, 1)
colour  = lerp(Start, End, Inverted ? 1 - k : k)
corners : (0,0) -> BL,  (1,0) -> BR,  (0,1) -> TL,  (1,1) -> TR
```

- **Pixel-exact** — indistinguishable from a true per-pixel ramp — iff
  `s >= |cos θ| + |sin θ|`, per §4. Always at `s >= sqrt(2)`; at `s >= 1` for axis-aligned angles.
  43 corpus objects are in the exact set immediately, 78 more sit adjacent to it.
- **Approximate** below that: the saturated flat bands become part of the slope, so the ramp renders
  too soft. Never a wrong set of colours, only a wrong distribution. The 43 objects at `gs = 0.25`
  are the visible cases.
- **Radial cannot be a corner fill at all**, so stage 1 renders it flat at the centre colour
  `k(0.5, 0.5) = 0` → `Start` (or `End` when inverted) — byte for byte today's behaviour for those
  26 objects, while the round trip becomes exact anyway. That asymmetry is the point of the staging.
- **Theme references must be resolved before blending, not after.** A naive corner fill produces
  literal colours and silently unhooks 137 objects from theme changes. `Color4X4KeyState` already
  receives the theme matrix and already handles `ColorType.ThemeRef` per corner, so resolve both ends
  against it and blend the resolved colours — exactly what `GameManager.cs:2641-2644` does.

### 6.5 Rendering, stage 2 — the real ramp

Keep the four corner slots and add **one** instanced `float4 _GradientParams` =
`(shapeCode, cos θ / s, sin θ / s, invertedFlag)`, with `_ColorBL` carrying the start colour and
`_ColorBR` the end when `shapeCode != 0`. Evaluate `k` in the **fragment** stage: radial is
non-linear, and a vertex-stage evaluation would band badly across the coarse baked shape meshes (the
current shader does its bilinear blend in the vertex stage, `Unlit2D_Opaque.shader:163-165`, which is
correct only because a bilinear field is exactly reconstructible there).

**A dynamic branch on `shapeCode`, not a `shader_feature` keyword.** The root `CLAUDE.md` states that
level content draws through exactly two materials, opaque and transparent; a keyword variant would
multiply that per gradient type. Cost: one extra instanced `float4` per shape and one fragment
branch, no new material.

### 6.6 Rotation and colour animation

**Object rotation.** Our `shapeUV = positionOS.xy + 0.5` (`Unlit2D_Opaque.shader:156`) is object
space, so a rotated object rotates its gradient with it at both stages, and `Angle` composes
additively with the object's own rotation. Afterbeat is *implied* to do the same (§2.5) but that is
undetermined, and 168/212 gradient objects are rotated.

**Colour animation across keyframes.** `FrameMath.GetGlobalColor4X4`
(`Assets/Code/GamePlayer/Utils/FrameMath.cs:713-722`) resolves each keyframe's theme references and
then lerps corner-wise — the same model as Afterbeat's single `ColorValue` t driving both `Last/New`
and `LastExtra/NewExtra` (`ObjectManager.cs:1556-1620`). Stage 1 changes nothing here. At stage 2 the
`Angle`/`Scale` would blend too, which Afterbeat cannot express, which is why export rule 1 exists.

**Opacity.** Afterbeat interpolates one opacity (`ObjectManager.cs:1621-1626`) and applies it to both
ends; ours is per-corner RGBA. Export keeps the start's alpha (`Maps/ABColorMap.cs:270-273`, already
correct).

## 7. Staged work plan

Each stage is independently shippable. Stage 1 makes the round trip exact and changes no pixels;
stage 2 changes pixels and no data. Tests carry the SDK's three attributes without exception
(root `CLAUDE.md`, "Testing"): `[Author(Metadata.Author.Vertoker)]`,
`[Category(Metadata.Category.Self)]` = `"BH.SDK"` for SDK tests / `"BH.Core"` for the state test, and
exactly one difficulty.

**Stage 0 — the defects, on their own.** `Models/VgdObject.cs:62` `int → float`; import
`gr` in {90, 270} as `ColorVerticalKey`; export `gr`/`gs` for the Horizontal and Vertical variants.
Worth landing first because it is small, it is correct under any design, and it fixes 36 objects.
*Tests*: a float `gr` survives a deserialize/serialize cycle; `gr = 90` imports vertical and exports
back as 90; `gr = 0` stays horizontal. `Easy`.

**Stage 1 — the format.** `Color4X4KeyType.Gradient`, `GradientShape`, `ColorGradientKey`, the
converter case, the `Names.cs` constants, the importer and exporter maths of §6.1-6.2, and the corner
fill of §6.4 in `Color4X4KeyState`. Renders exactly as today; round-trips exactly.
*Tests*: every one of the 116 corpus triples survives `import → export` with `gt`/`gr`/`gs` and both
indices identical (`Hard`); `gs` absent reads as 1 and `gr` absent as 0 (`VeryEasy`); a keyframe with
only two values takes end index 0 (`VeryEasy`); `Angle = 360` does not normalise to 0 (`VeryEasy`);
the corner fill is pixel-exact at `s >= |cos θ| + |sin θ|` and the four corners match a directly
evaluated affine ramp (`Normal`); a track whose keys disagree on shape parameters reports
`gradient_animated` and exports the lowest-frame key's values (`Normal`); a theme-referenced gradient
still follows a theme change after the corner fill (`Normal`).

**Stage 2 — the ramp.** `_GradientParams` and the fragment-stage evaluation of §6.5 in both shaders,
plus the editor's variant picker. Changes pixels, changes no serialized data, and can be reverted
independently of stage 1.
*Tests*: not unit-testable at the shader level; covered by the manual check of §9 plus a
`Services.GameEditor` test that the inspector round-trips the four new fields through
`InspectorKeyColorView`'s build/emit pair (`Normal`).

**Stage 3 — parallax and documentation.** `Import/ABParallaxImporter.cs` gets the same treatment
(`ParallaxObject.cs:98-149` carries the identical three fields), and this folder's `README.md` loses
its gradient caveats.
*Tests*: extend the corpus sweep to assert no gradient object anywhere reports `gradient_*`
(`Extreme`, where the corpus sweep already sits).

## 8. Edit surface

SDK:

1. `Assets/Plugins/BulletHeroSDK/Models/Enums/Keyframes/Color4X4KeyType.cs` — `Gradient = 4`.
2. `Assets/Plugins/BulletHeroSDK/Models/Enums/Keyframes/GradientShape.cs` — new two-value enum.
3. `Assets/Plugins/BulletHeroSDK/Models/Keyframes/Primitives/ColorGradientKey.cs` — new class,
   mirroring `ColorHorizontalKey.cs` exactly: `[RuleContainer]`, `[RuleNotNull(typeof(Color4Value))]`
   on both colours, `GetModelType`, the `Clone`/`Copy`/`ICopyable<T>.Copy` triple, and the
   hand-written `Equals`. The SDK's `CLAUDE.md` flags `obj is T value && Equals(value)` as the single
   easiest place in this codebase to paste the wrong type name.
4. `Assets/Plugins/BulletHeroSDK/Serialization/Converters/CustomTypes/Color4X4KeyConverter.cs` — one
   case in `GetType`.
5. `Assets/Plugins/BulletHeroSDK/Models/Names.cs` — short keys for start / end / shape / inverted /
   angle / scale. Constants, never literals.
6. `Assets/Plugins/BulletHeroSDK/Interop/AfterBeat/Models/VgdObject.cs:62` — `int → float`.
7. `Assets/Plugins/BulletHeroSDK/Interop/AfterBeat/Import/ABObjectImporter.cs:830-861` —
   `BuildShapeColor`.
8. `Assets/Plugins/BulletHeroSDK/Interop/AfterBeat/Maps/ABColorMap.cs:239-274` — `ExportKey`, plus
   carrying the shape parameters out beside `ExportedColorKey`.
9. `Assets/Plugins/BulletHeroSDK/Interop/AfterBeat/Export/ABObjectExporter.cs:303-341` —
   `ExportColors` writes `gt`/`gr`/`gs`.

Unity project:

10. `Assets/Code/Core/Models/KeyStates/Color4X4KeyState.cs:73` — the new case, i.e. the corner fill of
    §6.4. The only place stage 1 needs maths.
11. `Assets/Code/Services/GameEditor/InspectorKey/Tracks/InspectorKeyColorView.cs:246-275` and
    `:331-334` — the variant picker and the four extra fields.
12. `Assets/Code/Core/Utils/ShapeShaderResolver.cs` — reads corners through the *state*, not the key
    types, so `Auto` opaque/transparent resolution should follow automatically once (10) lands.
    Verify rather than assume.
13. Stage 2 only: `Assets/Code/Shaders/Unlit2D_Opaque.shader`,
    `Assets/Code/Shaders/Unlit2D_Transparent.shader`,
    `Assets/Code/GamePlayer/Jobs/RenderShapeInstancesChunkJob.cs:77-81`, and the instanced-property
    plumbing beside it.

No `.asmdef` is added or changed, so Rule 5 is not in play.

## 9. Open questions

- **The manual check that settles §2.5, and it should happen before stage 2.** Author one Afterbeat
  level containing linear gradients at `gr` = 0 / 45 / 90 crossed with `gs` = 0.25 / 1 / 4, each on an
  unrotated object and on a copy rotated 45 degrees, plus one radial at `gs` = 0.5 / 1 / 2.
  Screenshot each in the source game, import, and compare. Three constants fall out: the angle's zero
  and its handedness; whether `gs` is a ramp length or a ramp frequency (the `gs = 0.25` group looks
  completely different under the two readings); and whether the gradient is object space — the rotated
  copies answer that on their own. Until then §6.4's formula is *our* convention, not a match. The
  round trip is exact either way, which is why this gates stage 2 and not stage 1.
- **Is `_Forwards` equivalent to swapping the two colours?** Undetermined (§2.2). If it is, the
  `Inverted` flag is redundant for storage and could be normalised away on import; if it is not, the
  current importer's colour swap is wrong. The same screenshots answer it.
- **Does a radial object's stale `gr` do anything?** `SetupMaterial` pushes `_Angle` for every
  `gt > 0` (`ObjectManager.cs:569-572`) while the editor hides the control for radial
  (`EditorElement_ShapeSettings.cs:316-317`). Six corpus objects carry one. If it is ignored, the
  round trip must still preserve it verbatim — the design does — but the renderer may ignore it too.
- **What `gs` means at its extremes.** The slider stops at `0.25` and `10`, and the corpus reaches
  `0.25` and `8.32`. Whether those are shader-meaningful bounds or just editor taste is undetermined,
  which is why the model's own range is wider.
- **Does the gradient survive on a text object?** `TextObject` colours are a flat `Color4Key`
  (`Import/ABObjectImporter.cs:815-818`), and no corpus gradient object is a text shape (`s = 4`:
  0 of 212). Legal source data all the same, and currently it would silently take the start colour.
- **Parallax objects.** `ParallaxObject.cs:98-149` has the same three fields and
  `Import/ABParallaxImporter.cs` ignores them exactly as the object importer does today. Neither
  corpus level has parallax objects at all (0 in both), so the path is entirely unexercised.
