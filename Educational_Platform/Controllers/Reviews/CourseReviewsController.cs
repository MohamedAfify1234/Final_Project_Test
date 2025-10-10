using Educational_Platform.BLL.Services.Courses;
using Educational_Platform.BLL.Services.Reviews;
using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.DAL.Entities.Reviews;
using Educational_Platform.DAL.Entities.Users;
using Educational_Platform.ViewModels.CourseReviewViewModels;
using Educational_Platform.ViewModels.CoursesViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers.Reviews
{
    public class CourseReviewsController : Controller
    {
        AppDbContext Context = new AppDbContext();
        CourseReviewBL CourseReviewBL = new CourseReviewBL();

        // GET: CourseReviews/index
        public IActionResult Index()
        {
            //var appDbContext = _context.CourseReviews.Include(c => c.Course).Include(c => c.User);
            //return View(await appDbContext.ToListAsync());
            List<CourseReview> CourseReviews = CourseReviewBL.GetAll();
            return View("Index", CourseReviews);
        }

        

        // GET: CourseReviews/Create
        public IActionResult Create()
        {
            CourseReviewViewModel CRVM = new CourseReviewViewModel();
            CRVM.Courses = new SelectList(Context.Courses.ToList(), "Id", "Title");
            CRVM.Users = new SelectList(Context.Set<User>().ToList(), "Id", "FullName");
            return View("Create", CRVM);
        }


        [HttpPost]

        public IActionResult Create(CourseReviewViewModel CRVM)
        {
            if (ModelState.IsValid)
            {
                CourseReview CourseReview = new CourseReview();
                CourseReview.Rating = CRVM.Rating;
                CourseReview.Comment = CRVM.Comment;
                CourseReview.ReviewDate = CRVM.ReviewDate;
                CourseReview.Id = Guid.NewGuid();
                CourseReview.ContentRating = CRVM.ContentRating;
                CourseReview.TeachingRating = CRVM.TeachingRating;
                CourseReview.IsApproved = CRVM.IsApproved;
                CourseReview.CourseId = CRVM.CourseId;
                CourseReview.UserId = CRVM.UserId;
                CourseReviewBL.Add(CourseReview);
                return RedirectToAction(nameof(Index));
            }
            CRVM.Courses = new SelectList(Context.Courses.ToList(), "Id", "Title");
            CRVM.Users = new SelectList(Context.Set<User>().ToList(), "Id", "FullName");
            return View("Create", CRVM);
        }

        // GET: CourseReviews/Edit/5
        public IActionResult Edit(Guid id)
        {
            CourseReview CourseReview = CourseReviewBL.GetById(id);
            CourseReviewViewModel CRVM = new CourseReviewViewModel();
            CRVM.Rating = CourseReview.Rating;
            CRVM.Comment = CourseReview.Comment;
            CRVM.ReviewDate = CourseReview.ReviewDate;
            CRVM.Id = CourseReview.Id;
            CRVM.ContentRating = CourseReview.ContentRating;
            CRVM.TeachingRating = CourseReview.TeachingRating;
            CRVM.IsApproved = CourseReview.IsApproved;
            CRVM.CourseId = CourseReview.CourseId;
            CRVM.UserId = CourseReview.UserId;
            if (CourseReview == null)
            {
                return NotFound();
            }
            CRVM.Courses = new SelectList(Context.Courses.ToList(), "Id", "Title");
            CRVM.Users = new SelectList(Context.Set<User>().ToList(), "Id", "FullName");
            return View("Edit", CRVM);
        }


        [HttpPost]
        public IActionResult Edit(Guid id, CourseReviewViewModel CRVM)
        {
            if (id != CRVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CourseReview OldCourseReview = CourseReviewBL.GetById(id);
                    OldCourseReview.Rating = CRVM.Rating;
                    OldCourseReview.Comment = CRVM.Comment;
                    OldCourseReview.ReviewDate = CRVM.ReviewDate;
                    OldCourseReview.Id = Guid.NewGuid();
                    OldCourseReview.ContentRating = CRVM.ContentRating;
                    OldCourseReview.TeachingRating = CRVM.TeachingRating;
                    OldCourseReview.IsApproved = CRVM.IsApproved;
                    OldCourseReview.CourseId = CRVM.CourseId;
                    OldCourseReview.UserId = CRVM.UserId;
                    CourseReviewBL.Update();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseReviewExists(CRVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            CRVM.Courses = new SelectList(Context.Courses.ToList(), "Id", "Title");
            CRVM.Users = new SelectList(Context.Set<User>().ToList(), "Id", "FullName");
            return View("Edit", CRVM);
        }

        // GET: CourseReviews/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CourseReview CourseReview = CourseReviewBL.GetById(id);

            if (CourseReview == null)
            {
                return NotFound();
            }

            return View("Delete",CourseReview);
        }

        // POST: CourseReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            CourseReview CourseReview = CourseReviewBL.GetById(id);
            if (CourseReview != null)
            {
                CourseReviewBL.Delete(CourseReview);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool CourseReviewExists(Guid id)
        {
            return Context.CourseReviews.Any(e => e.Id == id);
        }
    }
}
