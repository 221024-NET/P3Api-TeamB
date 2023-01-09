using ECommerce.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration["ConnectionStrings:ecommDB"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://ecommerceappteamb.github.io",
                                "https://ecommerceappteamb.github.io/P3UI-TeamB",
                                "https://webapp-tentacle-shop.azurewebsites.net")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(opt => opt.UseSqlServer(connString));
builder.Services.AddScoped<IContext>(provider => provider.GetService<Context>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
   {
       options.SwaggerEndpoint("/swagger/v1/swagger.json", "EComm-API");
   });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
