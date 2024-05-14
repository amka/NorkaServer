using Norka.Web.Models;

namespace Norka.Web.Services;

public interface INoteService
{
    public Task<IEnumerable<Note>> GetNotesAsync(string userId, int? page = 1, int? pageSize = 10);
    public Task<Note?> GetNoteAsync(string userId, string id);
    public Task<Note> CreateNoteAsync(Note note);
    public Task<Note?> UpdateNoteAsync(string userId, Note note);
    public Task ArchiveNoteAsync(string userId, string id);
    public Task DeleteNoteAsync(string userId, string id);
    public Task EncryptNotesAsync(string userId, string id, string password);
}