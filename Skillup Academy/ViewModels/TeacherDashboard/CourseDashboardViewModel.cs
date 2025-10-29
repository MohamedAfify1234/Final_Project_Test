namespace Skillup_Academy.ViewModels.TeacherDashboard
{
    public class CourseDashboardViewModel
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
       // public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalLessons { get; set; }
        public int TotalStudents { get; set; }
    }
}
