
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAll(
        int page = 1,
        int pageSize = 10)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination parameters.");
        }

        PagedResult<Product> result =
            await _service.GetAllAsync(page, pageSize);

        PagedResult<ProductDto> dto =
            new PagedResult<ProductDto>
            {
                Items = result.Items
                    .Select(x => _mapper.ToDto(x))
                    .ToList(),

                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };

        return Ok(dto);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        Product? product = await _service.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        ProductDto dto = _mapper.ToDto(product);

        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(30_000_000)]
    public async Task<ActionResult<ProductDto>> Create(
        [FromForm] CreateProductDto dto)
    {
        Product productCreate = _mapper.ToEntity(dto);

        Product? product;

        try
        {
            product = await _service.CreateAsync(
                productCreate,
                dto.Images);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }

        if (product == null)
        {
            return BadRequest(new
            {
                message = "Proizvod nije kreiran. Proveri da li kategorija postoji."
            });
        }

        ProductDto productDto = _mapper.ToDto(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            productDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> Update(
    int id,
    [FromBody] UpdateProductDto dto)
    {
        Product productUpdate = _mapper.ToEntityUpdate(dto);

        Product? product =
            await _service.UpdateAsync(id, productUpdate);

        if (product == null)
        {
            return NotFound();
        }

        ProductDto productDto = _mapper.ToDto(product);

        return Ok(productDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
