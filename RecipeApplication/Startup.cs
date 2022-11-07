using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeApplication.Data;
using Microsoft.EntityFrameworkCore;
using RecipeApplication.Models;
using RecipeApplication.Pages;
using Microsoft.AspNetCore.Authorization;
using RecipeApplication.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

namespace RecipeApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Этот метод вызывается средой выполнения. Используйте этот метод для добавления служб в контейнер.
        public void ConfigureServices(IServiceCollection services)
        {
            //var connString = Configuration.GetConnectionString("Default Connection");//Строка подключения берется из конфигурации, из раздела ConnectionStrings.
            //services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connString)); //Регистрируем DbContext,используя его в качестве обобщенного параметра
            //Указываем провайдера базы данных в параметрах настройки для DbContext
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//-можно заменить первые 2 строчки
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)//Добавляет сервисы Identity в контейнер внедрения зависимостей и использует специальный тип пользователя, ApplicationUser
                .AddEntityFrameworkStores<MyDbContext>();//Убедитесь, что вы используете имя существующего DbContext
                                                         // requires
                                                         // using Microsoft.AspNetCore.Identity.UI.Services;
                                                         // using WebPWrecover.Services;                                           
                                                         //services.AddTransient<IEmailSender, EmailSender>(); //Добавьте EmailSender в качестве временной службы.
                                                         //services.Configure<AuthMessageSenderOptions>(Configuration);//Зарегистрируйте AuthMessageSenderOptions экземпляр конфигурации.



            services.AddRazorPages().AddRazorRuntimeCompilation();//метод для изменения кода страниц в реальном времени, перед включением сервиса, нужно установить NuGet packege

            services.AddScoped<RecipeService>();//внедрение сервисов
            services.AddScoped<IAuthorizationHandler, IsRecipeOwnerHandler>();//добавить обработчик IsRecipeOwnerHandler в контейнер внедрения зависимостей.

            services.AddAuthentication().AddGoogle(googleOptions =>//добавление аутентификации google
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            //добавление политики авторизации
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanManageRecipe", policyBuilder => policyBuilder.AddRequirements(new IsRecipeOwnerRequirement()));
            });
        }

        // Этот метод вызывается средой выполнения. Используйте этот метод для настройки конвейера HTTP-запросов.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //Значение HSTS по умолчанию равно 30 дням. Возможно, вы захотите изменить это для производственных сценариев, см. https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//Добавляем AuthenticationMiddleware после метода UseRouting() и до метода UseAuthorization.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
