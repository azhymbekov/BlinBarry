using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlinBerry.Services.Common.RecipeService.Models;

namespace BlinBerry.Services.Common.RecipeService
{
    public interface IRecipeService
    {
        IQueryable GetRecipes();

        Task SaveAsync(RecipeDto model);
    }
}
