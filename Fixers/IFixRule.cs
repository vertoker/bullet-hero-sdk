using BHSDK.Models;
using BHSDK.Validations;

namespace BHSDK.Fixers
{
    public interface IFixRule
    {
        void Fix(LevelIssue issue);
    }
}