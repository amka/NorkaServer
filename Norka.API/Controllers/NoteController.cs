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
    /// <summary>
    /// Get all notes that you created.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<Note>> GetNotesAsync()
    {
        var userId = userManager.GetUserId(HttpContext.User);
        return await db.Notes.Where(t => t.AuthorId == userId).ToListAsync();
    }

    /// <summary>
    /// Create new note and return it. Current user will be the author.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateNoteAsync(CreateNoteRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = userManager.GetUserId(HttpContext.User);

        var noteItem = new Note
        {
            AuthorId = userId!,
            Title = request.Title,
            Content = request.Content,
            IsArchived = request.IsArchived,
            IsDeleted = request.IsDeleted,
            IsEncrypted = request.IsEncrypted,
            CreatedAt = DateTime.Now,
            LastModifiedAt = DateTime.Now
        };

        db.Notes.Add(noteItem);
        await db.SaveChangesAsync();

        return CreatedAtAction("CreateNote", new { id = noteItem.Id }, noteItem);
    }

    /// <summary>
    /// Get note by id or return 404.
    /// Note that you can only get notes that you created.
    /// </summary>
    /// <param name="id">Note id</param>
    /// <returns>Note</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteAsync(string id)
    {
        var userId = userManager.GetUserId(HttpContext.User);
        var note = await db.Notes
            .Where(t => t.Id == id && t.AuthorId == userId)
            .FirstOrDefaultAsync();
        if (note == null) return NotFound();

        return Ok(note);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateNoteAsync(string id, UpdateNoteRequest request)
    {
        var userId = userManager.GetUserId(HttpContext.User);
        var note = await db.Notes
            .Where(t => t.Id == id && t.AuthorId == userId)
            .FirstOrDefaultAsync();
        if (note == null) return NotFound();

        // Update presented fields only
        note.Title = request.Title ?? note.Title;
        note.Content = request.Content ?? note.Content;
        note.IsArchived = request.IsArchived ?? note.IsArchived;
        note.IsDeleted = request.IsDeleted ?? note.IsDeleted;
        note.IsEncrypted = request.IsEncrypted ?? note.IsEncrypted;
        note.LastModifiedAt = DateTime.Now;
        await db.SaveChangesAsync();

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNoteAsync(string id)
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