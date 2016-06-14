using System.ComponentModel.DataAnnotations;


namespace OpenData.Framework.WebApp.Models
{
    public class SchemaViewModel
    {
        [Required]
        [Display(Name = "ProviderName")]
        public string Name { get; set; }
    }
}