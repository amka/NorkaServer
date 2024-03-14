using Microsoft.AspNetCore.Identity;
using Norka.API.Models;

namespace Norka.API.Entities;

public class ApplicationUser: IdentityUser
{
    public IEnumerable<Note> Notes { get; set; }
}