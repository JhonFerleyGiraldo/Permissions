using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("EmployeeForename")]
        [StringLength(100)]
        public string EmployeeForename { get; set; }

        [Required]
        [Column("EmployeeSurname")]
        [StringLength(100)]
        public string EmployeeSurname { get; set; }

        [Required]
        [Column("PermissionType")]
        public int PermissionTypeId { get; set; }

        [Required]
        [Column("PermissionDate")]
        [DataType(DataType.Date)]
        public DateTime PermissionDate { get; set; }

        [ForeignKey("PermissionTypeId")]
        public PermissionType PermissionType { get; set; }
    }
}
