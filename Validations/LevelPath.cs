using System.Reflection;
using System.Text;

namespace BHSDK.Validations
{
    public readonly struct LevelPath
    {
        public readonly PropertyInfo Property;
        public readonly int Index;
        
        public bool HasIndex => Index != -1;

        public LevelPath(PropertyInfo property)
        {
            Property = property;
            Index = -1;
        }
        public LevelPath(PropertyInfo property, int index)
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