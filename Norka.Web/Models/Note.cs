using System.ComponentModel.DataAnnotations;
using NanoidDotNet;

namespace Norka.Web.Models;

public class Note
{
    [MaxLength(32)] public string Id { get; set; } = Nanoid.Generate();

    // TODO: Replace with ApplicationUserId
    [MaxLength(32)] public required string UserId { get; set; }

    [MaxLength(500)] public string? Title { get; set; }
    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    public bool IsEncrypted { get; set; } = false;
}