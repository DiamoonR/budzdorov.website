using health.Data;
using health.Options;
using health.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plk.Blazor.DragDrop;
using Sitko.Blazor.CKEditor;
using Sitko.Blazor.CKEditor.Bundle;
using System.Net.Http;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace health
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SiteContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<Options.UserOptions>(Configuration.GetSection("Authentication:Site"));
            services.Configure<ConnectionStringOptions>(Configuration.GetSection("ConnectionStrings"));            

            services.AddAuthentication("Cookie")
                .AddCookie("Cookie", config =>
                 {
                     config.LoginPath = "/Login";
                     config.AccessDeniedPath = "/Login";
                 });
            
            services.AddHttpClient();
            services.AddScoped<HttpClient>();
            
            services.AddHeadElementHelper();
            services.AddBlazorDragDrop();
            services.AddCKEditor(Configuration, options =>
            {
                //options.ScriptPath = "/js/lib/ckeditor.js";
                options.EditorClassName = "ClassicEditor";                
            });
            services.AddCKEditorBundle(Configuration);
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();

            services.AddAuthorization();
            services.AddRazorPages(options=> 
                {
                    options.Conventions.AuthorizeAreaFolder("Admin", "/admin");
                }
            );
            services.AddServerSideBlazor();            

            services.AddSingleton<ConnectionStringOptions>();
            services.AddSingleton<IMyConfig, MyConfigRepository>();
        }        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHeadElementServerPrerendering();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
