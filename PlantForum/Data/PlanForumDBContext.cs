using Microsoft.EntityFrameworkCore;
using PlantForum.Data.Models;

namespace PlantForum.Data
{
    public class PlanForumDBContext : DbContext
    {
        public PlanForumDBContext(DbContextOptions<PlanForumDBContext> option) : base(option){ }
        public DbSet<User> Users { get; set; }
        public DbSet<FlowerPost> FlowerPosts { get; set; }  
    }
}
