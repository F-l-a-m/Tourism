using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Ninject;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;

namespace Tourism.WebUI.Infrastructure
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
            // put bindings here
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new List<Tour>
            {
                new Tour { Name = "Riga", Price = 25 },
                new Tour { Name = "Sigulda", Price = 179 },
                new Tour { Name = "Cesis", Price = 95 }
            });

            kernel.Bind<ITourRepository>().ToConstant(mock.Object);
        }
    }
}