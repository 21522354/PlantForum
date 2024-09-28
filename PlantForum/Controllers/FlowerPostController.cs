using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantForum.Data;
using PlantForum.Data.Models;
using PlantForum.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlantForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowerPostController : ControllerBase
    {
        private readonly PlanForumDBContext _context;
        private readonly IMapper _mapper;

        public FlowerPostController(PlanForumDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllFlowerPost")]
        public async Task<IActionResult> GetFlowerPosts()
        {
            var flowerPosts = await _context.FlowerPosts.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<FlowerPostReadDto>>(flowerPosts));
        }

        [HttpGet("GetFlowerPostById/{id}")]
        public async Task<IActionResult> GetFlowerPost(int id)
        {
            var flowerPost = await _context.FlowerPosts.FirstOrDefaultAsync(f => f.Id == id);

            if (flowerPost == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlowerPostReadDto>(flowerPost));
        }
        [Authorize]
        [HttpPost("PostFlowerPost")]
        public async Task<IActionResult> PostFlowerPost([FromBody] FlowerPostRequestDto flowerPostRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            flowerPostRequest.DateAdded = DateTime.UtcNow;
            var flowerPost = _mapper.Map<FlowerPost>(flowerPostRequest);
            _context.FlowerPosts.Add(flowerPost);
            await _context.SaveChangesAsync();
            var flowerPostRead = _mapper.Map<FlowerPostReadDto>(flowerPost);

            return Ok(flowerPostRead);  
        }
        [Authorize]
        [HttpPut("UpdateFlowerPost/{id}")]
        public async Task<IActionResult> PutFlowerPost(int id, [FromBody] FlowerPostRequestDto flowerPostRequest)
        {

            var existingPost = await _context.FlowerPosts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            // Update fields

            existingPost.Name = flowerPostRequest.Name;
            existingPost.Description = flowerPostRequest.Description;
            existingPost.BloomSeason = flowerPostRequest.BloomSeason;
            existingPost.ImageUrl = flowerPostRequest.ImageUrl;
            existingPost.UserId = flowerPostRequest.UserId;

            await _context.SaveChangesAsync();  

            return Ok("Update post successfully");
        }
        [Authorize]
        [HttpDelete("DeleteFlowerPost/{id}")]
        public async Task<IActionResult> DeleteFlowerPost(int id)
        {
            var flowerPost = await _context.FlowerPosts.FindAsync(id);
            if (flowerPost == null)
            {
                return NotFound();
            }

            _context.FlowerPosts.Remove(flowerPost);
            await _context.SaveChangesAsync();

            return Ok("Delete post successfully");
        }

        private bool FlowerPostExists(int id)
        {
            return _context.FlowerPosts.Any(f => f.Id == id);
        }
    }
}
