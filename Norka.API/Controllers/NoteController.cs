using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Norka.API.Data;
using Norka.API.Entities;
using Norka.API.Models;
using Norka.API.Models.Request;

namespace Norka.API.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class NoteController(NorkaDbContext db, UserManager<ApplicationUser> userManager) : Controller
{
    // GET
    [HttpGet]
    public async Task<List<Note>> GetTodosAsync()
    {
        var userId = userManager.GetUserId(HttpContext.User);
        return await db.Notes.Where(t => t.AuthorId == userId).ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodoAsync(CreateNoteRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = userManager.GetUserId(HttpContext.User);

        var noteItem = new Note
        {
            AuthorId = userId!,
            Title = request.Title,
            Content = request.Content,
        };

        db.Notes.Add(noteItem);
        await db.SaveChangesAsync();

        return CreatedAtAction("CreateTodo", new { id = noteItem.Id }, noteItem);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoAsync(string id)
    {
        var note = await db.Notes.Where(t => t.Id == id).FirstOrDefaultAsync();
        if (note == null) return NotFound();

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoAsync(string id)
    {
        var userId = userManager.GetUserId(HttpContext.User);
        var note = await db.Notes.Where(t =>
            t.Id == id && t.AuthorId == userId).FirstOrDefaultAsync();
        if (note == null) return NotFound();

        db.Notes.Remove(note);
        await db.SaveChangesAsync();

        return Ok();
    }
}