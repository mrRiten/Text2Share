using LikeMicroService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController(ILikeService likeService) : ControllerBase
    {
        private readonly ILikeService _likeService = likeService;

        // GET api/<LikeController>/5
        [HttpGet("{textId}")]
        public async Task<IActionResult> Get(int textId)
        {
            var like = await _likeService.GetLikeAsync(textId);

            if (like == null) { return NoContent(); }

            return Ok(like);
        }

        // POST api/<LikeController>/5
        [HttpPost("{textId}")]
        public async Task<IActionResult> Post(int textId)
        {
            await _likeService.CreateLikeAsync(textId);

            return Ok();
        }

        // DELETE api/<LikeController>/5
        [HttpDelete("{textId}")]
        public async Task<IActionResult> Delete(int textId)
        {
            await _likeService.DeleteLikeAsync(textId);

            return Ok();
        }
    }
}
