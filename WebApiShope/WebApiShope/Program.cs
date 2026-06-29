using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using Repositories;
using Services;
using StackExchange.Redis;
using System.Text;
using WebApiShope.MiddleWare;
using static System.Runtime.InteropServices.JavaScript.JSType;


var builder = WebApplication.CreateBuilder(args);

// Use environment variables as the primary runtime configuration source
// (for Elastic Beanstalk and other cloud deployments).
// appsettings files are loaded first so environment variables can override them in production.
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
if (args.Length > 0)
{
    builder.Configuration.AddCommandLine(args);
}

builder.Services.AddScoped<IUsersReposetory, UsersReposetory>();

builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IPasswordsService,PasswordsService>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();

builder.Services.AddScoped<IPlatformsReposetory, PlatformsReposetory>();

builder.Services.AddScoped<IPlatformsServise, PlatformsServise>();

builder.Services.AddScoped<IProductsReposetory, ProductsReposetory>();

builder.Services.AddScoped<Igemini, gemini>();

builder.Services.AddScoped<IGeminiPromptsReposetory, GeminiPromptsReposetory>();

builder.Services.AddScoped<ICreatePrompt, CreatePrompt>();

builder.Services.AddHttpClient<ChatBotServise>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ChatBot:BaseUrl"] ?? "http://localhost:8001/");
});
builder.Services.AddScoped<IChatBotServise, ChatBotServise>();

builder.Services.AddScoped<IGeminiServise, GeminiServise>();

builder.Services.AddScoped<IGeminiSdkChatService, GeminiSdkChatService>();

builder.Services.AddHttpClient<PayPalService>();

builder.Services.AddScoped<IRatingsReposetory, RatingsReposetory>();

builder.Services.AddScoped<IReviewsServise, ReviewsServise>();

builder.Services.AddScoped<ISiteTypesRepository, SiteTypesRepository>();

builder.Services.AddScoped<IRatingsServise, RatingsServise>();

builder.Services.AddScoped<ICartsReposetory, CartsReposetory>();

builder.Services.AddScoped<IOrdersServise, OrdersServise>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

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

var categoryCacheOptions = builder.Configuration
    .GetSection(CategoryCacheOptions.SectionName)
    .Get<CategoryCacheOptions>() ?? new CategoryCacheOptions();
builder.Services.AddSingleton(categoryCacheOptions);

builder.Services.AddScoped<IProductsServise, ProductsServise>();

builder.Services.AddScoped<ICartItemServise, CartItemServise>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ── Redis & Rate Limiting ────────────────────────────────────────────────────
// IConnectionMultiplexer is thread-safe and designed to be a long-lived singleton.
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(
        builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379"));

builder.Services.AddSingleton<IRedisRateLimitService, RedisRateLimitService>();

builder.Services.Configure<RateLimitOptions>(
    builder.Configuration.GetSection(RateLimitOptions.SectionName));
// ─────────────────────────────────────────────────────────────────────────────

// ── JWT ─────────────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IJwtService, JwtService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtCfg = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtCfg["Issuer"],
            ValidAudience            = jwtCfg["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtCfg["SecretKey"]!))
        };
    });
// ─────────────────────────────────────────────────────────────────────────────

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("https://prompt-shop-client-testpnoren.s3-website-us-east-1.amazonaws.com/", "http://prompt-shop-client-testpnoren.s3-website-us-east-1.amazonaws.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<MyShop330683525Context>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Host.UseNLog();
var app = builder.Build();
app.UseErrorMiddleware();
//app.UseRateLimiting();   // ← fixed-window rate limiter (early — before routing & auth)
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

app.UseCookieToken();   // reads JWT from access_token cookie → Authorization header
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
