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
                case "horegajian":

                    ViewBag.Title = "Hore Gajian";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017HoreGajian/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017HoreGajian/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Hore Gajian";
                    ViewBag.PromoTitle = "Hore Gajian";
                    ViewBag.PromoDescription = "Sudah gajian! Saatnya liburan bersama teman atau keluarga dengan bebas pilih hotel di mana saja dan bebas inap kapan saja.  Pesan sekarang dan nikmati diskon s.d.&nbsp;Rp1.000.000 kamar hotel domestik dan internasional di Travorama.";
                    ViewBag.KodePromo = "HOREGAJIAN";
                    ViewBag.PeriodeBooking = "26 - 30 September 2017";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan detail tamu");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-Voucher hotel dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar 9% (Maks. Rp 1.000.000) dan minimal transaksi Rp&nbsp;500.000");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pemesanan kamar hotel melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: 26 - 30 September 2017");
                    termList.Add("Periode inap: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal.");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu.");
                    ViewBag.Terms = termList;

                    return View("Promo");

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

                case "selasaspesial":

                    ViewBag.Title = "Selasa Spesial";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017SelasaSpesial/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017SelasaSpesial/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Selasa Spesial";
                    ViewBag.PromoTitle = "Selasa Spesial";
                    ViewBag.PromoDescription = "Promo spesial setiap Selasa dari Travorama untuk kamu yang ingin liburan! Nikmati diskon Rp 240.000 untuk semua hotel di dalam dan luar negeri.";
                    ViewBag.KodePromo = "SELASA240";
                    ViewBag.PeriodeBooking = "Setiap Selasa";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan detail tamu");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-Voucher hotel dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar Rp 240.000 dan minimal transaksi Rp 2.500.000");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pemesanan kamar hotel melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Selasa");
                    termList.Add("Periode inap: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu");
                    ViewBag.Terms = termList;

                    return View("Promo");

                case "promorabu":

                    ViewBag.Title = "Promo Rabu";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017PromoRabu/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017PromoRabu/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Promo Rabu";
                    ViewBag.PromoTitle = "Promo Rabu";
                    ViewBag.PromoDescription = "Business trip atau liburan akan lebih hemat dengan pesan tiket pesawat di Travorama dan nikmati diskon tiket pesawat Rp&nbsp;75.000 untuk semua destinasi domestik.";
                    ViewBag.KodePromo = "RABU75";
                    ViewBag.PeriodeBooking = "Setiap Rabu";
                    ViewBag.PeriodeTerbang = "Kapan Saja";

                    howToGet.Add("Cari dan pesan tiket pesawat melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan penumpang");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran Bank Transfer");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-ticket pesawat dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar Rp 75.000 dan minimum transaksi Rp&nbsp;1.500.000");
                    termList.Add("Berlaku untuk semua penerbangan domestik dengan maskapai Sriwijaya Air dan NAM Air");
                    termList.Add("Berlaku untuk pemesanan tiket pesawat melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk metode pembayaran Bank Transfer");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Rabu");
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

                case "jumathemat":

                    ViewBag.Title = "Jumat Hemat";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017JumatHemat/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017JumatHemat/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Jumat Hemat";
                    ViewBag.PromoTitle = "Jumat Hemat";
                    ViewBag.PromoDescription = "Spesial untuk kamu yang telah memesan tiket pesawat di Travorama, nikmati diskon Rp 100.000 untuk semua kamar hotel di domestik dan internasional.";
                    ViewBag.KodePromo = "JUMAT100";
                    ViewBag.PeriodeBooking = "Setiap Jumat";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    howToGet.Add("Pesan tiket pesawat ke destinasi mana saja di situs Travorama");
                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan detail tamu (masukkan akun email yang sudah kamu gunakan saat memesan tiket pesawat di situs Travorama)");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-Voucher hotel dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar Rp 100.000 dan minimal transaksi Rp 1.200.000");
                    termList.Add("Berlaku untuk kamu yang telah memesan tiket pesawat di situs Travorama");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pemesanan kamar hotel melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Jumat");
                    termList.Add("Periode inap: kapan saja");
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
                    ViewBag.PromoDescription = "Spesial di akhir pekan, kamu bebas pilih destinasi! Pesan tiket pesawat di Travorama<br>dan dapatkan diskon tiket pesawat s.d. Rp&nbsp;200.000 untuk semua destinasi domestik.";
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

                case "sundayfunday":

                    ViewBag.Title = "Sunday Funday";
                    ViewBag.ImageSrcDesktop = "/Assets/images/campaign/2017SundayFunday/landingpage_desktop.jpg";
                    ViewBag.ImageSrcMobile = "/Assets/images/campaign/2017SundayFunday/landingpage_mobile.jpg";
                    ViewBag.ImageAlt = "Sunday Funday";
                    ViewBag.PromoTitle = "Sunday Funday";
                    ViewBag.PromoDescription = "Ke mana pun dan kapan pun liburannya, tidak perlu khawatir habis budget. Kamu bisa nikmati diskon s.d. Rp 500.000 dengan pesan kamar hotel hanya di Travorama.";
                    ViewBag.KodePromo = "SUNDAY9";
                    ViewBag.PeriodeBooking = "Setiap Minggu";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    howToGet.Add("Cari dan pesan kamar hotel melalui situs Travorama");
                    howToGet.Add("Isi data pemesan dan detail tamu");
                    howToGet.Add("Pada halaman metode pembayaran, pilih metode pembayaran yang akan digunakan");
                    howToGet.Add("Masukkan kode promo dan klik gunakan, harga akan otomatis terpotong sesuai promo");
                    howToGet.Add("Selesaikan transaksi pembayaran");
                    howToGet.Add("E-Voucher hotel dikirimkan ke kamu");
                    ViewBag.HowToGet = howToGet;

                    termList.Add("Diskon sebesar 9% (Maks. Rp 500.000) dan minimal transaksi Rp 500.000");
                    termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
                    termList.Add("Berlaku untuk pemesanan kamar hotel melalui situs Travorama");
                    termList.Add("Kode promo dapat digunakan berkali-kali selama periode promo berlangsung");
                    termList.Add("Berlaku untuk semua metode pembayaran");
                    termList.Add("Promo ini tidak dapat digabungkan dengan promo lain");
                    termList.Add("Periode booking: setiap Minggu");
                    termList.Add("Periode inap: kapan saja");
                    termList.Add("Perhitungan refund dilakukan berdasarkan nominal yang dibayar setelah dipotong diskon, bukan harga awal");
                    termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu");
                    ViewBag.Terms = termList;

                    return View("Promo");

                case "promolandingpage":
                    return View("PromoLandingPage");


                default:
                    return RedirectToAction("Index", "Index");
            }
        }
    }
}