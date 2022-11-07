using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Data;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipes
{
    [Authorize]
    public class CreateRecipeModel : PageModel
    {
        [BindProperty]
        public CreateRecipeCommand Input { get; set; }
        private RecipeService _service;
        private readonly UserManager<ApplicationUser> _userService;
        public CreateRecipeModel(RecipeService service, UserManager<ApplicationUser> userService)
        {
            _service = service;
            _userService = userService;
        }
        public void OnGet()
        {
            Input = new CreateRecipeCommand();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var appUser = await _userService.GetUserAsync(User);
                    var id = await _service.CreateRecipe(Input, appUser);
                    return RedirectToPage("View", new { id = id });
                }
            }
            catch (Exception)
            {
                //TODO(ЗАДАЧА): Ошибка в журнале
                //Добавьте ошибку на уровне модели, используя пустой строковый ключ
                ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении рецепта");
            }
            //Если мы добрались сюда, значит, что-то пошло не так
            return Page();

        }
    }
}
