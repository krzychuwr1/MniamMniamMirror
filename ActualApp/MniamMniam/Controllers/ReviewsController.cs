using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MniamMniam.Data;
using MniamMniam.Models.CookBookModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MniamMniam.Models;

namespace MniamMniam.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reviews.Include(r => r.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Recipe)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [HttpGet]
        public async Task<IActionResult> Create(int recipeId)
        {
            var recipe = await 
                _context.Recipes
                .Include(rec => rec.ApplicationUser)
                .FirstOrDefaultAsync(rec => rec.Id == recipeId);

            if(recipe != null)
            {
                ViewData["RecipeId"] = recipe.Id;
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int recipeId, [Bind("Score,Text")] Review review)
        {
            if (ModelState.IsValid)
            {
                var recipe = await
                    _context.Recipes
                    .Include(rec => rec.ApplicationUser)
                    .FirstOrDefaultAsync(rec => rec.Id == recipeId);

                review.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                review.ApplicationUser = await _userManager.GetUserAsync(HttpContext.User);

                review.RecipeId = recipe.Id;
                review.CreatedAt = DateTime.Now;
                review.UpdatedAt = DateTime.Now;

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Recipes", new { id = recipeId });
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", review.RecipeId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", review.RecipeId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Score,Text")] Review review)
        {
            var oldReview = _context.Reviews.Include(rev => rev.ApplicationUser).Include(rev => rev.Recipe).FirstOrDefault(rev => rev.Id == id);

            if (ModelState.IsValid)
            {
                try
                {
                    oldReview.Text = review.Text;
                    oldReview.Score = review.Score;
                    oldReview.UpdatedAt = DateTime.Now;
                    //_context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", review.RecipeId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Recipe)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
