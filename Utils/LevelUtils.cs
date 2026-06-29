using BH.SDK.Models.Objects;
using BH.SDK.Models.SettingGroups;

namespace BH.SDK.Utils
{
    public static class LevelUtils
    {
        public static void SetObjectId(this RectObject obj, LevelSettings settings)
        {
            obj.ObjectId = settings.GetNextObjectId();
        }
        public static void SetParent(this RectObject obj, RectObject parentObj)
        {
            obj.ParentObjectId = parentObj.ObjectId;
        }
        public static void SetBounds(this RectObject obj, int startFrame, int endFrame)
        {
            obj.StartFrame = startFrame;
            obj.EndFrame = endFrame;
        }
    }
}