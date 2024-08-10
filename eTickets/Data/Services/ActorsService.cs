using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eTickets.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }


        /*
		public async Task<IEnumerable<Actor>> GetAllAsync()
		{
			var result = await _context.Actors.ToListAsync();
			return result;
		}

		public async Task<Actor> GetByIdAsync(int id)
		{
			var result = await _context.Actors.FirstOrDefaultAsync(n => n.Id == id);
			return result;
		}
        */


	
    }
}
