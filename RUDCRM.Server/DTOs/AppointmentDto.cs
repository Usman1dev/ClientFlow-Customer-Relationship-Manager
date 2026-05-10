using System.ComponentModel.DataAnnotations;

namespace RUDCRM.Server.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartTime { get; set; } = DateTime.Now;

    [Required]
    public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);

    [MaxLength(200)]
    public string? Location { get; set; }

    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
}
