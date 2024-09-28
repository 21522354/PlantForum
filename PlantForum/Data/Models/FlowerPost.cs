using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantForum.Data.Models
{
    public class FlowerPost
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BloomSeason { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // Thông tin người dùng
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
