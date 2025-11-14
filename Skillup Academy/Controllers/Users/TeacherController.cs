using Core.Interfaces.Users;
using Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Helper;
using Skillup_Academy.ViewModels.TeacherDashboard;
using Skillup_Academy.ViewModels.UsersViewModels;
using System.Security.Claims;

namespace Skillup_Academy.Controllers.Users
{
    //[Authorize]
    [Authorize(Roles = "Instructor,Admin")]
    [Route("Teacher/[action]")]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //------- for Admin Dashboard
        private readonly SaveImage _saveImage;
        private readonly FileService _fileService;
        public TeacherController(ITeacherRepository teacherRepository, UserManager<User> userManager, SaveImage saveImage, FileService fileService, SignInManager<User> signInManager)
        {
            _teacherRepository = teacherRepository;
            _userManager = userManager;
            _saveImage = saveImage;
            _fileService = fileService;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var teacher = await _userManager.FindByIdAsync(userId);

            if (teacher == null)
                return NotFound("Teacher not found.");

            if (!Guid.TryParse(teacher.Id.ToString(), out var teacherGuid))
                return BadRequest("Invalid teacher ID format.");

            var dashboardData = await _teacherRepository.GetTeacherDashboardAsync(teacherGuid);
            if (dashboardData == null)
                return NotFound("Dashboard data not available.");

            var coursesList = dashboardData.Courses
             .Select(c => new CourseDashboardViewModel
             {
                 CourseId = c.CourseId,
                 Title = c.Title,
                 Description = c.Description,
                 IsPublished = c.IsPublished,
                 CreatedDate = c.CreatedDate,
                 TotalLessons = c.TotalLessons,
                 TotalStudents = c.TotalStudents,
             })
             .ToList();

            var viewModel = new TeacherDashboardVM
            {
                TeacherName = teacher.FullName ?? string.Empty,
                TotalCourses = dashboardData.TotalCourses,
                TotalStudents = dashboardData.TotalStudents,
                Courses = coursesList
            };
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var teacher = await _userManager.FindByIdAsync(userId);
            if (teacher == null)
                return NotFound("Teacher not found.");
            if (!Guid.TryParse(teacher.Id.ToString(), out var teacherGuid))
                return BadRequest("Invalid teacher ID format.");
            var courses = await _teacherRepository.GetTeacherCoursesAsync(teacherGuid);
            var viewModel = courses.Select(c => new CourseDashboardViewModel
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description,
                IsPublished = c.IsPublished,
                CreatedDate = c.CreatedDate,
                TotalLessons = c.TotalLessons,
                TotalStudents = c.TotalStudents,
            }).ToList();
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Setting()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var teacher = await _userManager.FindByIdAsync(userId);
            if (teacher == null)
                return NotFound("Teacher not found.");

