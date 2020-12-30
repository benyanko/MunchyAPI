using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunchyAPI.Models;
using MunchyAPI.Services;
using Microsoft.AspNetCore.Routing;

namespace MunchyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService categoryService;
        private readonly ItemService itemService;

        public CategoriesController(CategoryService i_categoryService, ItemService i_itemService)
        {
            categoryService = i_categoryService;
            itemService = i_itemService;
        }

        // GET: api/Options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/Options/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Category>> GetById(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (category.ListOfItemId.Count > 0)
            {
                var tempList = new List<Item>();
                foreach (var itemId in category.ListOfItemId)
                {
                    var item = await itemService.GetByIdAsync(itemId);
                    if (itemId != null)
                    {
                        tempList.Add(item);
                    }
                }
                category.ListOfItem = tempList;
            }

            return Ok(category);
        }

        // POST: api/Options
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await categoryService.CreateAsync(category);
            return Ok(category);
        }

        // PUT: api/Options/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedCategory = await categoryService.GetByIdAsync(id);
            if (queriedCategory == null)
            {
                return NotFound();
            }
            category.Id = id;
            category.ListOfItemId = queriedCategory.ListOfItemId;
            await categoryService.UpdateAsync(id, category);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await itemService.DeleteByCategoryIdAsync(id);
            await categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
