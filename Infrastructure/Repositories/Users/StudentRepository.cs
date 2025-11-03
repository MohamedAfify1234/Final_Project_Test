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
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public StudentRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateStudentAsync(Student student, string password)
        {
            var result = await _userManager.CreateAsync(student, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student, "Student");
            }

            return result;
        }
        public async Task<IdentityResult> DeleteStudentAsync(Student student)
        {
            return await _userManager.DeleteAsync(student);
        }

        public async Task<List<Student>> GetAll()
        {
            return _context.Students.ToList();
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user as Student;
        }

        public async Task<IdentityResult> UpdateStudentAsync(Student student)
        {
            return await _userManager.UpdateAsync(student);
        }
        public async Task<int> GetTotalStudentCountAsync()
        {
            return await _context.Students.CountAsync();
        }
    }
}
