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

namespace MniamMniam.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMapper _mapper;

        private readonly IRecipesRepository recipesRepository;

        public RecipesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IRecipesRepository recipesRepository)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            this.recipesRepository = recipesRepository;

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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(new CreateRecipeViewModel()
            {
                AllTags = _context.Tags.ToList().Select(tag => new SelectListItem() { Text = tag.Name, Value = tag.Id.ToString() }),
                AllIngredients = _context.Ingredients.ToList().Select(ing => new SelectListItem() { Text = $"{ing.Name} {ing.Unit}", Value = ing.Id.ToString() })
            }
            );
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel recipeViewModel)
        {
            var recipe = new Recipe();
            //if (ModelState.IsValid) bring it back once we figure out why it says it is not valid when less than 3 ingredients chosen
            //{
                recipe.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                var now = DateTime.Now;
                recipe.CreatedAt = now;
                recipe.UpdatedAt = now;
                recipe.Name = recipeViewModel.Name;
                recipe.Text = recipeViewModel.Text;

                var tags = _context.Tags.Where(tag => recipeViewModel.SelectedTags.Contains(tag.Id));
                recipe.Tags = tags.Select(tag => new RecipeTag() { Recipe = recipe, Tag = tag }).ToList();

                var ingredient1 = _context.Ingredients.FirstOrDefault(ing => ing.Id == recipeViewModel.SelectedIngredient1);
                var ingredient2 = _context.Ingredients.FirstOrDefault(ing => ing.Id == recipeViewModel.SelectedIngredient2);
                var ingredient3 = _context.Ingredients.FirstOrDefault(ing => ing.Id == recipeViewModel.SelectedIngredient3);

                recipe.Ingredients = new List<RecipeIngredient>();

                if(recipeViewModel.SelectedIngredientAmount1.HasValue && ingredient1 != null)
                {
                    recipe.Ingredients.Add(new RecipeIngredient() { Recipe = recipe, Ingredient = ingredient1, Amount = recipeViewModel.SelectedIngredientAmount1.Value });
                }
                if (recipeViewModel.SelectedIngredientAmount2.HasValue && ingredient2 != null)
                {
                    recipe.Ingredients.Add(new RecipeIngredient() { Recipe = recipe, Ingredient = ingredient2, Amount = recipeViewModel.SelectedIngredientAmount2.Value });
                }
                if (recipeViewModel.SelectedIngredientAmount3.HasValue && ingredient3 != null)
                {
                    recipe.Ingredients.Add(new RecipeIngredient() { Recipe = recipe, Ingredient = ingredient3, Amount = recipeViewModel.SelectedIngredientAmount3.Value });
                }

                await recipesRepository.Add(recipe);
            
                return RedirectToAction("Index");

            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", recipe.ApplicationUserId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", recipe.ApplicationUserId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Text,Id")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            var oldRecipe = recipesRepository.GetRecipe(id);

            recipe = _mapper.Map(recipe, oldRecipe);

            if (ModelState.IsValid)
            {
                try
                {
                    await recipesRepository.Update(recipe);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", recipe.ApplicationUserId);
            return View(recipe);
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", recipe.ApplicationUserId);

            if (!_context.FavouriteRecipes.Any(fav => fav.RecipeId == id && fav.ApplicationUserId == userId))
            {
                var favouriteRecipe = new FavouriteRecipe()
                {
                    ApplicationUser = _context.Users.First(u => u.Id == userId),
                    Recipe = recipe
                };

                _context.FavouriteRecipes.Add(favouriteRecipe);

                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(FavouriteRecipes));
        }

        private bool RecipeExists(int id) => recipesRepository.GetRecipe(id) != null;
    }
}
