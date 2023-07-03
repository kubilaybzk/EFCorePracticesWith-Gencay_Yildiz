using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

internal class Program
{

    public class Urun
    {
        public int Id { get; set; }
        public string UrunAdi { get; set; }
        public float Fiyat { get; set; }
    }

    public class ApplicationDBContext : DbContext
    {

        public DbSet<Urun> Urunler { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=EFCoreDeletion;User ID=SA;Password=Password123;TrustServerCertificate=True");

        }



    }
    private static void Main(string[] args)
    {
        Random rand = new Random();
        ApplicationDBContext context = new();


        #region Veri tabanında bir kaç adet satır olsun diye fake data oluşturalım.
        //Urun addedProduct1 = new() { UrunAdi = "Urun1", Fiyat = rand.NextInt64() };
        //Urun addedProduct2 = new() { UrunAdi = "Urun2", Fiyat = rand.NextInt64() };
        //Urun addedProduct3 = new() { UrunAdi = "Urun3", Fiyat = rand.NextInt64() };
        //Urun addedProduct4 = new() { UrunAdi = "Urun4", Fiyat = rand.NextInt64() };
        //context.Urunler.AddRange(addedProduct1, addedProduct2, addedProduct3, addedProduct4);
        //context.SaveChanges();
        #endregion

        #region Veri Nasıl Silinir?
        /*
          Öncelikle amacımız silinecek veriye ulaşmak.
            Veri update işleminde yaptığımız gibi veriye erişmek için sorgusal işlemlerden yararlanabiliriz.
                Silinecek olan veri için bir değişkene yada bir obje olacak şekilde verimizi assign etmemiz lazım.

         */
        #endregion


        Urun urun = context.Urunler.FirstOrDefault(u => u.Id == 2);
        //Burada basit koşul ifadeleri oluşturabiliriz.
        if (urun != null)
        {
            context.Urunler.Remove(urun);
        }
        else
        {
            Console.WriteLine("Hata Mesajımız var !! ");
        }
        context.SaveChanges();

        #region Takip Edilmeyen Nesneler Nasıl Silinir? 
        //Takip edilemeyen nesneler derken üretilen yeni nesnelere göre silme işleminden bahsediyoruz.
        //Bunu bir önceki derste gördük .
        //Hiçbir farkı yok Update yerime Remove kullanılıyor
        #endregion

        Urun u = new()
        {
            Id = 3
        };
        context.Urunler.Remove(u);
        context.SaveChanges();

        #region EntityState İle Silme İşlemi

        #endregion
        //Urun u2 = new() { Id = 1 };
        //context.Entry(u2).State = EntityState.Deleted;
        //context.SaveChanges();


        #region RemoveRange kullanarak birden fazla ürün silme.

        #endregion
        //List<Urun> urunler =  context.Urunler.Where(u => u.Id >= 4 && u.Id <= 6).ToList();
        //context.Urunler.RemoveRange(urunler);
        //context.SaveChanges();
    }


}