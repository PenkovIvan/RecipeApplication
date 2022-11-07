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

        // ���� ����� ���������� ������ ����������. ����������� ���� ����� ��� ���������� ����� � ���������.
        public void ConfigureServices(IServiceCollection services)
        {
            //var connString = Configuration.GetConnectionString("Default Connection");//������ ����������� ������� �� ������������, �� ������� ConnectionStrings.
            //services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connString)); //������������ DbContext,��������� ��� � �������� ����������� ���������
            //��������� ���������� ���� ������ � ���������� ��������� ��� DbContext
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//-����� �������� ������ 2 �������
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)//��������� ������� Identity � ��������� ��������� ������������ � ���������� ����������� ��� ������������, ApplicationUser
                .AddEntityFrameworkStores<MyDbContext>();//���������, ��� �� ����������� ��� ������������� DbContext
                                                         // requires
                                                         // using Microsoft.AspNetCore.Identity.UI.Services;
                                                         // using WebPWrecover.Services;                                           
                                                         //services.AddTransient<IEmailSender, EmailSender>(); //�������� EmailSender � �������� ��������� ������.
                                                         //services.Configure<AuthMessageSenderOptions>(Configuration);//��������������� AuthMessageSenderOptions ��������� ������������.



            services.AddRazorPages().AddRazorRuntimeCompilation();//����� ��� ��������� ���� ������� � �������� �������, ����� ���������� �������, ����� ���������� NuGet packege

            services.AddScoped<RecipeService>();//��������� ��������
            services.AddScoped<IAuthorizationHandler, IsRecipeOwnerHandler>();//�������� ���������� IsRecipeOwnerHandler � ��������� ��������� ������������.

            services.AddAuthentication().AddGoogle(googleOptions =>//���������� �������������� google
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            //���������� �������� �����������
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanManageRecipe", policyBuilder => policyBuilder.AddRequirements(new IsRecipeOwnerRequirement()));
            });
        }

        // ���� ����� ���������� ������ ����������. ����������� ���� ����� ��� ��������� ��������� HTTP-��������.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //�������� HSTS �� ��������� ����� 30 ����. ��������, �� �������� �������� ��� ��� ���������������� ���������, ��. https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//��������� AuthenticationMiddleware ����� ������ UseRouting() � �� ������ UseAuthorization.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
