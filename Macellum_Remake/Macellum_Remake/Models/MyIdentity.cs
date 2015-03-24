namespace Macellum_Remake.Models
{
    /// <summary>
    /// Custom identity
    /// </summary>
    public class MyIdentity : System.Security.Principal.IIdentity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public MyIdentity(string name)
        {
            Name = name;
        }


        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Name); } 
        }
    }
}