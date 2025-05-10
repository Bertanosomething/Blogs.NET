using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using APP.Users.Domain;

namespace APP.Users.Domain
{
    public partial class UsersDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }

        public UsersDb(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BlogDb;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07B9846CC6");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Skill__3214EC070184B9E7");
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__User__3214EC070DEC3969");

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<UserSkill>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__UserSkil__3214EC07989B36E6");

                entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSkill_Skill");

                entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSkill_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}