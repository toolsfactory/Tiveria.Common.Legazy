using System;
using System.Security.Principal;

namespace Tiveria.Common.Security
{
    /// <summary>
    /// Implements a custom Identity class for use with the AspNetDb
    /// </summary>
    [Serializable()]
    public class CustomIdentity : IIdentity
    {
        private UserManagerBase _UserManager;
        public CustomIdentity(UserManagerBase usermanager)
        {
            if (usermanager == null)
                throw new ArgumentNullException();
            _UserManager = usermanager;
        }

        #region IIdentity Members

        string IIdentity.AuthenticationType
        {
            get { return "Tiveria UserManager"; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return _UserManager.HasRoles(); }
        }

        string IIdentity.Name
        {
            get { return _UserManager.CurentUser; }
        }

        #endregion
    }
}