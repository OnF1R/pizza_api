using pizza_api.Models;
using pizza_api.Models.Pagination;

namespace pizza_api.Services.Interfaces
{
    public interface IPizzaService
    {
        Task<List<Pizza>> GetPizzasAsync(PaginationParams @params, SortFilter sortFilter);
        Task<List<Pizza>> GetPizzasWithTypeAsync(PaginationParams @params, int pizzaType, SortFilter sortFilter);
        Task<List<Pizza>> GetPizzasWithCategoryAsync(PaginationParams @params, int pizzaType, SortFilter sortFilter);
        Task<Pizza> GetPizzaAsync(int id);
        Task<Pizza> AddPizzaAsync(Pizza pizza);
        Task<Pizza> UpdatePizzaAsync(Pizza pizza);
        Task<(bool, string)> DeletePizzaAsync(Pizza pizza);
        Task<List<Pizza>> SearchPizza(string pizzaTitle);
    }
}
