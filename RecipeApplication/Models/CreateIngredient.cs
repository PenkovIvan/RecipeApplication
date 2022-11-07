using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RecipeApplication.Data;

namespace RecipeApplication.Models
{
    public class CreateIngredient
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public decimal Quantity { get; set; }
        [Required]
        [StringLength(20)]
        public string Unit { get; set; }

        public Ingredient GetIngredient()
        {
            return new Ingredient { Name = Name, Quantity = Quantity, Unit = Unit };
        }
    }
}
