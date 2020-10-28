using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Harjoitus9_WebAPIHttpClientReact
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();

            services.AddHttpClient("sami", options =>
            {
                options.BaseAddress = new System.Uri("https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/measurements/");

            });

            //26.10.2020 Oliskohan t‰m‰ mennyt tuolla samalla clientilla...
            services.AddHttpClient("samisensors", options =>
            {
                options.BaseAddress = new System.Uri("https://sami.savonia.fi/Service/3.0/MeasurementsService.svc/json/sensors/");

            });


            // lis‰t‰‰n CORS politiikka
            services.AddCors(options => 
            { 
                options.AddPolicy("omacorspolitiikka", builder => 
                        {
                            builder.AllowAnyOrigin();//*url
                            builder.AllowAnyMethod();//*header
                            builder.AllowAnyHeader();//* method, get, put, post, delete jne
                        });
            });
            


            // In production, the React files will be served from this directory
            //T‰m‰nkin vois kommentoida, jos haluaa startata vain server-puolen
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build"; //T‰m‰ tulee, kunhan bildataan
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("omacorspolitiikka"); //CORS politiikka, olikohan merkityst‰ mihin kohti t‰m‰n laittaa, ennen vai j‰lkeen:

            app.UseHttpsRedirection();
            app.UseStaticFiles();                                          
            app.UseSpaStaticFiles();         //t‰m‰kin kommenttiin jos vain server puoli
            app.UseRouting();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start"); //Developer moodissa t‰m‰ k‰ynnist‰‰ Clientin, kpts package.json Jos haluat k‰ynnist‰‰ vain serveripuolen, kommentoi t‰m‰
                }
            });
        }
    }
}
