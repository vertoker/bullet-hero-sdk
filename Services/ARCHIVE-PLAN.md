# ARCHIVE-PLAN.md — level compression & archiving

Design notes for a not-yet-written `ArchiveService`, sibling of `CryptographyService` /
`SerializationService`. Written before implementation on purpose: the format choice here is a
*permanent* one (player data longevity is one of the stated reasons this SDK is open — see the root
`CLAUDE.md`), so the reasoning is recorded rather than re-derived later.

## What it has to do

Two jobs, one service:

1. **Compress** a level's files — the three serialization roots (`level`, `metadata`, `settings`) plus
   whatever resources the level folder carries (audio, textures, fonts).
2. **Archive selectively** — pack/unpack an arbitrary *subset* of a level folder, not always the whole
   thing. Sharing a level without its 40 MB source track, exporting metadata only for a listing page,
   re-packing one replaced texture.

Constraints: runs identically on PC, mobile (Android/iOS, IL2CPP/AOT), and server (.NET 8+).
The core `BulletHeroSDK` asmdef is `noEngineReferences: true` — this must stay true, so nothing here
may touch `UnityEngine`.

## Decision: ZIP via `System.IO.Compression.ZipArchive` (BCL only, zero dependencies)

The Unity project is on **`apiCompatibilityLevel: 6` = .NET Standard 2.1**, which ships
`ZipArchive`, `ZipFile`, `DeflateStream`, `GZipStream` and `CompressionLevel.SmallestSize`. Same
types exist unchanged on .NET 8+, so the server path is literally the same code.

Why ZIP specifically:

- **Selective work is native to the format.** An archive is a list of entries with a central
  directory; a single file can be added, replaced, read or dropped without rewriting the rest. That
  is requirement 2, for free. A `.tar.gz` structurally cannot do this — it is a stream format, and
  reading the last file means decompressing everything before it.
- **Random access.** `metadata.json` can be read out of a 50 MB archive without touching the audio.
  Level browsers, listing pages, and import previews all need exactly this.
- **It matches the "a level is just a folder of files" promise.** The root `CLAUDE.md` treats level
  portability as a feature: zip it, send it, unzip, play. Making the archive a real ZIP means the
  fallback path is the OS file manager, with no tooling from us. Same reasoning as `.osz`.
  Own extension (`.bhlevel`) over a plain ZIP payload — recognizable, still openable by anything.
- **Zero dependency.** Nothing to vendor, nothing to break on a new IL2CPP backend, nothing that can
  fail an app-store binary review over a native blob.

## Per-entry compression policy — not optional

The bulk of a level's bytes is `.ogg`/`.mp3`/`.png`, all already compressed. Running deflate over
them buys ~0–2% and costs real time and battery on mobile. So the policy is per-extension, not global:

| Entry | Level |
|---|---|
| `.json`, `.bson`, `.txt`, `.csv` | `CompressionLevel.SmallestSize` |
| `.ogg`, `.mp3`, `.wav`, `.png`, `.jpg`, `.webp`, `.mp4` | `CompressionLevel.NoCompression` (stored) |
| unknown | `CompressionLevel.Optimal` |

`.wav` is listed as *stored* on purpose despite being uncompressed PCM: it deflates poorly (~10-20%)
for a lot of CPU, and a level shipping raw wav is already making a size choice. Revisit only with
measurements.

Note `.bson` is stored-adjacent, not incompressible — BSON is a binary *encoding*, not a compression,
and still deflates well. It stays in the compressed row.

## Rejected alternatives

- **`BrotliStream` from the BCL** — the API is in netstandard2.1, but the Unity Mono/IL2CPP
  implementation is not dependable; expect `PlatformNotSupportedException` or a linker strip on
  mobile. If brotli is ever wanted, only via a managed NuGet port.
- **SharpZipLib** (already present in the Unity project as an Addressables dependency) — only buys
  zip64 >4 GB, archive encryption, tar/bzip2, and streaming into a non-seekable sink. None of those
  are needed, and the SDK must not gain a Unity-package dependency.
