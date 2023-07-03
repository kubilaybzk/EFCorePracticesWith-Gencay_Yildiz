using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

Console.WriteLine("fsfasfasfa");
ApplicationDbContext context = new();





/////////////////////////////////////////////////   One to One İlişkisel Senaryolarda Veri Ekleme    ////////////////////////////////////////////////////////////

#region One to One İlişkisel Senaryolarda Veri Ekleme
#region 1. Yöntem -> Principal Entity Üzerinden Dependent Entity Verisi Ekleme
Person person = new();
person.Name = "Kubilay";
person.Address = new() { PersonAddress = "Rize/Hemşin" };

await context.AddAsync(person);
await context.SaveChangesAsync();
#endregion

//Eğer ki principal entity üzerinden ekleme gerçekleştiriliyorsa dependent entity nesnesi verilmek zorunda değildir! Amma velakin, dependent entity üzerinden ekleme işlemi gerçekleştiriliyorsa eğer burada principal entitynin nesnesine ihtiyacımız zaruridir.

#region 2. Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme
Address address = new()
{
    PersonAddress = "Papaz Deresi/Ankara",
    Person = new() { Name = "Şuayip" }
};

await context.AddAsync(address);
await context.SaveChangesAsync();
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
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=OneToOneAddData;User ID=SA;Password=Password123;TrustServerCertificate=True");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
    }
}
#endregion





////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


