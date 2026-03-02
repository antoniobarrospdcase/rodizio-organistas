namespace RodizioOrganistas.Infrastructure.Identity;

public sealed class AdminCredentialsOptions
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
