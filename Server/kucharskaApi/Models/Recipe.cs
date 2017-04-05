using System.ComponentModel.DataAnnotations;

namespace kucharskaApi.Models
{
    /// <summary>
    /// Recipe
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// The recipe identifier.
        /// </summary>
        /// <value>
        /// The recipe identifier.
        /// </value>
        [Key]
        public int RecipeId { get; set; }

        /// <summary>
        /// The name of the recipe.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// The text of the recipe.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
    }
}