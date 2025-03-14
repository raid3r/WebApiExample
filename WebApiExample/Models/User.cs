using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApiExample.Models;

public class User : IdentityUser<int>
{
    public virtual ICollection<ToDoListItem> ToDoListItems { get; set; } = new List<ToDoListItem>();
}
