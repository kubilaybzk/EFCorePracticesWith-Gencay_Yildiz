using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

internal class Program
{


    public class ApplicationDBContext : DbContext
    {

        public DbSet<Urun> Urunler { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=EFPracticeV2;User ID=SA;Password=Password123;TrustServerCertificate=True");

        }



    }
    public class Urun
    {
        public int Id { get; set; }
        public string UrunAdi { get; set; }
        public float Fiyat { get; set; }
    }


    private static void Main(string[] args)
    {
        var rand = new Random();
        ApplicationDBContext context = new();

        #region Veri tabanında bir kaç adet satır olsun diye fake data oluşturalım.
        //Urun addedProduct1 = new() { UrunAdi = "Urun1", Fiyat = rand.NextInt64() };
        //Urun addedProduct2 = new() { UrunAdi = "Urun2", Fiyat = rand.NextInt64() };
        //Urun addedProduct3 = new() { UrunAdi = "Urun3", Fiyat = rand.NextInt64() };
        //Urun addedProduct4 = new() { UrunAdi = "Urun4", Fiyat = rand.NextInt64() };
        //context.Urunler.AddRange(addedProduct1, addedProduct2, addedProduct3, addedProduct4);
        //context.SaveChanges();
        #endregion


        #region Veri tabanından bir veriyi okuma ve update etme.
        /*
        Öncelikle bir veriyi okumak için her zamanki gibi veri tabanı ile beraber bir bağlantı kurmamız gerekiyor.
            Context ile bu bağlantıyı hallediyoruz.
        Daha sonra veriye göre bir sorgu stili kullanmamız gerekiyor genelde bunu 
            Linq kullanarak yapıyoruz bu şuanda biraz yabancı geliyor tamamen sql mantığı ile bir veriye erişebiliyoruz.
                
         */


        /* Davranışsal olarak verinin güncellenmesi demek daha önceden eklenmiş olan bir verinin üzerinden işlem yapmak demek
           Peki daha önceden eklenen veriye nasıl erişebiliriz. 
               Bunun için sorgu teknikleri kullanabiliriz şimdilik bunlardan bir tane kullanarak işleme başlayalım .
                   FirstOrDefauld kullanarak bu şartı sağlayan ilk elemanı bize getiren bir fonksiyona sahiniz.
        */
        #endregion

        Urun gelen = context.Urunler.FirstOrDefault(u => u.Id == 3);

        Console.WriteLine(gelen.UrunAdi);

        gelen.UrunAdi = "Araba";

        context.SaveChanges();

        #region Veri tabanında bir veriyi güncellerken yine oop nimetlerinden yararlanıyoruz.
        /* 
          Şimdi burada şöyle bir mantık söz konusu ,
            biz gelen filtrelenmiş bu datayı bir obje olarak yada bir variable olarak saklayabilir bir değişkene atayabiliriz.
                Bu sayede burada OOP kurallarına göre güncelleme yapma şansımız oluyor.

         */
        #endregion

        #region ChangeTracker Nedir? 
        //ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu
        //bir mekanizmadır. Bu takip mekanizması sayesinde context üzerinden gelen verilerle
        //ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır!
        #endregion

        #region Update Fonksiyonu (ID gerektiren fonksiyon) !! 
        //ChangeTracker mekanizması tarafından takip edilmeyen nesnelerin güncellenebilmesi için Update fonksiyonu kullanılır!
        //Update fonksiyonunu kullanabilmek için kesinlikle ilgili nesnede Id değeri verilmelidir! Bu değer güncellenecek(update sorgusu oluşturulacak) verinin hangisi olduğunu ifade edecektir.
        //context.Urunler.Update(urun);
        //await context.SaveChangesAsync();
        #endregion

        Urun updateOneProduct = new Urun { Id = 1, Fiyat = 12, UrunAdi = "UpdateFunctions" };
        context.Urunler.Update(updateOneProduct);
        context.SaveChanges();

        #region EntityState Nedir? Bir entity instance'ının durumunu ifade eden bir referanstır.

        #endregion 
        Urun u = new();
        Console.WriteLine(context.Entry(u).State);

        #region EF Core Açısından Bir Verinin Güncellenmesi Gerektiği Nasıl Anlaşılıyor? (Ekstra)
        Urun urun = context.Urunler.FirstOrDefault(u => u.Id == 3);
        Console.WriteLine(context.Entry(urun).State);

        urun.UrunAdi = "Hilmi";

        Console.WriteLine(context.Entry(urun).State);

        context.SaveChanges();

        Console.WriteLine(context.Entry(urun).State);

        urun.Fiyat = 999;

        Console.WriteLine(context.Entry(urun).State);
        #endregion 


    }

}