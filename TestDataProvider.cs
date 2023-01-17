using NewFoodie.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace FoodieTests
{
    public class TestDataProvider
    {
        public static IEnumerable<RecipeItem> GetRecipeItemsForFirstSpicyFish()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() {Id = 1, Name = "Fish", Amount = "100g", RecipeId = 1},
                new RecipeItem() {Id = 2, Name = "Spicy", Amount = "200g", RecipeId = 1}
            };
        }

        public static IEnumerable<RecipeItem> GetecipeItemsForSpicyPork()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() { Id = 3, Name = "Pork", Amount = "100g", RecipeId = 2},
                new RecipeItem() { Id = 4, Name = "Spicy", Amount = "200g", RecipeId = 2}
            };
        }

        public static IEnumerable<RecipeItem> GetRecipeItemsForSecondSpicyFish()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() {Id = 7, Name = "Fish", Amount = "100g", RecipeId = 3},
                new RecipeItem() {Id = 8, Name = "Spicy", Amount = "200g", RecipeId = 3}
            };
        }

        public static IEnumerable<RecipeItem> GetRecipeItemsForSweetRibs()
        {
            return new List<RecipeItem>()
            {
                new RecipeItem() {Id = 5, Name = "Ribs", Amount = "1000g", RecipeId = 4},
                new RecipeItem() {Id = 6, Name = "Suger", Amount = "2000g", RecipeId = 4}
            };
        }        
    }
}
