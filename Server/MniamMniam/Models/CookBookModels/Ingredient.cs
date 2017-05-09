using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Models.CookBookModels
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public List<RecipeIngredient> Recipes { get; set; }
    }
}
