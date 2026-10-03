using Microsoft.AspNetCore.Mvc;
using webShop2.dto.category;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;

namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly CategoryMapper _mapper;

    public CategoryController(
        ICategoryService service,
        CategoryMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        List<Category> categories =
            await _service.GetAllAsync();

        List<CategoryDto> categoriesDto =
            new List<CategoryDto>();

        foreach (Category category in categories)
        {
            CategoryDto dto = _mapper.ToDto(category);
            categoriesDto.Add(dto);
        }

        return Ok(categoriesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        Category? category =
            await _service.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        CategoryDto dto = _mapper.ToDto(category);

        return Ok(dto);
    }

    [HttpGet("root")]
    public async Task<ActionResult<List<CategoryDto>>> GetRootCategories()
    {
        List<Category> categories =
            await _service.GetRootCategoriesAsync();

        List<CategoryDto> categoriesDto =
            new List<CategoryDto>();

        foreach (Category category in categories)
        {
            CategoryDto dto = _mapper.ToDto(category);
            categoriesDto.Add(dto);
        }

        return Ok(categoriesDto);
    }

    [HttpGet("{parentId}/children")]
    public async Task<ActionResult<List<CategoryDto>>> GetChildren(
        int parentId)
    {
        List<Category> categories =
            await _service.GetChildrenAsync(parentId);

        List<CategoryDto> categoriesDto =
            new List<CategoryDto>();

        foreach (Category category in categories)
        {
            CategoryDto dto = _mapper.ToDto(category);
            categoriesDto.Add(dto);
        }

        return Ok(categoriesDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
        CreateCategoryDto dto)
    {
        Category category = _mapper.ToEntity(dto);

        Category createdCategory =
            await _service.CreateAsync(category);

        CategoryDto categoryDto =
            _mapper.ToDto(createdCategory);

        return CreatedAtAction(
            nameof(GetById),
            new { id = categoryDto.Id },
            categoryDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(
        int id,
        UpdateCategoryDto dto)
    {
        Category category = new Category
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        Category? updatedCategory =
            await _service.UpdateAsync(id, category);

        if (updatedCategory == null)
        {
            return NotFound();
        }

        CategoryDto categoryDto =
            _mapper.ToDto(updatedCategory);

        return Ok(categoryDto);
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