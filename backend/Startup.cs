using FoodDiaryApi.App._Api.Calendar;
using FoodDiaryApi.App._Api.Calendar.Entry;
using FoodDiaryApi.App._Api.Calendar.Photo;
using FoodDiaryApi.App._Api.Cookbook;
using FoodDiaryApi.App.Client.Imgur;
using FoodDiaryApi.App.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WJBCommon.Lib.Api.Exception.Handler;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi
{
    public sealed class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IApiDatabase, ApiDatabase>();

            services.AddSingleton<IImgurClient, ImgurClient>();

            services.AddSingleton<IUnitOfWorkFactory<ICalendarUnitOfWork>, UnitOfWorkFactory<CalendarUnitOfWork>>();
            services.AddSingleton<ICalendarEntryService, CalendarEntryService>();
            services.AddSingleton<ICalendarPhotoService, CalendarPhotoService>();

            services.AddSingleton<IUnitOfWorkFactory<ICookbookUnitOfWork>, UnitOfWorkFactory<CookbookUnitOfWork>>();
            services.AddSingleton<ICookbookService, CookbookService>();

            services.AddControllers();
            services.AddHttpClient();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}