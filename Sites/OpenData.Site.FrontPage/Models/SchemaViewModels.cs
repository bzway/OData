using System.ComponentModel.DataAnnotations;


namespace OpenData.Sites.FrontPage.Models
{
    public class SchemaViewModel
    {
        [Required]
        [Display(Name = "ProviderName")]
        public string Name { get; set; }
    }
}