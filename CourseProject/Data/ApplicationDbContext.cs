using CourseProject.Domain.Enums;
using CourseProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegisteredUser>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admins");

            modelBuilder.Entity<RegisteredUser>()
                .HasMany(u => u.Advertisements)
                .WithOne(a => a.Author)
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Advertisements)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Advertisement>()
                .HasMany(a => a.Images)
                .WithOne(i => i.Advertisement)
                .HasForeignKey(i => i.AdvertisementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Автомобілі", Description = "Продаж та купівля авто" },
                new Category { Id = 2, Name = "Нерухомість", Description = "Квартири, будинки, оренда" },
                new Category { Id = 3, Name = "Електроніка", Description = "Телефони, ноутбуки, техніка" },
                new Category { Id = 4, Name = "Робота", Description = "Вакансії та пошук роботи" },
                new Category { Id = 5, Name = "Послуги", Description = "Різні послуги" },
                new Category { Id = 6, Name = "Одяг", Description = "Чоловічий, жіночий та дитячий одяг" },
                new Category { Id = 7, Name = "Дім і сад", Description = "Товари для дому та саду" },
                new Category { Id = 8, Name = "Тварини", Description = "Домашні тварини та товари для них" },
                new Category { Id = 9, Name = "Спорт", Description = "Спортивні товари" },
                new Category { Id = 10, Name = "Дитячі товари", Description = "Товари для дітей" },
                new Category { Id = 11, Name = "Інше", Description = "Інші оголошення" }
            );
        }
    }
}