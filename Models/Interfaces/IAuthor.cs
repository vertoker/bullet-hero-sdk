using BHSDK.Interfaces;
using BHSDK.Models.V1;

namespace BHSDK.Models.Interfaces
{
    public interface IAuthor : IModelReflection<AuthorV1, IAuthor>
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }
}