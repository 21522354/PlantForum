using System.ComponentModel.DataAnnotations;

namespace PlantForum.Data.Models
{
    public class User
    {
        [Key]
        public Guid id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
