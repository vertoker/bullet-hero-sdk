namespace BHSDK.Models.Interfaces
{
    public interface IUpdatable<in T>
    {
        public void Update(T src);
    }
}