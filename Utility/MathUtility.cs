using BHSDK.Models.Enum;
using UnityEngine;

namespace BHSDK.Utility
{
    public class MathUtility
    {
        public const float Rad2Deg = 57.295779513f;
        
        public static Vector3 GetCoefficient(Anchor anchor, ref Vector3 borderScreen)
        {
            switch (anchor)
            {
                case Anchor.Center_Middle: return Vector3.zero;
                case Anchor.Left_Top: return new Vector3(-borderScreen.x, borderScreen.y, 0);
                case Anchor.Center_Top: return new Vector3(0, borderScreen.y, 0);
                case Anchor.Right_Top: return new Vector3(borderScreen.x, borderScreen.y, 0);
                case Anchor.Left_Middle: return new Vector3(-borderScreen.x, 0, 0);
                case Anchor.Right_Middle: return new Vector3(borderScreen.x, 0, 0);
                case Anchor.Left_Bottom: return new Vector3(-borderScreen.x, -borderScreen.y, 0);
                case Anchor.Center_Bottom: return new Vector3(0, -borderScreen.y, 0);
                case Anchor.Right_Bottom: return new Vector3(borderScreen.x, -borderScreen.y, 0);
                default: return Vector3.zero;
            }
        }
        public static Vector2 CalculatePivot(float rot, Vector3 sca, Anchor pivot)
        {
            sca = GetCoefficient(pivot, ref sca) / -2f;
            var power = Mathf.Sqrt(sca.x * sca.x + sca.y * sca.y);
            var angle = Mathf.Atan2(sca.y, sca.x) * Rad2Deg + rot;
            return new Vector2(Mathf.Cos(angle / Rad2Deg), Mathf.Sin(angle / Rad2Deg)) * power;
        }
    }
}