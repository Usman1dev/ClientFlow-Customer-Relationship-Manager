using System.ComponentModel.DataAnnotations;

namespace RUDCRM.Server.DTOs;

public class LeadDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Contact name is required")]
    [MaxLength(100)]
    public string ContactName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Company { get; set; }

    [Required]
    public string Source { get; set; } = "Web";

    [Required]
    public string Status { get; set; } = "New";

    public decimal EstimatedValue { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
}
