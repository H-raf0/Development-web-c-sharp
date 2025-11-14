
namespace GameServerApi;

using Scalar.AspNetCore;
using GameServerApi.Models;
using Microsoft.AspNetCore.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<UserContext>();
        builder.Services.AddScoped<PasswordHasher<User>>();

        builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        /*
        builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins("https://csharp.nouvet.fr/front3"));
            });
        */

        var app = builder.Build();

        app.UseCors("AllowAll");
        // app.UseCors("AllowSpecificOrigin");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
