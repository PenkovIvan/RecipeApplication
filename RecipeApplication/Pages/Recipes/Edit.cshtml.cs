using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeApplication.Data;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipes
{
    [Authorize]
    public class EditModel : PageModel
    {
        [BindProperty]
        public RecipeUpdate Input { get; set; }
        private readonly RecipeService _service;
        private readonly IAuthorizationService _authService;

        public EditModel(RecipeService service, IAuthorizationService authService)
        {
            _service = service;
            _authService = authService;
        }

        public async Task<IActionResult> OnGet(int id)
        {

            var recipe = await _service.GetRecipe(id);
            var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }
            Input = await _service.GetRecipeForUpdate(id);
            if (Input is null)
            {
                // Если идентификатор не соответствует действительному рецепту, создайте страницу с ошибкой 404
                // TODO(ЗАДАЧА): Добавить промежуточное программное обеспечение с кодовыми страницами статуса, чтобы отобразить дружественную страницу 404
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var recipe = await _service.GetRecipe(Input.Id);
                var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
                if (!authResult.Succeeded)
                {
                    return new ForbidResult();
                }
                if (ModelState.IsValid)
                {
                    await _service.UpdateRecipe(Input);
                    return RedirectToPage("View", new { id = Input.Id });
                }
            }
            catch (Exception)
            {
                // TODO: Ошибка в журнале
                // Добавьте ошибку на уровне модели, используя пустой строковый ключ
                ModelState.AddModelError(string.Empty, "Произошла ошибка при сохранении рецепта");
            }

            //Если мы добрались сюда, значит, что-то пошло не так
            return Page();
        }
    }
}
