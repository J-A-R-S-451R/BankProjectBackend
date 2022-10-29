namespace FundraiserAPI.Domain
{
    public static class ErrorResponseCodes
    {
        public static string USER_ALREADY_EXISTS { get; } = "USERNAME_ALREADY_EXISTS";
        public static string PASSWORD_TOO_WEAK { get; } = "PASSWORD_TOO_WEAK";
        public static string USERNAME_NOT_VALID { get; } = "USERNAME_NOT_VALID";
        public static string USER_DNE { get; } = "USER_DNE";
        public static string PASSWORD_INCORRECT { get; } = "PASSWORD_INCORRECT";
    }
}
