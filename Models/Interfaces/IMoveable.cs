namespace BH.SDK.Models.Interfaces
{
    public interface IMoveable<in T>
    {
        public void Pull(T source);
    }
}