using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Connection String
builder.Services.AddDbContext<StoreContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DB-SqlLite"))
);

// Implement Dependancy Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add Generic Repository 15 10 2024
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add Auto Mapper 15 12 2024
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Cors 9 1 2025
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolice", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        //policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Mange to Access Files 15 12 2024
app.UseStaticFiles();
app.UseAuthorization();
app.UseCors("CorsPolice");
app.MapControllers();

// Run unupdated DataBase Migrations before App Run

// use of ( using var scope ) to make it Disposable

using var scope = app.Services.CreateScope();
var Services = scope.ServiceProvider;
var context = Services.GetRequiredService<StoreContext>();
var logger = Services.GetRequiredService<ILogger<Program>>();

try
{
    // Push Migrations To DataBase
    await context.Database.MigrateAsync();
    // Insert Static Data in DataBase
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "Error happen when migrating process");
}

app.Run();
