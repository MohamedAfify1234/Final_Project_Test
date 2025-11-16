using Core.DTOs.TeacherDashboardDTOs;
using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Enums;
using Core.Interfaces.Users;
using Core.Models.Users;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Users
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public TeacherRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Teacher?> GetTeacherAsync(string teacherId)
        {
            return await _userManager.Users.OfType<Teacher>()
                 .FirstOrDefaultAsync(t => t.Id.ToString() == teacherId);
        }

        public async Task<List<CourseDashboardDTO>> GetTeacherCoursesAsync(Guid teacherId)
        {
            return await _context.Courses
                .Where(Courses => Courses.TeacherId == teacherId)
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .Select(c => new CourseDashboardDTO
                {
                    CourseId = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    IsPublished = c.IsPublished,
                    IsFree = c.IsFree,
                    AverageRating = c.AverageRating,
                    TotalDuration = c.TotalDuration,
                    CreatedDate = c.CreatedDate,
                    TotalLessons = c.Lessons.Count,
                    TotalStudents = c.Enrollments.Count

                })
                .ToListAsync();
        }

        public async Task<int> GetTotalStudentsAsync(Guid teacherId)
        {
            return await _context.Courses
                    .Where(c => c.TeacherId == teacherId)
                    .SelectMany(c => c.Enrollments.Select(e => e.StudentId))
                    .Distinct()
                    .CountAsync();
        }

        public async Task<TeacherDashboardDTO> GetTeacherDashboardAsync(Guid teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId.ToString());
            if (teacher == null) return null;

            var courses = await GetTeacherCoursesAsync(teacherId);
            var totalStudents = await GetTotalStudentsAsync(teacherId);

            return new TeacherDashboardDTO
            {
                InstructorName = teacher.FullName,
                TotalCourses = courses.Count,
                TotalStudents = totalStudents,
                Courses = courses.Select(c => new CourseDashboardDTO
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    IsPublished = c.IsPublished,
                    CreatedDate = c.CreatedDate,
                    TotalLessons = c.TotalLessons,
                    TotalStudents = c.TotalStudents
                }).ToList()
            };
        }


        public async Task<TeacherInfoDTO> GetTeacherInfoAsync(Guid teacherId)
        {
            var user = await _userManager.FindByIdAsync(teacherId.ToString());
            if (user == null)
                return null;
            var teacher = await _userManager.Users.OfType<Teacher>()
                                        .FirstOrDefaultAsync(t => t.Id == teacherId);
            return new TeacherInfoDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Bio = teacher?.Bio,
                Qualifications = teacher?.Qualifications,
                Expertise = teacher?.Expertise
            };
        }

        public async Task<IdentityResult> UpdateTeacherInfoAsync(Teacher teacher, Guid teacherId)
        {
            var User = await _userManager.FindByIdAsync(teacherId.ToString());
            if (User == null)
                return (IdentityResult.Failed(new IdentityError { Description = "Teacher not found." }));
            // لليوزر  تحديث البيانات
            User.FullName = teacher.FullName;
            User.Email = teacher.Email;
            User.PhoneNumber = teacher.PhoneNumber;
            User.ProfilePicture = teacher.ProfilePicture;
            var result = await _userManager.UpdateAsync(User);

            if (!result.Succeeded)
                return (IdentityResult.Failed(new IdentityError { Description = "Update failed." }));

            // الاضافيه تحديث البيانات
            var teacherToUpdate = await _context.Users
                                        .OfType<Teacher>()
                                        .FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacherToUpdate == null)
                return (IdentityResult.Failed(new IdentityError { Description = "Teacher not found in context." }));
            teacherToUpdate.Bio = teacher.Bio;
            teacherToUpdate.Qualifications = teacher.Qualifications;
            teacherToUpdate.Expertise = teacher.Expertise;
            _context.Teachers.Update(teacherToUpdate);
            await _context.SaveChangesAsync();

            return (IdentityResult.Success);
        }

        public async Task<StudentDetailsDTO> GetStudentDetailsAsync(Guid teacherId, Guid studentId)
        {
            //var user = await _userManager.FindByIdAsync(userId.ToString());
           // if (student == null) return null;
            var student = await _userManager.Users.OfType<Student>()
                .FirstOrDefaultAsync(s => s.Id == studentId);
            var Courses = await _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .Include(c => c.Enrollments
                .Where(s => s.StudentId == studentId))
                .ToListAsync();
            var EnrollmentDate = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => e.EnrolledAt)
                .ToListAsync();

            return new StudentDetailsDTO
            {
                StudentId = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                ProfilePicture = student.ProfilePicture,
                CoursesCount = Courses.Count(),
                Courses = Courses.Select(c => new StudentCourseDTO
                {
                    CourseId = c.Id,
                    CourseTitle = c.Title,
                }).ToList()
            };
        }
         public Task<int> TotalStudentOfCourse(Guid CourseId, Guid teacherId)
        {
            var count = _context.Enrollments
                .Where(e => e.CourseId == CourseId && e.Course.TeacherId == teacherId)
                .Select(e => e.StudentId)
                .Distinct()
                .Count();
            return Task.FromResult(count);
        }

        public async Task<StudentListDTO> GetStudentsAsync(Guid teacherId, int pageNumber, int pageSize, string? searchQuery, Guid? courseId, string? status)
        {
            // 1. نجيب الطلاب المرتبطين بالكورسات بتاعة المدرس
            var studentsQuery = _context.Users
                .OfType<Student>() // Student كلاس بيرث من IdentityUser
                .Where(s => s.Enrollments
                .Any(sc => sc.Course.TeacherId == teacherId))
                .AsQueryable();
            // 2. تطبيق فلتر البحث بالكلمة المفتاحية
            if (!string.IsNullOrEmpty(searchQuery))
            {
                studentsQuery = studentsQuery.Where(s =>
                    s.FullName
                    .Contains(searchQuery) ||
                    s.Email
                    .Contains(searchQuery));
            }
            // 3. فلتر بالكورس
            if (courseId.HasValue)
            {
                studentsQuery = studentsQuery.Where(s =>
                    s.Enrollments.Any(sc => sc.CourseId == courseId.Value));
            }
            // 4. فلتر بالحالة
            StudentStatus? selectedStatusEnum = null;
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<StudentStatus>(status, out var statusEnum))
            {
                studentsQuery = studentsQuery.Where(s => s.StudentStatus == statusEnum);
                selectedStatusEnum = statusEnum; // هنا بنخزن القيمة
            }

            // 5. إجمالي الطلاب قبل Paging
            var totalRecords = await studentsQuery.CountAsync();
            // 6. Paging وجلب البيانات
            var students = await studentsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentDTO
                {
                    StudentId = s.Id,
                    FullName = s.FullName,
                    Email = s.Email,
                    CoursesCount = s.Enrollments.Count,
                    CourseTitle = s.Enrollments.FirstOrDefault().Course.Title,
                    Status = s.StudentStatus
                })
                .ToListAsync();
            // 7. احصائيات الـDashboard
            var totalStudents = await _context.Users.OfType<Student>()
                .Where(s => s.Enrollments.Any(sc => sc.Course.TeacherId == teacherId))
                .CountAsync();

            var activeStudents = await _context.Users.OfType<Student>()
                .Where(s => s.Enrollments.Any(sc => sc.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Active)
                .CountAsync();

            var inactiveStudents = await _context.Users.OfType<Student>()
                .Where(s => s.Enrollments.Any(sc => sc.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Inactive)
                .CountAsync();
            var teacherCourses = await _context.Courses
       .Where(c => c.TeacherId == teacherId)
       .Select(c => new CourseDashboardDTO
       {
           CourseId = c.Id,
           Title = c.Title
       })
       .ToListAsync();
            return new StudentListDTO
            {
                Students = students,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                SearchQuery = searchQuery,
                SelectedCourseId = courseId.HasValue ? (Guid?)courseId.Value : null,
                SelectedStatus = selectedStatusEnum,
                TotalStudents = totalStudents,
                ActiveStudents = activeStudents,
                InactiveStudents = inactiveStudents,
                TeacherCourses = teacherCourses
            };

        }
        public Task<int> GetActiveStudentsAsync(Guid teacherId)
        {
            var count = _context.Users
                .OfType<Student>()
                .Where(s => s.Enrollments.Any(e => e.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Active)
                .Count();
            return Task.FromResult(count);
        }

        public Task<int> GetCompleteStudentsAsync(Guid teacherId)
        {
            var count = _context.Users
                .OfType<Student>()
                .Where(s => s.Enrollments.Any(e => e.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Completed)
                .Count();
            return Task.FromResult(count);
        }

        //public async Task<StudentListDTO> GetStudentListAsync(Guid teacherId)
        //{
        //    var teacher  = await _userManager.Users
        //        .OfType<Teacher>()
        //        .FirstOrDefaultAsync(t => t.Id == teacherId);
        //    var query = await _context.Courses.Include(c => c.Enrollments).Where(t => t.TeacherId == teacherId);
        //}







        // for Admin Dashboard
        public async Task<IdentityResult> CreateTeacherAsync(Teacher teacher, string password)
        {
            var result = await _userManager.CreateAsync(teacher, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacher, "Instructor");
            }

            return result;
        }


        public async Task<IdentityResult> DeleteTeacherAsync(Teacher teacher)
        {
            return await _userManager.DeleteAsync(teacher);
        }

        public async Task<List<Teacher>> GetAll()
        {
            return await _context.Set<Teacher>().ToListAsync();
        }

        public async Task<IdentityResult> UpdateTeacherAsync(Teacher teacher)
        {
            return await _userManager.UpdateAsync(teacher);
        }
        public async Task<int> GetTotalTeacherCountAsync()
        {
            return await _context.Teachers.CountAsync();
        }

        
    }
}
