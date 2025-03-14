using System.ComponentModel.DataAnnotations;

namespace WebApiExample.Models.Rest;

public class ToDoListItemRest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsComplete { get; set; }
    public DateOnly CreatedAt { get; set; }
}
