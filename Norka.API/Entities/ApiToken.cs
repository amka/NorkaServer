using System.Text.Json.Serialization;

namespace Norka.API.Entities;

public class ApiToken
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; }
    public string UserId { get; set; }
    [JsonIgnore] public ApplicationUser User { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
}