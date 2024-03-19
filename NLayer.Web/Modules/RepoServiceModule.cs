namespace NLayer.Web.Modules
{
    using Module = Autofac.Module;
    using System.Reflection;
    using Autofac;
    using NLayer.Repository;
    using NLayer.Service.Mapping;
    using NLayer.Repository.Repositories;
    using NLayer.Core.Repositories;
    using NLayer.Service.Services;
    using NLayer.Core.Services;
    using NLayer.Repository.UnitOfWorks;
    using NLayer.Core.UnitOfWorks;


    public class RepoServiceModule :Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {

        builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
        builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();




            var apiAssembly = Assembly.GetExecutingAssembly(); // api yerini aldığı için zaten o assembly içerisindeyim.
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); // Repository katmanının olduğu yerin assemblysini alıyorum 
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); // Service katmanının olduğu yerin assemblysini alıyorum 

            //sonu repository ile bitenleri ya da service ile bitenleri ekledim buraya 
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x=> x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //buradan sonra yapacağım şey program cs e gerekli kısımları ekleyeceğim.
            //programcs de belirlemiş olduğum
            //genericleri yukarıda yapacağım eklemeyi unitofwork generic değil 


            //web için cache kaldırıyorum


            base.Load(builder);
        }
    }
}
