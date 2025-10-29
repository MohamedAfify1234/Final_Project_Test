using Core.Interfaces.Users;
using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skillup_Academy.ViewModels.TeacherDashboard;
using System.Security.Claims;

namespace Skillup_Academy.Controllers.Users
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly UserManager<User> _userManager;
        public TeacherController(ITeacherRepository teacherRepository, UserManager<User> userManager)
        {
            _teacherRepository = teacherRepository;
            _userManager = userManager;
        }

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
       
    }
}
