using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace APP.Users.Domain;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(100)]
    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Surname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }

    public int RoleId { get; set; }


    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();


    [NotMapped]
    public List<int> SkillIds
    {
        get => UserSkills?.Select(us => us.SkillId).ToList();
        set => UserSkills = value?.Select(v => new UserSkill() { SkillId = v }).ToList();
    }
}
