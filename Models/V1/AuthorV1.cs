using BHSDK.Models.Interfaces;

namespace BHSDK.Models.V1
{
    public class AuthorV1 : IAuthor
    {
        public string Name { get; set; }
        public int Order { get; set; }

        public AuthorV1()
        {
            Name = string.Empty;
            Order = 0;
        }
        public AuthorV1(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }
}