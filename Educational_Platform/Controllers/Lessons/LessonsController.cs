using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Lessons;
using Educational_Platform.DAL.Enums;
using Educational_Platform.DAL.Repositorys.Interfaces;
using Educational_Platform.ViewModels.LessonsViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Educational_Platform.Controllers.Lessons
{
    public class LessonsController : Controller
    {
        private readonly IRepository<Lesson> _repoLesson;
        private readonly IRepository<Course> _repoCourses;
		private readonly IMapper _mapper;

 		
        public LessonsController(IRepository<Lesson> repository, IMapper mapper, IRepository<Course> repoCourses )
        {
			_repoLesson = repository;
			_mapper = mapper;
			_repoCourses = repoCourses;
 		}

        [HttpGet]
        public IActionResult Index(Guid id)
        {
            var lessons = _repoLesson.Query().Where(i => i.CourseId == id).ToList();
            ViewBag.courseId = id;
			return View(lessons);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        { 
            var lesson = _repoLesson.Query().Include(c => c.Course).FirstOrDefault(i=>i.Id==id);

            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }


        [HttpGet]
        public async Task<IActionResult> Create(Guid coursId)
        { 
			ViewBag.Courses =new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
            CreateLessonViewModel model = new CreateLessonViewModel
            {
                CourseId=coursId
            };
 			return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLessonViewModel lesson)
        {
            if (ModelState.IsValid)
            { 
                var lessonEntity = _mapper.Map<Lesson>(lesson);
                 lessonEntity.Order=lesson.OrderInCourse;

                await _repoLesson.AddAsync(lessonEntity);

                var course = await _repoCourses.GetByIdAsync(lessonEntity.CourseId);
                course.TotalLessons += 1;
                course.TotalDuration += lesson.Duration;

                await _repoLesson.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id=lesson.CourseId});
            }
			ViewBag.Courses = new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
			return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        { 
            var lesson = await _repoLesson.GetByIdAsync(id);

			var lessonEntity = _mapper.Map<EditLessonViewModel>(lesson);

			if (lesson == null)
            {
                return NotFound();
            }
 			ViewBag.Courses = new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
			return View(lessonEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditLessonViewModel lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldLesson =  await _repoLesson.GetByIdAsync(id);

				var lessonEntity = _mapper.Map<Lesson>(lesson);
                lessonEntity.Id = id;
                lessonEntity.Order = lesson.OrderInCourse;

				var course = await _repoCourses.GetByIdAsync(lessonEntity.CourseId);
				course.TotalDuration -= oldLesson.Duration;

				course.TotalDuration += lesson.Duration;
                  
				_repoLesson.Update(lessonEntity);
                await _repoLesson.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index), new { id = lesson.CourseId });
            }
			ViewBag.Courses = new SelectList(await _repoCourses.GetAllAsync(), "Id", "Title");
			return View(lesson);
        }


		[HttpGet]
		public async Task<IActionResult> Delete(Guid id)
		{
			var lesson = await _repoLesson.GetByIdAsync(id);
			if (lesson == null)
			{
				return NotFound();
			}

			return View(lesson);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var lesson = await _repoLesson.GetByIdAsync(id);
             
			if (lesson != null)
			{
				_repoLesson.Delete(lesson);

				var course = await _repoCourses.GetByIdAsync(lesson.CourseId);
				course.TotalLessons += 1;
				course.TotalDuration += lesson.Duration;

			}
			await _repoLesson.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

         
    }
}
