using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using pizza_api.Database;
using pizza_api.Models;
using pizza_api.Models.Pagination;
using pizza_api.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace pizza_api.Services.Implimentations
{
    public class PizzaService : IPizzaService
    {
        private const string PizzaListCacheKey = "PizzaList";
        private readonly ApplicationContext _db;
        private IMemoryCache _cache;


        public PizzaService(ApplicationContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<List<Pizza>> GetPizzasAsync(PaginationParams @params)
        {
            try
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(90));

                if (_cache.TryGetValue(PizzaListCacheKey, out List<Pizza> pizzas))
                    return pizzas;

                pizzas = await _db.Pizzas.ToListAsync();

                _cache.Set(PizzaListCacheKey + $"_page_{@params.Page}", pizzas, cacheOptions);

                return pizzas;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Pizza> GetPizzaAsync(int id)
        {
            try
            {
                Pizza pizza = await _db.Pizzas.FindAsync(id);

                if (pizza == null)
                {
                    return null;
                }

                return pizza;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Pizza> AddPizzaAsync(Pizza pizza)
        {
            try
            {
                Pizza createdPizza = new Pizza()
                {
                    ImageUrl = pizza.ImageUrl,
                    Title = pizza.Title,
                    Types = pizza.Types,
                    Sizes = pizza.Sizes,
                    Category = pizza.Category,
                    Price = pizza.Price,
                    Rating = pizza.Rating,
                };

                await _db.Pizzas.AddAsync(createdPizza);

                await _db.SaveChangesAsync();

                return await _db.Pizzas.FindAsync(createdPizza.Id);
            }
            catch (Exception)
            {
                return null;
            }


        }

        public async Task<Pizza> UpdatePizzaAsync(Pizza pizza)
        {
            try
            {
                Pizza pizzaDB = await _db.Pizzas.FindAsync(pizza.Id);

                if (pizzaDB == null)
                {
                    return null;
                }

                pizzaDB.Title = pizza.Title;
                pizzaDB.ImageUrl = pizza.ImageUrl;
                pizzaDB.Sizes = pizza.Sizes;
                pizzaDB.Types = pizza.Types;
                pizzaDB.Price = pizza.Price;
                pizzaDB.Category = pizza.Category;
                pizzaDB.Rating = pizza.Rating;

                _db.Entry(pizzaDB).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                return pizzaDB;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeletePizzaAsync(Pizza pizza)
        {
            try
            {
                Pizza pizzaDB = await _db.Pizzas.FindAsync(pizza.Id);

                if (pizzaDB == null)
                {
                    return (false, "Pizza could not be found");
                }

                _db.Pizzas.Remove(pizza);

                await _db.SaveChangesAsync();

                return (true, "Pizza got deleted");
            }
            catch (Exception ex)
            {
                return (false, $"Error, {ex.Message}");
            }
        }

        public async Task<List<Pizza>> SearchPizza(string pizzaTitle)
        {
            try
            {
                if (pizzaTitle.Length < 3 || string.IsNullOrWhiteSpace(pizzaTitle))
                    return null;
                pizzaTitle = pizzaTitle.ToLower();

                var vns = await _db.Pizzas.Where(pizza => pizza.Title.ToLower().Contains(pizzaTitle)).ToListAsync(); // TODO

                return vns;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
