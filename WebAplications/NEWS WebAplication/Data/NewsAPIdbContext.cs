using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Evaluation;

namespace NEWS_WebAplication.Data
{  
    //public class NewsAPIdbContext: DbContext
    //{
    //    public DbSet<User> Users { get; set; }
    //    public DbSet<Forgot> Forgots { get; set; }

    //    public DbSet<Role> Roles { get; set; }
    //    public DbSet<UserRole> UserRoles { get; set; }

    //    public NewsAPIdbContext(DbContextOptions<NewsAPIdbContext> options): base(options)
    //    {
            
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //        modelBuilder.Entity<User>(entity =>
    //        {
    //            entity.ToTable("Users");
    //            entity.HasKey(a => a.UserId);

    //            entity.Property(a => a.Email).IsRequired().HasMaxLength(48);
    //            entity.Property(a => a.password).IsRequired();
    //            entity.Property(a => a.DisplayName).IsRequired().HasMaxLength(100);

    //            entity.HasMany(a => a.UserRole).WithOne(a => a.user).OnDelete(DeleteBehavior.NoAction);

    //        });

    //        modelBuilder.Entity<Forgot>(entity =>
    //        {

    //            entity.ToTable("PasswordReset");
    //            entity.HasKey(a => a.PasswordResetId);


    //        });


    //        modelBuilder.Entity<Role>(entity =>
    //        {
    //            entity.ToTable("Roles");
    //            entity.HasKey(a => a.RoleId);

    //            entity.Property(a => a.Name).IsRequired().HasMaxLength(50);
                

    //            entity.HasMany(a => a.UserRole).WithOne(a => a.role).OnDelete(DeleteBehavior.NoAction);

    //        });

    //        modelBuilder.Entity<UserRole>(entity =>
    //        {
    //            entity.ToTable("UserRoles");
    //            entity.HasKey(a => new {a.UserId,a.RoleId});

    //            entity.HasOne(a => a.role).WithMany(a => a.UserRole).OnDelete(DeleteBehavior.NoAction);
    //            entity.HasOne(a => a.user).WithMany(a => a.UserRole).OnDelete(DeleteBehavior.NoAction);

    //        });



    //    }
       
    //}

    //public class User
    //{
        
    //    public int UserId { get; set; }
    //    public string Email { get; set; }
    //    public string password { get; set; }
    //    public string DisplayName { get; set; }
    //    public List<UserRole> UserRole { get; set; }
    //}

    //public class Role
    //{
    //    public int RoleId { get; set; }
    //    public string Name { get; set; }

    //    public List<UserRole> UserRole { get; set; }
        
    //}

    //public class UserRole
    //{
    //    public int UserId { get; set; }
    //    public int RoleId { get; set; }

    //    public User user { get; set; }
    //    public Role role { get; set; }
    //}

    //public class Forgot
    //{
        
    //    public int PasswordResetId { get; set; }
    //    public int UserId { get; set; }
    //    public string token { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public DateTime ExpiredAt { get; set; }
    //}
}
