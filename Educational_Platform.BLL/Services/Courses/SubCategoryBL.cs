using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational_Platform.BLL.Services.Courses
{
    public class SubCategoryBL
    {
        AppDbContext Context = new AppDbContext();
        public List<SubCategory> GetAll()
        {
            return Context.SubCategories.ToList();
        }
        public void Add(SubCategory SubCategory)
        {
            Context.Add(SubCategory);
            Context.SaveChanges();
        }
        public SubCategory GetById(Guid Id)
        {
            return Context.SubCategories.FirstOrDefault(c => c.Id == Id);
        }
        public void Update()
        {
            Context.SaveChanges();
        }
        public void Delete(SubCategory subCategory)
        {
            Context.SubCategories.Remove(subCategory);
            Context.SaveChanges();
        }
    }
}
