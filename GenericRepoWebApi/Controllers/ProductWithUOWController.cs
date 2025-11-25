using GenericRepoWebApi.Dtos;
using GenericRepoWebApi.Entity;
using GenericRepoWebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GenericRepoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithUOWController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAllAsync();

            return Ok(products ?? Enumerable.Empty<Product>());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransactionAsync();
                var newProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price
                };
                var productResult = await _unitOfWork.GetRepository<Product>().AddAsync(newProduct);
                await _unitOfWork.SaveChangesAsync();

                var orderEntity = new Order
                {
                    OrderDate = DateTime.UtcNow,
                    ProductId = productResult.Id
                };

                await _unitOfWork.GetRepository<Order>().AddAsync(orderEntity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return Ok(StatusCode((int)HttpStatusCode.Created, new
                {
                    Id = productResult.Id
                }));
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork.productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
