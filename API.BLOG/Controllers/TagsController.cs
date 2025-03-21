using APP.Projects.Features.Tags;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.BLOG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Topics
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IQueryable<TagQueryResponse> query = await _mediator.Send(new TagQueryRequest());
            List<TagQueryResponse> list = await query.ToListAsync();
            if (list.Count > 0) // list.Any()
                return Ok(list);
            return NoContent();
        }

        // GET: api/Topics/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = await _mediator.Send(new TagQueryRequest());
            var item = await query.SingleOrDefaultAsync(t => t.Id == id);
            if (item is not null)
                return Ok(item);
            return NoContent();
        }

        // POST: api/Topics
        [HttpPost]
        public async Task<IActionResult> Post(TagCreateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    //return CreatedAtAction(nameof(Get), new { id = response.Id }, response); // 201
                    return Ok(response);
                }
                ModelState.AddModelError("TopicsPost", response.Message);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Topics
        [HttpPut]
        public async Task<IActionResult> Put(TagUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response.IsSuccessful)
                {
                    // return NoContent();
                    return Ok(response);
                }
                ModelState.AddModelError("TopicsPut", response.Message);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Topics/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new TagDeleteRequest() { Id = id });

            if (response.IsSuccessful)
            {
                // return NoContent();
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
