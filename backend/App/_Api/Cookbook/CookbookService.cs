using FoodDiaryApi.App._Api.Cookbook.Type;
using FoodDiaryApi.App.Database.Record;
using HtmlAgilityPack;
using System;
using WJBCommon.Lib.Api.Exception;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App._Api.Cookbook
{
    public interface ICookbookService
    {
        GetRecipesResponse GetRecipes();
        AddRecipeResponse AddRecipe(AddRecipeRequest request);
        void RemoveRecipe(Guid reference);
    }

    public sealed class CookbookService : ICookbookService
    {
        private const string GenericDodgyUrlMessage = "The given url was invalid. Please enter a new one and try again.";

        private readonly IUnitOfWorkFactory<ICookbookUnitOfWork> _unitOfWorkFactory;

        public CookbookService(IUnitOfWorkFactory<ICookbookUnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public GetRecipesResponse GetRecipes()
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var recipes = unitOfWork.Recipes.GetAll();

            return new GetRecipesResponse
            {
                Recipes = recipes.ConvertAll(x => new GetRecipesResponse.Recipe
                {
                    Reference = x.Reference,
                    Title = x.Title,
                    Url = x.Url,
                    ImageUrl = x.ImageUrl,
                    WebsiteName = x.WebsiteName,
                    Description = x.Description,
                    AddedAt = x.AddedAt,
                    Alias = x.Alias
                })
            };
        }

        public AddRecipeResponse AddRecipe(AddRecipeRequest request)
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var url))
                throw new ApiBadRequestException(GenericDodgyUrlMessage);

            var html = new HtmlWeb().Load(url);
            var metaTags = html.DocumentNode.SelectNodes("//meta");

            if (metaTags == null)
                throw new ApiBadRequestException(GenericDodgyUrlMessage);

            var recipe = new CookbookRecipeRecord
            {
                Reference = Guid.NewGuid(),
                AddedAt = DateTime.Now,
                Alias = string.IsNullOrEmpty(request.Alias) ? null : request.Alias
            };

            foreach (var metaTag in metaTags)
            {
                var property = metaTag.Attributes["property"] ?? metaTag.Attributes["name"];
                var content = metaTag.Attributes["content"];

                if (property == null || content == null)
                    continue;

                switch (property.Value.ToLower())
                {
                    case "og:title":
                        recipe.Title = content.Value;
                        break;
                    case "og:url":
                        recipe.Url = content.Value;
                        break;
                    case "og:image":
                        recipe.ImageUrl = content.Value;
                        break;
                    case "og:site_name":
                        recipe.WebsiteName = content.Value;
                        break;
                    case "og:description":
                        recipe.Description = content.Value;
                        break;
                }
            }

            using var unitOfWork = _unitOfWorkFactory.Create();

            unitOfWork.Recipes.Save(recipe);

            unitOfWork.Commit();

            return new AddRecipeResponse
            {
                Reference = recipe.Reference,
                Title = recipe.Title,
                Url = recipe.Url,
                ImageUrl = recipe.ImageUrl,
                WebsiteName = recipe.WebsiteName,
                Description = recipe.Description,
                AddedAt = recipe.AddedAt,
                Alias = recipe.Alias
            };
        }

        public void RemoveRecipe(Guid reference)
        {
            using var unitOfWork = _unitOfWorkFactory.Create();

            var recipe = unitOfWork.Recipes.GetByReference(reference);
            if (recipe == null)
                throw new ApiNotFoundException();

            unitOfWork.Recipes.Delete(recipe);

            unitOfWork.Commit();
        }
    }
}