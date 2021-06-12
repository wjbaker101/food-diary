namespace FoodDiaryApi.App.Client.Imgur.Type
{
    public sealed class ImgurResponse<TData> where TData : class
    {
        public TData Data { get; init; }
        public bool Success { get; init; }
        public int Status { get; init; }
    }
}