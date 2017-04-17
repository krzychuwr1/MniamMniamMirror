using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Models.CookBookModels
{
    public class Review
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        public string Text { get; set; }

        [ScaffoldColumn(false)]
        public int RecipeId { get; set; }

        [ScaffoldColumn(false)]
        public Recipe Recipe { get; set; }

        [ScaffoldColumn(false)]
        public int ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
