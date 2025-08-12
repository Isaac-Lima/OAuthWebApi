namespace OAuthWebApi.Domain.Requests
{
    public record ResgisterRequest
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
