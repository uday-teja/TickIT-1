using AutoMapper;
using System;
using TickIT.Core.Models;
using TickIT.Models;
using TickIT.Models.Models;

namespace TickIT.App
{
    public class MapperBootstrapper
    {
        public static IMapper CreateMapper()
        {
            return SetupConfig().CreateMapper();
        }

        public static MapperConfiguration SetupConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Message, Ticket>()
                    .ForMember(ticket=> ticket.Name, map=>map.MapFrom(message=>message.Subject))
                    .ForMember(ticket => ticket.Status,map=>map.MapFrom(message=>Enums.Status.Unassigned))
                    .ForMember(ticket => ticket.Priority,map=>map.MapFrom(message=>Enums.Priority.High))
                    .ForMember(ticket => ticket.Category,map=>map.MapFrom(message=>Enums.Category.Others))
                    .ForMember(ticket => ticket.Description, map=>map.MapFrom(message=>message.WebLink))
                    .ForMember(ticket => ticket.CreatedOn, map=>map.MapFrom(message=>DateTime.Now))
                    .ReverseMap();

            });
        }
    }
}
