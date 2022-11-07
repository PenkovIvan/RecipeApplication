using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApplication.Data;

namespace RecipeApplication.Models
{
    public class RecipeSummaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimeToCook { get; set; }
        public int NumOfIngred { get; set; }

        public static RecipeSummaryModel FromRecipe(Recipe recipe)
        {
            return new RecipeSummaryModel { Id = recipe.RecipeId, Name = recipe.Name, TimeToCook = $"{recipe.TimeToCook.Hours} часов, {recipe.TimeToCook.Minutes} минут" };
        }
    }
}
