using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class UserServiceUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
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

        _service =
            new UserService(_unitOfWorkMock.Object);
    }

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

    [Fact]
    public async Task CreateAsync_AddsUserAndSavesChanges()
    {
        User user = new User
        {
            Id = 1,
            Email = "test@test.com"
        };

        _userRepositoryMock
            .Setup(x => x.AddAsync(user))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        User result =
            await _service.CreateAsync(user);

        Assert.Equal(user, result);

        _userRepositoryMock.Verify(
            x => x.AddAsync(user),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUser_WhenUserExists()
    {
        User existingUser = new User
        {
            Id = 1,
            Email = "old@test.com"
        };

        User updateUser = new User
        {
            Id = 1,
            Email = "new@test.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingUser);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        User? result =
            await _service.UpdateAsync(1, updateUser);

        Assert.NotNull(result);
        Assert.Equal(updateUser, result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.Update(updateUser),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
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

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);

        User? result =
            await _service.UpdateAsync(1, updateUser);

        Assert.Null(result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.Update(It.IsAny<User>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesUser_WhenUserExists()
    {
        User user = new User
        {
            Id = 1,
            Email = "test@test.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.Delete(user),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((User?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _userRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _userRepositoryMock.Verify(
            x => x.Delete(It.IsAny<User>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}