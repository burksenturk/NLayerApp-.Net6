using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //[Route("api/[controller]")]   Bunlar artık Base de olduğu için benim burada bunlara ihtiyacım yok
    //[ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        //private readonly IService<Product> _service;
        private readonly IProductService _productService;

		public ProductsController(IMapper mapper, IService<Product> service, IProductService productService)
		{
			_mapper = mapper;
			//_service = service;
			_productService = productService;
		}
        [HttpGet("[action]")]
        public async Task<IActionResult>  GetProductstWithCategory()
        {
            return CreateActionResult(await _productService.GetProductWithCategory());  //business da diğer işlemelr halloldugu için tek satırda tamamladım
        }


		[HttpGet]
        public async Task<IActionResult> All()
        {   //genericoldukları için mecburen maplemeleri controllerda yapıyoruz
            var products = await _productService.GetAllAsync();
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList()); //async oludugunda IEnumarble döndüğü için ToList koydum
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await _productService.GetByIdAsync(id);
            var productsDtos = _mapper.Map<ProductDto>(products);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDtos));
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var products = await _productService.AddAsync(_mapper.Map<Product>(productDto));    //AddAsync Product entity istediği için mapleme yaptık
            var productsDtos = _mapper.Map<ProductDto>(products);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDtos));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id);
             await _productService.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

    }
}
