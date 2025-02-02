[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AngularSkilledHubProject.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AngularSkilledHubProject.App_Start.NinjectWebCommon), "Stop")]

namespace AngularSkilledHubProject.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using Ninject.Modules;
    using System.Collections.Generic;
    using System.Reflection;
    using ServiceLayer.Interfaces;
    using ServiceLayer.Services;
    using RepositoryLayer.Repositories.Interfaces;
    using RepositoryLayer.Repositories;
    using System.Data.Entity;
    using RepositoryLayer;
    using RepositoryLayer.Infrastructure;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                // Install our Ninject-based IDependencyResolver into the Web API config
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                RegisterServices(kernel);

                //var modules = new List<INinjectModule>
                //{
                //    new AngularSkilledHubProject.DependencyResolver()
                //};

                //kernel.Load(modules);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<DbContext>().To<SkilledHubDb>().InRequestScope();
            kernel.Bind(typeof(IEntityBaseRepository<>)).To(typeof(EntityBaseRepository<>)).InRequestScope();
            kernel.Bind<ICustomerService>().To<CustomerService>();
            kernel.Bind<IProfessionalService>().To<ProfessionalService>();
            kernel.Bind<ICommonService>().To<CommonService>();
            kernel.Bind<IBusinessHourService>().To<BusinessHourService>();
            kernel.Bind<IAppointmentService>().To<AppointmentService>();

            kernel.Bind<IDbFactory>().To<DbFactory>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            // kernel.Load(Assembly.GetExecutingAssembly());
        }        
    }
}
