namespace Norka.API.Models.Request;

public class CreateNoteRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    
    public bool IsArchived { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public bool IsEncrypted { get; set; } = false;
}