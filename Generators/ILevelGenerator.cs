using BHSDK.Models;

namespace BHSDK.Generators
{
    public interface ILevelGenerator<in TInputParameters>
    {
        public void Generate(Level level, TInputParameters parameters);
    }
}