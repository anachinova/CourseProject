using CourseProject.Data;
using CourseProject.Domain.Models;
using CourseProject.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AdvertisementManager>();

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var categories = new[]
    {
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
    };

    foreach (var category in categories)
    {
        var existingCategory = context.Categories.FirstOrDefault(c => c.Id == category.Id);

        if (existingCategory == null)
        {
            context.Categories.Add(category);
        }
        else
        {
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;
        }
    }


    if (!context.Admins.Any(a => a.Username == "admin" || a.Email == "admin@easymarket.local"))
    {
        context.Admins.Add(new Admin(
            0,
            "admin",
            "admin123",
            "admin@easymarket.local",
            "Адміністратор",
            "+380000000000"));
    }


    var demoUsers = new[]
    {
        new RegisteredUser(0, "olena", "olena123", "olena@example.com", "Олена", "+380501112233"),
        new RegisteredUser(0, "andrii", "andrii123", "andrii@example.com", "Андрій", "+380671112233"),
        new RegisteredUser(0, "maria", "maria123", "maria@example.com", "Марія", "+380931112233")
    };

    foreach (var demoUser in demoUsers)
    {
        if (!context.RegisteredUsers.Any(u => u.Username == demoUser.Username || u.Email == demoUser.Email))
        {
            context.RegisteredUsers.Add(demoUser);
        }
    }

    context.SaveChanges();

    var olena = context.RegisteredUsers.First(u => u.Username == "olena");
    var andrii = context.RegisteredUsers.First(u => u.Username == "andrii");
    var maria = context.RegisteredUsers.First(u => u.Username == "maria");

    var demoAdvertisements = new[]
    {
        new
        {
            Title = "iPhone 13 128 GB у гарному стані",
            Description = "Телефон працює швидко, батарея тримає добре. Є дрібні сліди користування, комплект із зарядним кабелем.",
            Price = 18500m,
            City = "Харків",
            CategoryId = 3,
            Author = olena,
            ImagePath = "/images/ad-phone.jpg",
            DaysAgo = 1
        },
        new
        {
            Title = "Ноутбук Lenovo IdeaPad для навчання",
            Description = "Ноутбук підходить для навчання, роботи з документами, браузера та онлайн-зустрічей. Стан охайний.",
            Price = 14900m,
            City = "Київ",
            CategoryId = 3,
            Author = andrii,
            ImagePath = "/images/ad-laptop.jpg",
            DaysAgo = 2
        },
        new
        {
            Title = "Volkswagen Golf 2012",
            Description = "Автомобіль у хорошому стані, економний двигун, салон чистий. Документи в порядку.",
            Price = 265000m,
            City = "Львів",
            CategoryId = 1,
            Author = andrii,
            ImagePath = "/images/ad-car.jpg",
            DaysAgo = 3
        },
        new
        {
            Title = "Велосипед міський 28 коліс",
            Description = "Зручний велосипед для міста та прогулянок. Передачі перемикаються, гальма працюють.",
            Price = 7200m,
            City = "Полтава",
            CategoryId = 9,
            Author = maria,
            ImagePath = "/images/ad-bike.jpg",
            DaysAgo = 4
        },
        new
        {
            Title = "Оренда 1-кімнатної квартири",
            Description = "Затишна квартира біля зупинки транспорту. Є меблі, пральна машина, холодильник та інтернет.",
            Price = 9500m,
            City = "Дніпро",
            CategoryId = 2,
            Author = olena,
            ImagePath = "/images/ad-apartment.jpg",
            DaysAgo = 5
        },
        new
        {
            Title = "Диван розкладний для вітальні",
            Description = "М'який розкладний диван, стан хороший. Підійде для квартири або дачі.",
            Price = 6800m,
            City = "Одеса",
            CategoryId = 7,
            Author = maria,
            ImagePath = "/images/ad-sofa.jpg",
            DaysAgo = 6
        },
        new
        {
            Title = "Куртка демісезонна чоловіча",
            Description = "Куртка у хорошому стані, розмір L. Колір темно-синій, без пошкоджень.",
            Price = 1200m,
            City = "Харків",
            CategoryId = 6,
            Author = andrii,
            ImagePath = "/images/ad-phone.jpg",
            DaysAgo = 7
        },
        new
        {
            Title = "Репетитор з математики",
            Description = "Підготовка до контрольних, НМТ та допомога з домашніми завданнями. Заняття онлайн або офлайн.",
            Price = 350m,
            City = "Київ",
            CategoryId = 5,
            Author = olena,
            ImagePath = "/images/ad-laptop.jpg",
            DaysAgo = 8
        },
        new
        {
            Title = "Вакансія продавця-консультанта",
            Description = "Потрібен продавець у магазин техніки. Графік позмінний, дружній колектив, навчання на місці.",
            Price = 18000m,
            City = "Львів",
            CategoryId = 4,
            Author = maria,
            ImagePath = "/images/ad-sofa.jpg",
            DaysAgo = 9
        },
        new
        {
            Title = "Кошеня шукає дім",
            Description = "Добре кошеня, привчене до лотка. Віддамо у відповідальні руки.",
            Price = 0m,
            City = "Запоріжжя",
            CategoryId = 8,
            Author = olena,
            ImagePath = "/images/ad-bike.jpg",
            DaysAgo = 10
        },
        new
        {
            Title = "Дитячий візочок 2 в 1",
            Description = "Візочок після однієї дитини, чистий, зручний, у комплекті люлька та прогулянковий блок.",
            Price = 4300m,
            City = "Черкаси",
            CategoryId = 10,
            Author = maria,
            ImagePath = "/images/ad-apartment.jpg",
            DaysAgo = 11
        },
        new
        {
            Title = "Настільна лампа для робочого столу",
            Description = "Лампа з регулюванням нахилу, підходить для навчання, роботи та читання.",
            Price = 650m,
            City = "Суми",
            CategoryId = 11,
            Author = andrii,
            ImagePath = "/images/ad-phone.jpg",
            DaysAgo = 12
        }
    };

    foreach (var demoAd in demoAdvertisements)
    {
        if (!context.Advertisements.Any(a => a.Title == demoAd.Title))
        {
            var category = context.Categories.First(c => c.Id == demoAd.CategoryId);

            var advertisement = new Advertisement(
                0,
                demoAd.Title,
                demoAd.Description,
                demoAd.Price,
                demoAd.City,
                category,
                demoAd.Author);

            advertisement.CategoryId = category.Id;
            advertisement.AuthorId = demoAd.Author.Id;
            advertisement.CreatedAt = DateTime.Now.AddDays(-demoAd.DaysAgo);

            advertisement.Images.Add(new AdvertisementImage
            {
                Path = demoAd.ImagePath,
                Advertisement = advertisement
            });

            context.Advertisements.Add(advertisement);
        }
    }


    context.SaveChanges();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();