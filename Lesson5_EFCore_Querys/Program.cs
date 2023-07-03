
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

Console.WriteLine("Hello, World!");

#region Bu konuya başlamadan önce dipnot.
/* şimdi öncelikle yapmamız gereken işlemlerden önce 
    burada konuyu daha iyi kavramamız için ilişkisel bir yapı kullanıcaz buna henüz değinmedik fakat
        konunun daha net kavranabilmesi için bunu biliyormuş gibi davranacağız yani üst tarafta bulunan kodlar hazır
            bir şekilde başka bir repodan alınmış olacak.

*/
#endregion

ApplicationDBContext context = new();

// En Temel Basit Bir Sorgulama Nasıl Yapılır?


#region Method Syntax Methodlar aracılığıyla yapılan sorgular
#endregion
//var urunler =  context.Urunler.ToList();

#region Query Syntax Query aracılığıyla yapılan sorgular

#endregion
//var urunler2 =  (from urun in context.Urunler
//                      select urun).ToList();

#region Sorguyu Execute Etmek İçin Ne Yapmamız Gerekmektedir?
/* Şimdi üst kısımda sorgular oluşturulduktan sonra ToListAsync diye bir fonksiyon kullandık bu ne işe yarıyor bunu bi düşünelim?
        Şimdi ToListAsync fonksiyonu yazmış olduğumuz sorgunun IQueryable mı yoksa IEnumerable mı onu öğrenmemiz lazım .
 */

#region IQueryable nedir ?
//Sorguya karşılık gelir.
//Ef core üzerinden yapılmış olan sorgunun execute edilmemiş halini ifade eder.
#endregion

#region IEnumerable nedir?
//Sorgunun çalıştırılıp/execute edilip verilerin in memorye yüklenmiş halini ifade eder.
#endregion

#region IQueryable olan bir değişken nasıl IEnumerarble olur ? (ilk yöntem) ToList();
/*
   Şimdi basitçe bu ikiliyi anlamış olduk,
       Bir sorgunun IQueryable mı yoksa IEnumerable olmalı onu nasıl anlayacağız ?

        var urunler2 =  (from urun in context.Urunler select urun);                şuan IQueryable'dır.
        var urunler2 =  (from urun in context.Urunler select urun).ToList();  IEnumerable'dır.

    Başka bir yöntemimiz daha var kodu execute etmek için;
        Buda IQueryable olan bir değişkeni çağırmak yani mesela IQueryable olmuş olan urunler2'yi bir foreach içinde yazmak.

 */
#endregion
//var urunler =  context.Urunler;
//var urunler =  context.Urunler.ToList();
#region IQueryable olan bir değişkeni çağırmak . (ikinci yöntem) 
/* 
İkinci yöntem olarak ne demiştik  IQueryable olan bir değişkeni çağırmak 
*/


//var urunlerim =  (from urun in context.Urunler select urun);
//foreach(Urun urun in urunlerim)
//{
//    Console.WriteLine("{0} adlı ürün {1}TL", urun.UrunAdi,urun.Fiyat);
//}

////Burada şöyle bir durum söz konusu burada şöyle bir şey yapsak.

//int urunId = 2; //Bir adet sayı tanımladık burada amacımız Id değeri 2'den fazla olan ürünleri ekrana basmak olacak 

//var urunler = from urun in context.Urunler
//              where urun.Id > urunId    //Sorgumuza eklememizi yaptık
//              select urun;

//urunId = 200;  //burada değerimizi 200 olarak güncelledik.
#endregion
/*
 Şimdi burada şöyle bir durum söz konusu bu kodu foreach ile çalıştırırsak biz 
    {Urunler tablomuzda 0 'dan 10'a kadar ürünler olduğunu düşünelim}
        Karşımıza hiçbir sonuç çıkarmayacaktır bunun sebebi burada öncelikle bizim query oluşturmamız ve bunu execute etmememiz.
            Yani bu şu anlama geliyor urunler değerinin query string hali memory'de tutulutor ama veri tabanı ile herhangi bir iletişim
                Kurmadığı için 2^den 10'a kadar olan ürünleri depolamıyor.
                    Çalışma anı urunId=200'den sonra bir foeach içinde olduğu için kodumuz yukarıdan aşağı gelirken 
                        o sırada execute edildiği için urunId değerini 200 olarak görüyor
                
 Buna Deferred Execution (Ertelenmiş Çalışma) deniyor.
                    
 */

