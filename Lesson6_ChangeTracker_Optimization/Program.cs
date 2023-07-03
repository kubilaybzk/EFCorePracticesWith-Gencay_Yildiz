// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

Console.WriteLine("Hello, World!");


ETicaretContext context = new();

#region Change Tracking Neydi?
//Context nesnesi üzerinden gelen tüm nesneler/veriler otomatik olarak bir
//takip mekanizması tarafından izlenirler. İşte bu takip mekanizmasına Change Tracker denir.
//Change Traker ile nesneler üzerindeki değişiklikler/işlemler takip edilerek netice itibariyle
//bu işlemlerin fıtratına uygun sql sorgucukları generate edilir. İşte bu işleme de Change Tracking denir. 
#endregion


#region ChangeTracker Propertysi
//Takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekşetirmemizi sağlayan bir propertydir.
//Context sınıfının base class'ı olan DbContext sınıfının bir member'ıdır.

var urunlerChangeTracker = await context.Urunler.ToListAsync();

var datasChangeTracker = context.ChangeTracker.Entries();  //Takip edilen tüm nesneleri entityEntry tipinde getirecek.

Console.WriteLine("");

//urunler[6].Fiyat = 123; //Update
//context.Urunler.Remove(urunler[7]); //Delete
//urunler[8].UrunAdi = "asdasd"; //Update


//var datas = context.ChangeTracker.Entries();

//await context.SaveChangesAsync();
//Console.WriteLine();
#endregion


#region DetectChanges Metodu güncel tracking verilerinden emin olabilmek için değişişiklerin algıulanmasını opsiyonel olarak gerçekleştirmek 
//EF Core, context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri Change Tracker sayesinde takip edebilmekte ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntüleri(snapshot)'ini oluşturabilir.
//Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir. SaveChanges fonksiyonu çağrıldığı anda nesneler EF Core tarafından otomatik kontrol edilirler.
//Ancak, yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişişiklerin algıulanmasını opsiyonel olarak gerçekleştirmek isteyebiliriz. İşte bunun için DetectChanges fonksiyonu kullanılabilir ve her ne kadar EF Core değişikleri otomatik algılıyor olsa da siz yine de iradenizle kontrole zorlayabilirsiniz.

//var urunDetectChanges = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3);
//urunDetectChanges.Fiyat = 123;

//context.ChangeTracker.DetectChanges();
//await context.SaveChangesAsync();

#endregion

#region AutoDetectChangesEnabled Property'si DetectChanges metodunun otomatik olarak tetiklenmesinin ayarlanması
//İlgili metotlar(SaveChanges, Entries) tarafından DetectChanges metodunun otomatik olarak tetiklenmesinin konfigürasyonunu yapmamızı sağlayan proeportydir.
//SaveChanges fonksiyonu tetiklendiğinde DetectChanges metodunu içerisinde default olarak çağırmaktadır. Bu durumda DetectChanges fonksiyonunun kullanımını irademizle yönetmek ve maliyet/performans optimizasyonu yapmak istediğimiz durumlarda AutoDetectChangesEnabled özelliğini kapatabiliriz.
#endregion


#region Entries Metodu izlenen her entity nesnesinin bigisini EntityEntry türünden elde etmemizi sağlar
//Context'te ki Entry metodunun koleksiyonel versiyonudur.
//Change Tracker mekanizması tarafından izlenen her entity nesnesinin bigisini EntityEntry türünden elde etmemizi sağlar ve belirli işlemler yapabilmemize olanak tanır.
//Entries metodu, DetectChanges metodunu tetikler. Bu durum da tıpkı SaveChanges'da olduğu gibi bir maliyettir. Buradaki maliyetten kaçınmak için AuthoDetectChangesEnabled özelliğine false değeri verilebilir.

var urunler = await context.Urunler.ToListAsync();
urunler.FirstOrDefault(u => u.Id == 7).Fiyat = 41241; //Update
context.Urunler.Remove(urunler.FirstOrDefault(u => u.Id == 2)); //Delete
urunler.FirstOrDefault(u => u.Id == 10).UrunAdi = "asdasd"; //Update

var EntriesDatas = context.ChangeTracker.Entries().ToList();

EntriesDatas.ForEach(e =>
{
    Console.WriteLine(e);

    if (e.State == EntityState.Modified)
    {

        Console.WriteLine(e.State.ToString() + "Modified");
    }
    else if (e.State == EntityState.Deleted)
    {
        Console.WriteLine(e.State.ToString() + "Deleted");
    }
    //...
});

#endregion


#region AcceptAllChanges Metodu
//SaveChanges() veya SaveChanges(true) tetiklendiğinde EF Core herşeyin yolunda olduğunu varsayarak track ettiği verilerin takibini keser
//yeni değişikliklerin takip edilmesini bekler. Böyle bir durumda beklenmeyen bir durum/olası bir hata söz konusu olursa eğer
//EF Core takip ettiği nesneleri brakacağı için bir düzeltme mevzu bahis olamayacaktır.

//Haliyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges metotları girecektir.

//SaveChanges(False), EF Core'a gerekli veritabanı komutlarını yürütmesini
//söyler ancak gerektiğinde yeniden oynatılabilmesi için değişikleri beklemeye/nesneleri takip etmeye devam eder.
//Taa ki AcceptAllChanges metodunu irademizle çağırana kadar!

//SaveChanges(false) ile işlemin başarılı olduğundan emin olursanız AcceptAllChanges metodu ile nesnelerden takibi kesebilirsiniz.

//var urunler = await context.Urunler.ToListAsync();
//urunler.FirstOrDefault(u => u.Id == 7).Fiyat = 123; //Update
//context.Urunler.Remove(urunler.FirstOrDefault(u => u.Id == 8)); //Delete
//urunler.FirstOrDefault(u => u.Id == 9).UrunAdi = "asdasd"; //Update

//await context.SaveChangesAsync(false);
//context.ChangeTracker.AcceptAllChanges();

#endregion

#region HasChanges Metodu
//Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisini verir.
//Arkaplanda DetectChanges metodunu tetikler.
//var result = context.ChangeTracker.HasChanges();
#endregion

#region Entity States
//Entity nesnelerinin durumlarını ifade eder.

#region Detached
//Nesnenin change tracker mekanizması tarafıdnan takip edilmediğini ifade eder.
//Urun urun = new();
//Console.WriteLine(context.Entry(urun).State);
//urun.UrunAdi = "asdasd";
//await context.SaveChangesAsync();
#endregion

#region Added
//Veritabanına eklenecek nesneyi ifade eder. Adeed henüz veritabanına işlenmeyen veriyi ifade eder. SaveChanges fonksiyonu çağrıldığında insert sorgusu oluşturucalşığı anlamını gelir.
//Urun urun = new() { Fiyat = 123, UrunAdi = "Ürün 1001" };
//Console.WriteLine(context.Entry(urun).State);
//await context.Urunler.AddAsync(urun);
//Console.WriteLine(context.Entry(urun).State);
//await context.SaveChangesAsync();
//urun.Fiyat = 321;
//Console.WriteLine(context.Entry(urun).State);
//await context.SaveChangesAsync();
#endregion

#region Unchanged
//Veritabanından sorgulandığından beri nesne üzerinde herhangi bir değişiklik yapılmadığını ifade eder. Sorgu neticesinde elde edilen tüm nesneler başlangıçta bu state değerindedir.
//var urunler = await context.Urunler.ToListAsync();

//var data = context.ChangeTracker.Entries();
//Console.WriteLine();
#endregion

#region Modified
//Nesne üzerinde değşiiklik/güncelleme yapıldığını ifade eder. SaveChanges fonksiyonu çağrıldığında update sorgusu oluşturulacağı anlamına gelir.
//var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 3);
//Console.WriteLine(context.Entry(urun).State);
//urun.UrunAdi = "asdasdasdasdasd";
//Console.WriteLine(context.Entry(urun).State);
//await context.SaveChangesAsync(false);
//Console.WriteLine(context.Entry(urun).State);
#endregion

#region Deleted
//Nesnenin silindiğini ifade eder. SaveChanges fonksiyonu çağrıldığında delete sorgusu oluşturuculağı anlamına gelir.
//var urun = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 4);
//context.Urunler.Remove(urun);
//Console.WriteLine(context.Entry(urun).State);
//context.SaveChangesAsync();
#endregion
#endregion


#region Context Nesnesi Üzerinden  Tracker
var urunChange = await context.Urunler.FirstOrDefaultAsync(u => u.Id == 553);
urunChange.Fiyat = 123;
urunChange.UrunAdi = "Silgi"; //Modified | Update

#region Entry Metodu55

#region OriginalValues Property'si
var fiyat = context.Entry(urunChange).OriginalValues.GetValue<float>(nameof(Urun.Fiyat));
var urunAdi = context.Entry(urunChange).OriginalValues.GetValue<string>(nameof(Urun.UrunAdi));
Console.WriteLine();
#endregion

#region CurrentValues Property'si
//var urunAdi = context.Entry(urun).CurrentValues.GetValue<string>(nameof(Urun.UrunAdi));
#endregion

#region GetDatabaseValues Metodu
//var _urun = await context.Entry(urun).GetDatabaseValuesAsync();
#endregion
#endregion
#endregion










public class ETicaretContext : DbContext
{
    public DbSet<Urun> Urunler { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=ChangeTrackerOptim;User ID=SA;Password=Password123;TrustServerCertificate=True");

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {

            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
public class Urun
{
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }
}



//fAKE ürün ekleyelim 
/*
 
for(int i=0; i < 1000; i++)
{
    context.Urunler.Add(new Urun
    {
        UrunAdi = i.ToString() + "Numarlı ürün",
        Fiyat = i ^ 3 + 20 * i + i
    });
}
context.SaveChanges();

 */