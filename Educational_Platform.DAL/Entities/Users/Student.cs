using Educational_Platform.DAL.Entities.Exams;

namespace Educational_Platform.DAL.Entities.Users
{
	public class Student : User
	{ 

		// الإحصائيات
		public int CompletedCourses { get; set; }          // عدد الكورسات المكتملة
		public int TotalEnrollments { get; set; }          // إجمالي الاشتراكات


		// العلاقات الخاصة
		public ICollection<ExamAttempt> ExamAttempt { get; set; }       // الامتحانات
	
	}
}
