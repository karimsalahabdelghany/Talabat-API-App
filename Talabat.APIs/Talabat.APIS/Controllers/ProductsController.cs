using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOs;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Repositiries;
using Talabat.Core.Secifications;

namespace Talabat.APIS.Controllers
{
    
    public class ProductsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        public ProductsController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        //Get all Products
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        { 
            var spec =new ProductWithBrandAndTypeSpecifications( Params);
            var Products =await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            //OkObjectResult Result = new OkObjectResult(Products);
            //return Result;
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList< ProductToReturnDto>>(Products);
            //var ReturnedObject = new Pagination<ProductToReturnDto>()
            //{
            //    PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Data = MappedProducts
            //};
            
            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            var Count =await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize,MappedProducts,Count));
        }
        //Get productbyid
        //baseurl/products/id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),200)]
        [ProducesResponseType(typeof (ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec= new ProductWithBrandAndTypeSpecifications(id);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var mappedproduct = _mapper.Map<Product,ProductToReturnDto>(Product);
            return Ok(mappedproduct);
        }

        //Get all Types
        //baseurl/api/products/Types
        [HttpGet("Types")]
        public async Task <ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types =await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }
        //Get all brands
        [HttpGet("Brands")]
        public async Task <ActionResult<IReadOnlyList<ProductBrand>>>GetBrands()
        {
            var Brands =await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }

    }
}
