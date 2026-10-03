using Microsoft.AspNetCore.Mvc;
using webShop2.dto.item;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _service;
    private readonly ItemMapper _mapper;

    public ItemController(
        IItemService service,
        ItemMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemDto>>> GetAll()
    {
        List<Item> items =
            await _service.GetAllAsync();

        List<ItemDto> dto = new List<ItemDto>();

        foreach (Item item in items)
        {
            dto.Add(_mapper.ToDto(item));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetById(int id)
    {
        Item? item =
            await _service.GetByIdAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        ItemDto dto = _mapper.ToDto(item);

        return Ok(dto);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<List<ItemDto>>> GetByOrder(
        int orderId)
    {
        List<Item> items =
            await _service.GetByOrderAsync(orderId);

        List<ItemDto> dto = new List<ItemDto>();

        foreach (Item item in items)
        {
            dto.Add(_mapper.ToDto(item));
        }

        return Ok(dto);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<List<ItemDto>>> GetByProduct(
        int productId)
    {
        List<Item> items =
            await _service.GetByProductAsync(productId);

        List<ItemDto> dto = new List<ItemDto>();

        foreach (Item item in items)
        {
            dto.Add(_mapper.ToDto(item));
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(
        CreateItemDto dto)
    {
        Item itemCreate =
            _mapper.ToEntity(dto);

        Item item =
            await _service.CreateAsync(itemCreate);

        ItemDto itemDto =
            _mapper.ToDto(item);

        return CreatedAtAction(
            nameof(GetById),
            new { id = itemDto.Id },
            itemDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemDto>> Update(
        int id,
        ItemDto dto)
    {
        Item itemUpdate =
            _mapper.ToEntityX(dto);

        Item? item =
            await _service.UpdateAsync(id, itemUpdate);

        if (item == null)
        {
            return NotFound();
        }

        ItemDto itemDto =
            _mapper.ToDto(item);

        return Ok(itemDto);
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