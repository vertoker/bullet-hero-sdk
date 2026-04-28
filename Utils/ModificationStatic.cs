using System;
using System.Collections.Generic;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Objects;
using Object = BHSDK.Models.Objects.Object;

namespace BHSDK.Utils
{
    public static class ModificationStatic
    {
        private static readonly Dictionary<ObjectPropertyPath, Action<Object, Modification>> _objectModifications;

        private static void Add(ObjectPropertyPath path, Action<Object, Modification> action)
        {
            _objectModifications.Add(path, action);
        }

        public static void Apply(Object obj, Modification modification)
        {
            var action = _objectModifications[modification.Path];
            action(obj, modification);
        }
        
        static ModificationStatic()
        {
            _objectModifications = new Dictionary<ObjectPropertyPath, Action<Object, Modification>>(100);

            AddObject();
            AddTextureObject();
            
            AddEffectObject_Core();
            AddEffectObject_Forces();
            AddEffectObject_Shapes();
            AddEffectObject_Angle();
            AddEffectObject_Scale();
            AddEffectObject_Color();
        }
        
        private static void AddObject()
        {
            Add(ObjectPropertyPath.Object_Name, Object_Name);
            Add(ObjectPropertyPath.Object_Visible, Object_Visible);
            Add(ObjectPropertyPath.Object_StartFrame, Object_StartFrame);
            Add(ObjectPropertyPath.Object_EndFrame, Object_EndFrame);
            Add(ObjectPropertyPath.Object_Layer, Object_Layer);
            Add(ObjectPropertyPath.Object_Pivot, Object_Pivot);
            
            Add(ObjectPropertyPath.Object_Pos, Object_Pos);
            Add(ObjectPropertyPath.Object_Pos_Index, Object_Pos_Index);
            Add(ObjectPropertyPath.Object_Pos_Index_Frame, Object_Pos_Index_Frame);
            Add(ObjectPropertyPath.Object_Pos_Index_Ease, Object_Pos_Index_Ease);
            Add(ObjectPropertyPath.Object_Pos_Index_Value, Object_Pos_Index_Value);
            Add(ObjectPropertyPath.Object_Pos_Index_Anchor, Object_Pos_Index_Anchor);
            
            Add(ObjectPropertyPath.Object_Rot, Object_Rot);
            Add(ObjectPropertyPath.Object_Rot_Index, Object_Rot_Index);
            Add(ObjectPropertyPath.Object_Rot_Index_Frame, Object_Rot_Index_Frame);
            Add(ObjectPropertyPath.Object_Rot_Index_Ease, Object_Rot_Index_Ease);
            Add(ObjectPropertyPath.Object_Rot_Index_Value, Object_Rot_Index_Value);
            
            Add(ObjectPropertyPath.Object_Sca, Object_Sca);
            Add(ObjectPropertyPath.Object_Sca_Index, Object_Sca_Index);
            Add(ObjectPropertyPath.Object_Sca_Index_Frame, Object_Sca_Index_Frame);
            Add(ObjectPropertyPath.Object_Sca_Index_Ease, Object_Sca_Index_Ease);
            Add(ObjectPropertyPath.Object_Sca_Index_Value, Object_Sca_Index_Value);
            return;
            
            void Object_Name(Object obj, Modification mod)
                => obj.Name = (string)mod.Value; 
            void Object_Visible(Object obj, Modification mod)
                => obj.Visible = (bool)mod.Value; 
            void Object_StartFrame(Object obj, Modification mod)
                => obj.StartFrame = (int)mod.Value; 
            void Object_EndFrame(Object obj, Modification mod)
                => obj.EndFrame = (int)mod.Value; 
            void Object_Layer(Object obj, Modification mod)
                => obj.Layer = (int)mod.Value; 
            void Object_Pivot(Object obj, Modification mod)
                => obj.Pivot.Value = (IVector2)mod.Value;
            
            void Object_Pos(Object obj, Modification mod)
                => obj.Pos = (List<Pos>)mod.Value;
            void Object_Pos_Index(Object obj, Modification mod)
                => obj.Pos[mod.Index] = (Pos)mod.Value;
            void Object_Pos_Index_Frame(Object obj, Modification mod)
                => obj.Pos[mod.Index].Frame = (int)mod.Value;
            void Object_Pos_Index_Ease(Object obj, Modification mod)
                => obj.Pos[mod.Index].Ease = (EaseType)mod.Value;
            void Object_Pos_Index_Value(Object obj, Modification mod)
                => obj.Pos[mod.Index].Vector2 = (IVector2)mod.Value;
            void Object_Pos_Index_Anchor(Object obj, Modification mod)
                => obj.Pos[mod.Index].Anchor.Value = (IVector2)mod.Value;
            
            void Object_Rot(Object obj, Modification mod)
                => obj.Rot = (List<Rot>)mod.Value;
            void Object_Rot_Index(Object obj, Modification mod)
                => obj.Rot[mod.Index] = (Rot)mod.Value;
            void Object_Rot_Index_Frame(Object obj, Modification mod)
                => obj.Rot[mod.Index].Frame = (int)mod.Value;
            void Object_Rot_Index_Ease(Object obj, Modification mod)
                => obj.Rot[mod.Index].Ease = (EaseType)mod.Value;
            void Object_Rot_Index_Value(Object obj, Modification mod)
                => obj.Rot[mod.Index].Angle = (IFloat)mod.Value;
            
            void Object_Sca(Object obj, Modification mod)
                => obj.Sca = (List<Sca>)mod.Value;
            void Object_Sca_Index(Object obj, Modification mod)
                => obj.Sca[mod.Index] = (Sca)mod.Value;
            void Object_Sca_Index_Frame(Object obj, Modification mod)
                => obj.Sca[mod.Index].Frame = (int)mod.Value;
            void Object_Sca_Index_Ease(Object obj, Modification mod)
                => obj.Sca[mod.Index].Ease = (EaseType)mod.Value;
            void Object_Sca_Index_Value(Object obj, Modification mod)
                => obj.Sca[mod.Index].Vector2 = (IVector2)mod.Value;
        }

