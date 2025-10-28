using System.Diagnostics;
using Core.Models.Courses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.Models;
using Skillup_Academy.ViewModels.SearchViewModels;

namespace Skillup_Academy.Controllers
{ 
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _context;

		public HomeController(ILogger<HomeController> logger,AppDbContext Context)
        {
            _logger = logger;
			_context = Context;
		}

        public IActionResult Index()
        {
            return View();
        }
 
 		[HttpGet]
		public async Task<IActionResult> SearchResults( string? query, List<Guid>? categoryIds, bool isfree, int page = 1)
		{
			int pageSize = 9;  

 			var results = _context.Courses
				.Include(c => c.Teacher)  
				.Include(c => c.SubCategory)  
				.Where(c => c.IsPublished) 
				.AsQueryable();

 			if (!string.IsNullOrWhiteSpace(query))
			{
 				string normalizedQuery = query.ToLower();

				results = results.Where(c =>
					c.Title.ToLower().Contains(normalizedQuery) ||
					c.Description.ToLower().Contains(normalizedQuery) ||
					(c.Teacher != null && c.Teacher.FullName.ToLower().Contains(normalizedQuery))
				);
			}

 			if (categoryIds != null && categoryIds.Any())
			{
 				results = results.Where(c =>
					categoryIds.Contains(c.SubCategoryId ?? Guid.Empty) ||
					categoryIds.Contains(c.CategoryId ?? Guid.Empty)|| c.IsFree==isfree
				);
			}
			 
 			int totalResults = await results.CountAsync();

 			var pagedCourses = await results
				.OrderByDescending(c => c.CreatedDate) // ????? ????
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

 			var viewModel = new SearchResultsViewModel
			{
				Courses = pagedCourses,
				TotalResults = totalResults,
				CurrentPage = page,
				PageSize = pageSize,
				Query = query,
				SelectedCategoryIds = categoryIds,
				IsFree=isfree,
 				Categories = await _context.CourseCategories.ToListAsync(),
				SubCategories = await _context.SubCategories.ToListAsync()
			};

			return View(viewModel);
		}


		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
