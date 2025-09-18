using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentViewModel Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var studentEntity = await _context.Students.FindAsync(id);
            if (studentEntity == null)
                return NotFound();

            // Map entity to ViewModel
            Student = new StudentViewModel
            {
                ID = studentEntity.ID,
                FirstMidName = studentEntity.FirstMidName,
                LastName = studentEntity.LastName,
                EnrollmentDate = studentEntity.EnrollmentDate
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Find the existing entity
            var studentToUpdate = await _context.Students.FindAsync(Student.ID);
            if (studentToUpdate == null)
                return NotFound();

            // Map ViewModel back to entity
            studentToUpdate.FirstMidName = Student.FirstMidName;
            studentToUpdate.LastName = Student.LastName;
            studentToUpdate.EnrollmentDate = Student.EnrollmentDate;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Student {Student.FirstMidName} {Student.LastName} updated successfully.";

            return RedirectToPage("./Index");
        }
    }
}
