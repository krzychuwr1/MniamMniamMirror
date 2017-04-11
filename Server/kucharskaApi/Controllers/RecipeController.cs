using kucharskaApi.DB;
using kucharskaApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace kucharskaApi.Controllers
{
    /// <summary>
    /// Part of API responsible for recipe actions
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RecipeController : ApiController
    {
        ///<summary>
        ///Gets all recipes
        ///</summary>
        [HttpGet]
        [ActionName(nameof(Read))]
        [ResponseType(typeof(IEnumerable<Recipe>))]
        public IHttpActionResult Read()
        {
            using (var db = new ApplicationDbContext())
            {
                return Ok(db.Recipes.ToList());
            }
        }

        /// <summary>
        /// Gets the recipe for specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(nameof(Read))]
        [ResponseType(typeof(Recipe))]
        public IHttpActionResult Read(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                return Ok(db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id));
            }
        }

        /// <summary>
        /// Updates the recipe with the specified identifier.
        /// If identifier is not provided, it creates the new recipe instead.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="text">The text.</param>
        [HttpPost]
        [ActionName(nameof(Update))]
        public IHttpActionResult Update([FromBody]Recipe recipe)
        {
            using (var db = new ApplicationDbContext())
            {
                if(recipe.RecipeId == 0)
                {
                    db.Recipes.Add(recipe);
                    db.SaveChanges();
                    return Ok(recipe.RecipeId);
                }
                else
                {
                    var rec = db.Recipes.FirstOrDefault(r => r.RecipeId == recipe.RecipeId);
                    if (rec != null)
                    {
                        db.SaveChanges();
                        return Ok(recipe.RecipeId);
                    }
                    else
                    {
                        return Content(HttpStatusCode.BadRequest, "No recipe with such Id");
                    }
                }
                
            }
        }

        /// <summary>
        /// Deletes the recipe with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpPost]
        [ActionName(nameof(Delete))]
        public IHttpActionResult Delete([FromBody]int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var recipe = db.Recipes.FirstOrDefault(rec => rec.RecipeId == id);
                if(recipe != null)
                {
                    db.Recipes.Remove(recipe);
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "No recipe with such Id");
                }

            }
        }
    }
}
