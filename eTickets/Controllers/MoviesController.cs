using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            /*
             *SELECT [m].[Id], [m].[CinemaId], [m].[Description], [m].[EndDate], [m].[ImageURL], [m].[MovieCategory], [m].[Name], [m].[Price], [m].[ProducerId], [m].[StartDate], [c].[Id], [c].[Description], [c].[Logo], [c].[Name]
               FROM [Movies] AS [m]
               INNER JOIN [Cinemas] AS [c] ON [m].[CinemaId] = [c].[Id]
             */
            return View(allMovies);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allMovies.Where(n => n.Name.Contains(searchString) 
                || n.Description.Contains(searchString)).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allMovies);
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
		public async Task<IActionResult> Create() 
		{
			/*
			ViewData["Welcome"] = "Welcome to our store";
			ViewBag.Description = "This is the store description";
			return View();		
			*/
			var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
			ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
			return View();
        }

		[HttpPost]
		public async Task<IActionResult> Create(NewMovieVM movie)
		{
			if (!ModelState.IsValid)
			{
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movie);
			}
			await _service.AddNewMovieAsync(movie);
			return RedirectToAction(nameof(Index));
		}




        //Get: Movies/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            /*
			ViewData["Welcome"] = "Welcome to our store";
			ViewBag.Description = "This is the store description";
			return View();		
			*/
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null)
            {
                return View("NotFound");
            }
            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList()
            };
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movie);
            }
            await _service.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }


        
    }
}
