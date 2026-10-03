using Microsoft.AspNetCore.Mvc;
using webShop2.dto.user;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(
        CreateUserDto dto)
    {
        User userCreate =
            _mapper.ToEntity(dto);

        User user =
            await _service.CreateAsync(userCreate);

        UserDto userDto =
            _mapper.ToDto(user);

        return CreatedAtAction(
            nameof(GetById),
            new { id = userDto.Id },
            userDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(
        int id,
        CreateUserDto dto)
    {
        User userUpdate =
            _mapper.ToEntity(dto);

        User? user =
            await _service.UpdateAsync(id, userUpdate);

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