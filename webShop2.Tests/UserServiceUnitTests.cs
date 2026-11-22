
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using webShop2.model;
using webShop2.model.enums;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class UserServiceUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly UserService _service;

    public UserServiceUnitTests()
    {
        _userRepositoryMock =
            new Mock<IUserRepository>();

        _unitOfWorkMock =
            new Mock<IUnitOfWork>();

        _unitOfWorkMock
            .Setup(x => x.Users)
            .Returns(_userRepositoryMock.Object);

        Mock<IUserStore<User>> userStoreMock =
            new Mock<IUserStore<User>>();

        _userManagerMock =
            new Mock<UserManager<User>>(
                userStoreMock.Object,
                Options.Create(new IdentityOptions()),
                Mock.Of<IPasswordHasher<User>>(),
                Array.Empty<IUserValidator<User>>(),
                Array.Empty<IPasswordValidator<User>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<User>>>());

        _service =
            new UserService(
                _unitOfWorkMock.Object,
                _userManagerMock.Object);
    }

    // GET BY ID

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
    {
        User user = new User
        {
            Id = 1,
            Email = "test@test.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);

        User? result =
            await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(user, result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);

        User? result =
            await _service.GetByIdAsync(1);

        Assert.Null(result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    // GET ALL

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        List<User> users =
        [
            new User
            {
                Id = 1,
                Email = "user1@test.com"
            },
            new User
            {
                Id = 2,
                Email = "user2@test.com"
            }
        ];

        _userRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        List<User> result =
            await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(users, result);

        _userRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    // GET BY EMAIL

    [Fact]
    public async Task GetByEmailAsync_ReturnsUser_WhenUserExists()
    {
        User user = new User
        {
            Id = 1,
            Email = "test@test.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        User? result =
            await _service.GetByEmailAsync("test@test.com");

        Assert.NotNull(result);
        Assert.Equal(user, result);

        _userRepositoryMock.Verify(
            x => x.GetByEmailAsync("test@test.com"),
            Times.Once);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@test.com"))
            .ReturnsAsync((User?)null);

        User? result =
            await _service.GetByEmailAsync("test@test.com");

        Assert.Null(result);

        _userRepositoryMock.Verify(
            x => x.GetByEmailAsync("test@test.com"),
            Times.Once);
    }

    // UPDATE

    [Fact]
    public async Task UpdateAsync_UpdatesUser_WhenUserExists()
    {
        User existingUser = new User
        {
            Id = 1,
            FirstName = "Old",
            LastName = "Name",
            Email = "test@test.com",
            IsActive = true
        };

        User updateUser = new User
        {
            Id = 1,
            FirstName = "New",
            LastName = "Name",
            Email = "test@test.com",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(existingUser);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(
                existingUser,
                UserRoles.Admin))
            .ReturnsAsync(false);

        _userManagerMock
            .Setup(x => x.UpdateAsync(existingUser))
            .ReturnsAsync(IdentityResult.Success);

        User? result =
            await _service.UpdateAsync(1, updateUser);

        Assert.NotNull(result);
        Assert.Equal("New", result.FirstName);
        Assert.Equal("Name", result.LastName);
        Assert.Equal("test@test.com", result.Email);
        Assert.NotEqual(default, result.UpdatedAt);

        _userManagerMock.Verify(
            x => x.FindByIdAsync("1"),
            Times.Once);

        _userManagerMock.Verify(
            x => x.UpdateAsync(existingUser),
            Times.Once);

        _userManagerMock.Verify(
            x => x.SetEmailAsync(
                It.IsAny<User>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEmail_WhenEmailChanges()
    {
        User existingUser = new User
        {
            Id = 1,
            Email = "old@test.com",
            IsActive = true
        };

        User updateUser = new User
        {
            Id = 1,
            Email = "new@test.com",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(existingUser);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(
                existingUser,
                UserRoles.Admin))
            .ReturnsAsync(false);

        //_userManagerMock
        //    .Setup(x => x.SetEmailAsync(
        //        existingUser,
        //        "new@test.com"))
        //    .ReturnsAsync(IdentityResult.Success);
        _userManagerMock
    .Setup(x => x.SetEmailAsync(
        existingUser,
        "new@test.com"))
    .Callback<User, string>((user, email) =>
    {
        user.Email = email;
    })
    .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.SetUserNameAsync(
                existingUser,
                "new@test.com"))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.UpdateAsync(existingUser))
            .ReturnsAsync(IdentityResult.Success);

        User? result =
            await _service.UpdateAsync(1, updateUser);

        Assert.NotNull(result);
        Assert.Equal("new@test.com", result.Email);

        _userManagerMock.Verify(
            x => x.SetEmailAsync(
                existingUser,
                "new@test.com"),
            Times.Once);

        _userManagerMock.Verify(
            x => x.SetUserNameAsync(
                existingUser,
                "new@test.com"),
            Times.Once);

        _userManagerMock.Verify(
            x => x.UpdateAsync(existingUser),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        User updateUser = new User
        {
            Id = 1,
            Email = "new@test.com"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((User?)null);

        User? result =
            await _service.UpdateAsync(1, updateUser);

        Assert.Null(result);

        _userManagerMock.Verify(
            x => x.UpdateAsync(It.IsAny<User>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenDeactivatingLastAdmin()
    {
        User admin = new User
        {
            Id = 1,
            Email = "admin@test.com",
            IsActive = true
        };

        User updateUser = new User
        {
            Id = 1,
            Email = "admin@test.com",
            IsActive = false
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(admin);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(
                admin,
                UserRoles.Admin))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.GetUsersInRoleAsync(UserRoles.Admin))
            .ReturnsAsync((IList<User>)new List<User>
            {
                admin
            });

        InvalidOperationException exception =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdateAsync(1, updateUser));

        Assert.Equal(
            "The last active administrator cannot be deactivated.",
            exception.Message);

        _userManagerMock.Verify(
            x => x.UpdateAsync(It.IsAny<User>()),
            Times.Never);
    }

    // DELETE

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenUserIsDeleted()
    {
        User user = new User
        {
            Id = 1,
            Email = "test@test.com",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(
                user,
                UserRoles.Admin))
            .ReturnsAsync(false);

        _userManagerMock
            .Setup(x => x.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _userManagerMock.Verify(
            x => x.FindByIdAsync("1"),
            Times.Once);

        _userManagerMock.Verify(
            x => x.DeleteAsync(user),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((User?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _userManagerMock.Verify(
            x => x.DeleteAsync(It.IsAny<User>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenDeletingLastActiveAdmin()
    {
        User admin = new User
        {
            Id = 1,
            Email = "admin@test.com",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(admin);

        _userManagerMock
            .Setup(x => x.IsInRoleAsync(
                admin,
                UserRoles.Admin))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.GetUsersInRoleAsync(UserRoles.Admin))
            .ReturnsAsync((IList<User>)new List<User>
            {
                admin
            });

        InvalidOperationException exception =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.DeleteAsync(1));

        Assert.Equal(
            "The last active administrator cannot be deleted.",
            exception.Message);

        _userManagerMock.Verify(
            x => x.DeleteAsync(It.IsAny<User>()),
            Times.Never);
    }
}
