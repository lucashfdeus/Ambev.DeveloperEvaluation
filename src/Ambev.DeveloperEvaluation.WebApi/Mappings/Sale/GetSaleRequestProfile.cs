using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Sale
{
    public class GetSaleRequestProfile : Profile
    {
        public GetSaleRequestProfile()
        {
            CreateMap<Guid, GetSaleCommand>()
                .ConstructUsing(id => new GetSaleCommand(id));

            CreateMap<GetSaleResult, GetSaleResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<GetSaleItemResult, GetSaleItemResponse>()
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled));
        }
    }
}
