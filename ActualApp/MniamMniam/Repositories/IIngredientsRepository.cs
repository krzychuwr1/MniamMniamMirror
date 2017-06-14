using MniamMniam.Data;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Repositories
{
    public interface IIngredientsRepository
    {
        IEnumerable<Ingredient> GetAllIngredients();
    }

    public class IngredientsRepository : IIngredientsRepository
    {
        private readonly ApplicationDbContext db;

        public IngredientsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Ingredient> GetAllIngredients() => db.Ingredients;
    }
}
