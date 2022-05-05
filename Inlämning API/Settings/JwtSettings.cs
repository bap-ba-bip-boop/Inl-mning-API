namespace Inlämning_API.Settings;

public class JwtSettings
{
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public string? Key { get; set; }
}
