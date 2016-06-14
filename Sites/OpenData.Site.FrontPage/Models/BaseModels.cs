using OpenData.Common;
using OpenData.Data;
using System.ComponentModel.DataAnnotations;

namespace OpenData.Site.FrontPage.Models
{
    public class SearchViewModel
    {
        [Required]
        public string Name { get; set; }
        public PagedList<DynamicEntity> SearchResult { get; set; }
    }
    public class ImportFileViewModel
    {
        [Required]
        public string Name { get; set; }
    }
    public class ExecuteModel
    {
        [Required]
        [Display(Name = "Email")]


        public string Js { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
    }
}