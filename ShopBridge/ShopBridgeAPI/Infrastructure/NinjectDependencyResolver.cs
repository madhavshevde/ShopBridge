using Ninject;
using ShopBridge.Domain.Abstract;
using ShopBridge.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBridgeAPI.Infrastructure
{
	public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;
		public NinjectDependencyResolver(IKernel kernelParam)
		{
			kernel = kernelParam;
			AddBindings();
		}
		public object GetService(Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}
		public IEnumerable<object> GetServices(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}
		private void AddBindings()
		{
			kernel.Bind<IProductRepository>().To<EFProductRepository>();
		}
	}
}