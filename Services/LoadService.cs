namespace BHSDK.Services
{
    public class LoadService
    {
        private readonly CompatibilityService _compatibilityService;

        public LoadService()
        {
            _compatibilityService = new CompatibilityService();
        }
        public LoadService(CompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }
    }
}