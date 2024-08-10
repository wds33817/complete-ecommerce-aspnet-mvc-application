using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using eTickets.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService _service;

        public ActorsController(IActorsService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Actors/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

		
		[HttpPost]
        public async Task<IActionResult> Create([Bind("FullName, ProfilePictureURL, Bio")]Actor actor)
        {
			ModelState.Remove(nameof(Actor.Actors_Movies)); // 方法一 

			if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

		/*方法二
		[HttpPost]
		public async Task<IActionResult> Create(ActorViewModel actorViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(actorViewModel);
			}

			var actor = new Actor
			{
				ProfilePictureURL = actorViewModel.ProfilePictureURL,
				FullName = actorViewModel.FullName,
				Bio = actorViewModel.Bio
			};

			_service.Add(actor);
			return RedirectToAction(nameof(Index));
		}
        */

        //Get: Actors/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }
            return View(actorDetails);
        }




		//Get: Actors/Create
		public async Task<IActionResult> Edit(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null)
			{
				return View("NotFound");
			}
			return View(actorDetails);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, ProfilePictureURL, Bio")] Actor actor)
		{
			ModelState.Remove(nameof(Actor.Actors_Movies)); // 方法一 

			if (!ModelState.IsValid)
			{
				return View(actor);
			}
			await _service.UpdateAsync(id, actor);
			return RedirectToAction(nameof(Index));
		}



		//Get: Actors/Delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null)
			{
				return View("NotFound");
			}
			return View(actorDetails);
		}


		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null)
			{
				return View("NotFound");
			}
			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
