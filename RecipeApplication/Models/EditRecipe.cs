using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeApplication.Models
{
    public class EditRecipe
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Range(0, 24), DisplayName("Время приготовления (ч)")]
        public int TimeToCookHours { get; set; }

        [Required,Range(0,59), DisplayName("Время приготовления (мин)")]
        public int TimeToCookMinuts { get; set; }

        public string Method { get; set; }

        [DisplayName("Vegetarian?")]
        public bool IsVegetarian { get; set; }
        [DisplayName("Vegan?")]
        public bool IsVegan { get; set; }

    }
}
