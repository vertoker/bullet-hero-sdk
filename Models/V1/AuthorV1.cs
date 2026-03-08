using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class AuthorV1 : IAuthor
    {
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
    }
}