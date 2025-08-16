namespace OAuthWebApi.Domain.Exceptions
{
    public class ExternalLoginProviderException(string provider, string message)
        : Exception($"Ocorreu um erro ao tentar fazer login com provedor externo: {provider} erro: {message}")        
    {
    }
}
