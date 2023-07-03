using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda Veri Güncelleme

#region Bir kaç adet Fake data ekleyelim.

//Person person = new()
//{
//    Name = "Kubilay Bozak",
//    Address = new()
//    {
//        PersonAddress = "Yenimahalle/ANKARA"
//    }
//};

//Person person2 = new()
//{
//    Name = "Mauro Icardi"
//};

//await context.AddAsync(person);
//await context.AddAsync(person2);
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Esas tablodaki veriye bağımlı veriyi değiştirme

//Esas tablolarda veri değiştirirken mevcut olan verinin öncelikle join olarak ele alınması gerekir çünkü normal şartlarda biz değerleri ele alırken ,
//ilişkili tablolarda her bir değer bir obje olarak ele alıyoruz burada tek bir obje olarak ele almamız gerekiyor tablolarda olan değerleri .



//Burada soru işareti ekleme sebebimiz bu değerler null gelebilir bunu bildiriyoruz aslında bu bir hata değil vscode bunu size göstermek amacıyla uyarı olduğu için altını çiziyor.

//Person? editedPerson = await context.Persons.Include(p => p.Address).FirstOrDefaultAsync(p => p.Id == 1);

#region Yöntem 1 Normal olarak adres değiştirme işlemlerinde direkt olarak verileri değiştirebiliriz

//editedPerson.Address.PersonAddress = "İlişkiyi ortadan  kaldırmadan düzenledik";

//await context.SaveChangesAsync();

#endregion

#region Yöntem2 İlişkisel olarak bağlantıyı tamamen kaldırıp veri güncelleme işlemi.

//şimdi diyelim ki bir şey oldu tablo değişti tabloya bir alan eklendi falan yada verileri değiştirmek yerine verileri tamamen kaldırıp yeniden eklemek istedik diyelim
//Öncelikle ilk yapmamız gereken verileri tamamen ortadan kaldırmak daha sonra yeni address değerinimizi bu şeçilen person nesnesine assign etmek.

//context.Remove(editedPerson.Address);
//await context.SaveChangesAsync();


//editedPerson.Address = new Address()
//{
//    PersonAddress = "Yeni eklenen adres"
//};

//await context.SaveChangesAsync();



#endregion


//Person? person = await context.Persons
//    .Include(p => p.Address)
//    .FirstOrDefaultAsync(p => p.Id == 1);

//context.Addresses.Remove(person.Address);
//person.Address = new()
//{
//    PersonAddress = "Yeni adres"
//};

//await context.SaveChangesAsync();
#endregion

#region 2. Durum | Bağımlı verinin ilişkisel olduğu ana veriyi güncelleme


#region Düz mantık ile düzeltmeyi deneyelim  Hatalı Yöntem.
//Burada adres içinde bulunan id değerleri bir key'e karşılık geliyor bundan dolayı düz mantık ile malesef silmeyi yapamıyoruz.
//Burada ilk olarak atamalar key value değerlerine göre aktarıldığı için otomatik olarak değiştirme  işlemi yapamıyoruz.


//Address? address = await context.Addresses.FindAsync(1);
//address.Id = 2;
//await context.SaveChangesAsync();

#endregion


#region EFCore tarafından önerilen yöntem


//öncelikle Adress bilgilerini inmemory yaparak elimizde tutalım ve daha sonra veri tabanından kaldıralım.

//Address? address = await context.Addresses.FindAsync(3);
//context.Addresses.Remove(address);
//await context.SaveChangesAsync();

//Daha sonra inmemort olan adres değerimizi biz bir değere atayalım.

//Person? person = await context.Persons.FindAsync(2);
//address.Person = person;
//await context.Addresses.AddAsync(address);
//await context.SaveChangesAsync();
#endregion



#region Adres bilgilerini veri tabanında daha önce hiç olmayan bir kullanıcıya'da atayabiliriz.

//Şimdi burada şöyle bir yöntem deneyeceğiz, adres bilgilerimize göre bir kullanıcı eklemek gereksinimimiz olsun.

//Address? address = await context.Addresses.FindAsync(2);
//context.Addresses.Remove(address);
//await context.SaveChangesAsync();

