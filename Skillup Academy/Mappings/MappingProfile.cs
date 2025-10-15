using AutoMapper;
using Core.Models.Courses;
using Core.Models.Lessons;
using Skillup_Academy.ViewModels.CoursesViewModels;
using Skillup_Academy.ViewModels.LessonsViewModels;
namespace Skillup_Academy.Mappings
{
	public class MappingProfile: Profile
	{
		public MappingProfile()
		{

			CreateMap<Course, CreateCourseViewModel>().ReverseMap();
			CreateMap<Lesson, CreateLessonViewModel>().ReverseMap();
			CreateMap<Course, EditCourseViewModel>().ReverseMap();
			CreateMap<Lesson, EditLessonViewModel>().ReverseMap();


			CreateMap<Course, Course>().ReverseMap();
		    // الشرح 
			//CreateMap<Notification, NotificationDTO>();
			//CreateMap<NotificationDTO, Notification>();

			//CreateMap<Notificationv, NotificationDTO>().ReverseMap();
			 
			//طريقه الاستخدام 
			//private readonly IMapper _mapper;
			//var entity = _mapper.Map<Notification>(NotificationDTO);      Notification  كده هتكون ك 



		}
	}
}
