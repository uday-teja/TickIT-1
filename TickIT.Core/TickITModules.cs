using Autofac;

namespace TickIT.Core
{
    public class TickITModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new TickITRepositoryManager()).SingleInstance();
        }
    }
}
