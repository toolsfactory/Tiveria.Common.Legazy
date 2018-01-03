using System;
using System.Linq;
using System.Threading;

namespace Tiveria.Common.Security
{
    public abstract class UserManagerBase
    {

        protected IUserManagementProvider _Provider;
        private string _Username;
        private string[] _UserRoles;

        public bool Enabled
        { get { return (_Provider != null); } }

        public string CurentUser
        { get { return _Username; } }

        public void SetProvider(IUserManagementProvider provider)
        {
            if (provider == null)
                throw new Exception("no valid provider set");

            _Provider = provider;
        }

        public bool Login(string username, string password)
        {
            if (!Enabled)
                return false;

            bool result = _Provider.IsValidUsernameAndPassword(username, password);
            if (result)
            {
                _Username = username;
                string roles = _Provider.GetUserRoles(username).ToLower();
                 _UserRoles = roles.Split(",".ToCharArray(), 100, StringSplitOptions.RemoveEmptyEntries);
                for(int i=0; i<_UserRoles.Count(); i++)
                    _UserRoles[i] = _UserRoles[i].Trim();
                CustomPrincipal principal = new CustomPrincipal(new CustomIdentity(this), this);
                AppDomain currentdomain = Thread.GetDomain();
                currentdomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.UnauthenticatedPrincipal);
                Thread.CurrentPrincipal = principal;
                AfterBaseLogin();
            }
            return result;
        }

        public bool IsInRole(string role)
        {
            if (!Enabled)
                return false;

            return _UserRoles.Contains(role.ToLower());
        }

        public bool HasRoles()
        {
            if (!Enabled)
                return false;

            return _UserRoles.Count()>0;
        }

        protected virtual void AfterBaseLogin()
        {}
    }
}
