using System.ComponentModel.DataAnnotations;

namespace ComplexModelBinding.Models
{
    /// <summary>
    /// Represents an individual Instructor
    /// </summary>
    public class Instructor
    {
        /// <summary>
        /// The Instructor's Primary Key
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The Instructor's full legal name
        /// </summary>
        public string FullName { get; set; }
    }
}
