namespace BHSDK.Models.Enum
{
    public enum ObjectPropertyPath : ushort
    {
        None = 0,
        
        // ---------------------------------------------------------------------------------------------
        // Object (0)
        // ---------------------------------------------------------------------------------------------
        
        Object_Name = 1,
        Object_Visible = 2,
        Object_StartFrame = 3,
        Object_EndFrame = 4,
        Object_Layer = 5,
        Object_Pivot = 6,
        
        Object_Pos = 21,
        Object_Pos_Index = 22,
        Object_Pos_Index_Frame = 23,
        Object_Pos_Index_Ease = 24,
        Object_Pos_Index_Value = 25,
        Object_Pos_Index_Anchor = 26,
        
        Object_Rot = 31,
        Object_Rot_Index = 32,
        Object_Rot_Index_Frame = 33,
        Object_Rot_Index_Ease = 34,
        Object_Rot_Index_Value = 35,
        
        Object_Sca = 41,
        Object_Sca_Index = 42,
        Object_Sca_Index_Frame = 43,
        Object_Sca_Index_Ease = 44,
        Object_Sca_Index_Value = 45,
        
        // ---------------------------------------------------------------------------------------------
        // TextureObject (100)
        // ---------------------------------------------------------------------------------------------
        
        Texture_Collider = 101,
        Texture_ColliderId = 102,
        Texture_TextureResourceId = 103,
        Texture_SublingId = 104,
        
        Texture_Clr = 111,
        Texture_Clr_Index = 112,
        Texture_Clr_Index_Frame = 113,
        Texture_Clr_Index_Ease = 114,
        Texture_Clr_Index_Value = 115,
        
        // ---------------------------------------------------------------------------------------------
        // EffectObject (200)
        // ---------------------------------------------------------------------------------------------
        
        // Core (200)
        
        Effect_Core_Loop = 201,
        Effect_Core_ParticleCount = 202,
        Effect_Core_LifetimeBounds = 203,
        Effect_Core_HasStopLocalFrame = 204,
        Effect_Core_StopLocalFrame = 205,
        Effect_Core_TextureResourceId = 206,
        Effect_Core_ParticlePivot = 207,
        
        // Forces (220)
        
        Effect_Forces_StartGravityMin = 221,
        Effect_Forces_StartGravityMax = 222,
        Effect_Forces_StartVelocityMin = 223,
        Effect_Forces_StartVelocityMax = 224,
        Effect_Forces_StartAngularVelocityMin = 225,
        Effect_Forces_StartAngularVelocityMax = 226,
        Effect_Forces_LinearVelocity = 227,
        Effect_Forces_OrbitalVelocity = 228,
        Effect_Forces_OrbitalCenterOffset = 229,
        Effect_Forces_VelocitySpeed = 230,
        Effect_Forces_LinearForce = 231,
        
        // Shape & Shape Spread (250)
        
        Effect_Shape = 251,
        Effect_Shape_CircleRadius = 252,
        Effect_Shape_CircleThickness = 253,
        Effect_Shape_CircleArc = 254,
        Effect_Shape_LineStart = 255,
        Effect_Shape_LineEnd = 256,
        Effect_Shape_ConeTopRadius = 257,
        Effect_Shape_ConeBaseRadius = 258,
        Effect_Shape_ConeArc = 259,
        Effect_Shape_ConeHeight = 260,
        Effect_Shape_TorusRadiusMinor = 261,
        Effect_Shape_TorusRadiusMajor = 262,
        Effect_Shape_TorusArc = 263,
        
        Effect_ShapeSpread_Type = 281,
        Effect_ShapeSpread_Spread = 282,
        Effect_ShapeSpread_Speed = 283,
        
        // Angle (300)
        
        Effect_Angle_Type = 301,
        Effect_Angle_Angle = 302,
        Effect_Angle_Curve = 303,
        Effect_Angle_SpeedRange = 304,
        Effect_Angle_AngleA = 305,
        Effect_Angle_AngleB = 306,
        
        // Scale (320)
        
        Effect_Scale_Type = 321,
        Effect_Scale_Scale = 322,
        Effect_Scale_CurveX = 323,
        Effect_Scale_CurveY = 324,
        Effect_Scale_SpeedRange = 325,
        Effect_Scale_ScaleA = 326,
        Effect_Scale_ScaleB = 327,
        
        // Color (340)
        
        Effect_Color_Type = 341,
        Effect_Color_Color = 342,
        Effect_Color_Gradient = 343,
        Effect_Color_SpeedRange = 344,
        Effect_Color_ColorA = 345,
        Effect_Color_ColorB = 346,
        
        // ---------------------------------------------------------------------------------------------
        // TextObject (400)
        // ---------------------------------------------------------------------------------------------
        
        Text_Text = 401,
        Text_FontResourceId = 402,
        
        Text_Direction = 411,
        Text_WordWrap = 412,
        Text_HorizontalAlignment = 413,
        Text_VerticalAlignment = 414,
        Text_OverEdge = 415,
        Text_UnderEdge = 416,
        Text_LeadingDistribution = 417,
        
        Text_Sizes = 441,
        Text_Sizes_Index = 442,
        Text_Sizes_Index_Frame = 443,
        Text_Sizes_Index_Ease = 444,
        Text_Sizes_Index_Value = 445,
        
        Text_Clr = 451,
        Text_Clr_Index = 452,
        Text_Clr_Index_Frame = 453,
        Text_Clr_Index_Ease = 454,
        Text_Clr_Index_Value = 455,
        
        Text_FontSizes = 461,
        Text_FontSizes_Index = 462,
        Text_FontSizes_Index_Frame = 463,
        Text_FontSizes_Index_Ease = 464,
        Text_FontSizes_Index_Value = 465,
    }
}