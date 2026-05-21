using System.Reflection;
using System.Text;

namespace BH.SDK.Validations
{
    public readonly struct RulePath
    {
        public readonly PropertyInfo Property;
        public readonly int Index;
        
        public bool HasIndex => Index != -1;

        public RulePath(PropertyInfo property)
        {
            Property = property;
            Index = -1;
        }
        public RulePath(PropertyInfo property, int index)
        {
            Property = property;
            Index = index;
        }

        public override string ToString()
        {
            return HasIndex ? $"{Property.Name}[{Index}]" : Property.Name;
        }
        public void Append(StringBuilder builder)
        {
            builder.Append(Property.Name);
            if (!HasIndex) return;
            builder.Append("[");
            builder.Append(Index);
            builder.Append("]");
        }
    }
}