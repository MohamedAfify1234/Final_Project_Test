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
    public class SubCategoriesController : Controller
    {
        AppDbContext Context = new AppDbContext();
        SubCategoryBL SubCategoryBL = new SubCategoryBL();
        CourseCategoryBL CourseCategoryBL = new CourseCategoryBL();
        // GET: SubCategories
        public IActionResult Index()
        {
            //return View(await _context.CourseCategories.ToListAsync());
            List<SubCategory> SubCategories = SubCategoryBL.GetAll();
            return View("Index", SubCategories);
        }

        // GET: /SubCategories/Create
        public IActionResult Create()
        {
            SubCategoryViewModel SCVM = new SubCategoryViewModel();
            SCVM.Categories = new SelectList(CourseCategoryBL.GetAll(), "Id", "Name");
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
                SubCategoryBL.Add(SubCategory);
                return RedirectToAction(nameof(Index));
            }
            SCVM.Categories = new SelectList(CourseCategoryBL.GetAll(), "Id", "Name");
            return View("Create", SCVM);
        }





        // GET: SubCategories/Edit/5
        public IActionResult Edit(Guid id)
        {
            SubCategory SubCategory = SubCategoryBL.GetById(id);
            SubCategoryViewModel SCVM = new SubCategoryViewModel();
            SCVM.Id = SubCategory.Id;
            SCVM.Name = SubCategory.Name;
            SCVM.Description = SubCategory.Description;
            SCVM.IsActive = SubCategory.IsActive;;
            if (SubCategory == null)
            {
                return NotFound();
            }
            SCVM.Categories = new SelectList(CourseCategoryBL.GetAll(), "Id", "Name");
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
                try
                {
                    SubCategory OldSubCategory = SubCategoryBL.GetById(id);
                    OldSubCategory.Id = SCVM.Id;
                    OldSubCategory.Name = SCVM.Name;
                    OldSubCategory.Description = SCVM.Description;
                    OldSubCategory.IsActive = SCVM.IsActive;
                    SubCategoryBL.Update();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(SCVM.Id))
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
            SCVM.Categories = new SelectList(CourseCategoryBL.GetAll(), "Id", "Name");
            return View("Edit", SCVM);
        }

        // GET: SubCategories/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SubCategory SubCategory = SubCategoryBL.GetById(id);

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
            SubCategory SubCategory = SubCategoryBL.GetById(id);
            if (SubCategory != null)
            {
                SubCategoryBL.Delete(SubCategory);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool SubCategoryExists(Guid id)
        {
            return Context.SubCategories.Any(e => e.Id == id);
        }
    }
}
