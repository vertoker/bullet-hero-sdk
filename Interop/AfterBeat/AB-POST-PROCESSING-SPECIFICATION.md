What parameter I should take (taken from PA and my research)

## Shared Effects

Bloom (BLM) (HEAVY IN ANY CASE, PHONES DON'T LIKE IT)
- Bloom Threshold - 0f (always, not a parameter)
- Bloom Intensity - Intensity (0f-10f)
- Bloom Scatter - Scatter (0f-1f)
- Bloom Color - Color
- Bloom Filter (player choose in settings: high - Gaussian, mid - Dual, low - Kawase)
- PA Integration
  - Bloom Intensity (0f-50f) => x / 5f => Intensity (0f-10f)
  - Bloom Diffusion (5f-30f) => 1f / (Mathf.Max(x, 5f) / 30f) => Scatter (0f-1f)

Chromatic Abberation (Chroma) (CHR)
- Chromatic Intensity - Intensity (0f-1f) (limited in Inspector, need to remove it)
- PA Integration
  - Chromatic Intensity (0f-8f) => Intensity (0f-1f)

Vignette (VGN)
- Color
- Center ( Vec2 (0f-1f, 0f-1f) (0.5f, 0.5f) )
- Intensity (0f-1f) (limited in Inspector, need to remove it)
- Smoothness (0.01f-1f) (limited in Inspector, need to remove it)
- Rounded (bool)
- PA Integration
  - Intensity (0f-100f) => Intensity (0f-1f)
  - Smoothness (-25f-25f) => Mathf.Max(x, 0.02f) / 2f => Smoothness (0.01f-1f)
  - Color - Color
  - Force Round - Rounded
  - Center - Center

Lens Distorion (LNS)
- Intensity (-1f - 1f)
- Multiplier (Vec2, (0f-1f, 0f-1f) )
- Center (Vec2, (0f-1f, 0f-1f) )
- Scale (0.01f - 5f) (default is 1f, be careful with it)
- PA Integration
  - Intensity (-80f - 80f) => x / 80f => Intensity (-1f - 1f)
  - Center ( Vec2 (-0.5f - 0.5f, -0.5f - 0.5f) (-0.5f - 0.5f, -0.5f - 0.5f) )
=> Mathf.Clamp01(x + 0.5f) => Center (Vec2, (0f-1f, 0f-1f) )

Film Grain (Grain) (GRN)
- Type (Then 1-2 , Medium 1-6, Large 1-2)
- Intensity (0f-1f) (limited in Inspector, need to remove it)
TODO can't interpetend size from PA

Color Curves (CCV)
- Hue Vs Hue (Hue) (0f-1f, default 0.5f) (on start)
- Sat Vs Sat (Sat) (0f-1f, default 0.5f) (on start)
- PA Integration
  - Hue (0f-360f) => Mathf.Repeat((x / 360f) + 0.5f, 1f) => Hue (0f-1f)
  - Sat - default 0.5f

Digital Glitch (DGL) (source - https://github.com/saimarei/URPGlitch)
(HEAVY IN ANY CASE, PHONES DON'T LIKE IT)
- Intensity (0f-1f)
- PA Integration
  - Glitch Intensity (0f-1f) - Intensity (0f-1f)
  - Glitch Speed - Not Implemented
  - Glitch Width - Not Implemented

Background Gradient (not post processing, highest layer and quad in it, 
custom material and shader, 2 colors)
- Intensity (0f-1f) (alpha channel)
- Rotation (0f-360f, default 180f) (in angles)
- Color A and Color B
- PA Integration
  - Gradient Intensity (0f-2f) => Clamp01(x) => Intensity (0f-1f)
  - Gradient Rotation (0f-1f, default 0.5f) => Clamp01(x) * 360f => Rotation (0f-360f)
  - Color A and Color B => Color A and Color B

Shake Effect - TODO find normal realization for effect (not post processing)

Theme - TODO made more extended realization (not post processing)

## Unique Effects

Lift Gamma Gain (LGG)
- Lift (bool)
- Lift Color (only RGB)
- Lift Alpha (0f-2f, default 1f)
- Gamma (bool)
- Gamma Color (only RGB)
- Gamma Alpha (0f-2f, default 1f)
- Gain (bool)
- Gain Color (only RGB)
- Gain Alpha (0f-2f, default 1f)

Shadows Midtones Highlights (SMH)
- Shadows (bool)
- Shadows Color (only RGB)
- Shadows Alpha (0f-2f, default 1f)
- Midtones (bool)
- Midtones Color (only RGB)
- Midtones Alpha (0f-2f, default 1f)
- Highlights (bool)
- Highlights Color (only RGB)
- Highlights Alpha (0f-2f, default 1f)
(here's graph like in Post Processing menu)
- Shadow Limits (Vec2, always 0 <= start <= end) (default 0, 0.3)
- Highlight Limits (Vec2, always 0 <= start <= end) (default 0.55, 1)

White Balance (WBL)
- Temperature (-100f - 100f, default 0f)
- Tint (-100f - 100f, default 0f)

Motion Blur (MBR) (HEAVY IN ANY CASE, PHONES DON'T LIKE IT)
- Quality (client settings variable, he set it himself)
- Intensity (0f-1f, default 0f)
- Clamp (0.2f, predefined)

Analog Glitch (AGL) (source - https://github.com/saimarei/URPGlitch)
(HEAVY IN ANY CASE, PHONES DON'T LIKE IT)
- Scan Line Jitter (0f-1f)
- Vertical Jump (0f-1f)
- Horizontal Shake (0f-1f)
- Color Drift (0f-1f)
