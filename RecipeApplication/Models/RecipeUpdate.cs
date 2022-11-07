using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApplication.Data;

namespace RecipeApplication.Models
{
    public class RecipeUpdate:EditRecipe
    {
        public int Id { get; set; }

        public void UpdateRecipe(Recipe recipe)
        {
            recipe.Name = Name;
            recipe.TimeToCook = new TimeSpan(TimeToCookHours, TimeToCookMinuts, 0);
            recipe.Method = Method;
            recipe.IsVegetarian = IsVegetarian;
            recipe.IsVegan = IsVegan;
        }
    }
}
