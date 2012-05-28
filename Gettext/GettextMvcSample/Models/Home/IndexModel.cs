using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GettextMvcSample.Models.Home
{
    public class IndexModel : BaseModel
    {
        public string Message { get; set; }


        [Required(ErrorMessage = "Hello world from Required attribute")]
        [Display(Name = "Email Field")]
        public string Email { get; set; }

        [MaxLength(5, ErrorMessage = "Hello world from MaxLength attribute")]
        [Display(Name = "Too Long Field")]
        public string TooLong { get; set; }


    }
}