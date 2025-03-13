namespace WebApiExample.Models;

public class ToToListItemFilter
{
    public string? Title { get; set; }
    public bool? IsComplete { get; set; }
    public DateOnly? CreatedAt { get; set; }
}
