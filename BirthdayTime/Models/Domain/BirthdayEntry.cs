using System.ComponentModel.DataAnnotations;

namespace BirthdayTime.Models.Domain;

public class BirthdayEntry
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public string? Email { get; set; }

    public byte[]? Photo { get; set; }
}

