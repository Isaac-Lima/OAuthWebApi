
namespace OAuthWebApi.Domain.Exceptions;

public class RegisterFailedException(IEnumerable<string> errorDescriptions) : 
    Exception($"O cadastro falhou com os seguintes erros: {string.Join(Environment.NewLine, errorDescriptions)}");

