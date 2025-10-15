using Core.Interfaces.Courses;
using Core.Models.Courses;
using Infrastructure.Data;
using Infrastructure.Services.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.ViewModels.CoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skillup_Academy.Controllers.Courses
{
    public class SubCategoriesController : Controller
    {

        ICourseCategoryRepsitory CourseCategoryRepsitory;
        ISubCategoryRepository SubCategoryRepsitory;
        public SubCategoriesController(ICourseCategoryRepsitory _CourseCategoryRepsitory, ISubCategoryRepository _SubCategoryRepsitory)
        {
            CourseCategoryRepsitory = _CourseCategoryRepsitory;
            SubCategoryRepsitory = _SubCategoryRepsitory;
        }
        // GET: SubCategories
        public IActionResult Index()
        {
            //return View(await _context.CourseCategories.ToListAsync());
            List<SubCategory> SubCategories = SubCategoryRepsitory.GetAll();
            return View("Index", SubCategories);
        }

        // GET: /SubCategories/Create
        public IActionResult Create()
        {
            SubCategoryViewModel SCVM = new SubCategoryViewModel();
            SCVM.Categories = new SelectList(CourseCategoryRepsitory.GetAll(), "Id", "Name");
            return View("Create", SCVM);
        }


        [HttpPost]
        public IActionResult SaveCreate(SubCategoryViewModel SCVM)
        {
            if (ModelState.IsValid)
            {
                SubCategory SubCategory = new SubCategory();
                SubCategory.Name = SCVM.Name;
                SubCategory.Description = SCVM.Description;
                SubCategory.IsActive = SCVM.IsActive;
                SubCategory.Id = Guid.NewGuid();
                SubCategory.CategoryId = SCVM.CategoryId;
                SubCategoryRepsitory.Add(SubCategory);
                SubCategoryRepsitory.Save();
                return RedirectToAction(nameof(Index));
            }
            SCVM.Categories = new SelectList(CourseCategoryRepsitory.GetAll(), "Id", "Name");
            return View("Create", SCVM);
        }

        // GET: SubCategories/Edit/5
        public IActionResult Edit(Guid id)
        {
            SubCategory SubCategory = SubCategoryRepsitory.GetById(id);
            SubCategoryViewModel SCVM = new SubCategoryViewModel();
            SCVM.Id = SubCategory.Id;
            SCVM.Name = SubCategory.Name;
            SCVM.Description = SubCategory.Description;
            SCVM.IsActive = SubCategory.IsActive;;
            if (SubCategory == null)
            {
                return NotFound();
            }
            SCVM.Categories = new SelectList(CourseCategoryRepsitory.GetAll(), "Id", "Name");
            return View("Edit", SCVM);
        }

        // POST: SubCategories/Edit/5
        [HttpPost]
        public IActionResult Edit(Guid id, SubCategoryViewModel SCVM)
        {
            if (id != SCVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                    SubCategory OldSubCategory = SubCategoryRepsitory.GetById(id);
                    OldSubCategory.Id = SCVM.Id;
                    OldSubCategory.Name = SCVM.Name;
                    OldSubCategory.Description = SCVM.Description;
                    OldSubCategory.IsActive = SCVM.IsActive;
                    SubCategoryRepsitory.Update(OldSubCategory);
                    SubCategoryRepsitory.Save();

                return RedirectToAction(nameof(Index));
            }
            SCVM.Categories = new SelectList(CourseCategoryRepsitory.GetAll(), "Id", "Name");
            return View("Edit", SCVM);
        }

        // GET: SubCategories/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubCategory SubCategory = SubCategoryRepsitory.GetById(id);

            if (SubCategory == null)
            {
                return NotFound();
            }

            return View("Delete", SubCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            SubCategory SubCategory = SubCategoryRepsitory.GetById(id);
            if (SubCategory != null)
            {
                SubCategoryRepsitory.Delete(SubCategory);
                SubCategoryRepsitory.Save();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}
