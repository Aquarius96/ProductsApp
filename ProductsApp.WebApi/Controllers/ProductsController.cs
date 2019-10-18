using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductsController(IProductLogic logic,
            IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(ProductForCreationDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var result = await _logic.Create(product);

            if(!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var productToReturn = _mapper.Map<ProductDto>(productDto);

            return CreatedAtAction(nameof(Create), productToReturn);
        }

        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _logic.GetAllActive();
            var productsToReturn = _mapper.Map<IEnumerable<ProductDto>>(result.Value);
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

            var productToReturn = _mapper.Map<ProductDto>(result.Value);
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
        public async Task<IActionResult> Update(int id, ProductForCreationDto productDto)
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

            _mapper.Map(productDto, productResult.Value);

            var result = await _logic.Update(productResult.Value);

            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var productToReturn = _mapper.Map<ProductDto>(productDto);

            return Ok(productToReturn);
        }
    }
}