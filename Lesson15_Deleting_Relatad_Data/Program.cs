using Microsoft.EntityFrameworkCore;

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda Veri Silme

//Burada id değeri 1 olan bir person değerinin adres bilgisini silmek isteyelim.
//Person değeri kalacak sadece adress değeri yok olacak.

//Person? person = await context.Persons
//    .Include(p => p.Address)
//    .FirstOrDefaultAsync(p => p.Id == 1);  
//Burada Iqueryable olacağı için tipimiz FirstOrDefault kullanabiliriz , Find kullanamayız. 
//Burada bulunan sorguda hem adres hem kişilerin bilgileri elimize ulaşıyor.

//if (person != null)
//    context.Addresses.Remove(person.Address);
//await context.SaveChangesAsync();


#endregion

#region One to Many İlişkisel Senaryolarda Veri Silme
// Birebir silme işlemleri ile neredeyse tamamen aynı mantık tek sıkıntımız birden fazla dependent verimiz olabilir.

//Blog? blog = await context.Blogs
//    .Include(b => b.Posts)
//    .FirstOrDefaultAsync(b => b.Id == 1);

// Şimdi bir çok bir ilişki istiyoruz ve sadece tek bir değeri silmek istiyoruz biz bu silmek istediğimiz değere ulaşabilelim ki bunu silelim.

//Post? post = blog.Posts.FirstOrDefault(p => p.Id == 2);


//context.Posts.Remove(post);
//await context.SaveChangesAsync();
#endregion

#region Many to Many İlişkisel Senaryolarda Veri Silme

//Herhangi bir kitapa karşılık yazarları silmeye çalışırsan o yazarların başka kitapları olabilir burada bundan dolayı cross table üzerinden işlem yapmamız gerekir.

//Book? book = await context.Books
//    .Include(b => b.Authors)
//    .FirstOrDefaultAsync(b => b.Id == 2);

//Author? author = book.Authors.FirstOrDefault(a => a.Id == 2);
///*context.Authors.Remove(author);*/ //Yazarı silmeye kalkar!!!
//book.Authors.Remove(author);
//await context.SaveChangesAsync();

#endregion

#region Cascade Delete
//Bu davranış modelleri Fluent API ile konfigüre edilebilmektedir.

#region Cascade
//Principle tablodan silinen veriyle dependent tabloda bulunan ilişkili verilerin silinmesini sağlar.

//Blog? blog = await context.Blogs.FindAsync(1);
//context.Blogs.Remove(blog);
//await context.SaveChangesAsync();

#endregion

#region SetNull
//Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilere null değerin atanmasını sağlar.

//One to One senaryolarda eğer ki Foreign key ve Primary key kolonları aynı ise o zaman SetNull davranuışını KULLANAMAYIZ!



#endregion

#region Restrict
//Esas tablodan herhangi bir veri silinmeye çalışıldığında o veriye karşılık dependent table'da ilişkisel veri/ler varsa eğer bu sefer bu silme işlemini engellemesini sağlar.
#endregion

//Esas tablodan herhangi bir veri silinmeye çalışıldığında o veriye karşıluık dependen table'da ilişkisel veri yada veriler varsa eğer bu silme işlemini engellenmesini saplar.

Blog? blog = await context.Blogs.FindAsync(1);
context.Blogs.Remove(blog);
await context.SaveChangesAsync();
#endregion

#region Saving Data
//Person person = new()
//{
//    Name = "Kubilay",
//    Address = new()
//    {
//        PersonAddress = "Yenimahalle/ANKARA"
//    }
//};

//Person person2 = new()
//{
//    Name = "Çağrı"
//};

//await context.AddAsync(person);
//await context.AddAsync(person2);

//Blog blog = new()
//{
//    Name = "Kubilaybzk.dev Blog",
//    Posts = new List<Post>
//    {
//        new(){ Title = "1. Post" },
//        new(){ Title = "2. Post" },
//        new(){ Title = "3. Post" },
//    }
//};

//await context.Blogs.AddAsync(blog);
//await context.SaveChangesAsync();

//Book book1 = new() { BookName = "1. Kitap" };
//Book book2 = new() { BookName = "2. Kitap" };
//Book book3 = new() { BookName = "3. Kitap" };

//await context.SaveChangesAsync();

//Author author1 = new() { AuthorName = "1. Yazar" };
//Author author2 = new() { AuthorName = "2. Yazar" };
//Author author3 = new() { AuthorName = "3. Yazar" };

//book1.Authors.Add(author1);
//book1.Authors.Add(author2);

//book2.Authors.Add(author1);
//book2.Authors.Add(author2);
//book2.Authors.Add(author3);

//book3.Authors.Add(author3);

//await context.AddAsync(book1);
//await context.AddAsync(book2);
//await context.AddAsync(book3);
//await context.SaveChangesAsync();
#endregion

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Address Address { get; set; }
}
class Address
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }

    public Person Person { get; set; }
}
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
    public int? BlogId { get; set; }
    public string Title { get; set; }

    public Blog Blog { get; set; }
}
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
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=192.168.1.108;Initial Catalog=DeleteRelatedDatas;User ID=SA;Password=Password123;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Silme işlemlerinim burada modellediğimizden bahsetmiştik burada delete işlemleri için OnDelete adında bir fonksiyonumuz bulunuyor bunun üzerinden bu düzenlemeleri yapabiliriz
        //Yapılan her bir ekleme bir migration elde etmemizi sağlar.

        // .OnDelete(DeleteBehavior.Cascade); =>Default olarak ayarlanan silme işlemeleri.



        // .OnDelete(DeleteBehavior.SetNull); =>Default olarak silinen ana veriye bağlı olan verilerin key değerlerini null yaptığımız alan. PrimaryKey
        //şimdi burada şöyle bir hata almamız söz konusu Address tablomuzda id değerimiz için null olabilir gibisinden bir düzenleme yapmış olmamız lazım
        //Biz bunu ayarlamamıştık düz mantık devam etmiştik bunun için 





        // .OnDelete(DeleteBehavior.Cascade); =>Default olarak ayarlanan silme işlemeleri.

        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

    }
}