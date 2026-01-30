
using Microsoft.EntityFrameworkCore;
using React_dotnet.database;
using React_dotnet.Server.Middlewares;

namespace React_dotnet.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthenticationServices(builder.Configuration);
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            builder.Services.AddDbContext<CoreDbContext>(options => 
                options.UseSqlite(builder.Configuration.GetConnectionString(nameof(CoreDbContext)))
                
            );

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            using var coredbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
            coredbContext.Database.Migrate();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
               .AllowCredentials()
               .WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseMiddleware<AuthorizationHeaderSetterMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
