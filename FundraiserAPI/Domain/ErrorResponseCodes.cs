namespace FundraiserAPI.Domain
{
    public static class ErrorResponseCodes
    {
        public static string USER_ALREADY_EXISTS { get; } = "USERNAME_ALREADY_EXISTS";
        public static string PASSWORD_TOO_WEAK { get; } = "PASSWORD_TOO_WEAK";
        public static string USERNAME_NOT_VALID { get; } = "USERNAME_NOT_VALID";
        public static string USER_DNE { get; } = "USER_DNE";
        public static string PASSWORD_INCORRECT { get; } = "PASSWORD_INCORRECT";
        public static string INVALID_AUTH_TOKEN { get; } = "INVALID_AUTH_TOKEN";
        public static string FUNDRAISER_NOT_FOUND { get; } = "FUNDRAISER_NOT_FOUND";
        public static string DONATION_NAME_INVALID { get; } = "DONATION_NAME_INVALID";
        public static string DONATION_ADDRESS_INVALID { get; } = "DONATION_ADDRESS_INVALID";
        public static string DONATION_AMOUNT_INVALID { get; } = "DONATION_AMOUNT_INVALID";
        public static string DONATION_CREDIT_CARD_INVALID { get; } = "DONATION_CREDIT_CARD_INVALID";
        public static string DONATION_BANK_ACCOUNT_NUMBER_INVALID { get; } = "DONATION_BANK_ACCOUNT_NUMBER_INVALID";
        public static string DONATION_PAYMENT_TYPE_INVALID { get; } = "DONATION_PAYMENT_TYPE_INVALID";
    }
}
