
namespace Tiveria.Common.Security
{
    public interface IUserManagementProvider
    {
        bool IsValidUsernameAndPassword(string username, string password);
        string GetUserRoles(string username);
    }
}
