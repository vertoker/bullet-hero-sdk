using BH.SDK.Models.Enums.Resources;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Resources;

namespace BH.SDK.Generators.External
{
    // GeneratorRequirements.ExternalAnalysis says "some of my parameters come from the host". These
    // interfaces say WHICH ones, in a way a host can act on: it checks whether a parameters object
    // implements one, and if so offers the matching control (a file picker, a "detect beats" button)
    // and writes the result back before Run.
    //
    // The alternative - a host matching parameter fields by name - would make renaming a field a
    // silent breakage across an assembly boundary. An interface makes it a compile error.
    //
    // The SDK deliberately implements none of the analysis itself: it has no audio decoder, no FFT
    // and no image loader, and adding any of those would drag a platform dependency into a library
    // whose whole point is not having one.

    /// <summary> A generator that needs an audio file chosen and measured by the host. </summary>
    public interface IAudioFileInput
    {
        /// <summary> Where the clip lives, in whatever form UriType says. </summary>
        string AudioPath { get; set; }

        /// <summary> How the path should be interpreted when the level loads. </summary>
        ResourceUriType UriType { get; set; }

        /// <summary> Clip length in seconds. The SDK cannot decode audio, so this is the host's to
        /// fill; zero means "not measured yet" and the generator falls back to its own default. </summary>
        float DurationSeconds { get; set; }
    }

    /// <summary> A generator that draws an existing track's waveform, so it needs that waveform
    /// sampled by the host (Unity side: Timelines/Utils/AudioWaveformCache). </summary>
    public interface IWaveformInput
    {
        /// <summary> Which of the level's audio tracks to visualize. </summary>
        AudioResourceId Source { get; set; }

        /// <summary> Normalized peaks, one per bar, in the range [0,1]. Empty means "not sampled
        /// yet" - the generator then produces nothing rather than guessing a shape. </summary>
        float[] Peaks { get; set; }
    }

    /// <summary> A generator driven by beat positions the host detected. </summary>
    public interface IBeatFramesInput
    {
        /// <summary> Frames the beats land on. Empty means "not detected yet". </summary>
        int[] BeatFrames { get; set; }
    }

    // The odd one out, and deliberately not part of ExternalAnalysis: a generator can read the
    // framerate off GeneratorContext.Settings whenever it runs, so nothing about the RUN needs this.
    // It exists so a FORM can show the number next to the target the author is typing, which is the
    // only place a "what am I changing it from" reading belongs. A generator implementing it must
    // still take its own math from the context, never from this field - a host that never filled it
    // in has to produce a correct run, not a run against a zero.

    /// <summary> A generator whose form wants the current framerate on display beside its own. </summary>
    public interface ICurrentFramerateInput
    {
        /// <summary> The level's framerate as it is right now, mirrored in for display. </summary>
        int CurrentFramerate { get; set; }
    }

    /// <summary> A generator that turns an image into level content, so it needs the image decoded
    /// by the host (Unity side: PixelTexture from a Texture2D, see UnityExtensions). </summary>
    public interface IPixelTextureInput
    {
        /// <summary> Which of the level's texture resources the pixels came from - kept so the host
        /// can re-decode after a reimport. </summary>
        TextureResourceId Source { get; set; }

        /// <summary> The decoded image. Null means "not decoded yet". </summary>
        PixelTexture Texture { get; set; }
    }
}
