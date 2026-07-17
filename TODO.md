Serialization
- verify SerializationService stability/correctness across all keyframe/value/effect type combinations (round-trip tests + real saved level files), especially after the IRequiresDefaultSerializer refactor
- add binary (BSON) serialization via Newtonsoft, alongside the existing JSON path
- definitely sort out model versioning (CompatibilityService's per-type Version -> Type mapping only covers the top-level SaveData<T> types right now - nested/polymorphic model versioning across breaking model changes is unresolved)

Features after alpha release
- add difficulty metadata (watch OSU for more info)
- create trigger/event system (like PA, but more like GD)
  - first of all - see guide for gd editor
  - https://youtube.com/playlist?list=PLD3NfTCEL4uV7zI5QvMLTDs7qActqY5JB
  - or read PDF files
  - https://www.robtopgames.com/files/GDEditor-RU.pdf
  - https://www.robtopgames.com/files/GDRating-RU.pdf
  - https://www.robtopgames.com/files/GDLeaderboards-RU.pdf
- add sounds in timing, with custom audio support (tutorial level in JSaB)
- add JPG, PNG, SVG importing to textures and anywhere usage
- add tags as metadata for every object in level, only for editor
- add free ingame skins (and maybe add custom)

Features after successful full release
- add doxygen for all code
- console window with TUI, maybe fully supported SLI
- 2 game mode: camera follow to player using lerp, direction is always up
