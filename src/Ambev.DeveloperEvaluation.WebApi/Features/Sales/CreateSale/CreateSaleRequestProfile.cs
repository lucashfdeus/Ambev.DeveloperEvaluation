using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestProfile : Profile
    {
        public CreateSaleRequestProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, CreateSaleCommand.SaleItemCommand>();

            CreateMap<CreateSaleCommand, Sale>();
            CreateMap<CreateSaleCommand.SaleItemCommand, SaleItem>();

            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<SaleItemResult, SaleItemResponse>();

            CreateMap<SaleItem, SaleItemResponse>();
            CreateMap<CreateSaleResult, CreateSaleResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
