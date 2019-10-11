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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryLogic _logic;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryLogic logic,
            IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var result = await _logic.Create(category);

            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            categoryDto.Id = result.Value.Id;

            return CreatedAtAction(nameof(Create), categoryDto);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _logic.GetAllActive();
            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDto>>(result.Value);
            return Ok(categoriesToReturn);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _logic.GetById(id);
            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var categoryToReturn = _mapper.Map<CategoryDto>(result.Value);
            return Ok(categoryToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryResult = await _logic.GetById(id);
            if (!categoryResult.Success)
            {
                categoryResult.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var result = await _logic.Remove(categoryResult.Value);
            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, CategoryDto categoryDto)
        {
            if(id != categoryDto.Id)
            {
                return BadRequest();
            }

            var categoryResult = await _logic.GetById(id);

            if (!categoryResult.Success)
            {
                categoryResult.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            categoryResult.Value.Name = categoryDto.Name;

            var result = await _logic.Update(categoryResult.Value);

            if (!result.Success)
            {
                result.AddErrorToModelState(ModelState);
                return BadRequest(ModelState);
            }

            return Ok(categoryDto);
        }
    }
}