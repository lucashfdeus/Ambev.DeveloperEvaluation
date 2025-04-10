using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Profile for mapping between CreateSale entity and CreateUserResponse
    /// </summary>
    /// 
    public class CreateSaleProfile : AutoMapper.Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand.SaleItemCommand, SaleItem>();
            CreateMap<CreateSaleCommand, Sale>();
            CreateMap<Sale, CreateSaleResult>();
        }
    }
}

