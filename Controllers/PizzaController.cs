using Microsoft.AspNetCore.Mvc;
using pizza_api.Models;
using pizza_api.Models.Pagination;
using pizza_api.Services.Interfaces;
using System.Text.Json;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPizzas([FromQuery] PaginationParams @params, SortFilter sortFilter)
        {
            var pizzas = await _pizzaService.GetPizzasAsync(@params, sortFilter);

            var paginationMetadata = new PaginationMetadata(pizzas.Count(), @params.Page, @params.ItemsPerPage);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var paginatedPizzas = pizzas.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage);

            if (paginatedPizzas == null || paginatedPizzas.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Pizzas in database");
            }

            return StatusCode(StatusCodes.Status200OK, paginatedPizzas);
        }

        [HttpGet("WithType")]
        public async Task<IActionResult> GetPizzasWithType([FromQuery] PaginationParams @params, int pizzaType, SortFilter sortFilter)
        {
            var pizzas = await _pizzaService.GetPizzasWithTypeAsync(@params, pizzaType, sortFilter);

            var paginationMetadata = new PaginationMetadata(pizzas.Count(), @params.Page, @params.ItemsPerPage);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var paginatedPizzas = pizzas.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage);

            if (paginatedPizzas == null || paginatedPizzas.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Pizzas in database");
            }

            return StatusCode(StatusCodes.Status200OK, paginatedPizzas);
        }

        [HttpGet("WithCategory")]
        public async Task<IActionResult> GetPizzasWithCategory([FromQuery] PaginationParams @params, int pizzaCategory, SortFilter sortFilter)
        {
            var pizzas = await _pizzaService.GetPizzasWithCategoryAsync(@params, pizzaCategory, sortFilter);

            var paginationMetadata = new PaginationMetadata(pizzas.Count(), @params.Page, @params.ItemsPerPage);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var paginatedPizzas = pizzas.Skip((@params.Page - 1) * @params.ItemsPerPage).Take(@params.ItemsPerPage);

            if (paginatedPizzas == null || paginatedPizzas.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Pizzas in database");
            }

            return StatusCode(StatusCodes.Status200OK, paginatedPizzas);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetPizza(int id)
        {
            Pizza pizza = await _pizzaService.GetPizzaAsync(id);

            if (pizza == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Pizza found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, pizza);
        }

        [HttpPost]
        public async Task<ActionResult<Pizza>> AddPizza([FromQuery]Pizza pizza)
        {
            var pizzaDB = await _pizzaService.AddPizzaAsync(pizza);

            if (pizzaDB == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{pizza.Title} could not be added.");
            }

            return CreatedAtAction("GetPizza", new { id = pizzaDB.Id }, pizzaDB);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdatePizza(int id, [FromQuery]Pizza pizza)
        {
            if (id != pizza.Id)
            {
                return BadRequest();
            }

            Pizza pizzaDB = await _pizzaService.UpdatePizzaAsync(pizza);

            if (pizzaDB == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{pizza.Title} could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeletePizza(int id)
        {
            var pizza = await _pizzaService.GetPizzaAsync(id);
            (bool status, string message) = await _pizzaService.DeletePizzaAsync(pizza);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, pizza);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchPizza(string query)
        {
            var pizzas = await _pizzaService.SearchPizza(query);

            if (pizzas == null || !pizzas.Any())
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Pizzas in database with title '{query}'");
            }

            return StatusCode(StatusCodes.Status200OK, pizzas);
        }
    }
}
