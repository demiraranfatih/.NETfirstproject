using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Services;

namespace NLayer.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        // get api/products/GetProductsWithCategory
        //busines kod bulundurmayacağım burada. mapleme olayını burada yapacağım.

        private readonly IMapper _mapper;
      //  private readonly IService<Product> _service;
    //    private readonly IProductService productService;
         private readonly IProductService _service;

        //ctor ekliyorum
        public ProductsController(IMapper mapper, IService<Product> service,IProductService productService)
        {
            _mapper = mapper;
            //*    _service = service;
            //   this.productService = productService;
            _service = productService;
        }

        //get alldan  başlıyorum
        //bu bir http get isteği
        // get api/products/GetProductsWithCategory
        [HttpGet("[action]")]// action direkt metodun adını alıyor.
        public async Task<IActionResult> GetProductsWithCategory()
        {
            //   return CreateActionResult(await productService.GetProductWitCategory());
            return CreateActionResult(await _service.GetProductWitCategory());
        }
        //bu şekilde tek satırda da halledebilirim kodlama işini.
        //aslında bunu yapma kısmı controller kısmında minimum kod gerekiyordu bunu sagladı



        [HttpGet] // get api/products
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            //geriye dto dönmem lazım
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());
            /*hem ok var hem 200 ikiside ok demek zaten
            //bunun yerine her yerde yazmak yerine bir tane oluşturayım orada kullanırım
            //customebasecontroller oluşturuyorumç..
            //buna gerek kalmadı
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));*/
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));*/
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
            //3 satırda olayı daha temiz bir hale getirdik.
        }

        //getbyid id bekliyorum mysite.com/api/product/5 dersem idsi 5 olan gelecektir.
        //id istemede 
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
            //3 satırda olayı daha temiz bir hale getirdik.
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto) // burayı productnew dto yap dene
        {
            //yukarıda bir prodcutdto aldık bunu producta çevirmem gerekiyor
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            //sonrasında yine tam tersi
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
            //3 satırda olayı daha temiz bir hale getirdik.
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            //yukarıda bir prodcutdto aldık bunu producta çevirmem gerekiyor
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            //sonrasında yine tam tersi
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            //3 satırda olayı daha temiz bir hale getirdik. // burada geri data göndermiyor öyle ayarladık
        }
        // DELETE api/products/5
       // [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
            //3 satırda olayı daha temiz bir hale getirdik.
        }

    }
}
