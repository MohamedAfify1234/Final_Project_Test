using AutoMapper;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Exams;
using Educational_Platform.DAL.Entities.Learning;
using Educational_Platform.ViewModels.CoursesViewModels;
using Educational_Platform.ViewModels.ExamsViewModels;
using Educational_Platform.ViewModels.LearningViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Educational_Platform.Mappings
{
	public class MappingProfile: Profile
	{
		public MappingProfile()
		{

			CreateMap<Course, CreateCourseViewModel>().ReverseMap();
			CreateMap<Course, EditCourseViewModel>().ReverseMap();
			CreateMap<Course, Course>().ReverseMap();
			CreateMap<ExamViewModel,Exam>().ReverseMap();
            CreateMap<QuestionViewModel, Question>().ReverseMap();

            CreateMap<Question, QuestionListViewModel>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : ""))
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : ""))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : ""));

            CreateMap<Answer, AnswerViewModel>().ReverseMap();


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
