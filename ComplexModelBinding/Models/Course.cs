using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
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

    public class CourseIndexViewModel
    {
        /// <summary>
        /// The Courses Primary Key
        /// </summary>
        public int CourseID { get; set; }

        /// <summary>
        /// The Courses title
        /// </summary>
        [DisplayName("Course Title")]
        public string CourseTitle { get; set; }

        /// <summary>
        /// The Instructor's full legal name
        /// </summary>
        [DisplayName("Instructor")]
        public string InstructorFullName { get; set; }

        /// <summary>
        /// A brief description of the Course
        /// </summary>
        [DisplayName("Description")]
        public string CourseDescription { get; set; }
    }

    public class CourseCreateViewModel
    {
        /// <summary>
        /// The Courses title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A brief description of the Course
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A list containing all Instructors in the DB
        /// </summary>
        [ValidateNever]
        public List<Instructor> AllInstructors { get; set; }

        /// <summary>
        /// The ID of the selected Instructer for that course
        /// </summary>
        public int SelectedInstructorID { get; set; }
    }
}
