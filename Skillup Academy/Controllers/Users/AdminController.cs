using AutoMapper;
using Core.Interfaces;
using Core.Interfaces;
using Core.Interfaces.Courses;
using Core.Interfaces.Reviews;
using Core.Interfaces.Subscriptions;
using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Exams;
using Core.Models.Lessons;
using Core.Models.Reviews;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Infrastructure.Repositories.Courses;
using Infrastructure.Repositories.Reviews;
using Infrastructure.Repositories.Subscriptions;
using Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Skillup_Academy.ViewModels.AdminDashboard;
using Skillup_Academy.ViewModels.UsersViewModels;

namespace Skillup_Academy.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[action]")]
    public class AdminController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository; 
        private readonly IRepository<Course> _repository;
        private readonly ICourseCategoryRepsitory _categoryRepository; 
        private readonly ISubCategoryRepository _subCategoryRepository; 
        public AdminController(IStudentRepository studentRepository, ITeacherRepository teacherRepository
            , IRepository<Course> repository, ICourseCategoryRepsitory categoryRepository, ISubCategoryRepository subCategoryRepository)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _repository = repository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
        }
        // /Admin/DashBoard
        public async Task<IActionResult> DashBoard()
        {
            int totalStudents = await _studentRepository.GetTotalStudentCountAsync();
            int totalTeachers = await _teacherRepository.GetTotalTeacherCountAsync();
            int totalCourses = await _repository.GetTotalCourseCountAsync();
            int totalCategories = await _categoryRepository.GetTotalCategoryCountAsync();
            int totalSubCategories = await _subCategoryRepository.GetTotalSubCategoryCountAsync();

            var viewModel = new DashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalActiveCourses = totalCourses,
                TotalTeachers = totalTeachers,
                TotalCategories = totalCategories,
                TotalSubCategories = totalSubCategories
            };

            return View(viewModel);
        }
        
        }
}
