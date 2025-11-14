using System.Diagnostics;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models.Courses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.Models;
using Skillup_Academy.ViewModels.HomeViewModels;
using Skillup_Academy.ViewModels.SearchViewModels;
namespace Skillup_Academy.Controllers
{ 
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _context;
		private readonly IRepository<CourseCategory> _repCourCategory;

		public HomeController(ILogger<HomeController> logger,AppDbContext Context,IRepository<CourseCategory> RepCourCategory)
        {
            _logger = logger;
			_context = Context;
			_repCourCategory = RepCourCategory;
		}

        public async Task<IActionResult> Index()
        {
			HomeViewModel homeViewModel = new HomeViewModel();
			{
				homeViewModel.ListCategory = await _repCourCategory.Query()
					.Where(s=>s.SubCategories.Count!=0)
					.Select(e => new CourseCategoryInHomeViewModel { Image = e.Icon, Name = e.Name , Id =e.Id })
					.ToListAsync();

				homeViewModel.NewlyAdded = await _context.Courses
					.Where(c => c.IsPublished)
					.Include(c => c.Teacher)
					.Include(c => c.Category)
					.OrderByDescending(c => c.CreatedDate)
					.Take(6)
					.Select(c => new CourseNewlyInHomeViewModel
					{
						Id = c.Id,
						Title = c.Title,
						ShortDescription = c.ShortDescription,
						TotalDuration = c.TotalDuration,
						Image = c.ThumbnailUrl,
						IsFree = c.IsFree,
 						TeacherName = c.Teacher != null ? c.Teacher.FullName : "Unknown",
						CategoryName = c.Category != null ? c.Category.Name : "Unknown",
						AverageRating = c.AverageRating,
 					})
					.ToListAsync();

				homeViewModel.MostPopular = await _context.Courses
					.Where(c => c.IsPublished)
					.Include(c => c.Teacher)
					.Include(c=>c.Category)
					.OrderByDescending(c => c.TotalEnrollments)
					.Take(6)
					.Select(c => new CourseNewlyInHomeViewModel
					{
						Id = c.Id,
						Title = c.Title,
						ShortDescription = c.ShortDescription,
						TotalDuration = c.TotalDuration,
						Image = c.ThumbnailUrl,
						IsFree = c.IsFree,
						TeacherName = c.Teacher != null ? c.Teacher.FullName : "Unknown",
						CategoryName = c.Category != null ? c.Category.Name : "Unknown",
						AverageRating = c.AverageRating,
					})
					.ToListAsync();
			};

			return View(homeViewModel);
        }


		[HttpGet]
		public async Task<IActionResult> ViewAllCourseNewlyAdded(int page = 1)
		{
			int pageSize = 9;

			var NewlyAdded = await _context.Courses
				.Where(c => c.IsPublished)
				.Include(c => c.Teacher)
				.Include(c => c.Category)
				.OrderByDescending(c => c.CreatedDate)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(c => new CourseNewlyInHomeViewModel
				{
					Id = c.Id,
					Title = c.Title,
					ShortDescription = c.ShortDescription,
					TotalDuration = c.TotalDuration,
					Image = c.ThumbnailUrl,
					IsFree = c.IsFree,
					TeacherName = c.Teacher != null ? c.Teacher.FullName : "Unknown",
					CategoryName = c.Category != null ? c.Category.Name : "Unknown",
					AverageRating = c.AverageRating,
				})
				.ToListAsync();
			ViewBag.Page = page;
			return View(NewlyAdded);
		}


		[HttpGet]
		public async Task<IActionResult> ViewAllCourseMostPopular(int page = 1)
		{
			int pageSize = 9;

			var MostPopular = await _context.Courses
				.Where(c => c.IsPublished)
				.Include(c => c.Teacher)
				.Include(c => c.Category)
				.OrderByDescending(c => c.TotalEnrollments)
				.Skip((page-1) * pageSize)
				.Take(pageSize)
				.Select(c => new CourseNewlyInHomeViewModel
				{
					Id = c.Id,
					Title = c.Title,
					ShortDescription = c.ShortDescription,
					TotalDuration = c.TotalDuration,
					Image = c.ThumbnailUrl,
					IsFree = c.IsFree,
					TeacherName = c.Teacher != null ? c.Teacher.FullName : "Unknown",
					CategoryName = c.Category != null ? c.Category.Name : "Unknown",
					AverageRating = c.AverageRating,
				})
				.ToListAsync();
			ViewBag.Page = page;
			return View(MostPopular);
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
