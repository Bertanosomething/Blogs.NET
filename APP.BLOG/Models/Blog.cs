using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APP.BLOG.Models;

[Table("Blog")]
public partial class Blog
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Rating { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PublishDate { get; set; }

    public int UserId { get; set; }

    [InverseProperty("Blog")]
    public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();

    [ForeignKey("UserId")]
    [InverseProperty("Blogs")]
    public virtual User User { get; set; } = null!;
}
