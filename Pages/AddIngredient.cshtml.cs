using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;
using RecipeApp.Data;
using System.Security.Claims;

namespace RecipeApp.Pages
{
    public class AddIngredientModel : PageModel
    {
        private readonly RecipeAppContext _context;

        public AddIngredientModel(RecipeAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Ingredient Ingredient { get; set; }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var ingredient = new Models.Ingredient
                {
                    Name = Ingredient.Name,
                    Quantity = Ingredient.Quantity,
                    Unit = Ingredient.Unit,
                    RecipeId = id
                };

                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Recipe/Index");
        }
    }
}
