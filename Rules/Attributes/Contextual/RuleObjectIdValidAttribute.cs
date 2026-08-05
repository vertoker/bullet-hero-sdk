using System;
using System.Reflection;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Rules.Attributes
{
    // Validates a "regular" ObjectId reference (RectObject.ObjectId, ObjectIdModification.Prev/Next
    // ObjectId, ...): must be in the user-space range (ObjectId.IsValid(), value >= MinLevelValue).
    // The reserved negative ids are parent targets only - an object may attach to the camera, it
    // may not BE the camera - so use RuleParentObjectIdValid for properties that reference those.
    //
    // Scope-independent by design: an id is judged the same inside a prefab template as at level
    // scope. What still needs the graph pass is everything relational - uniqueness within a scope,
    // and whether the id is actually the key it is filed under.

    /// <summary> An object's own identity must be a user-space id. </summary>
    [AttributeUsage(PropertyTarget)]
    public class RuleObjectIdValidAttribute : BasePropertyRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(ObjectId).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, RuleContext context)
            => value is ObjectId objectId && objectId.IsValid();

        protected override void FixInternal(object target, PropertyInfo property, RuleContext context)
        {
            if (property.GetValue(target) is not ObjectId objectId) return;

            if (!objectId.IsValid())
                property.SetValue(target, ObjectId.MinLevel);
        }
    }
}
