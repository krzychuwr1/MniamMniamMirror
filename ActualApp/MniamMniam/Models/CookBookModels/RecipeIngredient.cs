using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Models.CookBookModels
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }

        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int Amount { get; set; }
    }
}
