using BHSDK.Models.Objects;
using BHSDK.Models.SettingGroups;

namespace BHSDK.Utils
{
    public static class LevelUtils
    {
        public static void SetObjectId(this Object obj, LevelSettings settings)
        {
            obj.ObjectId = settings.GetNextObjectId();
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