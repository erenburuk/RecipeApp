using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Models;

namespace RecipeApp.Pages.Recipe
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RecipeApp.Models.RecipeAppContext _context;

        public IndexModel(RecipeApp.Models.RecipeAppContext context)
        {
            _context = context;
        }

        public IList<Models.Recipe> Recipe { get; set; } = default!;
        public List<double> RecipeAverageRatingsList { get; set; } = default!;
        
        public async Task CalculateRatings()
        {
            // Get all recipes
            var AllRecipes = await (from re in _context.Recipes select re).ToListAsync();

            // Get all ratings
            var AllRatings = await (from ra in _context.Ratings select ra).ToListAsync();

            // Initialize and fill [BindProperty] RecipeAverageRatingsList containing average ratings
            RecipeAverageRatingsList = new List<Double>();

            foreach (var item in AllRecipes)
            {
                // find all ratings of the current recipe in foreach loop

                List<int?> reciperates = await (from x in _context.Ratings where x.RecipeId == item.Id select x.Value).ToListAsync();

                if (reciperates.Count >= 1)
                {
                    // calculate current recipe's average rating
                    double avg = CalculateRate.GetRating(reciperates);
                    avg = Math.Round(avg, 1); // round to 1 decimal places
                    RecipeAverageRatingsList.Add(avg);
                }
                else // If recipe has no ratings do NOT calculate average and add 0
                {
                    // Set values
                    RecipeAverageRatingsList.Add(0.0);
                }
            }
        }
       
        public string userId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        /*
        public async Task OnGetAsync()
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId
            if (_context.Recipes != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    Recipe = await _context.Recipes.Where(s => s.Title.Contains(SearchString)).ToListAsync();
                    await CalculateRatings();
                }
                else
                {
                    Recipe = await _context.Recipes.ToListAsync();
                }
            }
        }*/

        public async Task OnGetAsync()
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId
            if (_context.Recipes != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    Recipe = await _context.Recipes.Where(s => s.Title.Contains(SearchString)).ToListAsync();
                }
                else
                {
                    Recipe = await _context.Recipes.ToListAsync();
                }
                await CalculateRatings();
            }
        }
        /*
        public async Task OnGetAsync()
        {
            if (_context.Recipes != null)
            {
                Recipe = await _context.Recipes.ToListAsync();
                await CalculateRatings();
            }
        }*/
    }
}
