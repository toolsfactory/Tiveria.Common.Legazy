using System;
using System.ComponentModel.DataAnnotations;

namespace MVVMDemo
{
    public class Person// : Tiveria.Common.MVVM.ModelBase
    {
        [Required(ErrorMessage = "EMail is required")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$",
            ErrorMessage = "Wrong Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }

        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        [Required]
        [DataType("BirthDate", ErrorMessage = "Birthdate must be in past")]
        public DateTime BirthDate { get; set; }

        public Person()
        {
            BirthDate = DateTime.Now;
        }
    }
}
