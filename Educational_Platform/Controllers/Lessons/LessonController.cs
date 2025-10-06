using Microsoft.AspNetCore.Mvc;

namespace Educational_Platform.Controllers.Lessons
{
	public class LessonController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
