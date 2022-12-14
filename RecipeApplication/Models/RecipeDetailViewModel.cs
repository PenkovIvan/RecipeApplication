using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApplication.Data;

namespace RecipeApplication.Models
{
    public class RecipeDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public IEnumerable<Input> Ingredients { get; set; }

        public class Input
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }
}
