namespace BHSDK.Models.Interfaces
{
    public interface ICopyable<out T>
    {
        public T Copy();
    }
}