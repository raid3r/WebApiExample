using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApiExample.Models;

public class SiteContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public SiteContext(DbContextOptions options) : base(options) { }

    public DbSet<ToDoListItem> ToDoListItems { get; set; }
}