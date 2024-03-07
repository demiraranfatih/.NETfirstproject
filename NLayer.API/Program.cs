using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

namespace NLayer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //validationsu da ekleyece�im
            //filters dosyas�n� da ekledim add controllers i�erisinde
            builder.Services.AddControllers(options => { options.Filters.Add(new ValidateFilterAttribute()); }).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            //api taraf�nda olan validationu iptal edip kendi validationumu aktif ediyorum.
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //e�er ki unitofworkle kar��la��rsam Iunitofworkten miras al�nm��t�r

            //yazm�� oldu�um notfound filter'� ekliyorum
            builder.Services.AddScoped(typeof(NotFoundFilter<>));

            //generic repository'de ekliyorum.

            //servis klas� �u anda olu�turulmad�.

            //mapper eklemesini yap�yorum
            builder.Services.AddAutoMapper(typeof(MapProfile));


            builder.Services.AddMemoryCache();

            builder.Services.AddDbContext<AppDbContext>(x =>
            {

                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    //    option.MigrationsAssembly("Nlayer.Repository");  bu �ekildede olur ama a�a��daki daha iyi
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);//appdbcontexin typenin asemblysini al
                });

            });
            builder.Host.UseServiceProviderFactory
                (new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //yazm�� oldu�um middleware eklemesini yap�yorum.
            app.UseCustomException();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
