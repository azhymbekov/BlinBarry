using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Data.Repositories;
using BlinBerry.Services.Common.RecipeService;
using BlinBerry.Services.Common.RecipeService.Models;
using BlinBerry.Services.Common.SpendingService.Models;

namespace BlinBerry.Service.RecipeServise
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> recipeRepository;

        private readonly IMapper mapper;

        public RecipeService(IRepository<Recipe> recipeRepository, IMapper mapper)
        {
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
        }
        public IQueryable GetRecipes()
        {
            return mapper.ProjectTo<RecipeDto>(recipeRepository.AllAsNoTracking().OrderByDescending(x => x.Name));
        }

        public Task SaveAsync(RecipeDto model)
        {
            throw new System.NotImplementedException();
        }
    }
}
