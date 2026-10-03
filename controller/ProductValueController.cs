using Microsoft.AspNetCore.Mvc;
using webShop2.dto.productValue;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductValueController : ControllerBase
{
    private readonly IProductValueService _service;
    private readonly ProductValueMapper _mapper;

    public ProductValueController(
        IProductValueService service,
        ProductValueMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductValueDto>>> GetAll()
    {
        List<ProductValue> values =
            await _service.GetAllAsync();

        List<ProductValueDto> dto =
            new List<ProductValueDto>();

        foreach (ProductValue value in values)
        {
            dto.Add(_mapper.ToDto(value));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductValueDto>> GetById(int id)
    {
        ProductValue? value =
            await _service.GetByIdAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        ProductValueDto dto =
            _mapper.ToDto(value);

        return Ok(dto);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<List<ProductValueDto>>> GetByProduct(
        int productId)
    {
        List<ProductValue> values =
            await _service.GetByProductAsync(productId);

        List<ProductValueDto> dto =
            new List<ProductValueDto>();

        foreach (ProductValue value in values)
        {
            dto.Add(_mapper.ToDto(value));
        }

        return Ok(dto);
    }

    [HttpGet("attribute/{productAttributeId}")]
    public async Task<ActionResult<List<ProductValueDto>>> GetByAttribute(
        int productAttributeId)
    {
        List<ProductValue> values =
            await _service.GetByAttributeAsync(productAttributeId);

        List<ProductValueDto> dto =
            new List<ProductValueDto>();

        foreach (ProductValue value in values)
        {
            dto.Add(_mapper.ToDto(value));
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductValueDto>> Create(
        CreateProductValueDto dto)
    {
        ProductValue valueCreate =
            _mapper.ToEntity(dto);

        ProductValue value =
            await _service.CreateAsync(valueCreate);

        ProductValueDto valueDto =
            _mapper.ToDto(value);

        return CreatedAtAction(
            nameof(GetById),
            new { id = valueDto.Id },
            valueDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductValueDto>> Update(
        int id,
        CreateProductValueDto dto)
    {
        ProductValue valueUpdate = _mapper.ToEntity(dto);

        ProductValue? value =
            await _service.UpdateAsync(id, valueUpdate);

        if (value == null)
        {
            return NotFound();
        }

        ProductValueDto valueDto =
            _mapper.ToDto(value);

        return Ok(valueDto);
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