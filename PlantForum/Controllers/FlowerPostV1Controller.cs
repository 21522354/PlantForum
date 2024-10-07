using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantForum.Data.Models;
using PlantForum.Data;
using PlantForum.Dtos;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace PlantForum.Controllers
{
    [Route("api/v2/FlowerPost")]
    [ApiController]
    [ControllerName("FlowerPostV2")]
    public class FlowerPostV1Controller : ControllerBase
    {
        private readonly PlanForumDBContext _context;
        private readonly IMapper _mapper;

        public FlowerPostV1Controller(PlanForumDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllFlowerPost")]
        public async Task<IActionResult> GetFlowerPosts()
        {
            var flowerPosts = await _context.FlowerPosts.ToListAsync();
            var responseObject = new ResponseObject()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Get flower posts successfully",
                Data = _mapper.Map<IEnumerable<FlowerPostReadDto>>(flowerPosts)
            };
            return Ok(responseObject);
        }

        [HttpGet("GetFlowerPostById/{id}")]
        public async Task<IActionResult> GetFlowerPost(int id)
        {
            var flowerPost = await _context.FlowerPosts.FirstOrDefaultAsync(f => f.Id == id);

            if (flowerPost == null)
            {
                return NotFound(new ResponseObject()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Cannot find the Post with id = " + id,
                    Data = ""
                });
            }

            return Ok(new ResponseObject()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Get flower post successfully",
                Data = _mapper.Map<FlowerPostReadDto>(flowerPost)
            });
        }
        [Authorize]
        [HttpPost("PostFlowerPost")]
        public async Task<IActionResult> PostFlowerPost([FromBody] FlowerPostRequestDto flowerPostRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Some thing went wrong",
                    Data = ""
                });
            }

            flowerPostRequest.DateAdded = DateTime.UtcNow;
            var flowerPost = _mapper.Map<FlowerPost>(flowerPostRequest);
            _context.FlowerPosts.Add(flowerPost);
            await _context.SaveChangesAsync();
            var flowerPostRead = _mapper.Map<FlowerPostReadDto>(flowerPost);

            return Ok(new ResponseObject()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Get flower post successfully",
                Data = flowerPostRead
            });
        }
        [Authorize]
        [HttpPut("UpdateFlowerPost/{id}")]
        public async Task<IActionResult> PutFlowerPost(int id, [FromBody] FlowerPostRequestDto flowerPostRequest)
        {

            var existingPost = await _context.FlowerPosts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound(new ResponseObject()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Cannot find the Post with id = " + id,
                    Data = ""
                });
            }

            // Update fields

            existingPost.Name = flowerPostRequest.Name;
            existingPost.Description = flowerPostRequest.Description;
            existingPost.BloomSeason = flowerPostRequest.BloomSeason;
            existingPost.ImageUrl = flowerPostRequest.ImageUrl;
            existingPost.UserId = flowerPostRequest.UserId;

            await _context.SaveChangesAsync();

            return Ok(new ResponseObject()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Update flower post successfully",
                Data = ""
            });
        }
        [Authorize]
        [HttpDelete("DeleteFlowerPost/{id}")]
        public async Task<IActionResult> DeleteFlowerPost(int id)
        {
            var flowerPost = await _context.FlowerPosts.FindAsync(id);
            if (flowerPost == null)
            {
                return NotFound(new ResponseObject()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Cannot find the Post with id = " + id,
                    Data = ""
                });
            }

            _context.FlowerPosts.Remove(flowerPost);
            await _context.SaveChangesAsync();

            return Ok(new ResponseObject()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Delete post successfully",
                Data = ""
            });
        }

        private bool FlowerPostExists(int id)
        {
            return _context.FlowerPosts.Any(f => f.Id == id);
        }
    }
}
