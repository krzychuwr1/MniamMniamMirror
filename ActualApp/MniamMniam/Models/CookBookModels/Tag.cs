using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Models.CookBookModels
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<RecipeIngredient> Recipes { get; set; }
    }
}
