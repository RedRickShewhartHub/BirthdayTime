using BirthdayTime.Models.Domain;
using BirthdayTime.Models.DTOs;
using BirthdayTime.Services.Implementations;
using BirthdayTime.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayTime.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BirthdayController : ControllerBase
{
    private readonly IBirthdayService _service;

    public BirthdayController(IBirthdayService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entries = await _service.GetAllAsync();
        return Ok(entries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entry = await _service.GetByIdAsync(id);
        if (entry == null)
            return NotFound();
        return Ok(entry);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BirthdayEntry entry)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.AddAsync(entry);
        return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
    }

    [HttpPost("with-photo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateWithPhoto([FromForm] BirthdayEntryWithPhotoDto form)
    {
        byte[]? photoBytes = null;

        if (form.Photo != null)
        {
            using var ms = new MemoryStream();
            await form.Photo.CopyToAsync(ms);
            photoBytes = ms.ToArray();
        }

        var entry = new BirthdayEntry
        {
            Id = Guid.NewGuid(),
            FullName = form.FullName,
            DateOfBirth = form.DateOfBirth,
            Email = form.Email,
            Photo = photoBytes
        };

        await _service.AddAsync(entry);

        return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BirthdayEntry entry)
    {
        if (id != entry.Id)
            return BadRequest();

        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _service.UpdateAsync(entry);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/upload-photo")]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile file)
    {
        var entry = await _service.GetByIdAsync(id);
        if (entry == null)
            return NotFound();

        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран");

        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            entry.Photo = ms.ToArray();
        }

        await _service.UpdateAsync(entry);
        return Ok(new { entry.Id });
    }

    [HttpGet("{id}/photo")]
    public async Task<IActionResult> GetPhoto(Guid id)
    {
        var entry = await _service.GetByIdAsync(id);
        if (entry == null || entry.Photo == null)
            return NotFound();

        return File(entry.Photo, "image/jpeg");
    }
}
