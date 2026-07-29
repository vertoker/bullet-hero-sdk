using System.Reflection;
using System.Text;

namespace BH.SDK.Validations
{
    public readonly struct RulePath
    {
        public readonly PropertyInfo Property;
        public readonly object Key;

        public bool HasKey => Key != null;

        public RulePath(PropertyInfo property)
        {
            Property = property;
            Key = null;
        }
        // key is either a List/array index (int) or a dictionary key (ObjectId, ThemeId, ...)
        public RulePath(PropertyInfo property, object key)
        {
            Property = property;
            Key = key;
        }

        public override string ToString()
        {
            return HasKey ? $"{Property.Name}[{Key}]" : Property.Name;
        }
        public void Append(StringBuilder builder)
        {
            builder.Append(Property.Name);
            if (!HasKey) return;
            builder.Append("[");
            builder.Append(Key);
            builder.Append("]");
        }
    }
}
