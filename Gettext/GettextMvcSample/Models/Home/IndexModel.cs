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

        public IndexAnotherModel AnotherModel { get; set; }

        public class IndexAnotherModel : BaseModel, IValidatableObject
        {
            [Display(Name = "Password")]
            public string Password { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                return new[]
                       {
                           new ValidationResult("Hello world from custom validator",
                                                new[]
                                                    {
                                                        "Password"
                                                    }),
                       };
            }
        }

        public IndexModel()
        {
            
        }
    }

}