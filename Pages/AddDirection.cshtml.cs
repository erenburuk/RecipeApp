using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApp.Models;
using RecipeApp.Data;
using System.Security.Claims;

namespace RecipeApp.Pages
{
    public class AddDirectionModel : PageModel
    {
        private readonly RecipeAppContext _context;

        public AddDirectionModel(RecipeAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Direction Direction { get; set; }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var direction = new Direction
                {
                    StepNumber = Direction.StepNumber,
                    Description = Direction.Description,
                    RecipeId = id
                };

                _context.Directions.Add(direction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Recipe/Index");
        }
    }
}
