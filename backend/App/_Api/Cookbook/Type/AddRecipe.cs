using System;

namespace FoodDiaryApi.App._Api.Cookbook.Type
{
    public sealed class AddRecipeRequest
    {
        public string Url { get; init; }
        public string Alias { get; init; }
    }

    public sealed class AddRecipeResponse
    {
        public Guid Reference { get; init; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteName { get; set; }
        public string Description { get; set; }
        public DateTime AddedAt { get; init; }
        public string Alias{ get; init; }
    }
}