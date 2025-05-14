using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PermissionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
