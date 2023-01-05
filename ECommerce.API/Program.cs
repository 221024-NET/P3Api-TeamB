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
            policy.WithOrigins("https://ecommerceappteamb.github.io/P3UI-TeamB/")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});



//builder.Services.AddSingleton<IRepository>
//    (sp => new SQLRepository(connString, sp.GetRequiredService<ILogger<SQLRepository>>()));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Emtity framework chage -- Bryon
builder.Services.AddDbContext<Context>(opt => opt.UseSqlServer(connString));
builder.Services.AddScoped<IContext>(provider => provider.GetService<Context>());

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
   {
       options.SwaggerEndpoint("/swagger/v1/swagger.json", "EComm-API");
   });
}*/

app.UseCors();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
