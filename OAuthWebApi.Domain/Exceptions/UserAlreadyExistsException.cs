namespace OAuthWebApi.Domain.Exceptions;

public class UserAlreadyExistsException(string email) : Exception($"O usuário com o email {email} já está cadastrado");

