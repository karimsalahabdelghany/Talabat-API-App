using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.APIS.Helper;
using Talabat.APIS.MiddleWares;
using Talabat.Core.Models;
using Talabat.Core.Models.Identity;
using Talabat.Core.Repositiries;
using Talabat.Repositiory;
using Talabat.Repositiory.Data;
using Talabat.Repositiory.Identity;

namespace Talabat.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            #region Configure Services Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowAnyOrigin();
                });
            });
             
            
            #endregion

            var app = builder.Build();
            #region Update -Database
            //StoreContext dbcontext = new StoreContext (); //invalid
            //await dbcontext.Database.MigrateAsync();
            using var Scope = app.Services.CreateScope();
            //group of services that it's lifetime scoped
            var Services = Scope.ServiceProvider;
            // services it self
            var loggerfactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = Services.GetRequiredService<StoreContext>();
                //ASk Clr for creating object from StoreContext Explicitly
                await DbContext.Database.MigrateAsync(); //update-database
                // Scope.Dispose();
                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();
                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
               await StoreContextSeed.SeedAsync(DbContext);
            }
            catch(Exception ex) 
            {
                var logger =loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occured During Appling The Migration");
            }

            #endregion

            #region Configure-Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwaggerMiddelwares();
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
    }
}
