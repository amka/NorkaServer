using Microsoft.AspNetCore.Identity;

namespace Norka.Web.Models;

public class ApplicationUser : IdentityUser
{
    
    public IEnumerable<Note> Notes { get; set; }
}