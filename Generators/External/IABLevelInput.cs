namespace BH.SDK.Generators.External
{
    // The same bargain the rest of External/ makes, for a different reason. The others exist
    // because the SDK has no decoder; this one exists because the SDK reads no FILES. A level
    // folder is a directory somebody picked, on a platform this library knows nothing about, so
    // the host opens it and hands over what it found.
    //
    // Text rather than paths, deliberately: it keeps this side free of IO, and it lets the same
    // generator run against a folder, a zip entry, or a paste from a web page without knowing which.

    /// <summary> A generator that builds a level out of an Afterbeat level folder the host read. </summary>
    public interface IABLevelInput
    {
        /// <summary> Contents of level.vgd. Empty means the host found nothing, and the generator
        /// must then produce nothing rather than an empty level. </summary>
        string LevelJson { get; set; }

        /// <summary> Contents of the metadata document, if the folder had one. </summary>
        string MetaJson { get; set; }

        /// <summary> File name of the song inside the folder, so the generator can reference it as
        /// a level resource. The host is what copies the file itself. </summary>
        string AudioFileName { get; set; }

        /// <summary> Where the folder was, for the report to name. Never opened by the SDK. </summary>
        string SourceFolder { get; set; }

        /// <summary> How long that song is, in seconds. A level in this foreign format has no
        /// length of its own - its timeline IS its song - so this is what the converted level's
        /// length is taken from. Zero when the host could not measure it, and only then is the
        /// length derived from the level's own content instead. </summary>
        float AudioLengthSeconds { get; set; }
    }
}