        private static void AddTextureObject()
        {
            Add(ObjectPropertyPath.Texture_Collider, Texture_Collider);
            Add(ObjectPropertyPath.Texture_ColliderId, Texture_ColliderId);
            Add(ObjectPropertyPath.Texture_TextureResourceId, Texture_TextureResourceId);
            Add(ObjectPropertyPath.Texture_SublingId, Texture_SublingId);
            
            Add(ObjectPropertyPath.Texture_Clr, Texture_Clr);
            Add(ObjectPropertyPath.Texture_Clr_Index, Texture_Clr_Index);
            Add(ObjectPropertyPath.Texture_Clr_Index_Frame, Texture_Clr_Index_Frame);
            Add(ObjectPropertyPath.Texture_Clr_Index_Ease, Texture_Clr_Index_Ease);
            Add(ObjectPropertyPath.Texture_Clr_Index_Value, Texture_Clr_Index_Value);
            return;
            
            void Texture_Collider(Object obj, Modification mod)
                => ((TextureObject)obj).Collider = (bool)mod.Value;
            void Texture_ColliderId(Object obj, Modification mod)
                => ((TextureObject)obj).ColliderId = (int)mod.Value;
            void Texture_TextureResourceId(Object obj, Modification mod)
                => ((TextureObject)obj).TextureResourceId = (int)mod.Value;
            void Texture_SublingId(Object obj, Modification mod)
                => ((TextureObject)obj).SublingIndex = (int)mod.Value;
            
            void Texture_Clr(Object obj, Modification mod)
                => ((TextureObject)obj).Clr = (List<Clr>)mod.Value;
            void Texture_Clr_Index(Object obj, Modification mod)
                => ((TextureObject)obj).Clr[mod.Index] = (Clr)mod.Value;
            void Texture_Clr_Index_Frame(Object obj, Modification mod)
                => ((TextureObject)obj).Clr[mod.Index].Frame = (int)mod.Value;
            void Texture_Clr_Index_Ease(Object obj, Modification mod)
                => ((TextureObject)obj).Clr[mod.Index].Ease = (EaseType)mod.Value;
            void Texture_Clr_Index_Value(Object obj, Modification mod)
                => ((TextureObject)obj).Clr[mod.Index].Value = (IColor)mod.Value;
        }
        