#endregion




// *********************** Çoğul Veri Getirme Sorguları ***********************

#region ToListAsync
//Üretilen sorguyu execute ettirmemizi sağlayan fonksiyondur.


//var urunler =  context.Urunler.ToList();
#endregion

#region Where
//Oluşturulan sorguya where şartı eklememizi sağlayan bir fonksiyondur.


var urunlerFunction = context.Urunler.Where(u => u.Id > 5).ToList();
//ShowResults(urunlerFunction);

var urunlerQuery = (from urun in context.Urunler where urun.Id > 5 && urun.UrunAdi.EndsWith("7") select urun).ToList();
//ShowResults(urunlerQuery);
#endregion

#region OrderBy (Ascending) (Artan sıralama)
//Sorgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur. (Ascending) (Artan sıralama)

var urunlerOrderBy = context.Urunler.Where(u => u.Id > 2 || u.UrunAdi.EndsWith("4")).OrderBy(u => u.UrunAdi);
//Burada bilmemiz gereken önemli trick orderBy yaparken içinde sıralama yapılacak olan kolonun adını belirmemiz.
//Mesela fiyata göre sıralama yapmak istersek yada urun adına göre yapmak istersek içinde bunu yine belirtmemiz lazım.


var urunlerOrderBy2 = (from urun in context.Urunler
                       where urun.Id > 500 || urun.UrunAdi.StartsWith("2")
                       orderby urun.UrunAdi
                       select urun).ToList();

#endregion

#region ThenBy  (Ascending)  OrderBy'a göre aynı değerlere sahip olan elemanlar için farklı bir sıralama sağlayan fonksiyon.
//OrderBy üzerinde yapılan sıralama işlemini farklı kolonlarada uygulamamızı sağlayan bir fonksiyondur. (Ascending) 

//var urunlerThenBy =  context.Urunler.Where(u => u.Id > 5 || u.UrunAdi.EndsWith("2"))
//    .OrderBy(u => u.UrunAdi)
//    .ThenBy(u => u.Fiyat).ThenBy(u => u.Id).ToList();

//Mesela burada bulunan sorgumuzda 50'den büyük ve 52 , 52 olmak üzere 2 farklı değer olduğunu düşünelim.
//Burada şöyle bir durum ile karşılaşıyoruz 2 tane değer var ama bunların mesela hangisinin üste hangisinin alta geçeceğini başka
//Kolonlara göre düzenlemek istiyoruz örnek olarak ; 
//Bize iki tane ürün geldi aynı isme sahip olan bunları fiyatına göre listelemek isteriz bir diğer durumda.
//gibi gibi.






#endregion

#region OrderByDescending OrderBy'ın tam tersi (Azalan Sıralama)
//Descending olarak sıralama yapmamızı sağlayan bir fonksiyondur.
var urunlerOrderByDescending = context.Urunler.OrderByDescending(u => u.Fiyat).ToList();

var urunlerOrderByDescendingQuery = (
                     from urun in context.Urunler
                     orderby urun.UrunAdi descending
                     select urun).ToList();
#endregion

#region ThenByDescending
//OrderByDescending üzerinde yapılan sıralama işlemini farklı kolonlarada uygulamamızı sağlayan bir fonksiyondur. (Ascending)
var urunlerThenByDescending = context.Urunler.OrderByDescending(u => u.Id)
    .ThenByDescending(u => u.Fiyat)
    .ThenBy(u => u.UrunAdi).ToList();
#endregion




// *********************** Tekil Veri Getiren Sorgulama Fonksiyonları ***********************

