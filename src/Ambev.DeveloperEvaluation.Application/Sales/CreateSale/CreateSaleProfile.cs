using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Profile for mapping between CreateSale entity and CreateUserResponse
    /// </summary>
    /// 
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<Sale, CreateSaleCommand>();
            CreateMap<CreateSaleCommand.SaleItemCommand, SaleItem>();

            CreateMap<Sale, CreateSaleResult>();
            CreateMap<SaleItem, SaleItemResult>();
        }
    }
}

