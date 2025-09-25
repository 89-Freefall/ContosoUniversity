using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            // --- Students ---
            var alexander = new Student { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2016-09-01") };
            var alonso = new Student { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2018-09-01") };
            var anand = new Student { FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2019-09-01") };
            var barzdukas = new Student { FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2018-09-01") };
            var li = new Student { FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2018-09-01") };
            var justice = new Student { FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2017-09-01") };
            var norman = new Student { FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2019-09-01") };
            var olivetto = new Student { FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2011-09-01") };

            var students = new Student[] { alexander, alonso, anand, barzdukas, li, justice, norman, olivetto };
            context.AddRange(students);

            // --- Instructors ---
            var abercrombie = new Instructor { FirstMidName = "Kim", LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") };
            var fakhouri = new Instructor { FirstMidName = "Fadi", LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06") };
            var harui = new Instructor { FirstMidName = "Roger", LastName = "Harui", HireDate = DateTime.Parse("1998-07-01") };
            var kapoor = new Instructor { FirstMidName = "Candace", LastName = "Kapoor", HireDate = DateTime.Parse("2001-01-15") };
            var zheng = new Instructor { FirstMidName = "Roger", LastName = "Zheng", HireDate = DateTime.Parse("2004-02-12") };

            var instructors = new Instructor[] { abercrombie, fakhouri, harui, kapoor, zheng };
            context.AddRange(instructors);

            // --- Office Assignments ---
            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment { Instructor = fakhouri, Location = "Smith 17" },
                new OfficeAssignment { Instructor = harui, Location = "Gowan 27" },
                new OfficeAssignment { Instructor = kapoor, Location = "Thompson 304" }
            };
            context.AddRange(officeAssignments);

            // --- Departments ---
            var english = new Department { Name = "English", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), Administrator = abercrombie };
            var mathematics = new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), Administrator = fakhouri };
            var engineering = new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), Administrator = harui };
            var economics = new Department { Name = "Economics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), Administrator = kapoor };

            var departments = new Department[] { english, mathematics, engineering, economics };
            context.AddRange(departments);

            context.SaveChanges(); // Save so IDs are set

            // --- Enrollments ---
            var courses = new Course[]
            {
                new Course { CourseID = 1050, Title = "Chemistry", Credits = 3, Department = engineering, Instructors = new List<Instructor>{ kapoor, harui } },
                new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3, Department = economics, Instructors = new List<Instructor>{ zheng } },
                new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3, Department = economics, Instructors = new List<Instructor>{ zheng } },
                new Course { CourseID = 1045, Title = "Calculus", Credits = 4, Department = mathematics, Instructors = new List<Instructor>{ fakhouri } },
                new Course { CourseID = 3141, Title = "Trigonometry", Credits = 4, Department = mathematics, Instructors = new List<Instructor>{ harui } },
                new Course { CourseID = 2021, Title = "Composition", Credits = 3, Department = english, Instructors = new List<Instructor>{ abercrombie } },
                new Course { CourseID = 2042, Title = "Literature", Credits = 4, Department = english, Instructors = new List<Instructor>{ abercrombie } }
            };

            // --- Add courses if they don't exist (idempotent) ---
            foreach (var c in courses)
            {
                if (!context.Courses.Any(x => x.CourseID == c.CourseID))
                    context.Courses.Add(c);
            }

            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment { Student = alexander, Course = courses[0], Grade = Grade.A },
                new Enrollment { Student = alexander, Course = courses[1], Grade = Grade.C },
                new Enrollment { Student = alexander, Course = courses[2], Grade = Grade.B },
                new Enrollment { Student = alonso, Course = courses[3], Grade = Grade.B },
                new Enrollment { Student = alonso, Course = courses[4], Grade = Grade.B },
                new Enrollment { Student = alonso, Course = courses[5], Grade = Grade.B },
                new Enrollment { Student = anand, Course = courses[0] },
                new Enrollment { Student = anand, Course = courses[1], Grade = Grade.B },
                new Enrollment { Student = barzdukas, Course = courses[0], Grade = Grade.B },
                new Enrollment { Student = li, Course = courses[5], Grade = Grade.B },
                new Enrollment { Student = justice, Course = courses[6], Grade = Grade.B }
            };

            context.AddRange(enrollments);
            context.SaveChanges();
        }

        // --- XML seeding helper ---
        public static void SeedCoursesFromXml(SchoolContext context, string xmlFile)
        {
            var doc = XDocument.Load(xmlFile);

            foreach (var element in doc.Root.Elements("Course"))
            {
                int courseId = int.Parse(element.Element("CourseID")!.Value);
                string title = element.Element("Title")!.Value;
                int credits = int.Parse(element.Element("Credits")!.Value);
                string deptName = element.Element("Department")!.Value;
                string instructorName = element.Element("Instructor")!.Value;

                if (context.Courses.Any(c => c.CourseID == courseId))
                    continue;

                var department = context.Departments.FirstOrDefault(d => d.Name == deptName)
                                 ?? throw new InvalidOperationException($"Department '{deptName}' not found.");

                var instructor = context.Instructors.FirstOrDefault(i => i.LastName == instructorName)
                                 ?? throw new InvalidOperationException($"Instructor '{instructorName}' not found.");

                var course = new Course
                {
                    CourseID = courseId,
                    Title = title,
                    Credits = credits,
                    Department = department,
                    Instructors = new List<Instructor> { instructor }
                };

                context.Courses.Add(course);
            }

            context.SaveChanges();
        }
    }
}