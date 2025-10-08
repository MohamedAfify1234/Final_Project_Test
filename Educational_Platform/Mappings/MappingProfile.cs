using AutoMapper;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Lessons;
using Educational_Platform.ViewModels.CoursesViewModels;
using Educational_Platform.ViewModels.LessonsViewModels;
namespace Educational_Platform.Mappings
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
