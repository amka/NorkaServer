namespace Norka.API.Models.Request;

public class UpdateNoteRequest
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool? IsArchived { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsEncrypted { get; set; }
}