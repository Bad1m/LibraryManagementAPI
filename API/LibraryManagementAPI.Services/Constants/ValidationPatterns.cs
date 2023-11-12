namespace LibraryManagementAPI.Services.Constants
{
    public class ValidationPatterns
    {
        public const string IsbnPattern = "^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\\d-]+$";
    }
}