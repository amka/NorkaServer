using Microsoft.AspNetCore.Identity;

namespace Norka.API.Models;

public class ApplicationUser: IdentityUser
{
    public IEnumerable<Note> Todos { get; set; }
}