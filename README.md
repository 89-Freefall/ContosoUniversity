# Week 4 Assignmnet 

- Contoso University tutorial Get Started and CRUD Operations
- _Layout.cshtml with Bootstrap grid for a responsive layout
- Added a Bootstrap navbar to _Layout.cshtml
- Links include: About, Student, Courses, Instructors, Departments
- Navbar renders consistently across all pages
- Only the Students link is functional (per tutorial)
- Repeated content (footer) extracted into a partial view (_Footer.cshtml)
- Rendered in _Layout.cshtml using <partial name="_Footer" />
- Labels linked to inputs using asp-for tag helper
- Error messages displayed clearly using asp-validation-for tag helper
- Keyboard focus order tested and logical


# Week 5 Assignment 

- CRUD and Sorting, Filtering, Paging, and Grouping
- Add Create and Edit forms bound to a ViewModel
- Add data annotations and client-side validation, and handle validation errors
- Use Post/Redirect/Get with TempData to show a success message after a successful POST
- Update README.md with short test plan

## Test Plan
Test plan for Week 5 Contoso University assignment. I went through 
the main features to make sure everything works as expected.

## 1. Students Page (Index)
- Load the Students page and check that the list shows all students in the database.  
- Test sorting by Last Name and Enrollment Date. Make sure the list updates correctly.  
- Test the search box by typing a student’s first or last name and verify it filters the list.  
- Test paging buttons Previous and Next to see that students are split across pages correctly.

## 2. Create Student
- Go to Create New and try adding a student with valid data. Confirm that it appears in the list after redirect.  
- Try submitting empty fields to see that validation messages appear.

## 3. Edit Student
- Select a student and change data. Confirm that changes are saved and a success message shows.  
- Test validation by entering invalid data or leaving required fields blank.

## 4. Delete Student
- Delete a student and make sure the list updates. Confirm that a success message appears.

## 5. General
- Make sure success messages show for create, edit, and delete actions.  
- Check responsive layout: pages should work on mobile, tablet, and desktop.

# Week 6 Assignment
- XML file `SeedData.xml` contains the initial data for the Contoso University database. It is organized 
  into main sections for each type of entity:
  - **Students:** FirstName, LastName, EnrollmentDate
  - **Instructors:** FirstName, LastName, HireDate
  - **Departments:** Name, Budget, StartDate, Administrator
  - **Courses:** Title, Credits, Department
  - **Enrollments:** Student, Course, Grade
- This file is used to seed the database initially, and the seeding method is idempotent, so running it 
  multiple times does not create duplicate entries.
- Migration files are in the Migrations/ folder (InitialCreate is included).
- CRUD works for Students and Instructors. TempData messages show for Create, Edit, and Delete.
- Student Details page shows enrollments with courses using eager loading.
- I tested sorting, filtering, paging, and all CRUD operations to make sure everything works.
