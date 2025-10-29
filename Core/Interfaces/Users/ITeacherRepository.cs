using Core.DTOs;
using Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Users
{
    public interface ITeacherRepository 
    {
        Task<Teacher?> GetTeacherAsync(string teacherId);
        Task<List<CourseDashboardDTO>> GetTeacherCoursesAsync(Guid teacherId);
        Task<int> GetTotalStudentsAsync(Guid teacherId);
        Task<TeacherDashboardDTO> GetTeacherDashboardAsync(Guid teacherId);
        
        
       

    }
}
