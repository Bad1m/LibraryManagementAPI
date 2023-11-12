namespace LibraryManagementAPI.Services.Constants
{
    public class UserErrors
    {
        public const string UsernameRequired = "Username is required.";
        public const string FirstNameRequired = "First name is required.";
        public const string LastNameRequired = "Last name is required.";
        public const string PatronymicRequired = "Patronymic is required.";
        public const string EmailRequired = "Email is required.";
        public const string InvalidEmailFormat = "Invalid email format.";
        public const string PasswordRequired = "Password is required.";
        public const string UsernameAlreadyExists = "Username already exists.";
        public const string UserNotFound = "User not found.";
        public const string InvalidPassword = "Invalid password.";
        public const string FailedToDeleteUser = "Failed to delete the user.";
    }
}