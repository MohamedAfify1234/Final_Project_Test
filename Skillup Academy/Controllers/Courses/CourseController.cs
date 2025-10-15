using AutoMapper;
using Core.Models.Courses;
using Core.Enums;
using Skillup_Academy.ViewModels.CoursesViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces;

namespace Educational_Platform.Controllers.Courses
{
    public class CourseController : Controller
    {
        private readonly IRepository<Course> _repository;
        private readonly IMapper _mapper;

        public CourseController(IRepository<Course> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // /Course/ShowAll
        public async Task<IActionResult> ShowAll()
        {
            var allCourse = await _repository.GetAllAsync();
            return View(allCourse);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseViewModel VM)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(VM);

                course.CreatedDate = DateTime.Now;
                course.UpdatedDate = DateTime.Now;
                if (course.IsPublished == true)
                    course.PublishedDate = DateTime.Now;

                await _repository.AddAsync(course);
                await _repository.SaveChangesAsync();
                return RedirectToAction("ShowAll");
            }
            return View(VM);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            var courseVM = _mapper.Map<EditCourseViewModel>(course);
            return View(courseVM);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCourseViewModel newCourse)
        {
            if (ModelState.IsValid)
            {
                var oldCourse = await _repository.GetByIdAsync(newCourse.Id);
                if (oldCourse == null)
                {
                    return NotFound();
                }
                oldCourse.Title = newCourse.Title;
                oldCourse.Description = newCourse.Description;
                oldCourse.ShortDescription = newCourse.ShortDescription;
                oldCourse.ThumbnailUrl = newCourse.ThumbnailUrl;
                oldCourse.PreviewVideoUrl = newCourse.PreviewVideoUrl;
                oldCourse.IsFree = newCourse.IsFree;
                oldCourse.TotalLessons = newCourse.TotalLessons;
                oldCourse.TotalDuration = newCourse.TotalDuration;
                oldCourse.UpdatedDate = DateTime.Now;
                if (oldCourse.IsPublished == false && newCourse.IsPublished == true)
                {
                    oldCourse.PublishedDate = DateTime.Now;

                }
                oldCourse.IsPublished = newCourse.IsPublished;


                _repository.Update(oldCourse);
                await _repository.SaveChangesAsync();
                return RedirectToAction("ShowAll");
            }
            return View(newCourse);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var course = _repository.GetByIdAsync(id).Result;
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            _repository.Delete(course);
            await _repository.SaveChangesAsync();
            return RedirectToAction("ShowAll");
        }


    }
}
