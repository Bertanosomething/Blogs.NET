using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APP.BLOG.Models;

public partial class ProjectsDb : DbContext
{
    public ProjectsDb()
    {
    }

    public ProjectsDb(DbContextOptions<ProjectsDb> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<BlogTag> BlogTags { get; set; }



    public virtual DbSet<Tag> Tags { get; set; }


//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BlogDb;Trusted_Connection=True;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Blog>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Blog__3214EC073B8EAFE4");

//            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Blog_User");
//        });

//        modelBuilder.Entity<BlogTag>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__BlogTag__3214EC079AE2F13C");

//            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTags)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_BlogTag_Blog");

//            entity.HasOne(d => d.Tag).WithMany(p => p.BlogTags)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_BlogTag_Tag");
//        });

       

//        modelBuilder.Entity<Tag>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC07E91463D2");
//        });


//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
