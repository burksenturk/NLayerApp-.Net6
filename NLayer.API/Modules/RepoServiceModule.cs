﻿using Autofac;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;  //modül autofac tin modülünü kullansın.

namespace NLayer.API.Modules
{
	public class RepoServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
			builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

			builder.RegisterType<ProductServiceWtihCahching>().As<IProductService>();


			var apiAssembly = Assembly.GetExecutingAssembly();
			var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //AppDbContext den katmanın adını anlıyor dinamik olarak.
			var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //bu da öyle

			builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope(); //sonu Repository olanlar...

			builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();




			// InstancePerLifetimeScope => Scope a denk geliyor EfCore daki
		}
	}
}
