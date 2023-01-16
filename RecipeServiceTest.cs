using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using NewFoodie.Models;
using Moq;

using NewFoodie.Services.EFServices;
using FoodieTests;
using Microsoft.EntityFrameworkCore;

namespace NewFoodie.Areas.Identity.Pages.Account.Manage.Tests
{
    [TestClass()]
    public class RecipeServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("rececipMemoeryDb").Options;
        private AppDbContext dbContext;
        private EFRecipeService _recipeService;

        private List<Recipe> OriginalAllRecipes;

        //private List<Recipe> RecipesAddSweetRibs;
        //private List<Recipe> RecipesDeleteSpicyFish;
        //private List<Recipe> NoRecipesFound;
        private Recipe RecipeSweetRib;

        public RecipeServiceTest()
        {
        }

        //Prepare data
        [TestInitialize()]
        public void Setup()
        {
            dbContext = new AppDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            //_mockDbContext = new Mock<AppDbContext>();
            _recipeService = new EFRecipeService(dbContext);

            OriginalAllRecipes = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = TestDataProvider.GetRecipeItemsForSpicyFish().ToList() },
                new Recipe() { Id = 2, Name = "Spicy Pork", RecipeItems = TestDataProvider.GetecipeItemsForSpicyPork().ToList() }
            };

            //RecipesAddSweetRibs = new List<Recipe>()
            //{
            //    new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = TestDataProvider.GetRecipeItemsForSpicyFish().ToList() },
            //    new Recipe() { Id = 2, Name = "Spicy Pork", RecipeItems = TestDataProvider.GetecipeItemsForSpicyPork().ToList() },
            //    new Recipe() { Id = 3, Name = "Sweet Ribs", RecipeItems = TestDataProvider.GetRecipeItemsForSweetRibs().ToList() }
            //};

            //RecipesDeleteSpicyFish = new List<Recipe>()
            //{
            //    new Recipe() { Id = 2, Name = "Spicy Pork", RecipeItems = TestDataProvider.GetecipeItemsForSpicyPork().ToList() }
            //};

            RecipeSweetRib = new Recipe()
            {
                Id = 3,
                Name = "Sweet Ribs",
                RecipeItems = TestDataProvider.GetRecipeItemsForSweetRibs()
            };
        }

        [TestCleanup]
        public void CleanUp() { 
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [TestMethod()]
        public void AddRecipeTest()
        {
            //Arrange
            var expected = RecipeSweetRib;

            //Act
            _recipeService.AddRecipe(expected);

            var found = dbContext.Recipes.Find(expected.Id);

            //Assert
            Assert.AreEqual(found.Name, expected.Name);
            Assert.AreEqual(found.RecipeItems, expected.RecipeItems);
        }

        [TestMethod()]
        public void GetAllRecipesTest()
        {
            //Arrange
            var searchCriteria = new RecipeCriteria
            {
                SearchCategory = "Ingredient",
                SearchCriterion = "No this Ingredient"
            };

            dbContext.Recipes.AddRange(OriginalAllRecipes);
            dbContext.SaveChanges();

            //Act
            var found = _recipeService.GetAllRecipes();
            var countOfResults = found.Count();

            //Assert
            Assert.IsTrue(countOfResults == OriginalAllRecipes.Count); ;
        }


        [TestMethod()]
        public void DeleteRecipeTest()
        {
            //Arrange
            dbContext.Add(RecipeSweetRib);
            dbContext.SaveChanges();

            //Act
            _recipeService.DeleteRecipe(RecipeSweetRib);
            var found = dbContext.Recipes.Find(RecipeSweetRib.Id);

            //Assert
            Assert.IsNull(found);
        }

        [TestMethod()]
        public void EditRecipeTest()
        {
            //Arrange
            var expected = "modified introduction";
            dbContext.Add(RecipeSweetRib);
            dbContext.SaveChanges();

            //Act
            var modifiedRecipe = RecipeSweetRib;
            modifiedRecipe.Introduction = expected;
            _recipeService.EditRecipe(modifiedRecipe);

            var found = dbContext.Recipes.Find(RecipeSweetRib.Id);

            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(expected, found.Introduction); ;
        }
    }
}