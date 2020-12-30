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
    public class ItemsController : ControllerBase
    {
        private readonly ItemService itemsService;
        private readonly OptionsService optionsService;
        private readonly CategoryService categoryService;

        public ItemsController(ItemService i_itemService, OptionsService i_optionsService, CategoryService i_categoryService)
        {
            itemsService = i_itemService;
            optionsService = i_optionsService;
            categoryService = i_categoryService;
        }

        // GET: api/Options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Options>>> GetAll()
        {
            var options = await itemsService.GetAllAsync();
            return Ok(options);
        }

        // GET: api/Options/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Options>> GetById(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await itemsService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var queriedCategory = await categoryService.GetByIdAsync(item.CategoryId);
            if (queriedCategory == null)
            {
                return NotFound();
            }

            if (item.ListOfOptiosnId.Count > 0)
            {
                var tempList = new List<Options>();
                foreach (var optionsId in item.ListOfOptiosnId)
                {
                    var options = await optionsService.GetByIdAsync(optionsId);
                    if (options != null)
                    {
                        tempList.Add(options);
                    }
                }
                item.ListOfOptions = tempList;
            }

            return Ok(item);
        }


        // POST: api/Options
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedCategory = await categoryService.GetByIdAsync(item.CategoryId);
            if (queriedCategory == null)
            {
                return NotFound();
            }

            await itemsService.CreateAsync(item);
            await categoryService.AddItemAsync(item.CategoryId, item.Id);
            return Ok(item);
        }

        // PUT: api/Options/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedItems = await itemsService.GetByIdAsync(id);
            if (queriedItems == null)
            {
                return NotFound();
            }

            item.Id = id;
            item.ListOfOptiosnId = queriedItems.ListOfOptiosnId;
            item.CategoryId = queriedItems.CategoryId;
            await itemsService.UpdateAsync(id, item);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await itemsService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await categoryService.DeleteItemAsync(item.CategoryId, id);
            await itemsService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:length(24)}/{optionsId:length(24)}")]
        public async Task<IActionResult> AddOptions(string id, string optionsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedItems = await itemsService.GetByIdAsync(id);
            if (queriedItems == null)
            {
                return NotFound();
            }

            var queriedOptions = await optionsService.GetByIdAsync(optionsId);
            if (queriedOptions == null)
            {
                return NotFound();
            }

            await itemsService.AddOptionsAsync(id, optionsId);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}/{optionsId:length(24)}")]
        public async Task<IActionResult> DeleteOptions(string id, string optionsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedItems = await itemsService.GetByIdAsync(id);
            if (queriedItems == null)
            {
                return NotFound();
            }

            var queriedOptions = await optionsService.GetByIdAsync(optionsId);
            if (queriedOptions == null)
            {
                return NotFound();
            }

            await itemsService.DeleteOptionsAsync(id, optionsId);
            return NoContent();
        }
    }
}
