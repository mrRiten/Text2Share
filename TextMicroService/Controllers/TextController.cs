using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextMicroService.Application.Services;
using TextMicroService.Core.Models;

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
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var text = await _textService.GetTextAsync(id, false);

            if (text == null)
            {
                return BadRequest("No items");
            }
            
            return Ok(text);
        }

        // GET api/<TextController>?token=asd
        [HttpGet]
        public async Task<IActionResult> Get(string token)
        {
            var text = await _textService.GetTextAsync(token);

            if (text == null) { return NotFound(); }

            return Ok(text);
        }

        [HttpGet("/Token")]
        [Authorize]
        public async Task<IActionResult> GetToken(int textId)
        {
            var textToken = await _textService.GetTextTokenAsync(textId);

            if (textToken == null) { return NotFound(); }

            return Ok(textToken);
        }

        // POST api/<TextController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(TextUpload model)
        {
            await _textService.CreateTextAsync(model);

            return Ok();
        }

        // PUT api/<TextController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id,TextUpload model)
        {
            await _textService.UpdateTextAsync(id, model);

            return Ok();
        }

        // DELETE api/<TextController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _textService.DeleteTextAsync(id);

            return Ok();
        }
    }
}
