using Educational_Platform.DAL.Entities.Courses;
using Microsoft.AspNetCore.Mvc;

namespace Educational_Platform.Controllers.Courses
{
	public class CourseCategoryController : Controller
	{ 
		public IActionResult ShowAll()
		{
			return View();
		}



	}
}