        private static void AddEffectObject_Core()
        {
            Add(ObjectPropertyPath.Effect_Core_Loop, Effect_Core_Loop);
            Add(ObjectPropertyPath.Effect_Core_ParticleCount, Effect_Core_ParticleCount);
            Add(ObjectPropertyPath.Effect_Core_LifetimeBounds, Effect_Core_LifetimeBounds);
            Add(ObjectPropertyPath.Effect_Core_HasStopLocalFrame, Effect_Core_HasStopLocalFrame);
            Add(ObjectPropertyPath.Effect_Core_StopLocalFrame, Effect_Core_StopLocalFrame);
            Add(ObjectPropertyPath.Effect_Core_TextureResourceId, Effect_Core_TextureResourceId);
            Add(ObjectPropertyPath.Effect_Core_ParticlePivot, Effect_Core_ParticlePivot);
            return;
            
            void Effect_Core_Loop(Object obj, Modification mod)
                => ((EffectObject)obj).Core.Loop = (bool)mod.Value;
            void Effect_Core_ParticleCount(Object obj, Modification mod)
                => ((EffectObject)obj).Core.ParticleCount = (uint)mod.Value;
            void Effect_Core_LifetimeBounds(Object obj, Modification mod)
                => ((EffectObject)obj).Core.LifetimeBounds = (IVector2)mod.Value;
            void Effect_Core_HasStopLocalFrame(Object obj, Modification mod)
                => ((EffectObject)obj).Core.HasStopLocalFrame = (bool)mod.Value;
            void Effect_Core_StopLocalFrame(Object obj, Modification mod)
                => ((EffectObject)obj).Core.StopLocalFrame = (int)mod.Value;
            void Effect_Core_TextureResourceId(Object obj, Modification mod)
                => ((EffectObject)obj).Core.TextureResourceId = (int)mod.Value;
            void Effect_Core_ParticlePivot(Object obj, Modification mod)
                => ((EffectObject)obj).Core.ParticlePivot.Value = (IVector2)mod.Value;
        }
        private static void AddEffectObject_Forces()
        {
            Add(ObjectPropertyPath.Effect_Forces_StartGravityMin, Effect_Forces_StartGravityMin);
            Add(ObjectPropertyPath.Effect_Forces_StartGravityMax, Effect_Forces_StartGravityMax);
            Add(ObjectPropertyPath.Effect_Forces_StartVelocityMin, Effect_Forces_StartVelocityMin);
            Add(ObjectPropertyPath.Effect_Forces_StartVelocityMax, Effect_Forces_StartVelocityMax);
            Add(ObjectPropertyPath.Effect_Forces_StartAngularVelocityMin, Effect_Forces_StartAngularVelocityMin);
            Add(ObjectPropertyPath.Effect_Forces_StartAngularVelocityMax, Effect_Forces_StartAngularVelocityMax);
            Add(ObjectPropertyPath.Effect_Forces_LinearVelocity, Effect_Forces_LinearVelocity);
            Add(ObjectPropertyPath.Effect_Forces_OrbitalVelocity, Effect_Forces_OrbitalVelocity);
            Add(ObjectPropertyPath.Effect_Forces_OrbitalCenterOffset, Effect_Forces_OrbitalCenterOffset);
            Add(ObjectPropertyPath.Effect_Forces_VelocitySpeed, Effect_Forces_VelocitySpeed);
            Add(ObjectPropertyPath.Effect_Forces_LinearForce, Effect_Forces_LinearForce);
            return;

            void Effect_Forces_StartGravityMin(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.StartGravityMin = (IFloat)mod.Value;
            void Effect_Forces_StartGravityMax(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.StartGravityMax = (IFloat)mod.Value;
            void Effect_Forces_StartVelocityMin(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.StartVelocityMin = (IVector2)mod.Value;
            void Effect_Forces_StartVelocityMax(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.StartVelocityMax = (IVector2)mod.Value;
            void Effect_Forces_StartAngularVelocityMin(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.StartAngularVelocityMin = (IFloat)mod.Value;
            void Effect_Forces_StartAngularVelocityMax(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.StartAngularVelocityMax = (IFloat)mod.Value;
            void Effect_Forces_LinearVelocity(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.LinearVelocity = (IVector2)mod.Value;
            void Effect_Forces_OrbitalVelocity(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.OrbitalVelocity = (IVector3)mod.Value;
            void Effect_Forces_OrbitalCenterOffset(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.OrbitalCenterOffset = (IVector3)mod.Value;
            void Effect_Forces_VelocitySpeed(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.VelocitySpeed = (IFloat)mod.Value;
            void Effect_Forces_LinearForce(Object obj, Modification mod)
                => ((EffectObject)obj).Forces.LinearForce = (IVector2)mod.Value;
        }
        private static void AddEffectObject_Shapes()
        {
            
        }
        private static void AddEffectObject_Angle()
        {
            
        }
        private static void AddEffectObject_Scale()
        {
            
        }
        private static void AddEffectObject_Color()
        {
            
        }
    }
}