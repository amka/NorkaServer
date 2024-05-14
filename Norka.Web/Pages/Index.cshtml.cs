using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Norka.Web.Models;
using Norka.Web.Services;

namespace Norka.Web.Pages;

[Authorize]
public class IndexModel(INoteService noteService, ILogger<IndexModel> logger) : PageModel
{
    public IEnumerable<Note> Notes { get; set; }

    public async Task<IActionResult> OnGetAsync([FromServices] UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.GetUserAsync(User);
        var userId = user?.Id;
        if (userId == null) return Unauthorized();
        Notes = await noteService.GetNotesAsync(userId, pageSize: PageSize, page: PageNum);
        return Page();
    }

    [BindProperty(SupportsGet = true)] public int? PageNum { get; set; } = 1;

    [BindProperty(SupportsGet = true)] public int? PageSize { get; set; } = 10;

    public async Task<IActionResult> OnFetchNotesAsync([FromServices] UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.GetUserAsync(User);
        var userId = user?.Id;
        if (userId == null) return Unauthorized();
        Notes = await noteService.GetNotesAsync(userId, pageSize: PageSize, page: PageNum);

        return new JsonResult(Notes);
    }
}