namespace JWT_Token_Testing.Models;

public class Users
{
    public string? UserName { get; set; }
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}