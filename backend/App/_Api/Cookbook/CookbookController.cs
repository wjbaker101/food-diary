using FoodDiaryApi.App._Api.Cookbook.Type;
using Microsoft.AspNetCore.Mvc;
using System;
using WJBCommon.Lib.Api.Controller;

namespace FoodDiaryApi.App._Api.Cookbook
{
    [Route("api/cookbook")]
    public sealed class CookbookController : ApiController
    {
        private readonly ICookbookService _cookbookService;

        public CookbookController(ICookbookService cookbookService)
        {
            _cookbookService = cookbookService;
        }

        [HttpGet]
        [Route("recipes")]
        public IActionResult GetRecipes()
        {
            return ApiResult(_cookbookService.GetRecipes());
        }

        [HttpPost]
        [Route("recipe")]
        public IActionResult AddRecipe([FromBody] AddRecipeRequest request)
        {
            return ApiCreatedResult(_cookbookService.AddRecipe(request));
        }

        [HttpDelete]
        [Route("recipe/{reference:guid}")]
        public IActionResult RemoveRecipe([FromRoute] Guid reference)
        {
            _cookbookService.RemoveRecipe(reference);

            return ApiNoContentResult();
        }
    }
}