using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

using System;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;

        public CreateModel(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public StudentViewModel Student { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Map the ViewModel to the Student entity
            var newStudent = new Student
            {
                FirstMidName = Student.FirstMidName,
                LastName = Student.LastName,
                EnrollmentDate = Student.EnrollmentDate
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            // Optionally use TempData to show a success message (PRG pattern)
            TempData["SuccessMessage"] = $"Student {newStudent.FirstMidName} {newStudent.LastName} created successfully.";

            return RedirectToPage("./Index");
        }
    }
}