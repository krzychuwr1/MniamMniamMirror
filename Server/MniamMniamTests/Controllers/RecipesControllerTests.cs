using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MniamMniam.Controllers;
using MniamMniam.Repositories;
using Moq;
using System.Linq;
using MniamMniam.Models.CookBookModels;
using System.Collections.Generic;

namespace MniamMniamTests.Controllers
{
    [TestClass]
    public class RecipesControllerTests
    {
        Mock<IRecipesRepository> recipesRepo = new Mock<IRecipesRepository>();

        [TestMethod]
        public void IndexTest()
        {
            recipesRepo.Setup(repo => repo.GetAllRecipes()).Returns(
                new List<Recipe>()
                    {
                        new Recipe() { }
                    });

            var controller = new RecipesController(null, null, recipesRepo.Object, null, null, null, null);
            
            var result = controller.Index();
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
            
            var result = controller.Index();
        }
    }
}
