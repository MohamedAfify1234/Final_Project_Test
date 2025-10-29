using Core.Models.Courses;

namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class TeacherDashboardVM
    {
        public string TeacherName { get; set; }
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public List<CourseDashboardViewModel> Courses { get; set; }
    }
}
