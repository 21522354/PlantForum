using PlantForum.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlantForum.Dtos
{
    public class FlowerPostReadDto
    {
        public int Id { get; set; }     
        public string Name { get; set; }
        public string Description { get; set; }
        public string BloomSeason { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // Thông tin người dùng
        public Guid UserId { get; set; }
    }
}
