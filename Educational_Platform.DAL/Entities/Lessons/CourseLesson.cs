using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Courses;

namespace Educational_Platform.DAL.Entities.Lessons
{
	public class CourseLesson
	{
		public CourseLesson()
		{
			Id = Guid.NewGuid();
		}
		public Guid Id { get; set; }                       // العلاقة بين الكورس والدرس
		public int OrderInCourse { get; set; }             // ترتيب الدرس في الكورس
		public bool IsRequired { get; set; } = true;       // إجباري/اختياري
		

		// العلاقات
		public Guid CourseId { get; set; }                 // معرف الكورس
		public Course Course { get; set; }                 // الكورس

		public Guid LessonId { get; set; }                 // معرف الدرس
		public Lesson Lesson { get; set; }                 // الدرس
	}
}
