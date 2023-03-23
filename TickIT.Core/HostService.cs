using FluentScheduler;

namespace TickIT.Auth
{
    public class HostService
    {
        private readonly TickITRepositoryManager _tickITRepositoryManager;

        public HostService(TickITRepositoryManager tickITRepositoryManager)
        {
            _tickITRepositoryManager = tickITRepositoryManager;
            JobManager.Initialize();
        }

        public void OnStart()
        {
            JobManager.AddJob(_tickITRepositoryManager.PushToAzure, _ => _.ToRunEvery(10).Seconds());
        }

        public void OnStop()
        {

        }
    }
}
