
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webShop2.dto.user;
using webShop2.mapper;
using webShop2.model;
using webShop2.model.enums;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = UserRoles.Admin)]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly UserMapper _mapper;

    public UserController(
        IUserService service,
        UserMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        List<User> users =
            await _service.GetAllAsync();

        List<UserDto> dto =
            new List<UserDto>();

        foreach (User user in users)
        {
            dto.Add(_mapper.ToDto(user));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        User? user =
            await _service.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        UserDto dto =
            _mapper.ToDto(user);

        return Ok(dto);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetByEmail(
        string email)
    {
        User? user =
            await _service.GetByEmailAsync(email);

        if (user == null)
        {
            return NotFound();
        }

        UserDto dto =
            _mapper.ToDto(user);

        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(
        int id,
        UpdateUserDto dto)
    {
        User? user =
            await _service.UpdateAsync(
                id,
                new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    IsActive = dto.IsActive
                });

        if (user == null)
        {
            return NotFound();
        }

        UserDto userDto =
            _mapper.ToDto(user);

        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted =
            await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
