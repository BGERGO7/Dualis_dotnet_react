using Mapster;
using React_dotnet.database.Models;
using React_dotnet.Server.Dtos;

namespace React_dotnet.Server
{
    public class MapsterConfigurator
    {
        public static void Configure()
        {
            TypeAdapterConfig<ProductDto, Product>
                .NewConfig()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Description, s => s.Description)
                .Map(d => d.Price, s => s.Price);

            TypeAdapterConfig<Product, ProductDto>
                .NewConfig()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Description, s => s.Description)
                .Map(d => d.Price, s => s.Price);
        }   
    }
}
