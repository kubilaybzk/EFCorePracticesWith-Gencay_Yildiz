

using Microsoft.EntityFrameworkCore;

internal class Program
{

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


            optionsBuilder.UseSqlServer("Data Source=192.168.16.1;Initial Catalog=EFCoreSecond;User ID=SA;Password=Password123;TrustServerCertificate=True");

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


    static void Main(string[] args)
    {
        #region Peki crud işlemleri nasıl yapılır entitity framwork kullanarak nasıl database içine veri yazabilir çekebiliriz? 

        /* 
           İlk olarak EFCore'da verise bütün işlemleri yapmamız için bir tane context instance ihtiyacımız var.
                Bunu oluşturmak için normal bir class'dan bir obje  oluşturmamız lazım bu obje sayesinde veri tabanı işlemlerimizi yapacağız.


         */
        #endregion

        ApplicationDBContext context = new();

        #region Şimdi öncelikle şimdiye kadar öğrendiklerimizi birleştirelim.
        /*
            Veriye entity'nin instance değerleri karşı geliyordu.
            Tablomuza ise entity'nin kendisi geliyordu. Yani tablonun modeline karışılık geliyordu. 

        Yani biz eğer bir ürün eklemek istiyorsak öncelile bu ürünü o tablonun karşılığı olan entitiy'nin bir instance değeri olarak 
            Oluştrurmamız gerekecek.
                Şimdi öncelikle bu işleme başlayalım.
         */

        #endregion

        User addedUser = new() { UserName = "kubilay", Password = "Sifre12131gdsgsdgsdgs2" };
        User addedUser2 = new() { UserName = "Cağrı", Password = "Sifdsfare12131gdsgsdgds2" };
        User addedUser3 = new() { UserName = "Samet", Password = "Sfdsfsdsfsdgifre121312" };
        User addedUser4 = new() { UserName = "Fatih", Password = "Sifdsfare121dgsgsgsd312" };


        //Ekleme işlemleri


        /* şimdi yukarıda bir instance oluşturduk oluşturduğumuz bu instance değerini bizim
        database içine eklememiz için kullanabileceğimiz iki yöntem var ikiside aynı işleme yarar.

        */

        #region context.AddAsync Fonksiyonu  // context.DbSet.AddAsync Fonksiyonu
        /* 
         ekleme işlemlerinin tamamen asenkron olarak eklenmesini sağlayan yapı.  
        */
        #endregion

        #region context.DbSet.Add Fonksiyonu // context.DbSet.AddA Fonksiyonu
        /* 
        ekleme işlemlerinin tamamen asenkron dikkate alınmadan eklenmesini sağlayan yapı.
        */
        #endregion



        context.AddAsync(addedUser); // Tip Güvenliği yok .
        context.Users.AddAsync(addedUser2); //Tip güvenliği var


        context.Add(addedUser3); //Tip güvenliği yok
        context.Users.Add(addedUser4); //Tip güvenliği var

        #region SaveChanges Nedir? SaveChanges; insert, update ve delete sorgularını oluşturup bir transaction eşliğinde
        //  veritabanına gönderip execute eden fonksiyodur.
        //  Eğer ki oluşturulan sorgulardan herhangi biri başarısız olursa tüm işlemleri geri alır(rollback)
        #endregion

        context.SaveChanges();


        #region Şimdi burada şöyle bir durum söz konusu burada tanımlama yaparken id girmedim çünkü bunun primary key olarak biz tanımlarken otomatik olarak ef ayarlıyor.
        #endregion


        #region EF Core Açısından Bir Verinin Eklenmesi Gerektiği Nasıl Anlaşılıyor?

        #endregion

        //User addedUser6 = new() { UserName = "BehindMe", Password = "dsaşlfkasş" };
        //User addedUser7 = new() { UserName = "BehindMeV2", Password = "dsaşlfkasş2" };

        //Console.WriteLine(context.Entry(addedUser6).State);

        //context.AddAsync(addedUser6);

        //Console.WriteLine(context.Entry(addedUser7).State);

        //context.SaveChangesAsync();

        //Console.WriteLine(context.Entry(addedUser7).State);



        /* Şimdi konsol ekranlarında detach diye bir değer göreceğiz bu değer şu anlama geliyor 
              Bu obje bağımsız bir obje aslında şuan daha yürürlüğe girmemiş bir obje.
        */


        User addedUser8 = new() { UserName = "BehindMeV8 ", Password = "dsaşlfkasş8" };
        User addedUser9 = new() { UserName = "BehindMeV9 ", Password = "dsaşlfkasş9" };
        User addedUser10 = new() { UserName = "BehindMeV10", Password = "dsaşlfkasş10" };
        User addedUser11 = new() { UserName = "BehindMeV11", Password = "dsaşlfkasş11" };


        #region Peki birden fazla obje eklemek istersek tek bir sorguda bunu nasıl yapacağız. (AddRange)
        /*
         Normal tekli ekleme yönteminde iki farklı ekleme yöntemi olduğu gibi 
                   Çoklu ekleme yönteminde ise senkron asenkron ve tip güvenliği gibi seçeneklerimiz var.
                    
                    context.AddRange()              Normal ekleme
                    context.User.AddRange()         Tip Güvenlikli ekleme
                    context.AddRangeAsync()         Asenkron ekleme 
                    context.User.AddRangeAsync()    Asenkron  Tip Güvenlikli ekleme
         */
        #endregion

        context.Users.AddRange(addedUser8, addedUser9, addedUser10, addedUser11);
        context.SaveChanges();



    }


}