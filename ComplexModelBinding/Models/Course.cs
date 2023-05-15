using System.ComponentModel.DataAnnotations;

namespace ComplexModelBinding.Models
{
    /// <summary>
    /// Represent's an individual educational course that is taught by an instructor
    /// </summary>
    public class Course
    {
        /// <summary>
        /// The Courses Primary Key
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The Courses title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A brief description of the Course
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Instructor teaching the Course
        /// </summary>
        public Instructor Instructor { get; set; }
    }
}
