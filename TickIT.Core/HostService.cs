using FluentScheduler;
using System.Text.Json;
using System;
using TickIT.Auth.Models;
using Message = TickIT.Auth.Models.Message;
using System.IO;
using System.Collections.Generic;

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
        public static List<Message> GetMailsFromJson()
        {
            string fileName = "GraphData.json";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GraphApiResponse<Message>>(text)?.Value;
        }
    }
}
