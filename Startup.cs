using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; 
using CsvTopScorerApi.Data; 

public class Startup
{
    // Method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddDbContext<ScoreContext>(options =>
            options.UseSqlite("Data Source=scores.db")); 

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map attribute-routed controllers
        });
    }
}