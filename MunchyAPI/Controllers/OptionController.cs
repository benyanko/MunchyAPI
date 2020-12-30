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
    public class OptionController : ControllerBase
    {
        private readonly OptionsService optionsService;
        private readonly OptionService optionService;

        public OptionController(OptionService i_optionService, OptionsService i_optionsService)
        {
            optionsService = i_optionsService;
            optionService = i_optionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Options>>> GetAll()
        {
            var option = await optionService.GetAllAsync();
            return Ok(option);
        }

        // GET: api/Option/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Options>> GetById(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await optionService.GetByIdAsync(id);
            if (option == null)
            {
                return NotFound();
            }

            return Ok(option);

        }
        // POST: api/Option
        [HttpPost]
        public async Task<IActionResult> Create(Option option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var queriedOptions = await optionsService.GetByIdAsync(option.ParentId);
            if (queriedOptions == null)
            {
                return NotFound();
            }

            await optionService.CreateAsync(option);
            await optionsService.AddOptionAsync(option.ParentId, option.Id);
            return Ok(option);
        }

        // PUT: api/Option/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Option option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedOption = await optionService.GetByIdAsync(id);
            if (queriedOption == null)
            {
                return NotFound();
            }

            
            option.Id = id;
            option.ParentId = queriedOption.ParentId;
            await optionService.UpdateAsync(id, option);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var option = await optionService.GetByIdAsync(id);
            if (option == null)
            {
                return NotFound();
            }

            await optionsService.DeleteOptionAsync(option.ParentId, id);
            await optionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
