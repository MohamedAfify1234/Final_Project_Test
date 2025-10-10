using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Exams;
using Educational_Platform.DAL.Entities.Lessons;
using Educational_Platform.DAL.Entities.Reviews;
using Educational_Platform.DAL.Entities.Subscriptions;
using Educational_Platform.DAL.Entities.Users;
using Educational_Platform.DAL.Enums;

namespace Educational_Platform.ViewModels.CoursesViewModels
{
	public class CreateCourseViewModel
	{
		public string Title { get; set; }                   
		public string Description { get; set; }            
		public string ShortDescription { get; set; }        
		public string ThumbnailUrl { get; set; }            
		public string PreviewVideoUrl { get; set; }         

		public bool IsFree { get; set; } = false;           

		// الحالة
		public bool IsPublished { get; set; } = false;      
 
		// الإحصائيات
		public int TotalEnrollments { get; set; } = 0;          // عدد المسجلين
		public double AverageRating { get; set; } = 0;         // متوسط التقييم
		public int TotalLessons { get; set; } = 0;         // عدد الدروس
		public int TotalDuration { get; set; } = 0;       // المدة الإجمالية (دقائق)

		 
	}
}
