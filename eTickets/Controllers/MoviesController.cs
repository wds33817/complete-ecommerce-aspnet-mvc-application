using eTickets.Data;
using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service)
        {
			_service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);
            return View(allMovies);
        }
		//Get: Movies/Details/1
		public async Task<IActionResult> Details(int id)
		{
			var movieDetails = await _service.GetMovieByIdAsync(id);
			if (movieDetails == null)
			{
				return View("NotFound");
			}
			return View(movieDetails);
		}

		//Get: Movies/Create
		public IActionResult Create() 
		{
			ViewData["Welcome"] = "Welcome to our store";
			ViewBag.Description = "This is the store description";
			return View();		
		}
	}
}
