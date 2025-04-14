using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Profile for mapping GetSale feature requests to commands
    /// </summary>
    public class GetSaleRequestProfile : Profile
    {
        public GetSaleRequestProfile()
        {
            CreateMap<Guid, GetSaleCommand>()
            .ConstructUsing(id => new GetSaleCommand(id));

            CreateMap<GetSaleResult, GetSaleResponse>().ReverseMap();
        }
    }
}
