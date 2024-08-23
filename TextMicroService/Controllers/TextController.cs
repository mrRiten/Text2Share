using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TextMicroService.Application.Services;
using TextMicroService.Core.Attributes;
using TextMicroService.Core.Models;

namespace TextMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController(ITextService textService, IOptions<XSource> source) : ControllerBase
    {
        private readonly ITextService _textService = textService
            ?? throw new ArgumentNullException(nameof(textService));
        private readonly XSource _source = source.Value
            ?? throw new ArgumentNullException(nameof(source));

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
            var isAdmin = IsRequestFromSource();
            var text = await _textService.GetTextAsync(id, isAdmin);

            if (text == null)
            {
                return NotFound("No items");
            }

            return Ok(text);
        }

        // GET: api/Text/account/public/{userId}
        [HttpGet("account/public/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var isAdmin = IsRequestFromSource();
            var text = await _textService.GetAllTextByUserAsync(userId, isAdmin);

            if (text == null || text.Count == 0)
            {
                return NotFound("No items");
            }

            return Ok(text);
        }

        // GET: api/Text/account/privet
        [HttpGet("account/privet")]
        [Authorize]
        public async Task<IActionResult> GetByAccount()
        {
            var text = await _textService.GetAllUserTextAsync();

            if (text == null || text.Count == 0)
            {
                return NoContent();
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

        // GET: api/Text/token/{textId:int}
        [HttpGet("token/{textId:int}")]
        [Authorize]
        public async Task<IActionResult> GetToken(int textId)
        {
            var text = await _textService.GetTextAsync(textId, true);
            if (text?.PrivetToken == null)
            {
                return NotFound();
            }

            return Ok(text.PrivetToken);
        }

        // POST: api/Text
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(TextUpload model)
        {
            await _textService.CreateTextAsync(model);
            return CreatedAtAction(nameof(Get), new { id = model.UserId }, model);
        }

        // PATCH: api/Text/5
        [HttpPatch("{id:int}")]
        [ServiceFilter(typeof(ValidateSourceFilter))]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Text> patchDoc)
        {
            if (patchDoc == null) return BadRequest();

            foreach (var operation in patchDoc.Operations)
            {
                if (!_textService.IsAllowedPath(operation.path))
                {
                    return BadRequest($"Modification of the '{operation.path}' field is not allowed.");
                }
            }

            var text = await _textService.GetTextAsync(id, true);
            if (text == null) return NotFound();

            patchDoc.ApplyTo(text, ModelState);
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
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
            return NoContent();
        }

        private bool IsRequestFromSource()
        {
            return HttpContext.Request.Headers["X-Source"] == _source.Token;
        }
    }
}
