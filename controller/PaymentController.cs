using Microsoft.AspNetCore.Mvc;
using webShop2.dto.payment;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _service;
    private readonly PaymentMapper _mapper;

    public PaymentController(
        IPaymentService service,
        PaymentMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetAll()
    {
        List<Payment> payments =
            await _service.GetAllAsync();

        List<PaymentDto> dto = new List<PaymentDto>();

        foreach (Payment payment in payments)
        {
            dto.Add(_mapper.ToDto(payment));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        Payment? payment =
            await _service.GetByIdAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        PaymentDto dto =
            _mapper.ToDto(payment);

        return Ok(dto);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<List<PaymentDto>>> GetByOrder(
        int orderId)
    {
        List<Payment> payments =
            await _service.GetByOrderAsync(orderId);

        List<PaymentDto> dto = new List<PaymentDto>();

        foreach (Payment payment in payments)
        {
            dto.Add(_mapper.ToDto(payment));
        }

        return Ok(dto);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<PaymentDto>>> GetByStatus(
        string status)
    {
        List<Payment> payments =
            await _service.GetByStatusAsync(status);

        List<PaymentDto> dto = new List<PaymentDto>();

        foreach (Payment payment in payments)
        {
            dto.Add(_mapper.ToDto(payment));
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create(
        CreatePaymentDto dto)
    {
        Payment paymentCreate =
            _mapper.ToEntity(dto);

        Payment payment =
            await _service.CreateAsync(paymentCreate);

        PaymentDto paymentDto =
            _mapper.ToDto(payment);

        return CreatedAtAction(
            nameof(GetById),
            new { id = paymentDto.Id },
            paymentDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PaymentDto>> Update(
        int id,
        CreatePaymentDto dto)
    {
        Payment paymentUpdate = _mapper.ToEntity(dto);

        Payment? payment = await _service.UpdateAsync(id, paymentUpdate);

        if (payment == null)
        {
            return NotFound();
        }

        PaymentDto paymentDto =
            _mapper.ToDto(payment);

        return Ok(paymentDto);
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