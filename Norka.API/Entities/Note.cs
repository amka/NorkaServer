using System.Text.Json.Serialization;
using NanoidDotNet;

namespace Norka.API.Entities;

public class Note
{
    public string Id { get; set; } = Nanoid.Generate();

    public string AuthorId { get; set; }
    [JsonIgnore] public ApplicationUser Author { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }

    public bool IsArchived { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public bool IsEncrypted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastModifiedAt { get; set; } = DateTime.Now;
    public DateTime? SyncedAt { get; set; } = null;
}