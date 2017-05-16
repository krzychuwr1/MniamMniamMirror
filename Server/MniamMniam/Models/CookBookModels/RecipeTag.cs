﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Models.CookBookModels
{
    public class RecipeTag
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        public Tag Tag{ get; set; }

        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
