using System;
using System.Security.Principal;

namespace Tiveria.Common.Security
{
    /// <summary>
    /// Implements a custom Principal class that used the AspNetDb database
    /// to check for role membership
    /// </summary>
    [Serializable()]
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _Identity;
        private UserManagerBase _UserManager;

        public CustomPrincipal(CustomIdentity identity,UserManagerBase usermanager)
        {
            if ((usermanager == null) || (identity == null))
                throw new ArgumentNullException();
            _UserManager = usermanager;
            _Identity = identity;    
        }

        #region IPrincipal Members

        IIdentity IPrincipal.Identity
        {
            get { return _Identity; }
        }

        bool IPrincipal.IsInRole(string role)
        {
            return _UserManager.IsInRole(role);
        }

        #endregion
    }
}