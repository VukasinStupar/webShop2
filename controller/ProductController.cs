using Microsoft.AspNetCore.Mvc;
using webShop2.dto.productDto;
using webShop2.dto.ProductDto;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ProductMapper _mapper;

    public ProductController(
        IProductService service,
        ProductMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        List<Product> products =
            await _service.GetAllAsync();

        List<ProductDto> dto =
            new List<ProductDto>();

        foreach (Product product in products)
        {
            dto.Add(_mapper.ToDto(product));
        }

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        Product? product =
            await _service.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        ProductDto dto =
            _mapper.ToDto(product);

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        CreateProductDto dto)
    {
        Product productCreate =
            _mapper.ToEntity(dto);

        Product product =
            await _service.CreateAsync(productCreate);

        ProductDto productDto =
            _mapper.ToDto(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = productDto.Id },
            productDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(
        int id,
        CreateProductDto dto)
    {
        Product productUpdate = _mapper.ToEntity(dto);

        Product? product = await _service.UpdateAsync(id, productUpdate);

        if (product == null)
        {
            return NotFound();
        }

        ProductDto productDto =
            _mapper.ToDto(product);

        return Ok(productDto);
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