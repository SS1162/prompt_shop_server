using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Repositories;
using Services;
using WebApiShope.MiddleWare;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUsersReposetory, UsersReposetory>();

builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IPasswordsService,PasswordsService>();

builder.Services.AddScoped<IPlatformsReposetory, PlatformsReposetory>();

builder.Services.AddScoped<IPlatformsServise, PlatformsServise>();

builder.Services.AddScoped<IProductsReposetory, ProductsReposetory>();

builder.Services.AddScoped<Igemini, gemini>();

builder.Services.AddScoped<IGeminiPromptsReposetory, GeminiPromptsReposetory>();


builder.Services.AddScoped<IGeminiServise, GeminiServise>();


builder.Services.AddScoped<IRatingsReposetory, RatingsReposetory>();

builder.Services.AddScoped<IReviewsServise, ReviewsServise>();

builder.Services.AddScoped<ISiteTypesRepository, SiteTypesRepository>();

builder.Services.AddScoped<IRatingsServise, RatingsServise>();

builder.Services.AddScoped<ICartsReposetory, CartsReposetory>();

builder.Services.AddScoped<IOrdersServise, OrdersServise>();

builder.Services.AddScoped<IOrdersReposetory, OrdersReposetory>();

builder.Services.AddScoped<IStatusesReposetory, StatusesReposetory>();

builder.Services.AddScoped<IReviewsReposetory, ReviewsReposetory>();

builder.Services.AddScoped<IBasicSitesServise, BasicSitesServise>();

builder.Services.AddScoped<ISiteTypesService, SiteTypesService>();

builder.Services.AddScoped<IBasicSitesReposetory, BasicSitesReposetory>();

builder.Services.AddScoped<IMainCategoriesServise, MainCategoriesServise>();

builder.Services.AddScoped<IMainCategoriesReposetory, MainCategoriesReposetory>();

builder.Services.AddScoped<ICategoriesServise, CategoriesServise>();


builder.Services.AddScoped<ICategoriesReposetory, CategoriesReposetory>();

builder.Services.AddScoped<IProductsServise, ProductsServise>();

builder.Services.AddScoped<ICartItemServise, CartItemServise>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:5000") 
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddDbContext<MyShop330683525Context>(option => option.UseSqlServer
("Data Source=DESKTOP-R5RADSP;Initial Catalog=MyPromptShop;Integrated Security=True;Trust Server Certificate=True"));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Host.UseNLog();
var app = builder.Build();
//app.UseErrorMiddleware();
app.UseRatingMiddleware();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
       
    });
}


// Configure the HTTP request pipeline.
app.UseCors("AllowAngular");

app.UseHttpsRedirection();



app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
