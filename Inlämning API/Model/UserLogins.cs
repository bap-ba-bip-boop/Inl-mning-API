using System.ComponentModel.DataAnnotations;

namespace JWT_Token_Testing.Models;

public class UserLogins
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}