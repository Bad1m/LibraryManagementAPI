namespace LibraryManagementAPI.Data.Settings
{
    public class AuthSettings
    {
        public string Key { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}
