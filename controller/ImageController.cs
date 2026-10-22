using Microsoft.AspNetCore.Mvc;
using webShop2.dto.image;
using webShop2.mapper;
using webShop2.model;
using webShop2.services.core;
namespace webShop2.controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _service;
    private readonly ImageMapper _mapper;

    public ImageController(
        IImageService service,
        ImageMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost("upload/{productId}")]
    [RequestSizeLimit(10_000_000)]
    public async Task<ActionResult<ImageDto>> Upload(
    int productId,
    IFormFile file,
    [FromForm] bool isMain = false)
    {
        Console.WriteLine("UPLOAD DEBUG: usao u metodu");
        Console.WriteLine($"ProductId: {productId}");
        Console.WriteLine($"File postoji: {file != null}");
        Console.WriteLine($"File size: {file?.Length ?? 0}");

        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "Nije poslata slika ili je fajl prazan..."
                });
            }

            Image? image = await _service.UploadAsync(
                productId,
                file,
                isMain
            );

            Console.WriteLine(
                $"UPLOAD SERVICE RESULT: {(image == null ? "null" : $"Image ID = {image.Id}")}"
            );

            if (image == null)
            {
                return NotFound(new
                {
                    message = "Proizvod ne postoji."
                });
            }

            ImageDto dto = _mapper.ToDto(image);

            Console.WriteLine("UPLOAD MAPPER: uspešno mapiranje slike");

            return CreatedAtAction(
                nameof(GetById),
                new { id = image.Id },
                dto
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine("UPLOAD GRESKA:");
            Console.WriteLine(ex.ToString());

            return StatusCode(500, new
            {
                message = "Greška prilikom uploada slike.",
                detail = ex.Message
            });
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ImageDto>>> GetAll()
    {
        List<Image> images =
            await _service.GetAllAsync();

        List<ImageDto> imagesDto =
            new List<ImageDto>();

        foreach (Image image in images)
        {
            ImageDto dto = _mapper.ToDto(image);
            imagesDto.Add(dto);
        }

        return Ok(imagesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImageDto>> GetById(int id)
    {
        Image? image =
            await _service.GetByIdAsync(id);

        if (image == null)
        {
            return NotFound();
        }

        ImageDto dto = _mapper.ToDto(image);

        return Ok(dto);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<List<ImageDto>>> GetByProduct(
        int productId)
    {
        List<Image> images =
            await _service.GetByProductAsync(productId);

        List<ImageDto> imagesDto =
            new List<ImageDto>();

        foreach (Image image in images)
        {
            ImageDto dto = _mapper.ToDto(image);
            imagesDto.Add(dto);
        }

        return Ok(imagesDto);
    }

    [HttpGet("product/{productId}/main")]
    public async Task<ActionResult<ImageDto>> GetMainImage(
        int productId)
    {
        Image? image =
            await _service.GetMainImageAsync(productId);

        if (image == null)
        {
            return NotFound();
        }

        ImageDto dto = _mapper.ToDto(image);

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ImageDto>> Create(
        CreateImageDto dto)
    {
        Image image = _mapper.ToEntity(dto);

        Image createdImage =
            await _service.CreateAsync(image);

        ImageDto imageDto =
            _mapper.ToDto(createdImage);

        return CreatedAtAction(
            nameof(GetById),
            new { id = imageDto.Id },
            imageDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ImageDto>> Update(
        int id,
        UpdateImageDto dto)
    {
        Image? existingImage =
            await _service.GetByIdAsync(id);

        if (existingImage == null)
        {
            return NotFound();
        }

        _mapper.UpdateEntity(existingImage, dto);

        Image? updatedImage =
            await _service.UpdateAsync(existingImage);

        if (updatedImage == null)
        {
            return NotFound();
        }

        ImageDto imageDto =
            _mapper.ToDto(updatedImage);

        return Ok(imageDto);
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

    [HttpPut("{id}/main")]
    public async Task<ActionResult<ImageDto>> SetMain(int id)
    {
        Image? image = await _service.SetMainAsync(id);

        if (image == null)
        {
            return NotFound();
        }

        return Ok(_mapper.ToDto(image));
    }
}