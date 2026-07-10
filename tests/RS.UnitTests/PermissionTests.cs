using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RS.Domain.Enums;
using RS.Infrastructure.Authentication;

namespace RS.UnitTests
{
    [TestClass]
    public class PermissionTests
    {
        private PermissionAuthorizationHandler _handler = default!;
        private MockPermissionProvider _permissionProvider = default!;

        [TestInitialize]
        public void Setup()
        {
            _permissionProvider = new MockPermissionProvider();
            _handler = new PermissionAuthorizationHandler(_permissionProvider);
        }

        [TestMethod]
        [DataRow(UserRole.ADMIN, Permission.CreateProperty, true)]
        [DataRow(UserRole.ADMIN, Permission.ManageConfigurations, true)]
        [DataRow(UserRole.MANAGER, Permission.CreateProperty, true)]
        [DataRow(UserRole.MANAGER, Permission.ManageConfigurations, false)]
        [DataRow(UserRole.BUYER, Permission.CreateProperty, false)]
        public async Task HandleRequirementAsync_EvaluatesRolePermissionsCorrectly(
            UserRole userRole,
            Permission requiredPermission,
            bool shouldSucceed)
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, userRole.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var requirement = new PermissionRequirement(requiredPermission.ToString());
            var context = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.AreEqual(shouldSucceed, context.HasSucceeded);
        }

        [TestMethod]
        public async Task HandleRequirementAsync_Fails_WhenRoleIsInvalid()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, "INVALID_ROLE") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var requirement = new PermissionRequirement(Permission.CreateProperty.ToString());
            var context = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.IsFalse(context.HasSucceeded);
        }

        [TestMethod]
        public async Task HandleRequirementAsync_Fails_WhenPermissionIsInvalid()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, UserRole.ADMIN.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var requirement = new PermissionRequirement("NON_EXISTENT_PERMISSION");
            var context = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            // Act
            await _handler.HandleAsync(context);

            // Assert
            Assert.IsFalse(context.HasSucceeded);
        }

        private class MockPermissionProvider : IPermissionProvider
        {
            public Task<bool> HasPermissionAsync(UserRole role, string permissionName)
            {
                if (!Enum.TryParse<Permission>(permissionName, true, out var permission))
                {
                    return Task.FromResult(false);
                }

                if (role == UserRole.ADMIN)
                {
                    return Task.FromResult(true);
                }

                if (role == UserRole.MANAGER)
                {
                    var managerPermissions = new HashSet<Permission>
                    {
                        Permission.CreateProperty,
                        Permission.ReadMyProperties,
                        Permission.ReadProperty,
                        Permission.UpdateProperty,
                        Permission.DeleteProperty,
                        Permission.AddPropertyImages,
                        Permission.DeletePropertyImage,
                        Permission.SetPrimaryPropertyImage,
                        Permission.ChangePropertyActiveState
                    };
                    return Task.FromResult(managerPermissions.Contains(permission));
                }

                return Task.FromResult(false);
            }
        }
    }
}
