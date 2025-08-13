
namespace OAuthWebApi.Domain.Exceptions;

public class RegistrationFailedException(IEnumerable<string> errorDescriptions) : 
    Exception($"O cadastro falhou com os seguintes erros: {string.Join(Environment.NewLine, errorDescriptions)}");

