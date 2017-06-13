using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MniamMniam.Data;
using MniamMniam.Models.CookBookModels;
using Microsoft.AspNetCore.Identity;
using MniamMniam.Models;
using AutoMapper;
using MniamMniam.ViewModels;
using MniamMniam.Repositories;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MniamMniam.Controllers
{
    [Authorize]
    public class RecipesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMapper _mapper;

        private readonly IRecipesRepository recipesRepository;

        private readonly IUsersRepository usersRepository;

        private readonly ITagsRepository tagsRepository;

        private readonly IIngredientsRepository ingredientsRepository;

        private readonly IFavouriteRecipesRepository favouriteRecipesRepository;

        public RecipesController(
            UserManager<ApplicationUser> userManager, 
            IMapper mapper, 
            IRecipesRepository recipesRepository,
            IUsersRepository usersRepository,
            ITagsRepository tagsRepository,
            IIngredientsRepository ingredientsRepository,
            IFavouriteRecipesRepository favouriteRecipesRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            this.recipesRepository = recipesRepository;
            this.usersRepository = usersRepository;
            this.tagsRepository = tagsRepository;
            this.ingredientsRepository = ingredientsRepository;
            this.favouriteRecipesRepository = favouriteRecipesRepository;

        }

        // GET: Recipes
        public IActionResult Index() => View(recipesRepository.GetAllRecipes());

        [HttpGet]
        public IActionResult Index(string Name) => Name == null ? Index() : View(recipesRepository.GetFilteredRecipes(name: Name));

        public IActionResult MyRecipes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var recipes = recipesRepository.GetFilteredRecipes(userId: userId);
            return View(recipes);
        }

        public IActionResult FavouriteRecipes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var recipes = recipesRepository.GetFilteredRecipes(favouritesUserId: userId);
            return View(recipes);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = recipesRepository.GetRecipeWithReviews(id.Value);

            if (recipe == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(HttpContext.User);

            ViewBag.IsMyRecipe = recipe.ApplicationUserId == userId;
            ViewBag.UserId = userId;

            return View(recipe);
        }


        public IActionResult advancedSearch() => View(recipesRepository.GetAllRecipes());

        [HttpGet]
        public async Task<IActionResult> advancedSearch(string Name, string Text, string UserName, string TagName, string IngredientName)
        {
            if (Name == null && Text == null && UserName == null && TagName == null && IngredientName == null)
            {
                return advancedSearch();
            }
            if (Name == null) Name = "";
            if (Text == null) Text = "";
            if (UserName == null) UserName = "";
            if (TagName == null) TagName = string.Empty;
            if (IngredientName == null) IngredientName = string.Empty;

            var recipes = recipesRepository.GetFilteredRecipes(Name, Text, UserName, TagName, IngredientName);

            return View(recipes);
        }
        
        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(usersRepository.GetAllUsers(), "Id", "Id");
            return View(new CreateRecipeViewModel()
            {
                AllTags = tagsRepository.GetAllTags().Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.Id.ToString() }),
                AllIngredients = ingredientsRepository.GetAllIngredients().Select(ing => new SelectListItem() { Text = $"{ing.Name} {ing.Unit}", Value = ing.Id.ToString() })
            }
            );
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel recipeViewModel, List<IFormFile> files)
        {
            var recipe = new Recipe();
            if (ModelState.IsValid)
            {
                recipe.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                var now = DateTime.Now;
                recipe.CreatedAt = now;
                recipe.UpdatedAt = now;
                recipe.Name = recipeViewModel.Name;
                recipe.Text = recipeViewModel.Text;
                recipe.DetailedText = recipeViewModel.DetailedText;
                recipe.TimeNeeded = recipeViewModel.TimeNeeded;
              
                var tags = tagsRepository.GetAllTags().Where(tag => recipeViewModel.SelectedTags.Contains(tag.Id));
                recipe.Tags = tags.Select(tag => new RecipeTag() { Recipe = recipe, Tag = tag }).ToList();
                recipe.Ingredients = new List<RecipeIngredient>();

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var fileStream = formFile.OpenReadStream())
                        using (var ms = new MemoryStream())
                        {
                            fileStream.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            string Image = Convert.ToBase64String(fileBytes);
                            recipe.Image = Image;
                        }
                    }
                }
            
                foreach (var pair in recipeViewModel.SelectedIngredient.Zip(recipeViewModel.SelectedIngredientAmount, (ingredient, amount) => new { ingredient, amount }))
                {
                    var ingredient = ingredientsRepository.GetAllIngredients().FirstOrDefault(ing => ing.Id == pair.ingredient);
                    if (ingredient != null)
                    {
                        recipe.Ingredients.Add(new RecipeIngredient() { Recipe = recipe, Ingredient = ingredient, Amount = pair.amount });
                    }
                }
                
                await recipesRepository.Add(recipe);
            
                return RedirectToAction("Index");
            }

            ViewData["ApplicationUserId"] = new SelectList(usersRepository.GetAllUsers(), "Id", "Id", recipe.ApplicationUserId);
            return View(recipeViewModel);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editRecipeViewModel = new EditRecipeViewModel();
            var recipe = recipesRepository.GetRecipe(id.Value);
            if (recipe == null) return NotFound();

            editRecipeViewModel.Recipe = recipe;
            editRecipeViewModel.Name = recipe.Name;
            editRecipeViewModel.Text = recipe.Text;
            editRecipeViewModel.DetailedText = recipe.DetailedText;
            editRecipeViewModel.TimeNeeded = recipe.TimeNeeded;
            editRecipeViewModel.AllIngredients = ingredientsRepository.GetAllIngredients().Select(ing => new SelectListItem() { Text = $"{ing.Name} {ing.Unit}", Value = ing.Id.ToString() });
            editRecipeViewModel.AllTags = tagsRepository.GetAllTags().Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.Id.ToString() });
            ViewData["ApplicationUserId"] = new SelectList(usersRepository.GetAllUsers(), "Id", "Id", recipe.ApplicationUserId);
            return View(editRecipeViewModel);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EditRecipeViewModel recipeViewModel, List<IFormFile> files)
        {

            var recipe = recipesRepository.GetRecipe(id.Value);
            if (recipe == null) return NotFound();
            recipe.Name = recipeViewModel.Name;
            recipe.Text = recipeViewModel.Text;
            recipe.DetailedText = recipeViewModel.DetailedText;
            recipe.TimeNeeded = recipeViewModel.TimeNeeded;

            var tags = tagsRepository.GetAllTags().Where(tag => recipeViewModel.SelectedTags.Contains(tag.Id));
            if (recipeViewModel.SelectedTags != null)
            {
                var tagToAdd = tags.Select(tag => new RecipeTag() { Recipe = recipe, Tag = tag }).ToList();
                foreach (var tag in tagToAdd) recipe.Tags.Add(tag);
            }
            if(recipeViewModel.SelectedIngredient != null)
            foreach (var pair in recipeViewModel.SelectedIngredient.Zip(recipeViewModel.SelectedIngredientAmount, (ingredient, amount) => new { ingredient, amount }))
            {
                var ingredient = ingredientsRepository.GetAllIngredients().FirstOrDefault(ing => ing.Id == pair.ingredient);
                if (ingredient != null)
                {
                    recipe.Ingredients.Add(new RecipeIngredient() { Recipe = recipe, Ingredient = ingredient, Amount = pair.amount });
                }
            }

            foreach (var formFile in filess)
            {
                if (formFile.Length > 0)
                {
                    using (var fileStream = formFile.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string Image = Convert.ToBase64String(fileBytes);
                        recipe.Image = Image;
                    }
                }
            }

            var now = DateTime.Now;
            recipe.UpdatedAt = now;

            recipeViewModel.Recipe = recipe;
          

            try
            {
                await recipesRepository.Update(recipeViewModel.Recipe);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(recipeViewModel.Recipe.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        

        private bool RecipeExists(int id)
        {
            throw new NotImplementedException();
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = recipesRepository.GetRecipe(id.Value);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await recipesRepository.Remove(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddFavourite(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var recipe = recipesRepository.GetRecipe(id.Value);
            if (recipe == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            ViewData["ApplicationUserId"] = new SelectList(usersRepository.GetAllUsers(), "Id", "Id", recipe.ApplicationUserId);

            if (!favouriteRecipesRepository.GetAllFavouriteRecipes().Any(fav => fav.RecipeId == id && fav.ApplicationUserId == userId))
            {
                var favouriteRecipe = new FavouriteRecipe()
                {
                    ApplicationUser = usersRepository.GetAllUsers().First(u => u.Id == userId),
                    Recipe = recipe
                };

                await favouriteRecipesRepository.Add(favouriteRecipe);

            }


            return RedirectToAction(nameof(FavouriteRecipes));
        }

        public async Task<IActionResult> RemoveFavourite(int? id)
        {
            await favouriteRecipesRepository.Remove(id.Value);
            return RedirectToAction(nameof(FavouriteRecipes));
        }

        private byte[] ConvertToBytes(IFormFile image)
        {
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)image.Length);
            return CoverImageBytes;
        }

        

    }
}
