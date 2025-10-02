using System.Collections.Generic;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

public static class Utilities
{
    // For your main SchoolContext database (students, courses, enrollments)
    public static void ReinitializeDbForTests(SchoolContext context)
    {
        context.Enrollments.RemoveRange(context.Enrollments);
        context.Courses.RemoveRange(context.Courses);
        context.Students.RemoveRange(context.Students);
        context.SaveChanges();

        // Re-seed database
        DbInitializer.Initialize(context);
    }
}