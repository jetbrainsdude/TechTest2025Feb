using System.ComponentModel.DataAnnotations;

namespace Lexxika.Documents.Models
{
    /// <summary>
    /// Contains attempted login credentials.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        [StringLength(60, ErrorMessage = "The user name must be at least {2} characters long.", MinimumLength = 2)]
        public string UserName { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required]
        [StringLength(10, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
