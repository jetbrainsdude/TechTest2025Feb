using System.ComponentModel.DataAnnotations;

namespace Lexxika.Documents.Models
{
    /// <summary>
    /// Document for translation.
    /// </summary>
    public class TranslationDocument
    {
        /// <summary>
        /// Id of document.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// User Id of document.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Translation text.
        /// </summary>
        [Required]
        [StringLength(500, ErrorMessage = $"The document title may not be empty", MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>
        /// Translation text.
        /// </summary>
        [Required]
        [StringLength(900, ErrorMessage = $"The document may not be empty", MinimumLength = 1)]
        public string TranslationText { get; set; }
    }
}
