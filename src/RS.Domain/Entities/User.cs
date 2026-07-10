using System;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string? PasswordHash { get; private set; }
        public UserRole Role { get; private set; } = UserRole.BUYER;
        public UserStatus Status { get; private set; } = UserStatus.ACTIVE;

        public Guid? CreatedByUserId { get; private set; }

        public string? PasswordResetToken { get; private set; }
        public DateTime? PasswordResetExpiry { get; private set; }
        private User() { } // EF Core required

        public static User CreateBuyer(string fullName, string email, string phone, string passwordHash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

            return new User
            {
                Id = Guid.NewGuid(),
                FullName = fullName.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                Phone = phone.Trim(),
                PasswordHash = passwordHash,
                Role = UserRole.BUYER,
                Status = UserStatus.ACTIVE,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public static User CreateStaff(string fullName, string email, string phone, UserRole role)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            if (role == UserRole.BUYER)
                throw new ArgumentException("Staff role cannot be BUYER.");

            return new User
            {
                Id = Guid.NewGuid(),
                FullName = fullName.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                Phone = phone.Trim(),
                Role = role,
                Status = UserStatus.INACTIVE, // Needs activation
                CreatedAt = DateTimeOffset.UtcNow
            };
        }


        public void ActivateWithPassword(string passwordHash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

            PasswordHash = passwordHash;
            Status = UserStatus.ACTIVE;
            PasswordResetToken = null;
            PasswordResetExpiry = null;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetCreatedBy(Guid createdByUserId)
        {
            CreatedByUserId = createdByUserId;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateProfileSafe(string fullName, string phone)
        {
            FullName = fullName.Trim();
            Phone = phone.Trim();
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public string GeneratePasswordResetToken()
        {
            PasswordResetToken = Guid.NewGuid().ToString("N");
            PasswordResetExpiry = DateTime.UtcNow.AddHours(24);

            return PasswordResetToken;
        }


        public void SetStatus(UserStatus status)
        {
            Status = status;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }


}
