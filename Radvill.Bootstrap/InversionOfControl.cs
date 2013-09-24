using System.Web.Http;
using System.Web.Mvc;
using Radvill.Bootstrap.DependencyResolvers;
using StructureMap;

namespace Radvill.Bootstrap
{
    public class InversionOfControl
    {
        public static void Initialize(HttpConfiguration configuration)
        {
            ObjectFactory.Configure(x => x.Scan(y =>
            {
                y.AssemblyContainingType<Services.DataFactory.IDataFactory>();
                y.AssemblyContainingType<DataFactory.Public.DataFactory>();
                y.AssemblyContainingType<Security.Public.AuthenticationService>();
                y.AssemblyContainingType<Advisor.Public.AdviseManager>();
                y.LookForRegistries();
                y.WithDefaultConventions();
            }));

            ObjectFactory.AssertConfigurationIsValid();

            configuration.DependencyResolver = new StructureMapHttpDependencyResolver(ObjectFactory.Container);
            DependencyResolver.SetResolver(new StructureMapMvcDependencyResolver(ObjectFactory.Container));
        }
    }
}