using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;

        

        private readonly CategoryApiService _categoryApiService;

        public ProductsController(CategoryApiService categoryApiService , ProductApiService productApiService)
        {
            _categoryApiService = categoryApiService;
            _productApiService = productApiService;
        }

        public async Task<IActionResult> Index()
        {
            //bu şekilde artık tek satırda yapablirim.
            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        public async Task<IActionResult> Save()
        {
            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if(ModelState.IsValid)
            {
                await _productApiService.Saveasync(productDto);
                return RedirectToAction(nameof(Index));
                //yani diyor ki işlem tamamlanınca indexe geri dön
            }
            var categories = await _categoryApiService.GetAllAsync();
            
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiService.GetByIdAsync(id);
            var categories = await _categoryApiService.GetAllAsync();


            ViewBag.categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            
            if(ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name", productDto.CategoryId);
            return View(productDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
           var product = await _productApiService.GetByIdAsync(id);
            await _productApiService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
