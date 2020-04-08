using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlinBerry.Services.Common.RecipeService;
using BlinBerry.Services.Common.RecipeService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlinBerry.Controllers
{
    public class RecipeController : Controller
    {
        public readonly IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllRecipes()
        {
            var recipe = recipeService.GetRecipes();
            return View(recipe);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeData(double eggs, double kefir, double soda, double sugar, double vanila, double oil, double salt, string name)
        {
            var model = new RecipeDto
            {
                Name = name,
                Salt = salt,
                Sugar = sugar,
                Soda = soda,
                Oil = oil,
                Vanila = vanila,
                Kefir = kefir,
                Eggs = eggs
            };
            await recipeService.SaveAsync(model);
            return RedirectToAction("GetAllRecipes", "Recipe");
        }
    }
}