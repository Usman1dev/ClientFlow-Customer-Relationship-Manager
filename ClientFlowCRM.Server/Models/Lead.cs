using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientFlowCRM.Server.Models;

public class Lead
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ContactName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Company { get; set; }

    [Required]
    [MaxLength(30)]
    public string Source { get; set; } = "Web"; // Web, Referral, Social, Advertisement, Other

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "New"; // New, Contacted, Qualified, Lost, Converted

    [Column(TypeName = "decimal(18,2)")]
    public decimal EstimatedValue { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key
    public string? AssignedToUserId { get; set; }

    [ForeignKey("AssignedToUserId")]
    public ApplicationUser? AssignedToUser { get; set; }
}
