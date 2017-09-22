using System.Collections.Generic;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class PromoController : Controller
    {
        [Route("{langCode}/Promo/{name}")]
        public ActionResult Promo(string name)
        {
            var termList = new List<string>();
            var howToGet = new List<string>();
            switch (name.ToLower())
            {
                case "onlinerevolution":
                    return View("OnlineRevolution");
                case "onlinerevolutionwebview":
                    return View("OnlineRevolutionWebview");
                case "imlek":
                    return View("Imlek");
                case "btnterbanginhemat":

                    ViewBag.Title = "Payday Madness Permata Bank";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/PaydayMadness/PaydayMadness-page.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/PaydayMadness/PaydayMadness-page-mobile.jpg";
                    ViewBag.ImageAlt = "Payday Madness Permata Bank";
                    ViewBag.PromoTitle = "Payday Madness Permata Bank";
                    ViewBag.PromoDescription = "Yeay payday time! Saatnya memanjakan diri bersama keluarga atau sahabat dengan menginap di hotel-hotel terbaik pilihan kamu. Nikmati diskon 15% maksimal Rp 150.000 untuk setiap reservasi kamar hotel di Indonesia dan luar negeri dengan menggunakan metode pembayaran transfer bank melalui ATM Bank Permata.";
                    ViewBag.PeriodeBooking = "25 - 27 Agustus 2017";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    termList.Add("Diskon sebesar 15% (Maks. Rp 150.000). Tanpa minimum transaksi");
                    termList.Add("Berlaku untuk pemesanan setiap tanggal <span class=\"os-bold blue-txt\">25, 26, dan 27</span> setiap bulan");
                    termList.Add("Periode terbang dan inap kapan saja");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pembayaran menggunakan bank transfer supported by Permata Bank");
                    termList.Add("Berlaku untuk pemesanan melalui situs Travorama");
                    termList.Add("Program promo hanya dapat digunakan satu kali per hari untuk satu user");
                    termList.Add("Program tidak dapat digabungkan dengan voucher dan promo lainnya");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu");
                    ViewBag.Terms = termList;

                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Pilih metode pembayaran bank transfer supported by Permata Bank (Payday Madness)");
                    howToGet.Add("Harga akan otomatis terpotong sesuai program promo");
                    howToGet.Add("Lakukan pembayaran sesuai dengan instruksi");
                    howToGet.Add("Selesaikan proses pembayaran dan lakukan konfirmasi pembayaran");
                    howToGet.Add("E-voucher hotel akan dikirimkan ke alamat email Anda");
                    ViewBag.HowToGet = howToGet;

                    return View("BTNTerbanginHemat");
                case "btnterbanginhematwebview":
                    return View("BTNTerbanginHematWebview");
                case "hutbtn":
                    return View("HutBTN");
                case "hutbtnwebview":
                    return View("HutBTNWebview");
                case "harbolnas2016":
                    return View("Harbolnas2016");
                case "mataharimall":
                    return View("MatahariMall");
                case "paydaymegadeals":
                    return View("PaydayMegaDeals");

                case "paydaymadnesspermatabank":
                    ViewBag.Title = "Payday Madness Permata Bank";
                    ViewBag.ImageSrc = "/Assets/images/campaign/PaydayMadness/PaydayMadness-page-mobile.jpg";
                    ViewBag.ImageAlt = "Payday Madness Permata Bank";
                    ViewBag.PromoTitle = "Payday Madness Permata Bank";
                    ViewBag.PromoDescription = "Yeay payday time! Saatnya memanjakan diri bersama keluarga atau sahabat dengan menginap di hotel-hotel terbaik pilihan kamu. Nikmati diskon 15% maksimal Rp 150.000 untuk setiap reservasi kamar hotel di Indonesia dan luar negeri dengan menggunakan metode pembayaran transfer bank melalui ATM Bank Permata.";
                    ViewBag.PeriodeBooking = "25 - 27 Agustus 2017";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    termList.Add("Diskon sebesar 15% (Maks. Rp 150.000). Tanpa minimum transaksi");
                    termList.Add("Berlaku untuk pemesanan setiap tanggal <span class=\"os-bold blue-txt\">25, 26, dan 27</span> setiap bulan");
                    termList.Add("Periode terbang dan inap kapan saja");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pembayaran menggunakan bank transfer supported by Permata Bank");
                    termList.Add("Berlaku untuk pemesanan melalui situs Travorama");
                    termList.Add("Program promo hanya dapat digunakan satu kali per hari untuk satu user");
                    termList.Add("Program tidak dapat digabungkan dengan voucher dan promo lainnya");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu");
                    ViewBag.Terms = termList;

                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Pilih metode pembayaran bank transfer supported by Permata Bank (Payday Madness)");
                    howToGet.Add("Harga akan otomatis terpotong sesuai program promo");
                    howToGet.Add("Lakukan pembayaran sesuai dengan instruksi");
                    howToGet.Add("Selesaikan proses pembayaran dan lakukan konfirmasi pembayaran");
                    howToGet.Add("E-voucher hotel akan dikirimkan ke alamat email Anda");
                    ViewBag.HowToGet = howToGet;

                    return View("PaydayMadnessPermataBank");

                case "septemberseru":

                    ViewBag.Title = "September Seru";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017SeptemberSeru/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017SeptemberSeru/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "September Seru";
                    ViewBag.PromoTitle = "September Seru";
                    ViewBag.PromoDescription = "Liburan bersama teman atau keluarga akan lebih seru dengan bebas pilih hotel di mana saja, bebas inap kapan saja dan tanpa habis budget. Pesan sekarang dan nikmati diskon s.d. Rp1.000.000 kamar hotel domestik dan internasional di Travorama.";
                    ViewBag.KodePromo = "SEPTEMBERSERU9";
                    ViewBag.PeriodeBooking = "8 - 24 September 2017";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan detail tamu");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-Voucher hotel dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar 9% (Maks. Rp 1.000.000) dan minimal transaksi Rp 500.000");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pemesanan kamar hotel melalui situs Travorama ");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain ");
                    termList.Add("Periode booking: 8 - 24 September 2017");
                    termList.Add("Periode inap: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal.");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu.");
                    ViewBag.Terms = termList;

                    return View("Promo");

                case "goodmonday":

                    ViewBag.Title = "Good Monday";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017GoodMonday/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017GoodMonday/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Good Monday";
                    ViewBag.PromoTitle = "Good Monday";
                    ViewBag.PromoDescription = "Penawaran khusus untuk penerbangan tujuan Semarang, Yogyakarta, Jakarta, Tanjung Pinang dan Surabaya diskon s.d. Rp 300.000. Pesan sekarang di Travorama dan jadikan perjalanan kamu lebih berkesan!";
                    ViewBag.KodePromo = "MONDAY300";
                    ViewBag.PeriodeBooking = "Setiap Senin";
                    ViewBag.PeriodeTerbang = "Kapan Saja";

                    howToGet.Add("Cari dan pesan tiket pesawat melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan penumpang");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-ticket pesawat dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar 3% (Maks. Rp 300.000) dan minimum transaksi Rp 1.000.000");
                    termList.Add("Berlaku untuk penerbangan domestik tujuan Semarang, Yogyakarta, Jakarta, Tanjung Pinang dan Surabaya dengan maskapai Citilink, Sriwijaya, NAM Air dan Batik Air.");
                    termList.Add("Berlaku untuk pemesanan tiket pesawat melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Senin");
                    termList.Add("Periode terbang: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal.");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu.");
                    ViewBag.Terms = termList;

                    return View("Promo");

                case "kamisceria":

                    ViewBag.Title = "Kamis Ceria";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017KamisCeria/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017KamisCeria/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Kamis Ceria";
                    ViewBag.PromoTitle = "Kamis Ceria";
                    ViewBag.PromoDescription = "Saatnya kamu berlibur ke destinasi favorit di Indonesia tanpa habis budget dengan Travorama! Liburan lebih menyenangkan dengan diskon Rp 100.000 untuk penerbangan tujuan Bali, Lombok, Labuan Bajo, Batam dan Banda Aceh.";
                    ViewBag.KodePromo = "KAMIS100";
                    ViewBag.PeriodeBooking = "Setiap Kamis";
                    // ViewBag.PeriodeInap = "Kapan Saja";
                    ViewBag.PeriodeTerbang = "Kapan Saja";

                    howToGet.Add("Cari dan pesan tiket pesawat melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan penumpang");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-ticket pesawat dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar Rp 100.000 dan minimal transaksi Rp 2.500.000");
                    termList.Add("Berlaku untuk penerbangan domestik tujuan Bali, Lombok, Labuan Bajo, Batam dan Banda Aceh dengan maskapai Citilink, Sriwijaya, NAM Air dan Batik Air.");
                    termList.Add("Berlaku untuk pemesanan tiket pesawat melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Kamis");
                    termList.Add("Periode terbang: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal.");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu.");
                    ViewBag.Terms = termList;

                    return View("Promo");

                case "jalanjalansabtu":

                    ViewBag.Title = "Jalan Jalan Sabtu";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017JalanJalanSabtu/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017JalanJalanSabtu/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Jalan Jalan Sabtu";
                    ViewBag.PromoTitle = "Jalan Jalan Sabtu";
                    ViewBag.PromoDescription = "Spesial di akhir pekan, kamu bebas pilih destinasi! Pesan tiket pesawat di Travorama dan dapatkan diskon tiket pesawat s.d. Rp 200.000 untuk semua destinasi domestik.";
                    ViewBag.KodePromo = "JJSABTU4";
                    ViewBag.PeriodeBooking = "Setiap Sabtu";
                    ViewBag.PeriodeTerbang = "Kapan Saja";

                    howToGet.Add("Cari dan pesan tiket pesawat melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan penumpang");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran Bank Transfer");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-ticket pesawat dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar 4% (Maks. Rp 200.000) dan tanpa minimum transaksi");
                    termList.Add("Berlaku untuk semua penerbangan domestik dengan maskapai Citilink, Sriwijaya, NAM Air dan Batik Air.");
                    termList.Add("Berlaku untuk pemesanan tiket pesawat melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk metode pembayaran Bank Transfer");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Sabtu");
                    termList.Add("Periode terbang: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal.");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu.");
                    ViewBag.Terms = termList;

                    return View("Promo");
                default:
                    return RedirectToAction("Index", "Index");
            }
        }
    }
}