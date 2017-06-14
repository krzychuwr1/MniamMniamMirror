using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MniamMniam.Controllers;
using MniamMniam.Repositories;
using Moq;
using System.Linq;
using MniamMniam.Models.CookBookModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MniamMniamTests.Controllers
{
    [TestClass]
    public class RecipesControllerTests
    {
        Mock<IRecipesRepository> recipesRepo = new Mock<IRecipesRepository>();
        
        [TestMethod]
        public void IndexTest()
        {
            recipesRepo.Setup(repo => repo.GetAllRecipes()).Returns(recipes);

            var controller = new RecipesController(null, null, recipesRepo.Object, null, null, null, null);

            var result = controller.Index() as ViewResult;

            Assert.AreEqual(result.Model, recipes);
        }

        [TestMethod]
        public async Task AdvancedSearchTest()
        {
            string recText = "Recipe1Text";

            var filteredRecipes = recipes.Where(r => r.Text == recText);

            recipesRepo.Setup(repo => 
                repo
                .GetFilteredRecipes("", recText, "", "", "", "", ""))
                .Returns(filteredRecipes);

            var controller = new RecipesController(null, null, recipesRepo.Object, null, null, null, null);

            var result = await controller.advancedSearch(null, recText, null, null, null) as ViewResult;

            Assert.AreEqual(result.Model, filteredRecipes);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void IndexTestNoRepository()
        {
            recipesRepo.Setup(repo => repo.GetAllRecipes()).Returns(
                new List<Recipe>()
                    {
                        new Recipe() { }
                    });

            var controller = new RecipesController(null, null, null, null, null, null, null);
            
            controller.Index();
        }

        private List<Recipe> recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Id = 1,
                    Name = "Recipe1",
                    Text = "Recipe1Text",
                    ApplicationUserId = Guid.NewGuid().ToString()
                },
                new Recipe()
                {
                    Id = 2,
                    Name = "Recipe2",
                    Text = "Recipe2Text",
                    ApplicationUserId = Guid.NewGuid().ToString()
                }
            };
    }
}
