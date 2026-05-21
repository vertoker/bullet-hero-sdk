using BH.SDK.Models;

namespace BH.SDK.Generators
{
    public interface ILevelGenerator<in TInputParameters>
    {
        public void Generate(Level level, TInputParameters parameters);
    }
}