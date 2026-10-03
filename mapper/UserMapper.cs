using webShop2.dto.user;
using webShop2.model;

namespace webShop2.mapper;

public class UserMapper
{
    public UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }

    public User ToEntity(CreateUserDto dto)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
    }

    public void UpdateEntity(User user, UpdateUserDto dto)
    {
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.IsActive = dto.IsActive;
    }
}