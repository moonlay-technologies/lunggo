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
            ViewBag.ImageSrc = "/Assets/images/campaign/TerbanginHemat/TerbanginHemat-page-mobile.jpg";
            ViewBag.ImageAlt = "Terbangin Hemat (Terbang & Nginap Hemat)";
            ViewBag.PromoTitle = "Payday Madness";
            ViewBag.PromoDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            ViewBag.PeriodeBooking = "23 - 30 Juni 2017";
            ViewBag.PeriodeInap = "Kapan Saja";
            var termList = new List<string>();
            termList.Add("Diskon sebesar 15% (Maks. Rp 150.000). Tanpa minimum transaksi");
            termList.Add("Berlaku untuk pemesanan setiap tanggal 25, 26 dan 27 setiap bulan");
            termList.Add("Periode terbang dan inap kapan saja");
            termList.Add("Berlaku untuk semua hotel di dalam dan luar negeri");
            termList.Add("Berlaku untuk pembayaran menggunakan bank transfer supported by Permata Bank");
            termList.Add("Berlaku untuk pemesanan melalui situs Travorama");
            termList.Add("Program promo hanya dapat digunakan satu kali per hari untuk satu user");
            termList.Add("Program tidak dapat digabungkan dengan voucher dan promo lainnya");
            termList.Add("Travorama berhak penuh untuk mengubah syarat dan ketentuan tanpa pemberitahuan terlebih dahulu");
            ViewBag.Terms = termList;

            switch (name.ToLower())
            {
                case "onlinerevolution":
                    return View("OnlineRevolution");
                case "onlinerevolutionwebview":
                    return View("OnlineRevolutionWebview");
                case "imlek":
                    return View("Imlek");
                case "btnterbanginhemat":
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
                    return View("PaydayMadnessPermataBank");
                case "paydaymegadeals":
                    return View("PaydayMegaDeals");
                default:
                    return RedirectToAction("Index", "Index");
            }
        }
    }
}