using Core.Interfaces;
using E_commerce_Api.HubConfig;
using E_commerce_Api.Identity;
using E_commerce_Api.Interfaces;
using E_commerce_Api.middleware;
using E_commerce_Api.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
////builder.Services.AddSwaggerGenAuth();
// test signalR 6 2 2025
builder.Services.AddSignalR(opt => opt.EnableDetailedErrors = true);
// Add Connection String
builder.Services.AddDbContext<StoreContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DB-SqlLite"))
);

builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Identity-DB-SqlLite"))
);
//builder.Services.AddIdentityCore<AppUser>(Options =>
//{
//    Options.Lockout.MaxFailedAccessAttempts = 2;
//    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
//    Options.User.RequireUniqueEmail = true;
//    Options.SignIn.RequireConfirmedPhoneNumber = false;
//    Options.SignIn.RequireConfirmedEmail = false;
//    Options.SignIn.RequireConfirmedAccount = false;
//    Options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
//})
//                .AddEntityFrameworkStores<AppIdentityDbContext>()
//                .AddDefaultTokenProviders();
var jwt = builder.Configuration.GetSection("Token");

builder.Services.AddAuthentication(Option =>
{
    Option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(Options =>
{
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),

    };

});
// add Identity Service 11 2 2025
//builder.Services.AddIdentityCore<AppUser>(opt =>
//{

//}).AddEntityFrameworkStores<AppIdentityDbContext>()
//  .AddSignInManager<SignInManager<AppUser>>();


//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(opt =>
//    {
//        opt.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    });

//In case you are working with cookies
//builder.Services.AddAuthentication()

//    .AddCookie(IdentityConstants.ApplicationScheme)
//    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<AppIdentityDbContext>().AddApiEndpoints();

builder.Services.AddAuthorization();


// Implement Dependancy Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add Generic Repository 15 10 2024
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add Auto Mapper 15 12 2024
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Token Service 11 2 2025
builder.Services.AddScoped<ITokenService, TokenService>();

// Add Cors 9 1 2025
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolice", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        ///policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});
// add Swagger Config 11 2 2025
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-Commerce",
        Version = "v1"
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyToken\"",
    });
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                    new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    }
                },
                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolice");
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
// Mange to Access Files 15 12 2024
app.UseStaticFiles();
app.UseAuthentication();
// 11 2 2025 to enable Token
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGroup("/api/user").MapIdentityApi<AppUser>();
    endpoints.MapHub<MyHub>("/SignalR");
});

//app.MapControllers();
//app.MapIdentityApi<AppUser>();
// Create SignalR Hub Route
//app.MapHub<MyHub>("/SignalR");
// Run unupdated DataBase Migrations before App Run

// use of ( using var scope ) to make it Disposable

using var scope = app.Services.CreateScope();
var Services = scope.ServiceProvider;
var context = Services.GetRequiredService<StoreContext>();
var identityContext = Services.GetRequiredService<AppIdentityDbContext>();
var userManager = Services.GetRequiredService<UserManager<AppUser>>();
var logger = Services.GetRequiredService<ILogger<Program>>();

try
{
    // Push Migrations To DataBase
    await context.Database.MigrateAsync();

    // Push Identity Migrations To DataBase 11 2 2025
    await identityContext.Database.MigrateAsync();

    // Insert Static Data in DataBase
    await StoreContextSeed.SeedAsync(context);

    // Insert Identity Data 11 2 2025
    await SeedIdentityData.SeedUserAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "Error happen when migrating process");
}

await app.RunAsync();
