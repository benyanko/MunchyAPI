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
    public class OptionsController : ControllerBase
    {
        private readonly OptionsService optionsService;
        private readonly OptionService optionService;
        private readonly ItemService itemService;

        public OptionsController(OptionService i_optionService, OptionsService i_optionsService, ItemService i_itemService)
        {
            optionsService = i_optionsService;
            optionService = i_optionService;
            itemService = i_itemService;
        }

        // GET: api/Options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Options>>> GetAll()
        {
            var options = await optionsService.GetAllAsync();
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

            var options = await optionsService.GetByIdAsync(id);
            if (options == null)
            {
                return NotFound();
            }

            if (options.ListOfOptionId.Count > 0)
            {
                var tempList = new List<Option>();
                foreach (var optionId in options.ListOfOptionId)
                {
                    var option = await optionService.GetByIdAsync(optionId);
                    if (option != null)
                    {
                        tempList.Add(option);
                    }
                }
                options.ListOfOption = tempList;
            }

            return Ok(options);
        }

        // POST: api/Options
        [HttpPost]
        public async Task<IActionResult> Create(Options options)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           
            await optionsService.CreateAsync(options);
            return Ok(options);
        }

        // PUT: api/Options/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Options options)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedOptions = await optionsService.GetByIdAsync(id);
            if (queriedOptions == null)
            {
                return NotFound();
            }
            options.Id = id;
            options.ListOfOptionId = queriedOptions.ListOfOptionId;
            await optionsService.UpdateAsync(id, options);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var options = await optionsService.GetByIdAsync(id);
            if (options == null)
            {
                return NotFound();
            }

            await itemService.DeleteOptionsFromAllAsync(id);
            await optionService.DeleteParentOptionsAsync(id);
            await optionsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
