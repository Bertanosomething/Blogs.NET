using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Domain;


[Table("Skill")]
public partial class Skill
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Skill")]
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
