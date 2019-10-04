using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.Logic;
using ProductsApp.Logic.Interfaces;
using ProductsApp.Models;
using ProductsApp.WebApi.Dto;

namespace ProductsApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductLogic _logic;

        public ProductsController(IProductLogic logic)
        {
            _logic = logic;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };
            var result = await _logic.Create(product);

            if(!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            productDto.Id = result.Value.Id;
            productDto.CreatedDate = result.Value.CreatedDate;

            return CreatedAtAction(nameof(Create), productDto);
        }

        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _logic.GetAllActive();
            var productsToReturn = result.Value.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CreatedDate = p.CreatedDate
            });
            return Ok(productsToReturn);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _logic.GetById(id);
            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var productToReturn = new ProductDto
            {
                Id = result.Value.Id,
                Name = result.Value.Name,
                Description = result.Value.Description,
                Price = result.Value.Price,
                CreatedDate = result.Value.CreatedDate
            };
            return Ok(productToReturn);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productResult = await _logic.GetById(id);
            if (!productResult.Success)
            {
                productResult.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var result = await _logic.Remove(productResult.Value);
            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, ProductDto productDto)
        {
            if(id != productDto.Id)
            {
                return BadRequest();
            }

            var productResult = await _logic.GetById(id);

            if (!productResult.Success)
            {
                productResult.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            productResult.Value.Name = productDto.Name;
            productResult.Value.Description = productDto.Description;
            productResult.Value.Price = productDto.Price;

            var result = await _logic.Update(productResult.Value);

            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return Ok(productDto);
        }
    }
}