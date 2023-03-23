using Autofac;
using System.Diagnostics;
using System.Reflection;
using Topshelf;

namespace TickIT.Auth
{
    public class Program
    {
        private static IContainer _container;

        public static void Main(string[] args)
        {
            SetupContainer();
            SetupHost();
        }

        private static void SetupContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new TickITModules());
            containerBuilder.Register(_ => new HostService(_.Resolve<TickITRepositoryManager>()));
            _container = containerBuilder.Build();
        }

        private static void SetupHost()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
            System.Console.WriteLine($"Starting TickIT {version} service...");

            HostFactory.Run(x =>
            {
                x.UseNLog();
                x.Service<HostService>(_ =>
                    {
                        _.ConstructUsing(name => _container.Resolve<HostService>());
                        _.WhenStarted(s => s.OnStart());
                        _.WhenStopped(tc => tc.OnStop());
                    }
                );

                x.StartAutomaticallyDelayed();
                x.RunAsLocalSystem();

                x.SetDescription("Ticketing solution for outlook");
                x.SetDisplayName($"TickIT {version}");
                x.SetServiceName($"TickIT");
            });
        }
    }
}
