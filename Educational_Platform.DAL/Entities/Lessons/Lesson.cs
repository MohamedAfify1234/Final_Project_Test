using Educational_Platform.DAL.Entities.Learning;
using Educational_Platform.DAL.Enums;

namespace Educational_Platform.DAL.Entities.Lessons
{
	public class Lesson
	{
		public Lesson()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للدرس
		public string Title { get; set; }                  // عنوان الدرس
		public string Description { get; set; }            // وصف الدرس
		public string Content { get; set; }                // محتوى الدرس (نص/HTML)
		public string VideoUrl { get; set; }               // رابط الفيديو
		public string? AttachmentUrl { get; set; }         // رابط المرفقات
		public int Duration { get; set; }                  // المدة (دقائق)
		public int Order { get; set; }                     // ترتيب العرض
		public bool IsFree { get; set; } = false;          // درس مجاني للمعاينة
		public LessonType Type { get; set; }               // نوع الدرس


		// العلاقات العامة
		public ICollection<CourseLesson> CourseLessons { get; set; } // كورسات تحتوي هذا الدرس
		public ICollection<Question> Questions { get; set; }         // أسئلة على الدرس


	}
	 
}
