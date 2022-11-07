using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeApplication.Models;

namespace RecipeApplication.Pages.Recipes
{
    public class ViewModel : PageModel
    {
        public RecipeDetailViewModel Recipe { get; set; }
        public bool CanEditRecipe { get; set; }

        private readonly RecipeService _service;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<ViewModel> _log;
        public ViewModel(RecipeService service, IAuthorizationService authService, ILogger<ViewModel> log)
        {
            _service = service;
            _authService = authService;
            _log = log;
        }
        public async Task<IActionResult> OnGetAsync(int id) //��������� ������ Recipe ��� ������������� � IAuthorizationService.
        {
            Recipe = await _service.GetRecipeDetail(id);
            if (Recipe is null)
            {
                _log.LogWarning(12, "�� ������� ����� ������ � ��������������� {RecipeId}", id);
                //���� ������������� �� ������������� ��������������� �������, �������� �������� � ������� 404
                // TODO(������): �������� ������������� ����������� ����������� � �������� ���������� �������, ����� ���������� ������������� �������� 404
                return NotFound();
            }
            var recipe = await _service.GetRecipe(id);
            var isAuthorised = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");// ���������, ����� �� ������������ ����� ������������� ������.
            CanEditRecipe = isAuthorised.Succeeded;//������ �������� CanEditRecipe ������ PageModel ��������������� �������.
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var recipe = await _service.GetRecipe(id);
            var authResult = await _authService.AuthorizeAsync(User, recipe, "CanManageRecipe");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }
            await _service.DeleteRecipe(id);

            return RedirectToPage("/Index");
        }
    }
}
