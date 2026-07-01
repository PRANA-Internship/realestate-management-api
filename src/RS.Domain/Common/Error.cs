namespace RS.Domain.Common
{
    public sealed record Error(string Code, string? Description = null)
    {
        public static readonly Error None = new(string.Empty);

        public static class AuthErrors
        {
            public static readonly Error InvalidCredentials = new("AUTH_001", "Invalid email or password.");
            public static readonly Error UserInactive = new("AUTH_002", "This account is inactive.");
            public static readonly Error EmailNotVerified = new("AUTH_003", "Email address has not been verified.");
            public static readonly Error EmailAlreadyExists = new("AUTH_004", "An account with this email already exists.");
            public static readonly Error UserNotFound = new("AUTH_005", "No account found with this email.");
            public static readonly Error InvalidResetToken = new("AUTH_006", "Password reset token is invalid or expired.");
            public static readonly Error TokenAlreadyUsed = new("AUTH_007", "Password reset token has already been used.");
        }
    }
}
