using Core.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs
{
    public class TeacherDashboardDTO
    {
        public string InstructorName { get; set; }
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
       public ICollection<CourseDashboardDTO> Courses { get; set; }
    }
}
