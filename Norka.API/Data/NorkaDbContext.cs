using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Norka.API.Entities;

namespace Norka.API.Data;

public class NorkaDbContext : IdentityDbContext<ApplicationUser, UptimedRole, string>
{
    public NorkaDbContext(DbContextOptions<NorkaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Note> Notes { get; set; }

    public DbSet<ApiToken> ApiTokens { get; set; }
}