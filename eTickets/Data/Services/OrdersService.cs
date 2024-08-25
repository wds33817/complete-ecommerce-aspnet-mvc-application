using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
			_context = context;
		}
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
		{
			var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Movie).Include(n => n.User).ToListAsync();
            /* LINQ
			 * SELECT [o].[Id], [o].[Email], [o].[UserId], [a].[Id], [t].[Id], [t].[Amount], [t].[MovieId], [t].[OrderId], [t].[Price], [t].[Id0], [t].[CinemaId], [t].[Description], [t].[EndDate], [t].[ImageURL], [t].[MovieCategory], [t].[Name], [t].[Price0], [t].[ProducerId], [t].[StartDate], [a].[AccessFailedCount], [a].[ConcurrencyStamp], [a].[Email], [a].[EmailConfirmed], [a].[FullName], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
				FROM [Orders] AS [o]
				INNER JOIN [AspNetUsers] AS [a] ON [o].[UserId] = [a].[Id]
				LEFT JOIN (
					SELECT [o0].[Id], [o0].[Amount], [o0].[MovieId], [o0].[OrderId], [o0].[Price], [m].[Id] AS [Id0], [m].[CinemaId], [m].[Description], [m].[EndDate], [m].[ImageURL], [m].[MovieCategory], [m].[Name], [m].[Price] AS [Price0], [m].[ProducerId], [m].[StartDate]
					FROM [OrderItems] AS [o0]
					INNER JOIN [Movies] AS [m] ON [o0].[MovieId] = [m].[Id]
				) AS [t] ON [o].[Id] = [t].[OrderId]
				ORDER BY [o].[Id], [a].[Id], [t].[Id]
			 */
            if (userRole != "admin")
			{
				orders = orders.Where(n => n.UserId == userId).ToList();
			}

			return orders;
		}

		public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
		{
			var order = new Order()
			{
				UserId = userId,
				Email = userEmailAddress
			};
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			foreach (var item in items)
			{
				var orderItem = new OrderItem()
				{
					Amount = item.Amount,
					MovieId = item.Movie.Id,
					OrderId = order.Id,
					Price = item.Movie.Price
				};
				await _context.OrderItems.AddAsync(orderItem);
			}
			await _context.SaveChangesAsync();
		}
	}
}
