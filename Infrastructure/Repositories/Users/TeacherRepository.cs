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
            //return await _context.Users
            //    .OfType<Teacher>()
            //    .FirstOrDefaultAsync(t => t.Id.ToString() == teacherId);
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


    }
}
