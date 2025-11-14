using Core.DTOs;
using Core.Interfaces.Users;
using Core.Models.Users;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public  async Task<Teacher?> GetTeacherAsync(string teacherId)
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
            if(User == null)
                return (IdentityResult.Failed(new IdentityError { Description = "Teacher not found." }));
            // لليوزر  تحديث البيانات
            User.FullName = teacher.FullName;
            User.Email = teacher.Email;
            User.PhoneNumber = teacher.PhoneNumber;
            User.ProfilePicture = teacher.ProfilePicture;
            var result = await _userManager.UpdateAsync(User);
            
            if(!result.Succeeded)
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



        //Task<IdentityResult> ITeacherRepository.UpdateTeacherInfoAsync(Teacher teacher, Guid teacherId)
        //{
        //    if (!Guid.TryParse(teacherId.ToString(), out Guid teacherGuid))
        //        return false;

        //    var teacher = await _context.Teachers.FindAsync(teacherGuid);
        //    if (teacher == null)
        //        return false;

        //    // تحديث البيانات
        //    teacher.FullName = teacherInfo.FullName;
        //    teacher.Email = teacherInfo.Email;
        //    teacher.PhoneNumber = teacherInfo.PhoneNumber;
        //    teacher.Bio = teacherInfo.Bio;
        //    teacher.Qualifications = teacherInfo.Qualifications;
        //    teacher.Expertise = teacherInfo.Expertise;
        //    teacher.ProfilePicture = teacherInfo.ProfilePicture;

        //    _context.Teachers.Update(teacher);
        //    return await _context.SaveChangesAsync() > 0;
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

        //public async Task<Teacher> GetTeacherByIdAsync(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    return user as Teacher;
        //}

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
