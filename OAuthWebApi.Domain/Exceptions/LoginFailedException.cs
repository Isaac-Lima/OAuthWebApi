namespace OAuthWebApi.Domain.Exceptions;
public class LoginFailedException(string email) : Exception($"Credenciais de login inválidas: {email} ou senha.");
