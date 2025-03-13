using System.ComponentModel.DataAnnotations;

namespace WebApiExample.Models;

public class ToDoListItem
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
    public DateOnly CreatedAt { get; set; }
}
