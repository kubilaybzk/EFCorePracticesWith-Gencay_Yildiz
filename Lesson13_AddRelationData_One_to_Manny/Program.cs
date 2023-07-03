

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

Console.WriteLine("Selam");

ApplicationDbContext context = new();


#region One to Many İlişkisel Senaryolarda Veri Ekleme
#region 1. Yöntem -> Principal Entity Üzerinden Dependent Entity Verisi Ekleme


#region Nesne Referansı Üzerinden Ekleme

Blog blog = new() { Name = "KubilayBzk.dev Blog" };
blog.Posts.Add(new() { Title = "Post 1" });
blog.Posts.Add(new() { Title = "Post 2" });
blog.Posts.Add(new() { Title = "Post 3" });

await context.AddAsync(blog);
await context.SaveChangesAsync();
#endregion

#region Object Initializer Üzerinden Ekleme

Blog blog2 = new()
{
    Name = "A Blog",
    Posts = new HashSet<Post>() { new() { Title = "Post 4" }, new() { Title = "Post 5" } }
};

await context.AddAsync(blog2);
await context.SaveChangesAsync();
#endregion

#endregion

#region 2. Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme
Post post = new()
{
    Title = "Post 6",
    Blog = new() { Name = "B Blog" }
};

await context.AddAsync(post);
await context.SaveChangesAsync();
#endregion



#region 3. Yöntem -> Foreign Key Kolonu Üzerinden Veri Ekleme

//1. ve 2. yöntemler hiç olmayan verilerin ilişkisel olarak eklenmesini sağlarken,
//bu 3. yöntem önceden eklenmiş olan bir
//pricipal entity verisiyle yeni dependent entitylerin ilişkisel olarak eşleştirilmesini sağlamaktadır.

Post postv3 = new()
{
    BlogId = 1,
    Title = "Post 7"
};

await context.AddAsync(postv3);
await context.SaveChangesAsync();

#endregion


class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>();
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public string Title { get; set; }

    public Blog Blog { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=OneToManyAddData;User ID=SA;Password=Password123;TrustServerCertificate=True");

    }
}

#endregion