- **7z / RAR as the container** — better ratio on the text half, but the payload is dominated by
  already-compressed media where the ratio advantage vanishes, and it costs the "open it in any file
  manager" property plus a third-party writer.
- **A custom container format** — the one thing this project should not invent. Every hour spent on
  it is an hour not spent on the game, and it breaks the portability promise outright.

## Open extension: stronger compression for the text half

`level.json` on a dense level is the one place where deflate's ratio actually matters (thousands of
keyframes; deflate gets ~8-10x, zstd-19 gets ~15-20x and decompresses several times faster). If it
ever becomes worth it:

- **ZstdSharp.Port** (NuGet, netstandard2.0, fully managed, AOT/IL2CPP-safe). Stored as a
  `level.json.zst` entry inside the ZIP with `NoCompression`, so the container stays a plain ZIP and
  an outside tool can still list it.
- **K4os.Compression.LZ4** (NuGet, managed) — the other axis: speed, not size. For editor autosaves,
  undo snapshots, network payloads. Not for on-disk distribution.

NuGetForUnity is already installed in the consuming project (`com.github-glitchenzo.nugetforunity`),
so pulling either in is mechanical. **Hard criterion for any such package: fully managed, no native
`.so`/`.dylib`/`.a`.** A native binary is an immediate problem on iOS/Android/consoles.

Do not take this step speculatively — measure a real dense level against the BCL baseline first.

## Sketch

```
namespace BH.SDK.Services

sealed class ArchivePolicy            // which files, at which compression
    Func<string, bool> Filter          // relative path -> include
    Func<string, CompressionLevel> Level

class ArchiveService
    Pack(sourceDir, dstStream, ArchivePolicy, IProgress<float>, CancellationToken)
    Unpack(srcStream, dstDir, ArchivePolicy, IProgress<float>, CancellationToken)
    IReadOnlyList<ArchiveEntryInfo> List(srcStream)     // no extraction
    byte[] ReadEntry(srcStream, string relativePath)    // metadata preview
    void ReplaceEntry(archiveStream, relativePath, byte[])
```

Streams, not paths, in the signatures — the server has no level folder, and it keeps
`FileLoaderService` (Unity side) the only thing that knows about the filesystem. Path-taking
convenience overloads can wrap them.

Composes with `CryptographyService`: encrypt the packed stream, or an individual entry, without
either service knowing about the other.

## Rules the implementation must not miss

- **Zip Slip.** Every entry's destination must be verified to resolve *inside* the target directory
  after `Path.GetFullPath`. An entry named `../../../autoexec` is trivially craftable, and levels are
  user-generated content downloaded from strangers. This is the single most important line of code
  in the whole service.
- **Zip bombs.** Cap total uncompressed size and entry count during unpack; abort past the limit.
  Same threat model — a hostile 1 KB file must not be able to fill a phone's storage.
- **Entry paths are `/`-separated, always.** Never emit `Path.DirectorySeparatorChar` into an entry
  name, or Windows-packed archives break everywhere else.
- **Reproducibility.** `ZipArchiveEntry.LastWriteTime` defaults to now, so packing the same folder
  twice yields different bytes and defeats any checksum/dedup upstream. Pin it to a fixed epoch
  unless real timestamps are explicitly wanted.
- **Threading.** Compression is CPU-bound and must not run on the main thread on mobile. The Unity
  consumer wraps calls in `UniTask.RunOnThreadPool`; the SDK itself stays synchronous and just honors
  the `CancellationToken`/`IProgress` it is handed (no UniTask dependency in a `noEngineReferences`
  assembly). WebGL, if it is ever a target, has no threads — it needs chunked, per-frame driving,
  which is a consumer-side concern.

## Testing (`Tests/`, per the three-attribute rule)

- extension→`CompressionLevel` policy mapping — `VeryEasy`
- selective pack: filter excludes audio, archive contains exactly the expected entry set — `Easy`
- `ReadEntry` on `metadata.json` without full extraction — `Easy`
- Zip Slip: a crafted `../` entry must throw, and must write nothing — `Normal`
- round trip: pack → unpack → byte-identical tree, over a `MockData`-built level folder — `Hard`
