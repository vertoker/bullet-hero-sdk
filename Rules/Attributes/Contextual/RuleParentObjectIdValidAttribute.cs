using System;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Rules.Attributes
{
    // Validates a ParentObjectId reference (RectObject.ParentObjectId): unlike a regular ObjectId,
    // this also allows Null (no parent) and the special public game objects (Camera, LocalPlayer),
    // i.e. value >= ObjectId.MinLevelParentValue (see ObjectId.IsValidParent()).
    [AttributeUsage(PropertyTarget)]
    public class RuleParentObjectIdValidAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(ObjectId).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
            => value is ObjectId objectId && objectId.IsValidParent()
               && context is Level; // TODO add complex check for parenting and ids uniqueness

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (context is not Level) return;
            if (property.GetValue(target) is not ObjectId objectId) return;

            if (!objectId.IsValidParent())
                property.SetValue(target, ObjectId.Null);
        }
    }
}
