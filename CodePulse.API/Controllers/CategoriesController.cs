﻿using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CodePulse.API.Controllers
{
    //https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        /*   //using inside the mehtod this filed
private readonly ApplicationDbContext _dbContext;

//i want to use to injected dbcontext class in constaller
public CategoriesController(ApplicationDbContext dbContext)
{
this._dbContext = dbContext;
}*/
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto req)
        {
            //Map DTO to Domain model

            var category = new Category
            {
                Name = req.Name,
                UrlHandle = req.UrlHandle
            };


            //i want to use to injected dbcontext class in constaller
            //saving the data inside controller that is a bad practice remove 
            //add it to repository instead  

            /* await _dbContext.Categories.AddAsync(category);
             await _dbContext.SaveChangesAsync();*/



            /*          abstraction the implementation to the repository and the controller
        has no idea how to repository is able to talk and save these changes inside the database*/
            await categoryRepository.CreateAsync(category);

            //we never expose the domain model
            // return Ok(category)
            //Domain model to dto

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);

        }


        //GET: https://localhost:7091/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            //map domain model to DTO
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }



        //GET: https://localhost:7091/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }




        // PUT : https://localhost:7091/api/Categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            //Convert DTO to Domain Model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            category = await categoryRepository.UpdateAsync(category);
            if (category == null)
            {
                return NotFound();
            }
            // convert Domain model to Dto

            var respone = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(respone);
        }
    }
}
