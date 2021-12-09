using AspBlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AspBlazorApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {

    }

    public DbSet<ToDo> ToDos { get; set; }
}
