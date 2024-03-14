using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Norka.API.Models.Request;

public class CreateApiTokenRequest
{
    [Required]
    public string Title { get; set; }

    [Required] [DefaultValue(30)]
    public int ExpiresIn { get; set; }
}