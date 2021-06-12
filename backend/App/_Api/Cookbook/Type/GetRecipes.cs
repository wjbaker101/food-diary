using System;
using System.Collections.Generic;

namespace FoodDiaryApi.App._Api.Cookbook.Type
{
    public sealed class GetRecipesResponse
    {
        public List<Recipe> Recipes { get; init; }

        public sealed class Recipe
        {
            public Guid Reference { get; init; }
            public string Title { get; init; }
            public string Url { get; init; }
            public string ImageUrl { get; init; }
            public string WebsiteName { get; init; }
            public string Description { get; init; }
            public DateTime AddedAt { get; init; }
            public string Alias { get; init; }
        }
    }
}