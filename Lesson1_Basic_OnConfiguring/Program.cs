using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }

    #region Şimdi basit bir şekilde EFCore için konfigürasyon ayarlamalarını yapalım
    /* 
       Öncelikle EF bir ORM bunu unutmayalım yani object relation mapping işlemi yaptığımız bir framwork
       Object relational mapping kavramının tam olarak oob nimetlerinden yararlanarak kod geliştirmemizi veri manipülasyonunu yapmamızı sağlayan işlemlere denir. 
       Şimdi öncelikle bir EFCore projesinde yüklememiz gereken bazı paketler var 
       Bunlar EFCore EFTools EFDesing ve kullanılacak olan veri tabanına göre bir provider. 
       Bu paketleri Package manager console yada Nuget Clı ile yükledikten sonra gerekli işlemlere başlayabiliriz.

   */

    #endregion

    #region Öncelikle ilk yapılaması gereken bir adet DBContex oluşturmak peki bu nedir bu DBContext ? 
    /* 

          DBContext basitçe bizim veri tabanı ile ilişkiye geçmemizi sağlayan bir class aslında .
          LazyLoading gibi temel yapılandırmalar  ilerleyen süreçte bu konulara değineceğiz.
              Şimdi MSSql kullanarak veri tabanına bağlanmamızı sağlayan işlemleri gerçekleştirelim.
                      Öncelikle bu tarz  veri tabanına bağlanmak işlemlerini yapan foksiyonların 
                          DBContext olarak bir kalıtım alması gerekir ve 
                             genel olarak bu classların isimleri proje adına göre ProjeAdıDBContext olarak yada ApplicationDBContext gibi isimler ile genelleştirilir.



     */
    #endregion

    public class ApplicationDBContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        //Burada bulunan DbSet ile veri tabanında böyle bir tablo olması gerektiğini bildiriyoruz.
        //Generic olarak geliştirilen içerik olarak veri tabanında böyle bir  tablo olacak diye bildiriyoruz.
        //Genel olarak isimlendirmeler şöyle olacak , Entity tanımlamalarını yaparken entity adı çoğul olarak girilmez ,
        // Tablonunn isimlendirmesi ise çöğul olarak  ayarlanır .


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            optionsBuilder.UseSqlServer("Data Source=192.168.16.1;Initial Catalog=EFCoreFirst;User ID=SA;Password=Password123;TrustServerCertificate=True");

        }



    }

    #region  Entity tanımlamalarına burada başlayalım öncelikle entityler bizim için veri tabanları demek bunu basitçe aklımızda tutalım.
    //Şimdi burada şöyle bir  durum söz konusu her tablonun bir primary key değeri olması gerekir. 
    //Bunun genel olarak EF core tarafında şöyle bir genellemesi mevcuttur. 
    //ID
    //EntittyName
    //id 
    // olacak şekilde 3 farklı şekilde isimlendirme yapmamız genel olarak kabul edilmiş bir geçektir. 

    #endregion

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    #region Migration kullanarak ver tabanına bu yazmış olduğumuz tabloları ve veri tabanımızı oluşturalım.

    //Şimdi veri tabanı bağlantımızı ayarladık veri tabanı bağlantızı ayarladıktan sonra 
    //Veri tabanımızda Users adında bir tzsblo olacağını bildirdik peki bunu nasıl canlı olarak veri tabanına aktaracağız.  
    //Burada ise şöyle bir yöntem kullanıyoruz
    //Migration: 
    //Şimdilik basitçe şöyle bir açıklama yapabiliriz.
    //Migration basit bir şekilde senin code olarak oluşturmuş olduğun verileri
    //veri tabanına sql olarak Up ve Down foksiyonları oluşturarak update delete create gibi işlemleri yapan araç
    //Migration'ın şöyle bir güzelliği her bir migration asında bir yedek bu yedekler hem veri tabanında hemde
    //Projemiz içinde bir önceki migration 'a dönmemizi ve karşılaştığımız hataları yok etmemizi sağlıyor.

    //Her bir migration oluşturduktan sonra hemen database için işlem yapmaz bunun için
    // -Update Database komutu kullanırız.


    #endregion





}