using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APP.BLOG.Models;

[Table("BlogTag")]
public partial class BlogTag
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int TagId { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("BlogTags")]
    public virtual Blog Blog { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("BlogTags")]
    public virtual Tag Tag { get; set; } = null!;
}
