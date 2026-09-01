using FirstMvcWebapp.Data;
using FirstMvcWebapp.Dto;
using FirstMvcWebapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstMvcWebapp.Controllers
{
    [Authorize]
    public class DashboardController(AppDbContext context): Controller
    {
        public IActionResult Index()
        {
            var list = context.Products.Select(x=>new ProductDto {Id = x.Id , ProductName = x.ProductName, Description = x.Description , color = x.color,Price = x.Price}).ToList(); 
            return View(list);
        }

        public IActionResult Productform() 
        {
        return View();
        }
        public async Task<IActionResult> UpdateProductForm(int id)
        {
            var data = await context.Products.Select(x=> new ProductDto {Id = x.Id,
            ProductName = x.ProductName,
            Description = x.Description ,
            Price = x.Price,
            color= x.color}).FirstOrDefaultAsync(x=>x.Id == id);
            return View(data);
        }
        public async Task<IActionResult> CreateProduct(ProductDto dto) 
        {
            if (dto == null) 
            {
                ViewBag.ErrorMessage = "Please! fill all the details!";
                return View("Productform");
            }
            context.Products.Add(new Models.Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,               
                Price = dto.Price,
                color = dto.color 
            });
            await context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> UpdateProduct(ProductDto dto)
        {
            if (dto == null)
            {
                ViewBag.ErrorMessage = "Please fill all the details";
                return View("UpdateProductForm");
            }

            var data = context.Products.FirstOrDefault(x=> x.Id == dto.Id);

            data.ProductName = dto.ProductName;
            data.color = dto.color;
            data.Description = dto.Description;
            data.Price = dto.Price;

            context.Products.Update(data);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> DeleteProduct(int productid) 
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productid);
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
