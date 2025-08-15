namespace OAuthWebApi.Domain.Exceptions
{
    public class RefreshTokenException(string message) : Exception(message)
    {
    }
}
