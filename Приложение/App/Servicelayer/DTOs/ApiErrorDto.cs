namespace WebApi.DTOs
{
    public class ApiErrorDto(long Timmestamp, string Message, int ErrorCode)
    {
        public ApiErrorDto(string message, int errorCode) :
            this(DateTimeOffset.UtcNow.ToUnixTimeSeconds(), message, errorCode)
        {
        }
    }
}