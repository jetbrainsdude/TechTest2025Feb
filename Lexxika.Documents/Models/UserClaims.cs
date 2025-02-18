using System.Security.Claims;
using System.Security.Principal;

namespace Lexxika.Documents.Models
{
    /// <summary>
    /// User claims data extracted from JSON Web Token.
    /// </summary>
    public class UserClaims
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">Claims principal.</param>
        public UserClaims(IPrincipal user)
        {
            try
            {
                // initialise
                IsAdmin = false;
                UserName = string.Empty;

                var claimsPrincipal = user as ClaimsPrincipal;
                if (claimsPrincipal != null)
                {
                    foreach (var claim in claimsPrincipal.Claims)
                    {
                        if (claim.Type == "UserName")
                        {
                            UserName = claim.Value;
                        }
                        else if (claim.Type == ClaimTypes.Role)
                        {
                            IsAdmin = claim.Value == "Admin";
                        }
                    }
                }

                IsValid = UserName != string.Empty;
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// User name of authenticated user.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Is authenticated user an administrator.
        /// </summary>
        public bool IsAdmin { get; }

        /// <summary>
        /// Authentication completed successfully.
        /// </summary>
        public bool IsValid { get; }
        #endregion
    }
}
