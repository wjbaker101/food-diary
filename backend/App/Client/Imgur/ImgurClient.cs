using FoodDiaryApi.App.Client.Imgur.Type;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WJBCommon.Lib.Api.Exception;

namespace FoodDiaryApi.App.Client.Imgur
{
    public interface IImgurClient
    {
        string UploadImage(string imageAsBase64);
    }

    public sealed class ImgurClient : IImgurClient
    {
        private readonly HttpClient _imgurApi;

        public ImgurClient(HttpClient imgurApi)
        {
            imgurApi.BaseAddress = new Uri("https://api.imgur.com/3/");

            imgurApi.DefaultRequestHeaders.Add("Authorization", "Client-ID a4d1f91c473bae6");

            _imgurApi = imgurApi;
        }

        public string UploadImage(string imageAsBase64)
        {
            var image = Base64Only(imageAsBase64);
            
            var formData = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new("type", "base64"),
                new("image", image)
            });

            var response = _imgurApi.PostAsync("upload", formData).Result;

            if (!response.IsSuccessStatusCode)
                throw new ApiThirdPartyException();

            var result = response.Content.ReadFromJsonAsync<ImgurResponse<UploadImageResponse>>().Result;

            if (result == null)
                throw new ApiThirdPartyException();

            return result.Data.Link;
        }

        private string Base64Only(string base64)
        {
            if (!base64.StartsWith("data:"))
                return base64;

            const string divider = "base64,";
            var actualStart = base64.IndexOf(divider, StringComparison.OrdinalIgnoreCase);

            return base64.Substring(actualStart + divider.Length);
        }
    }
}