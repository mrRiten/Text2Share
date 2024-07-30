using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextMicroService.Application.Services;
using TextMicroService.Core.Models;

namespace TextMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly ITextService _textService;

        public TextController(ITextService textService)
        {
            _textService = textService;
        }

        // GET: api/Text
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var texts = await _textService.GetAllTextAsync(false);
            return Ok(texts);
        }

        // GET: api/Text/5
        [HttpGet("{id:int}")]
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

        // GET: api/Text/byToken/{token}
        [HttpGet("byToken/{token}")]
        public async Task<IActionResult> GetByToken(string token)
        {
            var text = await _textService.GetTextAsync(token);

            if (text == null)
            {
                return NotFound();
            }

            return Ok(text);
        }

        // GET: api/Text/token/{textId}
        [HttpGet("token/{textId:int}")]
        [Authorize]
        public async Task<IActionResult> GetToken(int textId)
        {
            var textToken = await _textService.GetTextTokenAsync(textId);

            if (textToken == null)
            {
                return NotFound();
            }

            return Ok(textToken);
        }

        // POST: api/Text
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(TextUpload model)
        {
            await _textService.CreateTextAsync(model);
            return Ok();
        }

        // PUT: api/Text/5
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, TextUpload model)
        {
            await _textService.UpdateTextAsync(id, model);
            return Ok();
        }

        // DELETE: api/Text/5
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _textService.DeleteTextAsync(id);
            return Ok();
        }
    }
}
