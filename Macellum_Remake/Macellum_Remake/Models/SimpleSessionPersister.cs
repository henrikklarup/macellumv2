using System.Web;

namespace Macellum_Remake.Models
{
    /// <summary>
    /// Simple session
    /// </summary>
    public class SimpleSessionPersister
    {
// ReSharper disable ConvertToConstant.Local
        private static readonly string UsernameSessionVar = "username";
// ReSharper restore ConvertToConstant.Local

        /// <summary>
        /// This is the saved var
        /// </summary>
        public static string Username
        {
            get
            {
                if (HttpContext.Current == null) return string.Empty;
                var sessionVar = HttpContext.Current.Session[UsernameSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set { HttpContext.Current.Session[UsernameSessionVar] = value; }
        }
    }
}