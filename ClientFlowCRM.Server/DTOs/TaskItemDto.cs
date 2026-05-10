using System.ComponentModel.DataAnnotations;

namespace ClientFlowCRM.Server.DTOs;

public class TaskItemDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    [Required]
    public string Priority { get; set; } = "Medium";

    [Required]
    public string Status { get; set; } = "Pending";

    public string? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
}
