using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.Web.Modules;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using System.Reflection;
using NLayer.Service.Mapping;
using FluentValidation.AspNetCore;
using NLayer.Service.Validations;
using NLayer.Web.Services;

namespace NLayer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); ;

            builder.Services.AddAutoMapper(typeof(MapProfile));

            builder.Services.AddDbContext<AppDbContext>(x =>
            {

                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    //    option.MigrationsAssembly("Nlayer.Repository");  bu þekildede olur ama aþaðýdaki daha iyi
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);//appdbcontexin typenin asemblysini al
                });

            });
            builder.Services.AddScoped(typeof(NotFoundFilter<>));


            builder.Services.AddHttpClient<ProductApiService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            });

            builder.Services.AddHttpClient<CategoryApiService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            });



            builder.Host.UseServiceProviderFactory
                (new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

            var app = builder.Build();
            // Configure the HTTP request pipeline.

            app.UseExceptionHandler("/Home/Error");
            if (!app.Environment.IsDevelopment())
            {
               

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}