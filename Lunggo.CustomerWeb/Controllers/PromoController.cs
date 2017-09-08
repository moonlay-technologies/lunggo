using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class PromoController : Controller
    {
        [Route("{langCode}/Promo/{name}")]
        public ActionResult Promo(string name)
        {

            switch (name.ToLower())
            {
                case "onlinerevolution":
                    return View("OnlineRevolution");
                case "onlinerevolutionwebview":
                    return View("OnlineRevolutionWebview");
                case "imlek":
                    return View("Imlek");
                case "btnterbanginhemat":
                    ViewBag.ImageSrc = "/Assets/images/campaign/TerbanginHemat/TerbanginHemat-page-mobile.jpg";
                    ViewBag.ImageAlt = "Terbangin Hemat (Terbang & Nginap Hemat)";
                    ViewBag.PromoTitle = "BTN Terbangin Hemat";
                    ViewBag.PromoDescription = "Spesial untuk pemegang Kartu Debit dan Kartu Kredit BTN, nikmati diskon hingga Rp 300.000 setiap reservasi tiket pesawat dan kamar hotel di seluruh dunia. Buat perjalanan kamu lebih hemat dengan menggunakan Kartu Debit dan Kartu Kredit BTN.";
                    ViewBag.PeriodeBooking = "1 Februari - 31 Maret 2017";
                    ViewBag.PeriodeInap = "Kapan Saja";

                    ViewBag.Terms = new List<string>
                    {
                        "Diskon sebesar 15% (Maks. Rp 150.000). Tanpa minimum transaksi",
                        "Berlaku untuk pemesanan setiap tanggal 25, 26 dan 27 setiap bulan",
                        "Periode terbang dan inap kapan saja",
                        "Berlaku untuk semua hotel di dalam dan luar negeri",
                        "Berlaku untuk pembayaran menggunakan bank transfer supported by Permata Bank",
                        "Berlaku untuk pemesanan melalui situs Travorama",
                        "Program promo hanya dapat digunakan satu kali per hari untuk satu user",
                        "Program tidak dapat digabungkan dengan voucher dan promo lainnya",
                        "Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu"
                    };
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
                case "paydaymadnesspermatabank":

                    ViewBag.Title = "Payday Madness Permata Bank";
                    ViewBag.ImageSrc = "/Assets/images/campaign/PaydayMadness/PaydayMadness-page-mobile.jpg";
                    ViewBag.ImageAlt = "Payday Madness Permata Bank";
                    ViewBag.PromoTitle = "Payday Madness Permata Bank";
                    ViewBag.PromoDescription = "Yeay payday time! Saatnya memanjakan diri bersama keluarga atau sahabat dengan menginap di hotel-hotel terbaik pilihan kamu. Nikmati diskon 15% maksimal Rp 150.000 untuk setiap reservasi kamar hotel di Indonesia dan luar negeri dengan menggunakan metode pembayaran transfer bank melalui ATM Bank Permata.";
                    ViewBag.PeriodeBooking = "25 - 27 Agustus 2017";
                    ViewBag.PeriodeInap = "Kapan Saja";


                    ViewBag.Terms = new List<string> {
                        "Diskon sebesar 15% (Maks. Rp 150.000). Tanpa minimum transaksi",
                        "Berlaku untuk pemesanan hotel setiap tanggal <span class=\"os-bold blue-txt\">25, 26, dan 27</span> setiap bulan",
                        "Periode inap kapan saja",
                        "Berlaku untuk semua hotel di dalam dan luar negeri",
                        "Berlaku untuk pembayaran menggunakan bank transfer Payday Madness",
                        "Berlaku untuk pemesanan melalui situs Travorama",
                        "Program promo hanya dapat digunakan satu kali per hari untuk satu user",
                        "Program tidak dapat digabungkan dengan voucher dan promo lainnya",
                        "Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu",
                    };

                    ViewBag.HowToGet = new List<string>
                    {
                        "Cari dan pesan kamar hotel melalui situs Travorama",
                        "Pilih metode pembayaran bank transfer Payday Madness",
                        "Harga akan otomatis terpotong sesuai program promo",
                        "Lakukan pembayaran sesuai dengan instruksi",
                        "Selesaikan proses pembayaran",
                        "E-voucher hotel akan dikirimkan ke alamat email Anda"
                    };

                    return View("PaydayMadnessPermataBank");
                case "paydaymegadeals":
                    return View("PaydayMegaDeals");
                default:
                    return RedirectToAction("Index", "Index");
            }
        }
    }
}