#region SingleAsync Eğer ki, sorgu neticesinde birden fazla veri geliyorsa ya da hiç gelmiyorsa her iki durumda da exception fırlatır.
#region Tek Kayıt Geldiğinde
//var urun =  context.Urunler.SingleAsync(u => u.Id == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
//var urun =  context.Urunler.SingleAsync(u => u.Id == 5555);
#endregion
#region Çok Kayıt Geldiğinde
//var urun =  context.Urunler.SingleAsync(u => u.Id > 55);
#endregion
#endregion

#region SingleOrDefaultAsync Eğer ki, sorgu neticesinde birden fazla veri geliyorsa exception fırlatır, hiç veri gelmiyorsa null döner.
#region Tek Kayıt Geldiğinde
//var urun =  context.Urunler.SingleOrDefaultAsync(u => u.Id == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
//var urun =  context.Urunler.SingleOrDefaultAsync(u => u.Id == 5555);
#endregion
#region Çok Kayıt Geldiğinde
//var urun =  context.Urunler.SingleOrDefaultAsync(u => u.Id > 55);
#endregion
#endregion

//Yapılan sorguda tek bir verinin gelmesi amaçlanıyorsa First ya da FirstOrDefault fonksiyonları kullanılabilir.

#region FirstAsync Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa hata fırlatır.
#region Tek Kayıt Geldiğinde
//var urun =  context.Urunler.FirstAsync(u => u.Id == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
//var urun =  context.Urunler.FirstAsync(u => u.Id == 5555);
#endregion
#region Çok Kayıt Geldiğinde
//var urun =  context.Urunler.FirstAsync(u => u.Id > 55);
#endregion
#endregion

#region FirstOrDefaultAsync Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa null değerini döndürür.
#region Tek Kayıt Geldiğinde
//var urun =  context.Urunler.FirstOrDefaultAsync(u => u.Id == 55);
#endregion
#region Hiç Kayıt Gelmediğinde
//var urun =  context.Urunler.FirstOrDefaultAsync(u => u.Id == 5555);
#endregion
#region Çok Kayıt Geldiğinde
//var urun =  context.Urunler.FirstAsync(u => u.Id > 55);
#endregion
#endregion

#region FindAsync Find fonksiyonu, primary key kolonuna özel hızlı bir şekilde sorgulama yapmamızı sağlayan bir fonksiyondur.
//Urun urun =  context.Urunler.FirstOrDefaultAsync(u => u.Id == 55);
//Urun urun =  context.Urunler.FindAsync(55);

#region Composite Primary key Durumu
//UrunParca u =  context.UrunParca.FindAsync(2, 5);
#endregion
#endregion

#region LastAsync Sorgu neticesinde gelen verilerden en sonuncusunu getirir. Eğer ki hiç veri gelmiyorsa hata fırlatır. OrderBy kullanılması mecburidir.
//var urun =  context.Urunler.OrderBy(u => u.Fiyat).LastAsync(u => u.Id > 55);
#endregion

#region LastOrDefaultAsync Sorgu neticesinde gelen verilerden en sonuncusunu getirir. Eğer ki hiç veri gelmiyorsa null döner. OrderBy kullanılması mecburidir.
//var urun =  context.Urunler.OrderBy(u => u.Fiyat).LastOrDefaultAsync(u => u.Id > 55);
#endregion

// *********************** Diğer Sorgulama Fonksiyonları ***********************

#region CountAsync Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(int) bizlere bildiren fonksiyondur.
//var urunler = ( context.Urunler.ToListAsync()).Count();
//var urunler =  context.Urunler.Count();
#endregion

#region LongCountAsync Oluşturulan sorgunun execute edilmesi neticesinde kaç adet satırın elde edileceğini sayısal olarak(long) bizlere bildiren fonksiyondur.
//var urunler =  context.Urunler.LongCountAsync(u => u.Fiyat > 5000);
#endregion

#region AnyAsync Sorgu neticesinde verinin gelip gelmediğini bool türünde dönen fonksiyondur. 
//var urunler =  context.Urunler.Where(u => u.UrunAdi.Contains("1")).Any();
//var urunler =  context.Urunler.AnyAsync(u => u.UrunAdi.Contains("1"));
#endregion

