using Microsoft.EntityFrameworkCore;
using MniamMniam.Data;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Repositories
{
    public interface IRecipesRepository
    {
        IEnumerable<Recipe> GetAllRecipes();

        IEnumerable<Recipe> GetAllRecipesWithReviews();

        IEnumerable<Recipe> GetFilteredRecipes(
            string name = "", 
            string text = "", 
            string userName = "", 
            string tagName = "", 
            string ingredientName = "",
            string userId = "",
            string favouritesUserId = "");

        Recipe GetRecipe(int id);

        Recipe GetRecipeWithReviews(int id);

        Task Remove(int id);

        Task Add(Recipe recipe);

        Task Update(Recipe recipe);
    }

    public class RecipesRepository : IRecipesRepository
    {
        ApplicationDbContext db;

        public RecipesRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        private IQueryable<Recipe> getAllRecipes()
            =>
            db.Recipes
            .Include(r => r.ApplicationUser)
            .Include(r => r.Tags).ThenInclude(tag => tag.Tag)
            .Include(r => r.Ingredients).ThenInclude(ing => ing.Ingredient);

        public IEnumerable<Recipe> GetAllRecipes() => getAllRecipes();

        public IEnumerable<Recipe> GetAllRecipesWithReviews() => getAllRecipes().Include(rec => rec.Reviews);

        public IEnumerable<Recipe> GetFilteredRecipes(
            string name = "", 
            string text = "", 
            string userName = "", 
            string tagName = "", 
            string ingredientName = "",
            string userId = "",
            string favouritesUserId = "")
        {
            var result = getAllRecipes();

            result = !string.IsNullOrEmpty(name) ? result.Where(rec => rec.Name.Contains(name)) : result;
            result = !string.IsNullOrEmpty(text) ? result.Where(rec => rec.Text.Contains(text)) : result;
            result = !string.IsNullOrEmpty(userName) ? result.Where(rec => rec.ApplicationUser.UserName.Contains(userName)) : result;
            result = !string.IsNullOrEmpty(tagName) ? result.Where(rec => rec.Tags.Any(tag => tag.Tag.Name.Contains(tagName))) : result;
            result = !string.IsNullOrEmpty(ingredientName) ? result.Where(rec => rec.Ingredients.Any(ing => ing.Ingredient.Name.Contains(ingredientName))) : result;
            result = !string.IsNullOrEmpty(userId) ? result.Where(rec => rec.ApplicationUserId == userId) : result;
            result = !string.IsNullOrEmpty(favouritesUserId) ? result.Where(rec => rec.Favourites.Any(fav => fav.ApplicationUserId == favouritesUserId)) : result;

            return result;
        }

        public Recipe GetRecipe(int id) => getAllRecipes().FirstOrDefault(rec => rec.Id == id);

        public Recipe GetRecipeWithReviews(int id) => getAllRecipes().Include(rec => rec.Reviews).FirstOrDefault(rec => rec.Id == id);

        public async Task Remove(int id)
        {
            var recipe = db.Recipes.FirstOrDefault(rec => rec.Id == id);
            db.Recipes.Remove(recipe);
            await db.SaveChangesAsync();
        }

        public async Task Add(Recipe recipe)
        {
            db.Recipes.Add(recipe);
            await db.SaveChangesAsync();
        }

        public async Task Update(Recipe recipe)
        {
            db.Update(recipe);
            await db.SaveChangesAsync();
        }
    }
}
