using AutoMapper;

namespace TickIT.App
{
    public class MapperBootstrapper
    {
        public static IMapper Mapper { get; set; }

        public static void CreateMapper()
        {
            Mapper = SetupConfig().CreateMapper();
        }

        public static MapperConfiguration SetupConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                

            });
        }
    }
}