//address.Person = new Person()
//{
//    Name = "Yeni eklenen kullanıcıy Adı."
//};

//await context.Addresses.AddAsync(address);
//await context.SaveChangesAsync();

#endregion

#endregion

#endregion







#region One to Many İlişkisel Senaryolarda Veri Güncelleme
#region Saving
//Blog blog = new()
//{
//    Name = "Kubilaubzk.dev Blog",
//    Posts = new List<Post>
//    {
//        new(){ Title = "1. Post" },
//        new(){ Title = "2. Post" },
//        new(){ Title = "3. Post" },
//    }
//};

//await context.Blogs.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Veri silmeden bağımlı değişkeni düzenleme Güncelleme İşlemi 


//Post? post = await context.Posts.FindAsync(3);

//post.Title = "Veri silmeden Güncelleme işlemi";

//await context.SaveChangesAsync();


#endregion

#region 2. Durum | Esas tablodaki veriye bağımlı verileri değiştirme

//Öncelikle veriyi silip veri ekleme işlemlerining gerçekleştiği durum.

//Blog? blog2 = await context.Blogs
//    .Include(b => b.Posts)
//    .FirstOrDefaultAsync(b => b.Id == 1);

//blog2.Posts.Add(new Post() { Title = "Selam" });
//context.SaveChanges();

//Post? silinecekPost = blog2.Posts.FirstOrDefault(p => p.Id == 5);
//silinecekPost.Title = "Delete edilmeden önce";


//blog2.Posts.Remove(silinecekPost);

//blog2.Posts.Add(new() { Title = "4. Post" });


await context.SaveChangesAsync();

#endregion


#region 3. Durum | Bağımlı verilerin ilişkisel olduğu ana veriyi güncelleme

#region 3.1 |  Bağımlı veriye yeni bir ana veri ekleme

//Post? post = await context.Posts.FindAsync(4);
//post.Blog = new()
//{
//    Name = "2. Blog"
//};
//await context.SaveChangesAsync();

#endregion

#region 3.2 | Bağımlı veri ile daha önce oluşturulan verinin ilişkilendirilmesi


//Post? post = await context.Posts.FindAsync(3);
//Blog? blog = await context.Blogs.FindAsync(2);
//post.Blog = blog;

//await context.SaveChangesAsync();

#endregion
#endregion

#endregion












#region Many to Many İlişkisel Senaryolarda Veri Güncelleme
#region Saving
//Book book1 = new() { BookName = "1. Kitap" };
//Book book2 = new() { BookName = "2. Kitap" };
//Book book3 = new() { BookName = "3. Kitap" };

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

#region 1. Örnek Hiç Data yoksa ekleme işlemi .
// 1. Kitap burada 3. Yazar ile ilişkili hale getirelim


//Book? book = await context.Books.FindAsync(1);
//Author? author = await context.Authors.FindAsync(3);
//book.Authors.Add(author);
//await context.SaveChangesAsync();

#endregion

#region 2. Örnek 3 id sine sahip olan yazarın sadece 1 id değerine sahip olan kitap ile ilişkisi olsun sadece.


//Author? author = await context.Authors
//    .Include(a => a.Books)
//    .FirstOrDefaultAsync(a => a.Id == 3);

//foreach (var book in author.Books)
//{
//    if (book.Id != 1)
//        author.Books.Remove(book);
//}

//await context.SaveChangesAsync();
#endregion

#region 3. Örnek

//kitaplar arasında id değeri 2 olan kitabın 1. yazar ile bağlantısı kesilsin.

Book? book = await context.Books
    .Include(b => b.Authors)
    .FirstOrDefaultAsync(b => b.Id == 2);

Author silinecekYazar = book.Authors.FirstOrDefault(a => a.Id == 1);
book.Authors.Remove(silinecekYazar);

//Author author = await context.Authors.FindAsync(3);
//book.Authors.Add(author);
//book.Authors.Add(new() { AuthorName = "4. Yazar" });

await context.SaveChangesAsync();
#endregion

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
    public int BlogId { get; set; }
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
        optionsBuilder.UseSqlServer("Data Source=192.168.1.106;Initial Catalog=UpdateRelatedDatas;User ID=SA;Password=Password123;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
    }
}

