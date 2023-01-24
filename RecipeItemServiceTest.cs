using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using NewFoodie.Models;
using NewFoodie.Services.EFServices;
using FoodieTests;
using Microsoft.EntityFrameworkCore;

namespace NewFoodie.Areas.Identity.Pages.Account.Manage.Tests
{
    [TestClass()]
    public class RecipeItemServiceTest
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("rececipMemoeryDb").Options;
        private AppDbContext _dbContext;
        private EFRecipeItemService _recipeItemService;

        private Recipe _recipeSweetRib;
        private RecipeItem _newRecipeItem;

        public RecipeItemServiceTest()
        {
        }

        //Prepare data
        [TestInitialize()]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            _recipeItemService = new EFRecipeItemService(_dbContext);

            _recipeSweetRib = new Recipe()
            {
                Id = 4,
                Name = "Sweet Ribs",
                RecipeItems = TestDataProvider.GetRecipeItemsForSweetRibs()
            };

            _newRecipeItem = new RecipeItem() { Id = 9, Name = "Soy Souce", Amount = "10g", RecipeId = 4 };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod()]
        public void AddRecipeItemTest()
        {
            //Arrange
            var expected = _newRecipeItem;
            expected.Recipe = _recipeSweetRib;

            //Act
            _recipeItemService.AddRecipeItem(expected);

            var found = _dbContext.RecipeItems.Find(expected.Id);

            //Assert
            Assert.AreEqual(found.Id, expected.Id);
            Assert.AreEqual(found.Name, expected.Name);
            Assert.AreEqual(found.Amount, expected.Amount);
        }

        [TestMethod()]
        public void DeleteRecipeItemTest()
        {
            //Arrange
            _dbContext.RecipeItems.Add(_newRecipeItem);
            _dbContext.SaveChanges();

            //Act
            _recipeItemService.DeleteRecipeItem(_newRecipeItem);
            var found = _dbContext.RecipeItems.Find(_newRecipeItem.Id);

            //Assert
            Assert.IsNull(found);
        }

        [TestMethod()]
        public void EditRecipeItemTest()
        {
            //Arrange
            var expectedAmout = "2000g";
            _dbContext.Recipes.Add(_recipeSweetRib);
            _dbContext.SaveChanges();

            //Act
            var modifiedRecipeItem = _recipeSweetRib.RecipeItems.FirstOrDefault(i=>i.Name == "Ribs");
            modifiedRecipeItem.Amount = expectedAmout;
            _recipeItemService.EditRecipeItem(modifiedRecipeItem);

            var found = _dbContext.RecipeItems.Find(modifiedRecipeItem.Id);

            //Assert
            Assert.IsNotNull(found);
            Assert.AreEqual(expectedAmout, found.Amount); ;
        }

        [TestMethod()]
        public void GetRecipeItemsByRecipeId()
        {
            //Arrange
            _dbContext.Recipes.Add(_recipeSweetRib);
            _dbContext.SaveChanges();

            var recipeId = 4;
            var expectedRecipeItems = _recipeSweetRib.RecipeItems;

            //Act
            var found = _recipeItemService.GetRecipeItemsByRecipeId(recipeId);

            //Assert
            Assert.IsNotNull(found);
            Assert.IsTrue(expectedRecipeItems.SequenceEqual(found));
        }
    }
}