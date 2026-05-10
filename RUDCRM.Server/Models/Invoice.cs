using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RUDCRM.Server.Models;

public class Invoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(30);

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue, Cancelled

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;

    [ForeignKey("CreatedByUserId")]
    public ApplicationUser? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
