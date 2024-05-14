using Microsoft.EntityFrameworkCore;
using Norka.Web.Data;
using Norka.Web.Models;

namespace Norka.Web.Services;

public class NoteService(NorkaDbContext db, ILogger<NoteService> logger, IEncryptionService encryptionService)
    : INoteService
{
    public async Task<IEnumerable<Note>> GetNotesAsync(string userId, int? page, int? pageSize)
    {
        return await db.Notes
            .AsNoTracking()
            .Where(n => n.ApplicationUserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page ?? 0) * (pageSize ?? 10))
            .Take(pageSize ?? 10)
            .ToListAsync();
    }

    public async Task<Note?> GetNoteAsync(string userId, string id)
    {
        return await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.ApplicationUserId == userId);
    }

    public async Task<Note> CreateNoteAsync(Note note)
    {
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;
        await db.Notes.AddAsync(note);
        return note;
    }

    public async Task<Note?> UpdateNoteAsync(string userId, Note note)
    {
        var existingNote = db.Notes.FirstOrDefault(n => n.Id == note.Id && n.ApplicationUserId == userId);
        if (existingNote == null) return null;
        existingNote.Content = note.Content ?? existingNote.Content;
        existingNote.Title = note.Title ?? existingNote.Title;
        existingNote.UpdatedAt = DateTime.UtcNow;

        db.Notes.Update(note);
        await db.SaveChangesAsync();
        return note;
    }

    public async Task ArchiveNoteAsync(string userId, string id)
    {
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.ApplicationUserId == userId);
        if (note == null) return;

        note.UpdatedAt = DateTime.UtcNow;
        note.IsArchived = true;

        await db.SaveChangesAsync();
    }

    public async Task DeleteNoteAsync(string userId, string id)
    {
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.ApplicationUserId == userId);
        if (note == null) return;

        note.DeletedAt = DateTime.UtcNow;
        note.IsDeleted = true;

        await db.SaveChangesAsync();
    }

    public async Task EncryptNotesAsync(string userId, string id, string password)
    {
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.ApplicationUserId == userId);
        if (note == null) return;

        note.Content = await encryptionService.EncryptAsync(password, note.Content);
        note.IsEncrypted = true;
        await db.SaveChangesAsync();
    }
}