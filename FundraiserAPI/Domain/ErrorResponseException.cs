namespace FundraiserAPI.Domain
{
    public class ErrorResponseException : Exception
    {
        public ErrorResponseException()
        {
        }
        
        public ErrorResponseException(string message, string errorCode)
            : base(message)
        {
            ErrorData = new ErrorResponseData()
            {
                ErrorCode = errorCode,
                ErrorMessage = message
            };
        }

        public ErrorResponseException(string message)
            : base(message)
        {
        }

        public ErrorResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ErrorResponseData? ErrorData { get; set; }
    }
}
