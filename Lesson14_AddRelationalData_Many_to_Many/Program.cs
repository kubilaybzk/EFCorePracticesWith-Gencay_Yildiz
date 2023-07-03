using Microsoft.EntityFrameworkCore;


Console.WriteLine("Hello, World!");
ApplicationDbContext context = new();

#region Many to Many İlişkisel Senaryolarda Veri Ekleme


#region 1. Yöntem
//n t n ilişkisi eğer ki default convention üzerinden tasarlanmışsa kullanılan bir yöntemdir.

Book book = new()
{
    BookName = "A Kitabı",
    Authors = new HashSet<Author>()
    {
        new(){ AuthorName = "Hilmi" },
        new(){ AuthorName = "Ayşe" },
        new(){ AuthorName = "Fatma" },
    }
};

await context.Books.AddAsync(book);
await context.SaveChangesAsync();



class Book
{
    public Book()
    {
        Authors = new HashSet<Author>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }

    public ICollection<Author> Authors { get; set; }
}

class Author
{
    public Author()
    {
        Books = new HashSet<Book>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public ICollection<Book> Books { get; set; }
}
#endregion

#region 2. Yöntem
//n t n ilişkisi eğer ki fluent api ile tasarlanmışsa kullanılan bir yöntemdir.

//Author author = new()
//{
//AuthorName = "Mustafa",
//Books = new HashSet<AuthorBook>() {
//        new(){ BookId = 1},
//        new(){ Book = new () { BookName = "B Kitap" } }
//    }
//};

//await context.AddAsync(author);
//await context.SaveChangesAsync();

//class Book
//{
//    public Book()
//    {
//        Authors = new HashSet<AuthorBook>();
//    }
//    public int Id { get; set; }
//    public string BookName { get; set; }

//    public ICollection<AuthorBook> Authors { get; set; }
//}

//class AuthorBook
//{
//    public int BookId { get; set; }
//    public int AuthorId { get; set; }
//    public Book Book { get; set; }
//    public Author Author { get; set; }
//}

//class Author
//{
//    public Author()
//    {
//        Books = new HashSet<AuthorBook>();
//    }
//    public int Id { get; set; }
//    public string AuthorName { get; set; }

//    public ICollection<AuthorBook> Books { get; set; }
//}
#endregion



class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=ManyToManyAddData;User ID=SA;Password=Password123;TrustServerCertificate=True");

    }
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<AuthorBook>()
    //        .HasKey(ba => new { ba.AuthorId, ba.BookId });

    //    modelBuilder.Entity<AuthorBook>()
    //        .HasOne(ba => ba.Book)
    //        .WithMany(b => b.Authors)
    //        .HasForeignKey(ba => ba.BookId);

    //    modelBuilder.Entity<AuthorBook>()
    //        .HasOne(ba => ba.Author)
    //        .WithMany(b => b.Books)
    //        .HasForeignKey(ba => ba.AuthorId);
    //}
}
#endregion