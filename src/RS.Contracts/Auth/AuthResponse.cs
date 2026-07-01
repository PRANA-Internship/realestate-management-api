namespace RS.Contracts.Auth
{
    public record AuthResponse(
        string AccessToken,
        string Email,
        string FullName,
        string Role
    );
}
