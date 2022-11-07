using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models;
using RecipeApplication.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace RecipeApplication.Pages
{
    public class RecipeService
    {
        readonly MyDbContext myDbContext;
        readonly ILogger _loger;
        public RecipeService(MyDbContext context, ILoggerFactory factory)
        {
            myDbContext = context;
            _loger = factory.CreateLogger<RecipeService>();
        }

        //загурзка списка элементов
        public async Task<List<RecipeSummaryModel>> GetRecipes()
        {
            return await myDbContext.Recipes
                .Where(r => !r.IsDeleted)
                .Select(x => new RecipeSummaryModel
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    TimeToCook = $"{x.TimeToCook.Hours} часов {x.TimeToCook.Minutes} минут",

                }).ToListAsync();
        }
        public async Task<Recipe> GetRecipe(int recipeId)
        {
            return await myDbContext.Recipes
                .Where(x => x.RecipeId == recipeId)
                 .SingleOrDefaultAsync();
        }

        public async Task<bool> DoesRecipeExistAsync(int id)
        {
            return await myDbContext.Recipes
                .Where(r => !r.IsDeleted)
                .Where(r => r.RecipeId == id)
                .AnyAsync();
        }

        //загрузка одной записи
        public async Task<RecipeDetailViewModel> GetRecipeDetail(int id)
        {
            return await myDbContext.Recipes
                .Where(x => x.RecipeId == id)
                .Where(x => !x.IsDeleted)
                .Select(x => new RecipeDetailViewModel
                {
                    Id = x.RecipeId,
                    Name = x.Name,
                    Method = x.Method,
                    Ingredients = x.Ingredients
                    .Select(input => new RecipeDetailViewModel.Input
                    {
                        Name = input.Name,
                        Quantity = $"{input.Quantity} {input.Unit}"
                    })
                })
                .SingleOrDefaultAsync();
        }


        public async Task<RecipeUpdate> GetRecipeForUpdate(int recipeId)
        {
            return await myDbContext.Recipes
                .Where(x => x.RecipeId == recipeId)
                .Where(x => !x.IsDeleted)
                .Select(x => new RecipeUpdate
                {
                    Name = x.Name,
                    Method = x.Method,
                    TimeToCookHours = x.TimeToCook.Hours,
                    TimeToCookMinuts = x.TimeToCook.Minutes,
                    IsVegan = x.IsVegan,
                    IsVegetarian = x.IsVegetarian,
                })
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Создайте новый рецепт
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>Идентификатор Id нового рецепта</returns>
        public async Task<int> CreateRecipe(CreateRecipeCommand cmd, ApplicationUser createdBy)
        {
            var recipe = cmd.ToRecipe(createdBy);
            myDbContext.Add(recipe);
            //recipe.LastModified = DateTimeOffset.UtcNow;
            await myDbContext.SaveChangesAsync();
            return recipe.RecipeId;
        }

        /// <summary>
        /// Обновлен существующий рецепт
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>Идентификатор Id нового рецепта</returns>
        public async Task UpdateRecipe(RecipeUpdate cmd)
        {
            var recipe = await myDbContext.Recipes.FindAsync(cmd.Id);
            if (recipe == null) { throw new Exception("Не удалось найти рецепт"); }
            if (recipe.IsDeleted) { throw new Exception("Невозможно обновить удаленный рецепт"); }

            cmd.UpdateRecipe(recipe);
            //recipe.LastModified = DateTimeOffset.UtcNow;
            await myDbContext.SaveChangesAsync();
        }

        /// <summary>
        ///Помечает существующий рецепт как удаленный
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>Идентификатор нового рецепта</returns>
        public async Task DeleteRecipe(int recipeId)
        {
            var recipe = await myDbContext.Recipes.FindAsync(recipeId);
            if (recipe is null) { throw new Exception("Не удалось найти рецепт"); }

            recipe.IsDeleted = true;
            await myDbContext.SaveChangesAsync();
        }
    }
}