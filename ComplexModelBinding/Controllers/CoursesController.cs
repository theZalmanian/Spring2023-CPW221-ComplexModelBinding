using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComplexModelBinding.Data;
using ComplexModelBinding.Models;

namespace ComplexModelBinding.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            // Get all Courses from the DB, including their Instructor
            List<CourseIndexViewModel> allCourses = await (from course in _context.Courses
                                                    join instructor in _context.Instructors
                                                        on course.Instructor.ID equals instructor.ID
                                                    orderby course.Title
                                                    select new CourseIndexViewModel
                                                    {
                                                        CourseID = course.ID,
                                                        CourseTitle = course.Title,
                                                        InstructorFullName = instructor.FullName,
                                                        CourseDescription = course.Description,
                                                    }).ToListAsync();

            // Display then on the Index page
            return View(allCourses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            // Create an instance of the Create VM
            CourseCreateViewModel viewModel = new();

            // Add all Instructors in DB to view model
            viewModel.AllInstructors = _context.Instructors.OrderBy(instructor => instructor.FullName).ToList();
            
            return View(viewModel);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel givenCourse)
        {
            // If all data is valid
            if (ModelState.IsValid)
            {
                // Create a course object with all the given data
                Course newCourse = new()
                {
                    Title = givenCourse.Title,
                    Description = givenCourse.Description,
                    Instructor = new Instructor()
                    {
                        ID = givenCourse.SelectedInstructorID
                    }
                }; 

                // tell EF that the instructor is an existing instructor
                _context.Entry(newCourse.Instructor).State = EntityState.Unchanged;

                // Add the course to the db
                _context.Add(newCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // Otherwise, repopulate the vm with all instructors in the DB
            givenCourse.AllInstructors = _context.Instructors.OrderBy(instructor => instructor.FullName).ToList();
            return View(givenCourse);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description")] Course course)
        {
            if (id != course.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
