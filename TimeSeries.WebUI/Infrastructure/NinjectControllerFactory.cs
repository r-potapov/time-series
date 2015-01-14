using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using TimeSeries.Domain.Abstract;
using TimeSeries.Domain.Concrete;

namespace TimeSeries.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            // размещение привязок
            ninjectKernel.Bind<ITimeSeriesRepository>().To<EFTimeSeriesRepository>();
        }
    }
}