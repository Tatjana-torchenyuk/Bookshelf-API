using Lib.DbContexts;
using Lib.Services;
using Microsoft.EntityFrameworkCore;


namespace BooksMVC
{

    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBooksRepository, InMemoryBooksRepository>();
            //services.AddScoped<IBooksRepository, EfBooksRepository>();
            services.AddControllers();

            var connection = _configuration.GetConnectionString("BooksDatabase");
            services.AddDbContext<BooksDbContext>(x => x.UseMySql(
                connection, 
                ServerVersion.AutoDetect(connection),
                mySqlOptions => mySqlOptions.MigrationsAssembly("BooksMVC")));

            // If you don't use viewModels -> Configure JSON serialization to ignore reference loops
            /*services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);*/

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("./swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            } else {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = context => context.Response.WriteAsync("OOPS!!!")
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
