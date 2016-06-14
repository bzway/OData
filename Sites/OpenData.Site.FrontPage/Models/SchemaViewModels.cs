using System.ComponentModel.DataAnnotations;


namespace OpenData.Site.FrontPage.Models
{
    public class SchemaViewModel
    {
        [Required]
        [Display(Name = "ProviderName")]
        public string Name { get; set; }
    }
}