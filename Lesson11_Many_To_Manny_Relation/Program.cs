﻿
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

Console.WriteLine("Hello, World!");





//İki entity arasındaki ilişkiyi navigation propertyler üzerinden çoğul olarak kurmalıyız. (ICollection, List)
//Default Convention'da cross table'ı manuel oluşturmak zorunda değiliz. EF Core tasarıma uygun bir şekilde cross table'ı kendisi otomatik basacak ve generate edecektir.
//Ve oluşturulan cross table'ın içerisinde composite primary key'i de otomatik oluşturmuş olacaktır.


#region Default Convention

//class Kitap
//{
//    public int Id { get; set; }
//    public string KitapAdi { get; set; }

//    public List<Yazar> Yazarlar { get; set; }
//}
//class Yazar
//{
//    public int Id { get; set; }
//    public string YazarAdi { get; set; }

//    public List<Kitap> Kitaplar { get; set; }
//}
#endregion

//Cross table manuel olarak oluşturulmak zorundadır.
//Entity'lerde oluşturduğumuz cross table entity si ile bire çok bir ilişki kurulmalı.
//CT'da composite primary key'i data annotation(Attributes)lar ile manuel kuramıyoruz. Bunun için de Fluent API'da çalışma yaopmamız gerekiyor.
//Cross table'a karşılık bir entity modeli oluşturuyorsak eğer bunu context sınıfı içerisinde DbSet property'si olarka bildirmek mecburiyetinde değiliz!
#region Data Annotations

//class Kitap
//{
//    public int Id { get; set; }
//    public string KitapAdi { get; set; }

//    public ICollection<KitapYazar> Yazarlar { get; set; }
//}
////Cross Table
//class KitapYazar
//{
//    [ForeignKey(nameof(Kitap))]
//    public int KId { get; set; }

//    [ForeignKey(nameof(Yazar))]
//    public int YId { get; set; }

//    public Kitap Kitap { get; set; }
//    public Yazar Yazar { get; set; }
//}
//class Yazar
//{
//    public int Id { get; set; }
//    public string YazarAdi { get; set; }

//    public ICollection<KitapYazar> Kitaplar { get; set; }
//}
#endregion
//Cross table manuel oluşturulmalı
//DbSet olarak eklenmesine lüzum yok, 
//Composite PK Haskey metodu ile kurulmalı!
#region Fluent API

class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }

    public ICollection<KitapYazar> Yazarlar { get; set; }
}
//Cross Table
class KitapYazar
{
    public int KitapId { get; set; }
    public int YazarId { get; set; }

    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }
}
class Yazar
{
    public int Id { get; set; }
    public string YazarAdi { get; set; }

    public ICollection<KitapYazar> Kitaplar { get; set; }
}
#endregion

class EKitapDbContext : DbContext
{
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=ManyToMany;User ID=SA;Password=Password123;TrustServerCertificate=True");

    }
    //Data Annotations
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<KitapYazar>()
    //        .HasKey(ky => new { ky.KId, ky.YId });
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitapYazar>()
            .HasKey(ky => new { ky.KitapId, ky.YazarId });

        modelBuilder.Entity<KitapYazar>()
            .HasOne(ky => ky.Kitap)
            .WithMany(k => k.Yazarlar)
            .HasForeignKey(ky => ky.KitapId);

        modelBuilder.Entity<KitapYazar>()
            .HasOne(ky => ky.Yazar)
            .WithMany(y => y.Kitaplar)
            .HasForeignKey(ky => ky.YazarId);
    }
}

