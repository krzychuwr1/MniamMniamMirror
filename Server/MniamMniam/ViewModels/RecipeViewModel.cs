using Microsoft.AspNetCore.Mvc.Rendering;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.ViewModels
{
    public class CreateRecipeViewModel
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public int[] SelectedTags { get; set; }

        public IEnumerable<SelectListItem> AllTags { get; set; }
    }
}
