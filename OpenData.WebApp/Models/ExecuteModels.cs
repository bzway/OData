using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenData.WebApp.Models
{
    public class ExecuteModel
    {
        [Required]
        [Display(Name = "Email")]


        public string Js { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
    }
}