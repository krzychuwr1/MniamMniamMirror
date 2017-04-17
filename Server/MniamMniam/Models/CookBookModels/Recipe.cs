using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Models.CookBookModels
{
    public class Recipe
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [ScaffoldColumn(false)]
        public DateTime UpdatedAt { get; set; }

        [ScaffoldColumn(false)]
        public string ApplicationUserId { get; set; }

        [Display(Name="Owner")]
        public ApplicationUser ApplicationUser { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
