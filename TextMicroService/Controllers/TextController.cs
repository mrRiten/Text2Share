using Microsoft.AspNetCore.Mvc;
using TextMicroService.Application.Services;
using TextMicroService.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TextMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController(ITextService textService) : ControllerBase
    {
        private readonly ITextService _textService = textService;

        // GET: api/<TextController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var texts = await _textService.GetAllTextAsync(false);

            return Ok(texts);
        }

        // GET api/<TextController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var text = await _textService.GetTextAsync(id);

            if (text == null)
            {
                return BadRequest("No items");
            }
            
            return Ok(text);
        }

        // POST api/<TextController>
        [HttpPost]
        public async Task<IActionResult> Post(TextUpload model)
        {
            await _textService.CreateTextAsync(model);

            return Ok();
        }

        // PUT api/<TextController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,TextUpload model)
        {
            await _textService.UpdateTextAsync(id, model);

            return Ok();
        }

        // DELETE api/<TextController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _textService.DeleteTextAsync(id);

            return Ok();
        }
    }
}
