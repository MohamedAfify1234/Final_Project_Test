using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Helper;
using Skillup_Academy.ViewModels.StudentsViewModels;
using Skillup_Academy.ViewModels.UsersViewModels;
using System.Collections.Generic;

namespace Skillup_Academy.Controllers.Users
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly SaveImage _saveImage;
        private readonly FileService _fileService;
        public StudentController(IStudentRepository studentRepository, SaveImage saveImage, FileService fileService)
        {
            _studentRepository = studentRepository;
            _saveImage = saveImage;
            _fileService = fileService;
        }
        public async Task<IActionResult> DashBoard()
        {
            var viewmodel = new StudentDashboardViewModel();
            return View("DashBoard", viewmodel);
        }
        public async Task<IActionResult> Index()
        {
            List<Student> Students = await _studentRepository.GetAll();
            return View("Index", Students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new StudentViewModel
            {
                IsActive = true 
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            var file = _fileService.GetDefaultAvatar();
            if (ModelState.IsValid)
            {
                var student = new Student
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

                var result = await _studentRepository.CreateStudentAsync(student, model.Password);

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
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var studentEntity = await _studentRepository.GetStudentByIdAsync(id);

            if (studentEntity == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = studentEntity.Id.ToString(), 
                UserName = studentEntity.UserName,
                Email = studentEntity.Email,
                FullName = studentEntity.FullName,
                PhoneNumber = studentEntity.PhoneNumber,
                RegistrationDate = studentEntity.RegistrationDate,
                LastLoginDate = studentEntity.LastLoginDate,
                IsActive = studentEntity.IsActive,
                TotalEnrollments = studentEntity.TotalEnrollments,
                CompletedCourses = studentEntity.CompletedCourses
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var studentToEdit = await _studentRepository.GetStudentByIdAsync(id);

            if (studentToEdit == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = studentToEdit.Id.ToString(), // ضروري تحويل الـ Guid إلى string
                Email = studentToEdit.Email,
                UserName = studentToEdit.UserName,
                FullName = studentToEdit.FullName,
                IsActive = studentToEdit.IsActive,
                RegistrationDate = studentToEdit.RegistrationDate,
                LastLoginDate = studentToEdit.LastLoginDate,
                PhoneNumber = studentToEdit.PhoneNumber,
                TotalEnrollments = studentToEdit.TotalEnrollments,
                CompletedCourses = studentToEdit.CompletedCourses
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, Student model)
        {
            var studentToUpdate = await _studentRepository.GetStudentByIdAsync(id);
            if (studentToUpdate == null) return NotFound();

            studentToUpdate.Email = model.Email;
            studentToUpdate.UserName = model.UserName;
            studentToUpdate.FullName = model.FullName;
            studentToUpdate.IsActive = model.IsActive;
            studentToUpdate.RegistrationDate = model.RegistrationDate;
            studentToUpdate.LastLoginDate = model.LastLoginDate;
            studentToUpdate.PhoneNumber = model.PhoneNumber;
            studentToUpdate.TotalEnrollments = model.TotalEnrollments;
            studentToUpdate.CompletedCourses = model.CompletedCourses;

            var result = await _studentRepository.UpdateStudentAsync(studentToUpdate);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var studentEntity = await _studentRepository.GetStudentByIdAsync(id);

            if (studentEntity == null)
            {
                return NotFound();
            }

            var viewModel = new StudentViewModel
            {
                Id = studentEntity.Id.ToString(),
                UserName = studentEntity.UserName,
                Email = studentEntity.Email,
                FullName = studentEntity.FullName,
                PhoneNumber = studentEntity.PhoneNumber,
                RegistrationDate = studentEntity.RegistrationDate,
                LastLoginDate = studentEntity.LastLoginDate,
                IsActive = studentEntity.IsActive,
                TotalEnrollments = studentEntity.TotalEnrollments,
                CompletedCourses = studentEntity.CompletedCourses
            };

            return View(viewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userToDelete = await _studentRepository.GetStudentByIdAsync(id);

            if (userToDelete != null)
            {
                var result = await _studentRepository.DeleteStudentAsync(userToDelete);

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
