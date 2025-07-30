using System.ComponentModel.DataAnnotations;

namespace BirthdayTime.Models.DTOs;

public class BirthdayEntryDto
{
    public Guid Id { get; set; } 
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhotoPath { get; set; }
}

public class BirthdayEntryWithPhotoDto
{
    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    public IFormFile? Photo { get; set; }
}