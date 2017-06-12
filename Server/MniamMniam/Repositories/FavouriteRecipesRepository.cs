using MniamMniam.Data;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Repositories
{
    public interface IFavouriteRecipesRepository
    {
        IEnumerable<FavouriteRecipe> GetAllFavouriteRecipes();
        Task Add(FavouriteRecipe favouriteRecipe);
        Task Remove(int id);
    }

    public class FavouriteRecipesRepository : IFavouriteRecipesRepository
    {
        private readonly ApplicationDbContext db;

        public FavouriteRecipesRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<FavouriteRecipe> GetAllFavouriteRecipes() => db.FavouriteRecipes;

        public async Task Add(FavouriteRecipe favourite)
        {
            db.FavouriteRecipes.Add(favourite);
            await db.SaveChangesAsync();
        }

        public async Task Remove(int id)
        {
            var favouriteRecipe = db.FavouriteRecipes.FirstOrDefault(rec => rec.Id == id);
            db.FavouriteRecipes.Remove(favouriteRecipe);
            await db.SaveChangesAsync();
        }
    }
}
