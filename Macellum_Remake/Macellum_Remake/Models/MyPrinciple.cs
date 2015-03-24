using System.Security.Principal;

namespace Macellum_Remake.Models
{
    /// <summary>
    /// Custom principle
    /// </summary>
    public class MyPrinciple : IPrincipal
    {
        readonly DatabaseRepo _repo = new DatabaseRepo();

        /// <summary>
        /// Attribute
        /// </summary>
        public bool IsInRoleAttribute { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="identity"></param>
        public MyPrinciple(MyIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            var usrRole = _repo.GetRoleByUsername(SimpleSessionPersister.Username);
            var isRole = usrRole.Equals(role);
            IsInRoleAttribute = isRole;
            return isRole;
        }

        public IIdentity Identity { get; private set; }
    }
}