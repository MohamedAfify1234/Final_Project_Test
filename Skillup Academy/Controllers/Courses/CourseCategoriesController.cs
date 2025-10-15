using Infrastructure.Services.Courses;
using Infrastructure.Data;
using Core.Models.Courses;
using Skillup_Academy.ViewModels.CoursesViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.Courses;

namespace Skillup_Academy.Controllers.Courses
{
    public class CourseCategoriesController : Controller
    {

        ICourseCategoryRepsitory CourseCategoryRepsitory;
        public CourseCategoriesController(ICourseCategoryRepsitory _CourseCategoryRepsitory)
        {
            CourseCategoryRepsitory = _CourseCategoryRepsitory;
        }
        // GET: /CourseCategories/index
        public IActionResult Index()
        {
            List<CourseCategory> CourseCategories = CourseCategoryRepsitory.GetAll();
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
                CourseCategoryRepsitory.Add(courseCategory);
                CourseCategoryRepsitory.Save();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", CCVM);
        }

        // GET: CourseCategories/Edit/5
        public IActionResult Edit(Guid id)
        {
            CourseCategory courseCategory = CourseCategoryRepsitory.GetById(id);
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

                    CourseCategory OldCourseCategory = CourseCategoryRepsitory.GetById(id);
                    OldCourseCategory.Id = CCVM.Id;
                    OldCourseCategory.Name = CCVM.Name;
                    OldCourseCategory.Description = CCVM.Description;
                    OldCourseCategory.IsActive = CCVM.IsActive;
                    OldCourseCategory.Icon = CCVM.Icon;
                    CourseCategoryRepsitory.Update(OldCourseCategory);
                    CourseCategoryRepsitory.Save();
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
            CourseCategory courseCategory = CourseCategoryRepsitory.GetById(id);

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
            CourseCategory courseCategory = CourseCategoryRepsitory.GetById(id);
            if (courseCategory != null)
            {
                CourseCategoryRepsitory.Delete(courseCategory);
                CourseCategoryRepsitory.Save();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}
