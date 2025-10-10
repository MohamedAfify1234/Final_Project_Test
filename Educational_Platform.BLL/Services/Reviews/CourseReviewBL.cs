using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Reviews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational_Platform.BLL.Services.Reviews
{
    public class CourseReviewBL
    {
        AppDbContext Context = new AppDbContext();
        public List<CourseReview> GetAll()
        {
            return Context.CourseReviews.ToList();
        }
        public void Add(CourseReview CourseReview)
        {
            Context.Add(CourseReview);
            Context.SaveChanges();
        }
        public CourseReview GetById(Guid Id)
        {
            return Context.CourseReviews.Include(CR => CR.Course)
                .Include(CR => CR.User)
                .FirstOrDefault(c => c.Id == Id);
        }
        public void Update()
        {
            Context.SaveChanges();
        }
        public void Delete(CourseReview CourseReview)
        {
            Context.CourseReviews.Remove(CourseReview);
            Context.SaveChanges();
        }
    }
}
