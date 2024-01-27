using pizza_api.Models;
using pizza_api.Models.Pagination;

namespace pizza_api.Services.Interfaces
{
    public interface IPizzaService
    {
        Task<List<Pizza>> GetPizzasAsync(PaginationParams @params);
        Task<Pizza> GetPizzaAsync(int id);
        Task<Pizza> AddPizzaAsync(Pizza pizza);
        Task<Pizza> UpdatePizzaAsync(Pizza pizza);
        Task<(bool, string)> DeletePizzaAsync(Pizza pizza);
        Task<List<Pizza>> SearchPizza(string pizzaTitle);
    }
}
