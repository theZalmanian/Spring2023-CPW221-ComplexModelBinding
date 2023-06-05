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
    public class InstructorsController : Controller
    {
        private readonly IInstructorRepository _instructorRepo;

        public InstructorsController(IInstructorRepository instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            return View(await _instructorRepo.GetAllInstructors());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor instructor = await _instructorRepo.GetInstructor(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FullName")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                await _instructorRepo.AddInstructor(instructor);
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor instructor = await _instructorRepo.GetInstructor(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FullName")] Instructor instructor)
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _instructorRepo.UpdateInstructor(instructor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await InstructorExists(instructor.ID))
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
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorRepo.GetInstructor(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get the given Instructor from the DB
            Instructor givenInstructor = await _instructorRepo.GetInstructor(id);
            
            if (givenInstructor != null)
            {
                _instructorRepo.DeleteInstructor(givenInstructor.ID);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> InstructorExists(int id)
        {
            return await _instructorRepo.GetInstructor(id) != null;
        }
    }
}
