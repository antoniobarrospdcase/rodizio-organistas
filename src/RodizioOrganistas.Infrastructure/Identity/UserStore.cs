using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace RodizioOrganistas.Infrastructure.Identity;

public sealed class UserStore(IOptions<AdminCredentialsOptions> credentialsOptions) : IUserStore
{
    private readonly AdminCredentialsOptions _credentials = credentialsOptions.Value;
    private readonly PasswordHasher<string> _passwordHasher = new();

    public bool ValidateCredentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(_credentials.Username) || string.IsNullOrWhiteSpace(_credentials.PasswordHash))
        {
            throw new InvalidOperationException(
                "Admin credentials are not configured. Set Authentication:Admin:Username and Authentication:Admin:PasswordHash.");
        }

        if (!string.Equals(username, _credentials.Username, StringComparison.Ordinal))
        {
            return false;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(username, _credentials.PasswordHash, password);
        return verificationResult is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
