using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{

    public class StudentViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; } = default!;

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        [Required(ErrorMessage = "Enrollment date is required")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
