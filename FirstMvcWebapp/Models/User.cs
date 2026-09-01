using System.ComponentModel.DataAnnotations;

namespace FirstMvcWebapp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
