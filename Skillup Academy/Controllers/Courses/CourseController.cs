using AutoMapper;
using Core.Interfaces;
using Core.Models.Courses;
using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.ViewModels.CoursesViewModels;

namespace Educational_Platform.Controllers.Courses
{
    public class CourseController : Controller
    {
        private readonly IRepository<Course> _repository;
		private readonly IRepository<SubCategory> _repoSubCategory;
		private readonly IRepository<CourseCategory> _repoCategory;
		private readonly IMapper _mapper;
		private readonly SaveImage _saveImage;
		private readonly UserManager<User> _userManager;

		public CourseController(IRepository<Course> repository,IRepository<SubCategory> repoSubCategory,
            IRepository<CourseCategory> repoCategory, IMapper mapper,SaveImage saveImage,UserManager<User> UserManager)
        {
            _repository = repository;
			_repoSubCategory = repoSubCategory;
			_repoCategory = repoCategory;
			_mapper = mapper;
			_saveImage = saveImage;
			_userManager = UserManager;
		}
        // /Course/ShowAll
        public async Task<IActionResult> ShowAll()
        {
            var allCourse = await _repository.GetAllAsync();
            return View(allCourse);
        }

        public async Task<IActionResult> AllCaurseIsPublished()
        {
            var allCourse = await _repository.Query().Where(c => c.IsPublished).ToListAsync();
            return View(allCourse);
        }
         
		[HttpGet]
        public async Task<IActionResult> Create()
        {
			ViewBag.SubCategory= new SelectList(await _repoSubCategory.GetAllAsync(), "Id", "Name");
            ViewBag.Category= new SelectList(await _repoCategory.GetAllAsync(), "Id", "Name");
			   
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
                course.PreviewVideoUrl =await _saveImage.SaveImgAsync(VM.PreviewVideo);
                course.ThumbnailUrl = await _saveImage.SaveImgAsync(VM.ThumbnailUrl);
                if (course.IsPublished == true)
                    course.PublishedDate = DateTime.Now;

				var userIdString = _userManager.GetUserId(User);
                Guid teacherId;
                if (!Guid.TryParse(userIdString, out teacherId))
                {
                    teacherId = Guid.NewGuid();
                }
                course.TeacherId = teacherId;


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

			ViewBag.SubCategory = new SelectList(await _repoSubCategory.GetAllAsync(), "Id", "Name");
			ViewBag.Category = new SelectList(await _repoCategory.GetAllAsync(), "Id", "Name");

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
                 
				if (newCourse.ThumbnailUrl != null) 
                    oldCourse.ThumbnailUrl = await _saveImage.SaveImgAsync(newCourse.ThumbnailUrlFile);
				 
				if (newCourse.PreviewVideoUrl != null) 
                    oldCourse.PreviewVideoUrl = await _saveImage.SaveImgAsync(newCourse.PreviewVideoFile);
				 
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
            var course = await _repository.Query()
                .Include(c=>c.Category)
                .Include(s=>s.SubCategory)
                .Include(t=>t.Teacher)
                .FirstAsync(i=>i.Id==id);

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
