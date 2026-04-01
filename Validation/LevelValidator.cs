namespace BHSDK.Validation
{
    // TODO make correct architecture for validation block, this is test class
    
    public class LevelValidator
    {
        public static bool Framerate(int framerate) => framerate > 0;
    }
}