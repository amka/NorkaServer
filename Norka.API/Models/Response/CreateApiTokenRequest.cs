namespace Norka.API.Models.Response;

public class CreateApiTokenResponse
{
    public string Title { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}