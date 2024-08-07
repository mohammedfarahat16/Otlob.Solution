using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.DTOs;
using Otlob.APIs.Errors;
using Otlob.APIs.Helpers;
using Otlob.Core;
using Otlob.Core.Entites;
using Otlob.Core.Repositories;
using Otlob.Core.Specifications;

namespace Otlob.APIs.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public ProductsController(IMapper _mapper
            ,IUnitOfWork _unitOfWork)
        {
            mapper = _mapper;
            unitOfWork = _unitOfWork;
        }
        //GetAll


        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);

            var MappedProducts = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            var CountSpec = new ProductWithFiltrationForCountSpec(Params);
            var Count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto> (Params.PageIndex , Params.PageSize, MappedProducts, Count));

        }


        //Get product

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var Product = await unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            if (Product is null)
            {
                return NotFound(new ApiResponse(404));
            }
            var MappedProduct = mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }


        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes() 
        {
            var Types = await unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }
    }
}