            if (!Guid.TryParse(teacher.Id.ToString(), out var teacherGuid))
                return BadRequest("Invalid teacher ID format.");
            var teacherInfo = await _teacherRepository.GetTeacherInfoAsync(teacherGuid);
            if (teacherInfo == null)
                return NotFound("Teacher info not available.");
            var viewModel = new TeacherInfoVM
            {
                FullName = teacherInfo.FullName,
                Email = teacherInfo.Email,
                PhoneNumber = teacherInfo.PhoneNumber,
                Bio = teacherInfo.Bio,
                Qualifications = teacherInfo.Qualifications,
                Expertise = teacherInfo.Expertise,
                ProfilePicture = teacherInfo.ProfilePicture
            };
            return View(viewModel);

        }
        [HttpPost]
        //public async Task<IActionResult> Setting(Teacher model, IFormFile ProfilePictureFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            if (userId == null)
        //                return Unauthorized();

        //            var teacher = await _userManager.FindByIdAsync(userId);
        //            if (teacher == null)
        //                return NotFound("Teacher not found.");

        //            // حفظ الصورة إذا تم رفعها
        //            if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
        //            {
        //                model.ProfilePicture = await _saveImage.SaveImgAsync(ProfilePictureFile);
        //            }

        //            // تحديث بيانات المعلم
        //            var result = await _teacherRepository.UpdateTeacherAsync(model, teacher.Id);

        //            if (result)
        //            {
        //                TempData["SuccessMessage"] = "Settings updated successfully!";
        //                return RedirectToAction("Settings");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Failed to update settings.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", $"An error occurred: {ex.Message}");
        //        }
        //    }

        //    return View(model);
        //}





        //////////////////////////////////////////////////////////// For Admin Dashboard////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Teacher> Teachers = await _teacherRepository.GetAll();
            return View("Index", Teachers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TeacherViewModel
            {
                IsActive = true
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            var file = _fileService.GetDefaultAvatar();

            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    RegistrationDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = model.IsActive,
                    ProfilePicture = model.ClientFile != null
                                     ? await _saveImage.SaveImgAsync(model.ClientFile)
                                     : await _saveImage.SaveImgAsync(file)
                };

                var result = await _teacherRepository.CreateTeacherAsync(teacher, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            if (!Guid.TryParse(id, out var teacherGuid))
            {
                return BadRequest("Invalid teacher ID format.");
            }
            var teacherEntity = await _teacherRepository.GetTeacherAsync(id);
            var courses = await _teacherRepository.GetTeacherCoursesAsync(teacherGuid);

            if (teacherEntity == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacherEntity.Id.ToString(),
                UserName = teacherEntity.UserName,
                Email = teacherEntity.Email,
                FullName = teacherEntity.FullName,
                PhoneNumber = teacherEntity.PhoneNumber,
                RegistrationDate = teacherEntity.RegistrationDate,
                LastLoginDate = teacherEntity.LastLoginDate,
                IsActive = teacherEntity.IsActive,
                TotalCourses = courses.Count
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var teacherToEdit = await _teacherRepository.GetTeacherAsync(id);

            if (teacherToEdit == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacherToEdit.Id.ToString(),
                Email = teacherToEdit.Email,
                UserName = teacherToEdit.UserName,
                FullName = teacherToEdit.FullName,
                IsActive = teacherToEdit.IsActive,
                RegistrationDate = teacherToEdit.RegistrationDate,
                LastLoginDate = teacherToEdit.LastLoginDate,
                PhoneNumber = teacherToEdit.PhoneNumber,
                TotalCourses = teacherToEdit.TotalCourses
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Teacher model)
        {
            var teacherToUpdate = await _teacherRepository.GetTeacherAsync(id);
            if (teacherToUpdate == null) return NotFound();

            teacherToUpdate.Email = model.Email;
            teacherToUpdate.UserName = model.UserName;
            teacherToUpdate.FullName = model.FullName;
            teacherToUpdate.IsActive = model.IsActive;
            teacherToUpdate.RegistrationDate = model.RegistrationDate;
            teacherToUpdate.LastLoginDate = model.LastLoginDate;
            teacherToUpdate.PhoneNumber = model.PhoneNumber;
            teacherToUpdate.TotalCourses = model.TotalCourses;

            var result = await _teacherRepository.UpdateTeacherAsync(teacherToUpdate);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var teacherEntity = await _teacherRepository.GetTeacherAsync(id);

            if (teacherEntity == null) return NotFound();

            var viewModel = new TeacherViewModel
            {
                Id = teacherEntity.Id.ToString(),
                UserName = teacherEntity.UserName,
                Email = teacherEntity.Email,
                FullName = teacherEntity.FullName,
                PhoneNumber = teacherEntity.PhoneNumber,
                RegistrationDate = teacherEntity.RegistrationDate,
                LastLoginDate = teacherEntity.LastLoginDate,
                IsActive = teacherEntity.IsActive,
                TotalCourses = teacherEntity.TotalCourses
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userToDelete = await _teacherRepository.GetTeacherAsync(id);

            if (userToDelete != null)
            {
                var result = await _teacherRepository.DeleteTeacherAsync(userToDelete);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
