using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using NewFoodie.Models;
using Moq;
using NewFoodie.Services.EFServices;
using FoodieTests;
using Microsoft.EntityFrameworkCore;
using Sentry.Protocol;

namespace NewFoodie.Areas.Identity.Pages.Account.Manage.Tests
{
    [TestClass()]
    public class RecipeServiceTest
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("rececipMemoeryDb").Options;
        private AppDbContext _dbContext;
        private EFRecipeService _recipeService;

        private List<Recipe> _originalAllRecipes;

        private Recipe _recipeSweetRib;

        public RecipeServiceTest()
        {
        }

        //Prepare data
        [TestInitialize()]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            _recipeService = new EFRecipeService(_dbContext);

            _originalAllRecipes = new List<Recipe>()
            {
                new Recipe() { Id = 1, Name = "Spicy Fish", RecipeItems = TestDataProvider.GetRecipeItemsForFirstSpicyFish().ToList() },
                new Recipe() { Id = 2, Name = "Spicy Pork", RecipeItems = TestDataProvider.GetecipeItemsForSpicyPork().ToList() },
                new Recipe() { Id = 3, Name = "Spicy Fish", RecipeItems = TestDataProvider.GetRecipeItemsForSecondSpicyFish().ToList() },
            };

            _recipeSweetRib = new Recipe()
            {
                Id = 4,
                Name = "Sweet Ribs",
                RecipeItems = TestDataProvider.GetRecipeItemsForSweetRibs()
            };
        }

        [TestCleanup]
        public void CleanUp() { 
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod()]
        public void AddRecipeTest()
        {
            //Arrange
            var expected = _recipeSweetRib;

            //Act
            _recipeService.AddRecipe(expected);

            var found = _dbContext.Recipes.Find(expected.Id);

            //Assert
            Assert.AreEqual(found.Name, expected.Name);
            Assert.AreEqual(found.RecipeItems, expected.RecipeItems);
        }

        [TestMethod()]
        public void GetAllRecipesTest()
        {
            // Arrange
            _dbContext.Recipes.AddRange(_originalAllRecipes);
            _dbContext.SaveChanges();

            //Act
            var found = _recipeService.GetAllRecipes();
            var countOfResults = found.Count();

            //Assert
            Assert.IsTrue(countOfResults == _originalAllRecipes.Count); ;
        }


        [TestMethod()]
        public void DeleteRecipeTest()
        {
            //Arrange
            _dbContext.Add(_recipeSweetRib);
            _dbContext.SaveChanges();

            //Act
            _recipeService.DeleteRecipe(_recipeSweetRib);
            var found = _dbContext.Recipes.Find(_recipeSweetRib.Id);

            //Assert
            Assert.IsNull(found);
        }

        [TestMethod()]
        public void EditRecipeTest()
        {
            //Arrange
            var expected = "modified introduction";
            _dbContext.Add(_recipeSweetRib);
            _dbContext.SaveChanges();

            //Act
            var modifiedRecipe = _recipeSweetRib;
            modifiedRecipe.Introduction = expected;
            _recipeService.EditRecipe(modifiedRecipe);

            var found = _dbContext.Recipes.Find(_recipeSweetRib.Id);

            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(expected, found.Introduction); ;
        }

        [TestMethod()]
        public void GetRecipeByIdTest()
        {
            //Arrange
            _dbContext.Recipes.AddRange(_originalAllRecipes);
            _dbContext.SaveChanges();

            var recipeId = 1;
            var expectedRecipeName = "Spicy Fish";

            //Act
            var found = _recipeService.GetRecipeById(recipeId);

            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(expectedRecipeName, found.Name); ;
        }

        [TestMethod()]
        public void GetRecipeByNameTest()
        {
            //Arrange
            _dbContext.Recipes.AddRange(_originalAllRecipes);
            _dbContext.SaveChanges();

            var recipeName = "Spicy Fish";
            var expectedRecipeNumber = 2;

            //Act
            var found = _recipeService.GetRecipesByRecipeName(recipeName).ToList();


            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(expectedRecipeNumber, found.Count()); 
        }

    }
}