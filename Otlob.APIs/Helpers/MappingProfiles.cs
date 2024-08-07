using AutoMapper;
using Otlob.APIs.DTOs;
using Otlob.Core.Entites;
using Otlob.Core.Entites.Identity;
using Otlob.Core.Entites.Order_Aggregate;

namespace Otlob.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                     .ForMember(d=>d.ProductType,O=>O.MapFrom(S=>S.ProductType.Name))
                     .ForMember(d=>d.ProductBrand,O=>O.MapFrom(S=>S.ProductBrand.Name))
                     .ForMember(d=>d.PictureUrl,O=>O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Core.Entites.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<AddressDto, Core.Entites.Order_Aggregate.Address>();

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                    .ForMember(d=>d.DeliveryMethod,O=>O.MapFrom(S=>S.DeliveryMethod.ShortName))
                    .ForMember(d=>d.DeliveryMethodCost,O=>O.MapFrom(S=>S.DeliveryMethod.Cost));


            CreateMap<OrderItem, OrderItemDto>()
                    .ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
                    .ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
                    .ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
                    .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());






        }
    }
}
