using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApplication.Models;

namespace RecipeApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RecipeService _service;
        private readonly ILogger<IndexModel> _log;
        /* public IEnumerable<RecipeSummaryModel> Recipes { get; private set; }*/// заменяем на  ICollection<RecipeSummaryModel> Recipes, чтобы работали метод логирования, например LogInformation
        public ICollection<RecipeSummaryModel> Recipes { get; private set; }
        public IndexModel(RecipeService service, ILogger<IndexModel> log)
        {
            _service = service;
            _log = log;
        }
        public async Task OnGet()
        {
            Recipes = await _service.GetRecipes();
            _log.LogInformation("Loaded {RecipeCount} recipes", Recipes.Count);

        }
    }
}
