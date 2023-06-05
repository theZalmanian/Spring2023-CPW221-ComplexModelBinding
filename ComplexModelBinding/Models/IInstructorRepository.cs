using ComplexModelBinding.Data;

namespace ComplexModelBinding.Models
{
    public interface IInstructorRepository
    {
        void SaveInstructor(Instructor instructor);
        IEnumerable<Instructor> GetAllInstructors();
        void DeleteInstructor (int instructorID);
        void UpdateInstructor (Instructor instructor);
        Instructor GetInstructor (int instructorID);
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
        public InstructorRepository(ApplicationDbContext _context)
        {
            _context = _context;
        }

        /// <summary>
        /// Removes the Instructor that has the given ID from the DB
        /// </summary>
        /// <param name="instructorID">The ID of the Instructor being removed from the DB</param>
        public void DeleteInstructor(int instructorID)
        {
            // Get the requested Instructor from the DB using it's ID
            Instructor instructor = GetInstructor(instructorID);
            
            // Remove that Instructor from the database 
            _context.Instructors.Remove(instructor);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets all Instructors in the DB and returns them in a collection, ordered by FullName
        /// </summary>
        /// <returns>A collection containing all Instructors in the DB, ordered by FullName</returns>
        public IEnumerable<Instructor> GetAllInstructors()
        {
            return _context.Instructors.OrderBy(currInstructor => currInstructor.FullName).ToList();
        }

        /// <summary>
        /// Gets and returns the Instructor in the DB that has the given ID
        /// </summary>
        /// <param name="instructorID">The ID of the Instructor being retrieved from the DB</param>
        /// <returns>The Instructor corresponding to the given ID</returns>
        public Instructor GetInstructor(int instructorID)
        {
            return _context.Instructors.SingleOrDefault(currInstructor => currInstructor.ID == instructorID);
        }

        /// <summary>
        /// Adds the given Instructor to the DB
        /// </summary>
        /// <param name="instructor">The Instructor being added to the DB</param>
        public void SaveInstructor(Instructor instructor)
        {
            _context.Instructors.Add(instructor); 
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates the entry for the given Instructor
        /// </summary>
        /// <param name="instructor">The Instructor whose data is being updated in the DB</param>
        public void UpdateInstructor(Instructor instructor)
        {
            _context.Add(instructor);
            _context.SaveChanges();
        }
    }
}
