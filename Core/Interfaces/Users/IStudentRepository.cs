using Core.Models.Courses;
using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Users
{
    public interface IStudentRepository
    {
        // CRUD Operations
        Task<List<Student>> GetAll();
        Task<Student> GetStudentByIdAsync(string id);

        Task<IdentityResult> UpdateStudentAsync(Student student);

      
        Task<IdentityResult> DeleteStudentAsync(Student student);
        Task<IdentityResult> CreateStudentAsync(Student student, string password);
        Task<int> GetTotalStudentCountAsync();

        // Additional Methods Specific to Students can be added here


    }
}
