using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otlob.APIs.Errors;
using Otlob.APIs.Extensions;
using Otlob.APIs.Helpers;
using Otlob.APIs.Middlewares;
using Otlob.Core.Entites;
using Otlob.Core.Entites.Identity;
using Otlob.Core.Repositories;
using Otlob.Repository;
using Otlob.Repository.Data;
using Otlob.Repository.Identity;
using StackExchange.Redis;

namespace Otlob.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Configure Service

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            } );

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options => 
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });



            builder.Services.AddDbContext<AppIdentityDbContext>(Options => 
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

            });

            builder.Services.AddApplicationServices();

          
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(Options => 
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });


            #endregion

            var app = builder.Build();



            #region Update DataBase

            //StoreContext dbContext = new StoreContext(); //invaild
            //await DbContext.Database.MigrateAsync();
            using var Scope = app.Services.CreateScope();

            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {

                var dbContext = Services.GetRequiredService<StoreContext>();

                await dbContext.Database.MigrateAsync();//Update-Database

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();

                await IdentityDbContext.Database.MigrateAsync();


                var UserManger = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManger);
                await StoreContextSeed.SeedAsync(dbContext);


            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error Occured during Updating Database (Applaying The Migration)");
            }







            #endregion




            #region Configure the HTTP request pipeline.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            #endregion
            app.Run();
        }

        private static void SelectMany()
        {
            throw new NotImplementedException();
        }
    }
}