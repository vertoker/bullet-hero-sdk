using BHSDK.Models.NoGame;
using BHSDK.Models.Objects;

namespace BHSDK.Utils
{
    public static class LevelUtils
    {
        public static void SetObjectId(this Object obj, LevelMeta meta)
        {
            obj.ObjectId = meta.GetNextObjectId();
        }
        public static void SetParent(this Object obj, Object parentObj)
        {
            obj.ParentObjectId = parentObj.ObjectId;
        }
        public static void SetBounds(this Object obj, int startFrame, int endFrame)
        {
            obj.StartFrame = startFrame;
            obj.EndFrame = endFrame;
        }
    }
}