# Versions

Historical snapshots and migrators for the model versioning system. See `VERSION-UPDATE.md` at
the SDK root for the full design/rationale. Short version: version numbers live only on aggregate
roots (types carrying `[DataVersion]`), not on every class - most models never appear here at all.

## Folder convention: generation-first, not domain-first

Organize by **format generation** (`V{major}_{minor}/`), not by domain. A single content update
usually touches several related domains at once (e.g. `Level`, `GameLevel`, `GameEvents` together),
so keeping everything that was true "as of generation X" in one folder makes a release's full
diff visible in one place, instead of scattering it across parallel per-domain trees.

```
Versions/
  V0_0/                          # one folder per historical generation, named after its own version
    LevelV0_0.cs                 # frozen snapshot of Level's own shape at this generation
    LevelSettingsV0_0.cs         # ...only for domains that actually differ from current at this generation
    GameLevelV0_0.cs
    GameEventsV0_0.cs
    LevelResourcesV0_0.cs
    AudioLevelV0_0.cs            # intentionally has NO [DataVersion] - see note below
    Migrations/
      LevelV0_0ToV1_0.cs          # LevelV0_0 -> Level (current)
      LevelSettingsV0_0ToV1_0.cs  # LevelSettingsV0_0 -> LevelSettings (current)
      GameLevelV0_0ToV1_0.cs
      GameEventsV0_0ToV1_0.cs
      LevelResourcesV0_0ToV1_0.cs
  V0_1/                          # next generation, whenever it exists, same shape
    ...
```

Rules that keep this from turning into a mess as more generations pile up:

- Only create a snapshot class for a domain if **that domain's own shape** actually changed at
  this generation. If `Theme` never changes, it never gets a `ThemeVX_Y.cs` file, ever.
- A domain's own frozen snapshot classes ALSO carry `[DataVersion(Domain, major, minor)]` with
  their OWN (old) numbers - `VersionedTypeRegistry` needs this to resolve version -> Type. Classes
  that exist only as another snapshot's implementation detail (not independently versioned
  themselves) don't need the attribute.
- **Important, easy to get wrong**: a container's property holding a nested `[DataVersion]`-tagged
  value must be typed using that domain's **current** class, not the old `VX_Y`-suffixed one -
  `VersionedEnvelopeConverter` always resolves the nested envelope's own version tag and upgrades
  it to the domain's current type before returning, regardless of what generation the *outer*
  container belongs to. So e.g. `LevelV0_0.Settings` is typed `LevelSettings` (current), never
  `LevelSettingsV0_0`, even though `LevelSettingsV0_0` still exists and is still registered.
- A domain that had **no** independent envelope yet at some generation (i.e. it was introduced
  later) gets a snapshot class with **no** `[DataVersion]` attribute at all, exactly like
  `AudioLevelV0_0` here - Newtonsoft then deserializes it as a plain nested object with no envelope
  unwrapping, and the *containing* domain's own migrator is responsible for constructing the
  current type from those raw fields by hand.
- Migrators for a generation live in that generation's own `Migrations/` subfolder, one class per
  domain, always named `<Domain>V{fromMajor}_{fromMinor}ToV{toMajor}_{toMinor}.cs` (e.g.
  `LevelV0_0ToV1_0.cs`) - spelling out both ends in the name even for a single-step chain, so it
  stays unambiguous once a domain accumulates more than one historical generation.
- Frozen snapshot classes never need the `IModel<T>` boilerplate (`Copy`/`Clone`/`Equals`/
  `GetHashCode`/`Reset`) that live models have - they're transient deserialization targets, not
  domain objects. Plain properties + `[JsonProperty]` (using literal strings, not the shared
  `Names.Xxx` constants, since those track *current* naming) is enough.
