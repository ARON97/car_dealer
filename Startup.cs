using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cars.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using cars.Core;
using cars.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace cars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ConfigureServices is used for dependency injection. IServiceCollection
        // container for all the dependencies in our application
        public void ConfigureServices(IServiceCollection services)
        {
            // PhotoSettings registration
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            // ICarRepository and it's implementation as a service for dependency injection
            services.AddScoped<ICarRepository, CarRepository>();
            // IPhotoRepository and it's implementation as a service for dependency injection
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            // IUnitOfWork and it's implementation as a service for dependency injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add depenedency injection for AutoMapper
            services.AddAutoMapper();
            // For example a dependency can be registered as
            // services.AddTransient<IRepository, Repository>();
            // This is a generic method with two generic parameters, the first one is an Interface
            // the second one is an implementation. We registering Repository as an implementation of IRepository interface
            
            // configure the dbContext as a service dependency injection. Using LAMBDA expression/Action that determines
            // what persistent store is going to be used like SQL server, SQLite, etc and what the connection string is
            services.AddDbContext<CarsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            // Add MVC framework services
            services.AddMvc();

             // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = "https://cars1.eu.auth0.com/";
                options.Audience = "https://api.car.com";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // This is a middleware. renders an exception with all its details
                app.UseDeveloperExceptionPage();
                // This is a middleware. Anytime client side files are changed. Webpack will automatically comiple the changes and push the changes into the browser
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    // Not reloading the pages after changes in clientside are made
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Another Middleware. We able be able to serve static files like imgaes, css, etc
            // Middleware that every ASP.NET applications need
            app.UseStaticFiles();

            // 2. Enable authentication middleware
            app.UseAuthentication();

            // Another Middleware. This middleware looks at the request and based on our routes it
            // will forward it to an action in a controller
            app.UseMvc(routes =>
            {
                // defines our route template
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
