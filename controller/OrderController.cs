using Microsoft.AspNetCore.Mvc;
using webShop2.dto.order;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly OrderMapper _mapper;

    public OrderController(
        IOrderService service,
        OrderMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        List<Order> orders =
            await _service.GetAllAsync();

        List<OrderDto> dto = new List<OrderDto>();

        foreach (Order order in orders)
        {
            dto.Add(_mapper.ToDto(order));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        Order? order =
            await _service.GetByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        OrderDto dto =
            _mapper.ToDto(order);

        return Ok(dto);
    }

    [HttpGet("number/{number}")]
    public async Task<ActionResult<OrderDto>> GetByNumber(
        string number)
    {
        Order? order =
            await _service.GetByNumberAsync(number);

        if (order == null)
        {
            return NotFound();
        }

        OrderDto dto =
            _mapper.ToDto(order);

        return Ok(dto);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<OrderDto>>> GetByStatus(
        string status)
    {
        List<Order> orders =
            await _service.GetByStatusAsync(status);

        List<OrderDto> dto = new List<OrderDto>();

        foreach (Order order in orders)
        {
            dto.Add(_mapper.ToDto(order));
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(
        CreateOrderDto dto)
    {
        Order orderCreate =
            _mapper.ToEntity(dto);

        Order order =
            await _service.CreateAsync(orderCreate);

        OrderDto orderDto =
            _mapper.ToDto(order);

        return CreatedAtAction(
            nameof(GetById),
            new { id = orderDto.Id },
            orderDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderDto>> Update(
        int id,
        UpdateOrderDto dto)
    {
        Order? order = await _service.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        Order? orderUpdate = await _service.UpdateAsync(id, order);
        if (orderUpdate == null)
        {
            return NotFound();
        }

        OrderDto orderDto = _mapper.ToDto(order);

        return Ok(orderDto);
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