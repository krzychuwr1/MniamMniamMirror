using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.ViewModels
{
    public class CreateRecipeViewModel
    {
        public string Name { get; set; }

        [Display(Name="Time")]
        public int TimeNeeded { get; set; }

        public string Text { get; set; }

        [Display(Name="How to prepare")]
        public string DetailedText { get; set; }

        public int[] SelectedTags { get; set; }

        public int[] SelectedIngredient { get; set; }

        public int[] SelectedIngredientAmount { get; set; }

        public IFormFile file { get; set; }

        public List<IFormFile> files { get; set; }

        public IEnumerable<SelectListItem> AllTags { get; set; }

        public IEnumerable<SelectListItem> AllIngredients { get; set; }
        
    }

    public class EditRecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public string Name { get; set; }

        [Display(Name = "Time")]
        public int TimeNeeded { get; set; }

        public string Text { get; set; }

        [Display(Name = "How to prepare")]
        public string DetailedText { get; set; }

        [ScaffoldColumn(false)]
        public string ApplicationUserId { get; set; }

        public int[] SelectedTags { get; set; }

        public int[] SelectedIngredient { get; set; }

        public int[] SelectedIngredientAmount { get; set; }

        public IFormFile file { get; set; }

        public List<IFormFile> files { get; set; }

        public IEnumerable<SelectListItem> AllTags { get; set; }

        public IEnumerable<SelectListItem> AllIngredients { get; set; }


    }
}
