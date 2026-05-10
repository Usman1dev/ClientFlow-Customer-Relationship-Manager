using System.ComponentModel.DataAnnotations;

namespace ClientFlowCRM.Server.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Customer is required")]
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(30);

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal TotalAmount { get; set; }

    [Required]
    public string Status { get; set; } = "Draft";

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public decimal TotalPaid { get; set; }
}

public class PaymentDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Invoice is required")]
    public int InvoiceId { get; set; }
    public string? InvoiceNumber { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.Now;

    [Required]
    public string PaymentMethod { get; set; } = "Cash";

    [MaxLength(50)]
    public string? ReferenceNumber { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}