#region MaxAsync Verilen kolondaki max değeri getirir.
//var fiyat =  context.Urunler.MaxAsync(u => u.Fiyat);
#endregion

#region MinAsync Verilen kolondaki min değeri getirir.
//var fiyat =  context.Urunler.MinAsync(u => u.Fiyat);
#endregion

#region Distinct Sorguda mükerrer kayıtlar varsa bunları tekilleştiren bir işleve sahip fonksiyondur.
//var urunler =  context.Urunler.Distinct().ToList();
#endregion

#region AllAsync Bir sorgu neticesinde gelen verilerin, verilen şarta uyup uymadığını kontrol etmektedir. Eğer ki tüm veriler şarta uyuyorsa true, uymuyorsa false döndürecektir.
//var m =  context.Urunler.AllAsync(u => u.Fiyat < 15000);
//var m =  context.Urunler.AllAsync(u => u.UrunAdi.Contains("a"));
#endregion

#region SumAsync Vermiş olduğumuz sayısal proeprtynin toplamını alır.
//var fiyatToplam =  context.Urunler.SumAsync(u => u.Fiyat);
#endregion

#region AverageAsync Vermiş olduğumuz sayısal proeprtynin aritmatik ortalamasını alır.
//var aritmatikOrtalama =  context.Urunler.AverageAsync(u => u.Fiyat);
#endregion

#region Contains Like '%...%' sorgusu oluşturmamızı sağlar.
//var urunler =  context.Urunler.Where(u => u.UrunAdi.Contains("7")).ToList();
#endregion

#region StartsWith /Like '...%' sorgusu oluşturmamızı sağlar.
//var urunler =  context.Urunler.Where(u => u.UrunAdi.StartsWith("7")).ToList();
#endregion

#region EndsWith Like '%...' sorgusu oluşturmamızı sağlar.
//var urunler =  context.Urunler.Where(u => u.UrunAdi.EndsWith("7")).ToList();
#endregion


/* Şimdi elde ettiğimiz verileri farklı türlere dönüştürmek istersek kullanabileceğimiz yöntemlere göz atalım */


#region ToDictionaryAsync => Sorgu neticesinde gelecek olan veriyi bir dictioanry olarak elde etmek/tutmak/karşılamak istiyorsak eğer kullanılır!
//var urunler =  context.Urunler.ToDictionaryAsync(u => u.UrunAdi, u => u.Fiyat);

//ToList ile aynı amaca hizmet etmektedir. Yani, oluşturulan sorguyu execute edip neticesini alırlar.
//ToList : Gelen sorgu neticesini entity türünde bir koleksiyona(List<TEntity>) dönüştürmekteyken,
//ToDictionary ise : Gelen sorgu neticesini Dictionary türünden bir koleksiyona dönüştürecektir.
#endregion

#region ToArrayAsync => Oluşturulan sorguyu dizi olarak elde eder.
//ToList ile muadil amaca hizmet eder. Yani sorguyu execute eder lakin gelen sonucu entity dizisi  olarak elde eder.
//var urunler =  context.Urunler.ToArray();
#endregion

#region Select => Select fonksiyonunun işlevsel olarak birden fazla davranışı söz konusudur,

#region 1. Select fonksiyonu, generate edilecek sorgunun çekilecek kolonlarını ayarlamamızı sağlamaktadır. 

//var urunler =  context.Urunler.Select(u => new Urun
//{
//    Id = u.Id,
//    Fiyat = u.Fiyat
//}).ToList();
#endregion


#region 2. Select fonksiyonu, gelen verileri farklı türlerde karşılamamızı sağlar. T, anonim

//Anonim olarak bir class tanımlayarak
//var urunler =  context.Urunler.Select(u => new 
//{
//    Id = u.Id,
//    Fiyat = u.Fiyat
//}).ToList();

//Yeni bir class oluşturalım UrunDeray adında buna geçirelim

