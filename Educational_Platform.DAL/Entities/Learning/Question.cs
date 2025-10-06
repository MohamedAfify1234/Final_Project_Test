using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Exams;
using Educational_Platform.DAL.Entities.Lessons;
using Educational_Platform.DAL.Entities.Users;
using Educational_Platform.DAL.Enums;

namespace Educational_Platform.DAL.Entities.Learning
{
	public class Question
	{
		public Question()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }                       // المعرف الفريد للسؤال
		public string Title { get; set; }                  // عنوان السؤال
		public string Content { get; set; }                // محتوى السؤال
 
		// المستخدم
		public DateTime AskedDate { get; set; } = DateTime.Now; // تاريخ السؤال
		public bool IsResolved { get; set; } = false;      // تم الحل/لم يتم


		// العلاقات		
		public Guid? LessonId { get; set; }                // الدرس المرتبط
		public Lesson Lesson { get; set; }                 // الدرس
	    public Guid? CourseId { get; set; }                // الكورس المرتبط
		public Course Course { get; set; }                 // الكورس
		public Guid? ExamId { get; set; }                  // الامتحان المرتبط
		public Exam Exam { get; set; }                     // الامتحان
		public Guid UserId { get; set; }                 // صاحب السؤال
		public User User { get; set; }                     // صاحب السؤال


		public ICollection<Answer> Answers { get; set; }   // الإجابات
	}

}
