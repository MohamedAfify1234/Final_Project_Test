using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Educational_Platform.ViewModels.LessonsViewModels
{
	public class CreateLessonViewModel
	{
 		public string Title { get; set; }              
		public string Description { get; set; }           
		public string? Content { get; set; }               
		public string? VideoUrl { get; set; }                
		public string? AttachmentUrl { get; set; }          
		public int Duration { get; set; }                  
 		public bool IsFree { get; set; } = false;          
 		public int OrderInCourse { get; set; }
		public LessonType Type { get; set; }               // نوع الدرس

		public Guid CourseId { get; set; }                 // معرف الكورس
 
	}
}