//var urunler =  context.Urunler.Select(u => new UrunDetay
//{
//    Id = u.Id,
//    Fiyat = u.Fiyat
//}).ToList();

#endregion




#endregion

//Şimdi ilişkisel verileri henüz görmedik ama biliyormuş gibi düşünelim.
//ilgili olan sorgulan entity içinde ilişkisel verileri barındıran bir koleksiyonda işlem yapmak isteyebiliriz.
//Şimdi şu kodu inceleyelim.

var urunler = context.Urunler.Select(u => new UrunDetay
{
    Id = u.Id,
    Fiyat = u.Fiyat
}).ToList();

/*
 Şimdi bu soruguda bizim bu derse başlamadan önce dipnot düştüğümüz bir konu vardı.
    Orda bahsettiğimiz ilişkisel olayı hala bilmiyoruz ama bilgiğimizi düşünelim
        şimdi burada amacımızı acıklayalım
            
            Bizim ürün classımız'da 
                public ICollection<Parca> Parcalar { get; set; }
            Böyle bir prop var bu şu anlama geliyor benim her bir class'ım parçalar diye bir tablo ile ilişki halinde .
                    ben bir soru ile bu parçalar'a erişmek istiyorsam INCLUDE kullanmam gerekiyor.
                        //Şuan bunu bilmiyoruz ama öğrenicez biliyormuş gibi davranalım.

            var urunler = context.Urunler.Include(u=>u.Parcalar).Select(u => 
            new UrunDetay{
                Id = u.Id,
                Fiyat = u.Fiyat
            }).ToList();
    
Şimdi burada Select ile Include kullanmamız imkansız çünkü Select bunu desteklemiyor .Select tek bir tablo için sorgu yapmamızı sağlayan bir fonksiyon


 */

#region SelectMany =>Select ile aynı amaca hizmet eder. Lakin, ilişkisel tablolar neticesinde gelen koleksiyonel verileri de tekilleştirip projeksiyon etmemizi sağlar.

/* 
 İki adet  değer alma sebebi geliştiriciler için ilk değişken ana tablo'da ilişkiye karşı gelen koloksiyon yapılandırması
        ikinci değişken ise ilişkisi olan tabloya erişimi sağlamak için gerken işlemler.

                .SelectMany(u => u.Parcalar, (u, p) => new
                {
                    u.Id,
                    u.Fiyat,
                    p.ParcaAdi
                }
 */

var urunlerManny = context.Urunler.Include(u => u.Parcalar).SelectMany(u => u.Parcalar, (u, p) => new
{
    u.Id,
    u.Fiyat,
    p.ParcaAdi
}).ToList();

Console.WriteLine("Wait a minute.");


#endregion



#region GroupBy Fonksiyonu Gruplama yapmamızı sağlayan fonksiyondur.
//var datas = await context.Urunler.GroupBy(u => u.Fiyat).Select(group => new
//{
//    Count = group.Count(),
//    Fiyat = group.Key
//}).ToListAsync();
#endregion





static void ShowResults(List<Urun> urunler)
{
    foreach (Urun urun in urunler)
    {
        Console.WriteLine(urun.UrunAdi);
    }
}

public class ApplicationDBContext : DbContext
{

    public DbSet<Urun> Urunler { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {


        optionsBuilder.UseSqlServer("Data Source=192.168.17.123;Initial Catalog=EFCoreQuerys;User ID=SA;Password=Password123;TrustServerCertificate=True");

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrunParca>().HasKey(up => new { up.UrunId, up.ParcaId });
    }



}
public class Urun
{
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }

    public ICollection<Parca> Parcalar { get; set; }
}
public class Parca
{
    public int Id { get; set; }
    public string ParcaAdi { get; set; }
}
public class UrunParca
{
    public int UrunId { get; set; }
    public int ParcaId { get; set; }

    public Urun Urun { get; set; }
    public Parca Parca { get; set; }
}
public class UrunDetay
{
    public int Id { get; set; }
    public float Fiyat { get; set; }
}