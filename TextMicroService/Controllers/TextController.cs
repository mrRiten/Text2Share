using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TextMicroService.Application.Services;
using TextMicroService.Core.Models;

namespace TextMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController(ITextService textService, IOptions<XSource> source) : ControllerBase
    {
        private readonly ITextService _textService = textService;
        private readonly XSource _source = source.Value;

        // GET: api/Text
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var texts = await _textService.GetAllTextAsync(false);
            return Ok(texts);
        }

        // GET: api/Text/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            Text? text;
            if (HttpContext.Request.Headers["X-Source"] == _source.Token)
            {
                text = await _textService.GetTextAsync(id, true);
            }
            else
            {
                text = await _textService.GetTextAsync(id, false);
            }

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

        // PATCH: api/Text/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Text> patchDoc)
        {
            if (HttpContext.Request.Headers["X-Source"] != _source.Token) { return BadRequest(); }

            if (patchDoc == null) { return BadRequest(); }

            var text = await _textService.GetTextAsync(id, true);
            text.PrivetToken = await _textService.GetTextTokenAsync(id);

            if (text == null) { return NotFound(); }

            patchDoc.ApplyTo(text, (patchError) =>
            {
                ModelState.AddModelError(patchError.AffectedObject.ToString(), patchError.ErrorMessage);
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _textService.UpdateTextAsync(text);

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
