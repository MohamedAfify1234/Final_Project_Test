using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Educational_Platform.BLL.Services.Courses
{
    public class CourseCategoryBL
    {
        AppDbContext Context = new AppDbContext();
        public List<CourseCategory> GetAll()
        {
            return Context.CourseCategories.ToList();
        }
        public void Add(CourseCategory CourseCategory)
        {
            Context.Add(CourseCategory);
            Context.SaveChanges();
        }
        public CourseCategory GetById(Guid Id)
        {
            return Context.CourseCategories.FirstOrDefault(c => c.Id == Id);
        }
        public void Update()
        {
            Context.SaveChanges();
        }
        public void Delete(CourseCategory cource)
        {
            Context.CourseCategories.Remove(cource);
            Context.SaveChanges();
        }
    }
}
