namespace PlantForum.Dtos
{
    public class FlowerPostRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BloomSeason { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // Thông tin người dùng
        public Guid UserId { get; set; }
    }
}
