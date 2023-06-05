using ComplexModelBinding.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ComplexModelBinding.Models
{
    public interface IInstructorRepository
    {
        Task AddInstructor(Instructor instructor);
        Task<IEnumerable<Instructor>> GetAllInstructors();
        Task DeleteInstructor (int instructorID);
        Task UpdateInstructor (Instructor instructor);
        Task<Instructor?> GetInstructor (int instructorID);
    }

    public class InstructorRepository : IInstructorRepository
    {
        /// <summary>
        /// The Database Context 
        /// </summary>
        private ApplicationDbContext _context;

        /// <summary>
        /// Setup DB Context
        /// </summary>
        /// <param name="_context">The Database Context</param>
        public InstructorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Removes the Instructor that has the given ID from the DB
        /// </summary>
        /// <param name="instructorID">The ID of the Instructor being removed from the DB</param>
        public async Task DeleteInstructor(int instructorID)
        {
            // Get the requested Instructor from the DB using it's ID
            Instructor? instructor = await GetInstructor(instructorID);
            
            // If the given instructor is not null
            if(instructor != null)
            {
                // Setup DB connection
                using DbConnection connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                // Setup RAW SQL command to Update all Courses taught by that Instructor to have a null Instructor
                using DbCommand updateCourses = connection.CreateCommand();
                updateCourses.CommandText = "UPDATE Courses SET InstructorID = null " +
                                            "WHERE InstructorID = " + instructor.ID; 

                await updateCourses.ExecuteNonQueryAsync();

                // Remove that Instructor from the database 
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets all Instructors in the DB and returns them in a collection, ordered by FullName
        /// </summary>
        /// <returns>A collection containing all Instructors in the DB, ordered by FullName</returns>
        public async Task<IEnumerable<Instructor>> GetAllInstructors()
        {
            return await _context.Instructors
                                 .OrderBy(currInstructor => currInstructor.FullName)
                                 .ToListAsync();
        }

        /// <summary>
        /// Gets and returns the Instructor in the DB that has the given ID
        /// </summary>
        /// <param name="instructorID">The ID of the Instructor being retrieved from the DB</param>
        /// <returns>The Instructor corresponding to the given ID</returns>
        public async Task<Instructor?> GetInstructor(int instructorID)
        {
            return await _context.Instructors
                                 .SingleOrDefaultAsync(currInstructor => currInstructor.ID == instructorID);
        }

        /// <summary>
        /// Adds the given Instructor to the DB
        /// </summary>
        /// <param name="instructor">The Instructor being added to the DB</param>
        public async Task AddInstructor(Instructor instructor)
        {
            _context.Instructors.Add(instructor); 
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the entry for the given Instructor
        /// </summary>
        /// <param name="instructor">The Instructor whose data is being updated in the DB</param>
        public async Task UpdateInstructor(Instructor instructor)
        {
            _context.Add(instructor);
            await _context.SaveChangesAsync();
        }
    }
}
