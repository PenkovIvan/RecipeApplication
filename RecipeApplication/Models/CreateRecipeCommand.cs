using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApplication.Data;

namespace RecipeApplication.Models
{
    public class CreateRecipeCommand : EditRecipe
    {
        public IList<CreateIngredient> Ingredients { get; set; } = new List<CreateIngredient>();

        public Recipe ToRecipe(ApplicationUser createdBy)
        {
            return new Recipe
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHours, TimeToCookMinuts, 0),
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Ingredients = Ingredients?.Select(x => x.GetIngredient()).ToList(),
                CreateById = createdBy.Id
            };
        }
    }
}
