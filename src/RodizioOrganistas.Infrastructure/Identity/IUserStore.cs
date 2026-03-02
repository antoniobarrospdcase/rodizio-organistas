namespace RodizioOrganistas.Infrastructure.Identity;

public interface IUserStore
{
    bool ValidateCredentials(string username, string password);
}
