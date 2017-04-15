using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MniamMniam.Models.CookBookModels;

namespace MniamMniam.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Recipe> Recipes { get; set; }
    }
}
