using Educational_Platform.BLL.Services.Courses;
using Educational_Platform.DAL.Data;
using Educational_Platform.DAL.Entities.Courses;
using Educational_Platform.ViewModels.CoursesViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers.Courses
{
    public class CourseCategoriesController : Controller
    {
        AppDbContext Context = new AppDbContext();
        CourseCategoryBL CourseCategoryBL = new CourseCategoryBL();
        // GET: CourseCategories/index
        public IActionResult Index()
        {
            List<CourseCategory> CourseCategories = CourseCategoryBL.GetAll();
            return View("Index", CourseCategories);
        }

        // GET: /CourseCategories/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SaveCreate(CourseCategoryViewModel CCVM)
        {
            if (ModelState.IsValid)
            {
                CourseCategory courseCategory = new CourseCategory();
                courseCategory.Name = CCVM.Name;
                courseCategory.Description = CCVM.Description;
                courseCategory.IsActive = CCVM.IsActive;
                courseCategory.Icon = CCVM.Icon;

                courseCategory.Id = Guid.NewGuid();
                CourseCategoryBL.Add(courseCategory);
                return RedirectToAction(nameof(Index));
            }
            return View("Create", CCVM);
        }

        // GET: CourseCategories/Edit/5
        public IActionResult Edit(Guid id)
        {
            CourseCategory courseCategory = CourseCategoryBL.GetById(id);
            CourseCategoryViewModel CCVM = new CourseCategoryViewModel();
            CCVM.Id = courseCategory.Id ;
            CCVM.Name = courseCategory.Name ;
            CCVM.Description = courseCategory.Description;
            CCVM.IsActive = courseCategory.IsActive;
            CCVM.Icon  = courseCategory.Icon;
            if (courseCategory == null)
            {
                return NotFound();
            }
            return View("Edit", CCVM);
        }

  
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CourseCategoryViewModel CCVM)
        {
            if (id != CCVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CourseCategory OldCourseCategory = CourseCategoryBL.GetById(id);
                    OldCourseCategory.Id = CCVM.Id;
                    OldCourseCategory.Name = CCVM.Name;
                    OldCourseCategory.Description = CCVM.Description;
                    OldCourseCategory.IsActive = CCVM.IsActive;
                    OldCourseCategory.Icon = CCVM.Icon;
                    CourseCategoryBL.Update();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseCategoryExists(CCVM.Id))
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
            return View("Edit", CCVM);
        }

        // GET: CourseCategories/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CourseCategory courseCategory = CourseCategoryBL.GetById(id);

            if (courseCategory == null)
            {
                return NotFound();
            }

            return View(courseCategory);
        }

        // POST: CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            CourseCategory courseCategory = CourseCategoryBL.GetById(id);
            if (courseCategory != null)
            {
                CourseCategoryBL.Delete(courseCategory);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool CourseCategoryExists(Guid id)
        {
            return Context.CourseCategories.Any(e => e.Id == id);
        }
    }
}
