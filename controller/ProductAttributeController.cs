using Microsoft.AspNetCore.Mvc;
using webShop2.dto.productAttribute;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductAttributeController : ControllerBase
{
    private readonly IProductAttributeService _service;
    private readonly ProductAttributeMapper _mapper;

    public ProductAttributeController(
        IProductAttributeService service,
        ProductAttributeMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductAttributeDto>>> GetAll()
    {
        List<ProductAttribute> attributes =
            await _service.GetAllAsync();

        List<ProductAttributeDto> dto =
            new List<ProductAttributeDto>();

        foreach (ProductAttribute attribute in attributes)
        {
            dto.Add(_mapper.ToDto(attribute));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductAttributeDto>> GetById(
        int id)
    {
        ProductAttribute? attribute =
            await _service.GetByIdAsync(id);

        if (attribute == null)
        {
            return NotFound();
        }

        ProductAttributeDto dto =
            _mapper.ToDto(attribute);

        return Ok(dto);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<List<ProductAttributeDto>>> GetByCategory(
        int categoryId)
    {
        List<ProductAttribute> attributes =
            await _service.GetByCategoryAsync(categoryId);

        List<ProductAttributeDto> dto =
            new List<ProductAttributeDto>();

        foreach (ProductAttribute attribute in attributes)
        {
            dto.Add(_mapper.ToDto(attribute));
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductAttributeDto>> Create(
        CreateProductAttributeDto dto)
    {
        ProductAttribute attributeCreate =
            _mapper.ToEntity(dto);

        ProductAttribute attribute =
            await _service.CreateAsync(attributeCreate);

        ProductAttributeDto attributeDto =
            _mapper.ToDto(attribute);

        return CreatedAtAction(
            nameof(GetById),
            new { id = attributeDto.Id },
            attributeDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductAttributeDto>> Update(
        int id,
        CreateProductAttributeDto dto)
    {
        ProductAttribute attributeUpdate = _mapper.ToEntity(dto);

        ProductAttribute? attribute = await _service.UpdateAsync(id, attributeUpdate);

        if (attribute == null)
        {
            return NotFound();
        }

        ProductAttributeDto attributeDto =
            _mapper.ToDto(attribute);

        return Ok(attributeDto);
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