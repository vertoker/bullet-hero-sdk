using System;
using System.Runtime.CompilerServices;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class ColorValue : IColor, ICopyable<ColorValue>
    {
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelR)]
        public float R { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelG)]
        public float G { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelB)]
        public float B { get; set; }
        
        [RuleInRange(ValueRules.MinColor, ValueRules.MaxColor)]
        [JsonProperty(Names.ChannelA)]
        public float A { get; set; }

        public ColorValue()
        {
            R = 1f;
            G = 1f;
            B = 1f;
            A = 1f;
        }
        public ColorValue(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public ColorType GetModelType() => ColorType.Value;

        IColor ICopyable<IColor>.Copy() => new ColorValue(R, G, B, A);
        public ColorValue Copy() => new(R, G, B, A);
        
        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9411765f, 0.9725491f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue aliceBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9411765f, 0.9725491f, 1f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9803922f, 0.9215687f, 0.8431373f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue antiqueWhite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9803922f, 0.9215687f, 0.8431373f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4980392f, 1f, 0.8313726f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue aquamarine
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4980392f, 1f, 0.8313726f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9411765f, 1f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue azure
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.9411765f, 1f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9607844f, 0.9607844f, 0.8627452f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue beige
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9607844f, 0.9607844f, 272f * (float)Math.E / 857f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.8941177f, 0.7686275f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue bisque
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.8941177f, 0.7686275f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue black
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.0f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9215687f, 0.8039216f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue blanchedAlmond
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9215687f, 0.8039216f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue blue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.0f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5411765f, 0.1686275f, 0.8862746f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue blueViolet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5411765f, 0.1686275f, 0.8862746f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6470588f, 0.1647059f, 0.1647059f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue brown
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6470588f, 0.1647059f, 0.1647059f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8705883f, 0.7215686f, 0.5294118f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue burlywood
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8705883f, 0.7215686f, 0.5294118f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.372549f, 0.6196079f, 0.627451f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue cadetBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.372549f, 0.6196079f, 0.627451f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4980392f, 1f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue chartreuse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.4980392f, 1f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8235295f, 0.4117647f, 0.1176471f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue chocolate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8235295f, 0.4117647f, 0.1176471f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0f, 0f, 0f)
        ///           </para>
        /// </summary>
        public static ColorValue clear
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.4980392f, 0.3137255f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue coral
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.4980392f, 0.3137255f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.3921569f, 0.5843138f, 0.9294118f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue cornflowerBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.3921569f, 0.5843138f, 0.9294118f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9725491f, 0.8627452f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue cornsilk
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9725491f, 272f * (float)Math.E / 857f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8627452f, 0.07843138f, 0.2352941f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue crimson
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(272f * (float)Math.E / 857f, 0.07843138f, 0.2352941f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 1f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue cyan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 1f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0f, 0.5450981f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.0f, 0.5450981f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0.5450981f, 0.5450981f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkCyan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.0f, 0.5450981f, 0.5450981f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7215686f, 0.5254902f, 0.04313726f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkGoldenRod
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.7215686f, 0.5254902f, 0.04313726f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6627451f, 0.6627451f, 0.6627451f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkGray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6627451f, 0.6627451f, 0.6627451f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0.3921569f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.3921569f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7411765f, 0.7176471f, 0.4196079f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkKhaki
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.7411765f, 0.7176471f, 0.4196079f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5450981f, 0f, 0.5450981f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkMagenta
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5450981f, 0.0f, 0.5450981f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.3333333f, 0.4196079f, 0.1843137f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkOliveGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.3333333f, 0.4196079f, 0.1843137f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.5490196f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkOrange
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 0.5490196f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6f, 0.1960784f, 0.8000001f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkOrchid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6f, 0.1960784f, 0.8000001f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5450981f, 0f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.5450981f, 0.0f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9137256f, 0.5882353f, 0.4784314f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkSalmon
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9137256f, 0.5882353f, 0.4784314f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5607843f, 0.7372549f, 0.5607843f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkSeaGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5607843f, 0.7372549f, 0.5607843f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.282353f, 0.2392157f, 0.5450981f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkSlateBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.282353f, 0.2392157f, 0.5450981f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1843137f, 0.3098039f, 0.3098039f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkSlateGray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1843137f, 0.3098039f, 0.3098039f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0.8078432f, 0.8196079f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkTurquoise
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.0f, 0.8078432f, 0.8196079f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5803922f, 0f, 0.8274511f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue darkViolet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5803922f, 0.0f, 0.8274511f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.07843138f, 0.5764706f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue deepPink
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.07843138f, 0.5764706f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0.7490196f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue deepSkyBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.7490196f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4117647f, 0.4117647f, 0.4117647f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue dimGray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4117647f, 0.4117647f, 0.4117647f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1176471f, 0.5647059f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue dodgerBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1176471f, 0.5647059f, 1f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6980392f, 0.1333333f, 0.1333333f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue firebrick
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6980392f, 0.1333333f, 0.1333333f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9803922f, 0.9411765f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue floralWhite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9803922f, 0.9411765f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1333333f, 0.5450981f, 0.1333333f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue forestGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1333333f, 0.5450981f, 0.1333333f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8627452f, 0.8627452f, 0.8627452f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gainsboro
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new ColorValue(272f * (float)Math.E / 857f, 272f * (float)Math.E / 857f, 272f * (float)Math.E / 857f,
                    1f);
            }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9725491f, 0.9725491f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue ghostWhite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9725491f, 0.9725491f, 1f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.8431373f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gold
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 0.8431373f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.854902f, 0.6470588f, 0.1254902f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue goldenRod
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.854902f, 0.6470588f, 0.1254902f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5f, 0.5f, 0.5f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray => gray5;

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5f, 0.5f, 0.5f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue grey => gray5;

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1f, 0.1f, 0.1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.1f, 0.1f, 0.1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.2f, 0.2f, 0.2f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.2f, 0.2f, 0.2f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.3f, 0.3f, 0.3f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.3f, 0.3f, 0.3f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4f, 0.4f, 0.4f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.4f, 0.4f, 0.4f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5f, 0.5f, 0.5f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.5f, 0.5f, 0.5f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6f, 0.6f, 0.6f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.6f, 0.6f, 0.6f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7f, 0.7f, 0.7f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray7
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.7f, 0.7f, 0.7f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8f, 0.8f, 0.8f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.8f, 0.8f, 0.8f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9f, 0.9f, 0.9f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue gray9
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.9f, 0.9f, 0.9f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 1f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue green
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 1f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6784314f, 1f, 0.1843137f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue greenYellow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6784314f, 1f, 0.1843137f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9411765f, 1f, 0.9411765f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue honeydew
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9411765f, 1f, 0.9411765f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.4117647f, 0.7058824f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue hotPink
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.4117647f, 0.7058824f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8039216f, 0.3607843f, 0.3607843f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue indianRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8039216f, 0.3607843f, 0.3607843f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.2941177f, 0f, 0.509804f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue indigo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.2941177f, 0.0f, 0.509804f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 1f, 0.9411765f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue ivory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 1f, 0.9411765f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9411765f, 0.9019608f, 0.5490196f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue khaki
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9411765f, 0.9019608f, 0.5490196f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9019608f, 0.9019608f, 0.9803922f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lavender
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9019608f, 0.9019608f, 0.9803922f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9411765f, 0.9607844f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lavenderBlush
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9411765f, 0.9607844f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4862745f, 0.9882354f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lawnGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4862745f, 0.9882354f, 0.0f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9803922f, 0.8039216f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lemonChiffon
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9803922f, 0.8039216f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6784314f, 0.8470589f, 0.9019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6784314f, 0.8470589f, 0.9019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9411765f, 0.5019608f, 0.5019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightCoral
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9411765f, 0.5019608f, 0.5019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8784314f, 1f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightCyan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.8784314f, 1f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9333334f, 0.8666667f, 0.509804f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightGoldenRod
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9333334f, 0.8666667f, 0.509804f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9803922f, 0.9803922f, 0.8235295f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightGoldenRodYellow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9803922f, 0.9803922f, 0.8235295f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8274511f, 0.8274511f, 0.8274511f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightGray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8274511f, 0.8274511f, 0.8274511f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5647059f, 0.9333334f, 0.5647059f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5647059f, 0.9333334f, 0.5647059f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.7137255f, 0.7568628f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightPink
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.7137255f, 0.7568628f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.627451f, 0.4784314f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightSalmon
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.627451f, 0.4784314f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1254902f, 0.6980392f, 0.6666667f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightSeaGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1254902f, 0.6980392f, 0.6666667f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5294118f, 0.8078432f, 0.9803922f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightSkyBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5294118f, 0.8078432f, 0.9803922f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5176471f, 0.4392157f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightSlateBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5176471f, 0.4392157f, 1f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4666667f, 0.5333334f, 0.6f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightSlateGray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4666667f, 0.5333334f, 0.6f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6901961f, 0.7686275f, 0.8705883f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightSteelBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6901961f, 0.7686275f, 0.8705883f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 1f, 0.8784314f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue lightYellow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 1f, 0.8784314f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1960784f, 0.8039216f, 0.1960784f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue limeGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1960784f, 0.8039216f, 0.1960784f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9803922f, 0.9411765f, 0.9019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue linen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9803922f, 0.9411765f, 0.9019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue magenta
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 0.0f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6901961f, 0.1882353f, 0.3764706f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue maroon
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6901961f, 0.1882353f, 0.3764706f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4f, 0.8039216f, 0.6666667f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumAquamarine
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4f, 0.8039216f, 0.6666667f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0f, 0.8039216f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.0f, 0.8039216f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7294118f, 0.3333333f, 0.8274511f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumOrchid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.7294118f, 0.3333333f, 0.8274511f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5764706f, 0.4392157f, 0.8588236f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumPurple
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5764706f, 0.4392157f, 0.8588236f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.2352941f, 0.7019608f, 0.4431373f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumSeaGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.2352941f, 0.7019608f, 0.4431373f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.482353f, 0.4078432f, 0.9333334f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumSlateBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.482353f, 0.4078432f, 0.9333334f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0.9803922f, 0.6039216f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumSpringGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.0f, 0.9803922f, 0.6039216f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.282353f, 0.8196079f, 0.8000001f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumTurquoise
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.282353f, 0.8196079f, 0.8000001f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7803922f, 0.08235294f, 0.5215687f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mediumVioletRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.7803922f, 0.08235294f, 0.5215687f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.09803922f, 0.09803922f, 0.4392157f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue midnightBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.09803922f, 0.09803922f, 0.4392157f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9607844f, 1f, 0.9803922f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mintCream
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9607844f, 1f, 0.9803922f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.8941177f, 0.882353f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue mistyRose
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.8941177f, 0.882353f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.8941177f, 0.7098039f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue moccasin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.8941177f, 0.7098039f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.8705883f, 0.6784314f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue navajoWhite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.8705883f, 0.6784314f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0f, 0.5019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue navyBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 0.0f, 0.5019608f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9921569f, 0.9607844f, 0.9019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue oldLace
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9921569f, 0.9607844f, 0.9019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5019608f, 0.5019608f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue olive
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5019608f, 0.5019608f, 0.0f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4196079f, 0.5568628f, 0.1372549f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue oliveDrab
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4196079f, 0.5568628f, 0.1372549f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.6470588f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue orange
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 0.6470588f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.2705882f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue orangeRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 0.2705882f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.854902f, 0.4392157f, 0.8392158f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue orchid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.854902f, 0.4392157f, 0.8392158f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9333334f, 0.909804f, 0.6666667f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue paleGoldenRod
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9333334f, 0.909804f, 0.6666667f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5960785f, 0.9843138f, 0.5960785f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue paleGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5960785f, 0.9843138f, 0.5960785f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6862745f, 0.9333334f, 0.9333334f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue paleTurquoise
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6862745f, 0.9333334f, 0.9333334f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8588236f, 0.4392157f, 0.5764706f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue paleVioletRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8588236f, 0.4392157f, 0.5764706f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.937255f, 0.8352942f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue papayaWhip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.937255f, 0.8352942f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.854902f, 0.7254902f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue peachPuff
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.854902f, 0.7254902f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8039216f, 0.5215687f, 0.2470588f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue peru
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8039216f, 0.5215687f, 0.2470588f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.7529413f, 0.7960785f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue pink
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.7529413f, 0.7960785f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8666667f, 0.627451f, 0.8666667f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue plum
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8666667f, 0.627451f, 0.8666667f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6901961f, 0.8784314f, 0.9019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue powderBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6901961f, 0.8784314f, 0.9019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.627451f, 0.1254902f, 0.9411765f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue purple
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.627451f, 0.1254902f, 0.9411765f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4f, 0.2f, 0.6f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue rebeccaPurple
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.4f, 0.2f, 0.6f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0f, 0f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue red
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 0.0f, 0.0f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7372549f, 0.5607843f, 0.5607843f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue rosyBrown
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.7372549f, 0.5607843f, 0.5607843f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.254902f, 0.4117647f, 0.882353f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue royalBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.254902f, 0.4117647f, 0.882353f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5450981f, 0.2705882f, 0.07450981f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue saddleBrown
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5450981f, 0.2705882f, 0.07450981f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9803922f, 0.5019608f, 0.4470589f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue salmon
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9803922f, 0.5019608f, 0.4470589f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9568628f, 0.6431373f, 0.3764706f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue sandyBrown
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9568628f, 0.6431373f, 0.3764706f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1803922f, 0.5450981f, 0.3411765f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue seaGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1803922f, 0.5450981f, 0.3411765f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9607844f, 0.9333334f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue seashell
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9607844f, 0.9333334f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.627451f, 0.3215686f, 0.1764706f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue sienna
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.627451f, 0.3215686f, 0.1764706f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.7529413f, 0.7529413f, 0.7529413f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue silver
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.7529413f, 0.7529413f, 0.7529413f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5294118f, 0.8078432f, 0.9215687f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue skyBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5294118f, 0.8078432f, 0.9215687f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4156863f, 0.3529412f, 0.8039216f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue slateBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4156863f, 0.3529412f, 0.8039216f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.4392157f, 0.5019608f, 0.5647059f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue slateGray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.4392157f, 0.5019608f, 0.5647059f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9803922f, 0.9803922f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue snow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9803922f, 0.9803922f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8627452f, 0.1921569f, 0.1960784f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue softRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(272f * (float)Math.E / 857f, 0.1921569f, 0.1960784f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.1882353f, 0.682353f, 0.7490196f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue softBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.1882353f, 0.682353f, 0.7490196f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.5490196f, 0.7882354f, 0.1411765f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue softGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.5490196f, 0.7882354f, 0.1411765f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.9333334f, 0.5490196f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue softYellow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.9333334f, 0.5490196f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 1f, 0.4980392f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue springGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(0.0f, 1f, 0.4980392f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.2745098f, 0.509804f, 0.7058824f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue steelBlue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.2745098f, 0.509804f, 0.7058824f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8235295f, 0.7058824f, 0.5490196f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue tan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8235295f, 0.7058824f, 0.5490196f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0f, 0.5019608f, 0.5019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue teal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.0f, 0.5019608f, 0.5019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8470589f, 0.7490196f, 0.8470589f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue thistle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8470589f, 0.7490196f, 0.8470589f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.3882353f, 0.2784314f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue tomato
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.3882353f, 0.2784314f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.2509804f, 0.8784314f, 0.8156863f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue turquoise
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.2509804f, 0.8784314f, 0.8156863f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9333334f, 0.509804f, 0.9333334f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue violet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9333334f, 0.509804f, 0.9333334f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.8156863f, 0.1254902f, 0.5647059f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue violetRed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.8156863f, 0.1254902f, 0.5647059f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9607844f, 0.8705883f, 0.7019608f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue wheat
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9607844f, 0.8705883f, 0.7019608f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 1f, 1f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue white
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ColorValue(1f, 1f, 1f, 1f);
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.9607844f, 0.9607844f, 0.9607844f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue whiteSmoke
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.9607844f, 0.9607844f, 0.9607844f, 1f); }
        }

        /// <summary>
        ///   <para>ColorValue Preset of RGBA(1f, 0.92f, 0.016f, 1f).</para>
        /// </summary>
        public static ColorValue yellow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.92156863f, 0.015686275f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(0.6039216f, 0.8039216f, 0.1960784f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue yellowGreen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(0.6039216f, 0.8039216f, 0.1960784f, 1f); }
        }

        /// <summary>
        ///   <para>
        ///               ColorValue Preset of RGBA(1f, 0.92f, 0.016f, 1f)
        ///           </para>
        /// </summary>
        public static ColorValue yellowNice
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorValue(1f, 0.92156863f, 0.015686275f, 1f); }
        }
    }
}