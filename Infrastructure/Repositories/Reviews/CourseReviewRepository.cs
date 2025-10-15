using Core.Interfaces.Reviews;
using Core.Models.Courses;
using Core.Models.Reviews;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Reviews
{
    public class CourseReviewRepository : ICourseReviewRepository
    {
        //AppDbContext Context = new AppDbContext();
        AppDbContext Context;
        public CourseReviewRepository(AppDbContext _Context)
        {
            Context  = _Context;
        }
        public List<CourseReview> GetAll()
        {
            return Context.CourseReviews.ToList();
        }
        public void Add(CourseReview CourseReview)
        {
            Context.Add(CourseReview);
        }
        public CourseReview GetById(Guid Id)
        {
            return Context.CourseReviews.Include(CR => CR.Course)
                .Include(CR => CR.User)
                .FirstOrDefault(c => c.Id == Id);
        }
        public void Update(CourseReview CourseReview)
        {
            Context.Update(CourseReview);
        }
        public void Delete(CourseReview CourseReview)
        {
            Context.CourseReviews.Remove(CourseReview);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
