using System;
using System.Collections.Generic;
using TickIT.Models;
using TickIT.Models.Models;

namespace TickIT.App.Helpers
{
    public class DataGenerator
    {
        public static List<Ticket> CreateTickets(int count)
        {
            List<Ticket> tasks = new();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(new Ticket()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Dummy task {i}",
                    Description = $"Dummy descripiton",
                    Status = (Enums.Status)(i % 3),
                    Priority = (Enums.Priority)(i % 3),
                    Category = (Enums.Category)(i % 4),
                    PercentageCompleted = 0,
                    DueDate = DateTime.Now.AddHours(i % 3 + 1),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                });
            }
            return tasks;
        }
    }
}
