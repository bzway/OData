using System.ComponentModel.DataAnnotations;

namespace OpenData.Framework.WebApp.Models
{
    public class FakeViewModel
    {

        [Display(Name = "token")]
        public string token { get; set; }


        [Display(Name = "signature")]
        public string signature { get; set; }

        [Display(Name = "timestamp")]
        public string timestamp { get; set; }


        [Display(Name = "nonce")]
        public string nonce { get; set; }

        [Display(Name = "data")]
        public string data { get; set; }


        [Display(Name = "result")]
        public string result { get; set; }
    }
}
