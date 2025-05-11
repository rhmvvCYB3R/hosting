using CafeApp.Api.Dtos.Mappers;
using CafeApp.BusinessLogic.Mappers;
using CafeApp.BusinessLogic.Services;
using CafeApp.BusinessLogic.Services.Interfaces;
using CafeApp.Data;
using CafeApp.Data.Repositories;
using CafeApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CafeApp.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add DbContext
        builder.Services.AddDbContext<CafeAppDbContext>(x =>
        {
            x.UseNpgsql(builder.Configuration.GetConnectionString("CafeAppDb"));
        });

        // AutoMapper Profiles
        builder.Services.AddAutoMapper(typeof(DishDtoMapperProfile));
        builder.Services.AddAutoMapper(typeof(CafeDtoMapperProfile));
        builder.Services.AddAutoMapper(typeof(CafeToEntityMapperProfile));
        builder.Services.AddAutoMapper(typeof(DishToEntityMapperProfile));
        builder.Services.AddAutoMapper(typeof(TableToEntityMapperProfile));

        // Repositories
        builder.Services.AddScoped<ICafeRepository, CafeRepository>();
        builder.Services.AddScoped<IDishRepository, DishRepository>();
        builder.Services.AddScoped<ITableRepository, TableRepository>();

        // Services
        builder.Services.AddScoped<ICafeService, CafeService>();
        builder.Services.AddScoped<IDishService, DishService>();

        // CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost5500", policy =>
            {
                policy.WithOrigins("http://localhost:5500")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Apply CORS
        app.UseCors("AllowLocalhost5500");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
