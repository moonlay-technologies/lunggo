using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Payment.Cache;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Processor;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Database;
using Lunggo.Framework.Pattern;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        private readonly PaymentProcessorService _processor;
        private readonly PaymentDbService _db;
        private readonly PaymentCacheService _cache;

        public PaymentService() : this(null)
        {

        }

        internal PaymentService(
            PaymentProcessorService paymentProcessorService = null,
            PaymentDbService paymentDbService = null,
            PaymentCacheService paymentCacheService = null)
        {
            _cache = paymentCacheService ?? new PaymentCacheService();
            _db = paymentDbService ?? new PaymentDbService();
            _processor = paymentProcessorService ?? new PaymentProcessorService();

            //Currency.SyncCurrencyData();
        }

        private static PaymentMedium GetPaymentMedium(PaymentMethod method, PaymentSubmethod submethod)
        {
            switch (method)
            {
                case PaymentMethod.BankTransfer:
                    //case PaymentMethod.Credit:
                    //case PaymentMethod.Deposit:
                    return PaymentMedium.Direct;
                case PaymentMethod.CreditCard:
                case PaymentMethod.MandiriClickPay:
                    return PaymentMedium.Veritrans;
                case PaymentMethod.VirtualAccount:
                    return PaymentMedium.Nicepay;
                case PaymentMethod.CimbClicks:
                case PaymentMethod.XlTunai:
                case PaymentMethod.TelkomselTcash:
                case PaymentMethod.IbMuamalat:
                case PaymentMethod.EpayBri:
                case PaymentMethod.DanamonOnlineBanking:
                case PaymentMethod.IndosatDompetku:
                case PaymentMethod.SbiiOnlineShopping:
                case PaymentMethod.BcaKlikpay:
                case PaymentMethod.DooEtQnb:
                case PaymentMethod.BtnMobileBanking:
                    return PaymentMedium.E2Pay;
                default:
                    return PaymentMedium.Undefined;
            }
        }

        public List<Surcharge> GetSurchargeList()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
            var clientId = identity.Claims.Single(claim => claim.Type == "Client ID").Value;
            var platform = Client.GetPlatformType(clientId);
            return platform == PlatformType.DesktopWebsite || platform == PlatformType.MobileWebsite
                ? new List<Surcharge>()
                {
                    new Surcharge
                    {
                        PaymentMethod = PaymentMethod.CreditCard,
                        Percentage = 2.5M,
                        Constant = 0
                    }
                }
                : new List<Surcharge>()
                {
                    new Surcharge
                    {
                        PaymentMethod = PaymentMethod.CreditCard,
                        Percentage = 0M,
                        Constant = 0
                    }
                };
        }

        public string GetBankBranch(string transferAccount)
        {
            switch (transferAccount)
            {
                case "1020006675802":
                    return "KC Jakarta Sudirman";
                default:
                    return null;
            }
        }

        public string GetBankName(PaymentMedium medium, PaymentMethod method, PaymentSubmethod submethod)
        {
            if (medium == PaymentMedium.Direct &&
                method == PaymentMethod.BankTransfer &&
                submethod == PaymentSubmethod.Mandiri)
                return "Bank Mandiri";

            if (medium == PaymentMedium.Veritrans &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Permata)
                return "Bank Permata";

            if (medium == PaymentMedium.Veritrans &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Other)
                return "Bank Permata";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.BCA)
                return "BCA";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Mandiri)
                return "Bank Mandiri";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.BNI)
                return "BNI";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Maybank)
                return "BII Maybank";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Permata)
                return "Bank Permata";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.KEBHana)
                return "KEB Hana Bank";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.BRI)
                return "BRI";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.CIMB)
                return "CIMB Niaga";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Danamon)
                return "Bank Danamon";

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Other)
                return "Bank Permata";

            return null;
        }

        public List<Tuple<string, List<string>>> GetInstruction(RsvPaymentDetails payment)
        {
            var medium = payment.Medium;
            var method = payment.Method;
            var submethod = payment.Submethod;
            var price = payment.FinalPriceIdr.ToString("####");
            var account = payment.TransferAccount;

            if (medium == PaymentMedium.Direct &&
                method == PaymentMethod.BankTransfer &&
                submethod == PaymentSubmethod.Mandiri)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Transfer</b>",
                        "Pilih <b>Ke Rekening Mandiri</b>",
                        "Input nomor rekening: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile/SMS/Internet Banking", new List<string>
                    {
                        "Lakukan seperti anda mentransfer ke <b>Rekening Mandiri</b> pada umumnya",
                        "Input nomor rekening: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Selesai"
                    }),
                };

            #endregion

            if (medium == PaymentMedium.Veritrans &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Permata)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("Jaringan ATM Bersama", new List<string>
                    {
                        "Masukkan PIN Anda.",
                        "Di Menu Utama, pilih <b>Transaksi Lainnya.</b>",
                        "Pilih <b>Transfer</b>.",
                        "Pilih <b>Antar Bank Online</b>.",
                        "Masukkan nomor <b>013 " + account + "</b> (kode 013 dan 16 digit nomor akun virtual pembayaran).",
                        "Masukkan jumlah harga tiket yang harus Anda bayar <b>Rp " + price + "</b> (jika jumlah yang dimasukkan tidak sama dengan jumlah tagihan, transaksi akan ditolak).",
                        "No. referensi dapat dikosongkan, lalu tekan <b>Benar</b>.",
                        "Di halaman konfirmasi transfer akan muncul jumlah yang dibayarkan, no. rekening tujuan. Jika informasinya telah cocok, tekan <b>Benar</b>.",
                        "Transaksi berhasil.",
                    }),
                    new Tuple<string, List<string>>("Jaringan ATM PRIMA", new List<string>
                    {
                        "Masukkan PIN Anda.",
                        "Di Menu Utama, pilih <b>Transaksi Lainnya</b>.",
                        "Pilih <b>Transfer</b>.",
                        "Pilih <b>Ke Rekening Bank Lain</b>.",
                        "Masukkan kode <b>013</b> (Kode Bank Permata) lalu tekan <b>Benar</b>.",
                        "Masukkan jumlah harga tiket yang harus Anda bayar <b>Rp " + price + "</b> (jika jumlah yang dimasukkan tidak sama dengan jumlah tagihan, transaksi akan ditolak).",
                        "Masukkan <b>" + account + "</b> (16 digit nomor akun virtual pembayaran) lalu tekan <b>Benar</b>.",
                        "Di halaman konfirmasi transfer akan muncul jumlah yang dibayarkan, no. rekening tujuan. Jika informasinya telah cocok, tekan <b>Benar</b>.",
                        "Transaksi berhasil.",
                    }),
                    new Tuple<string, List<string>>("Jaringan ATM Alto", new List<string>
                    {
                        "Masukkan PIN Anda.",
                        "Di Menu Utama, pilih <b>Transaksi Lainnya</b>.",
                        "Pilih <b>Transaksi Pembayaran</b>.",
                        "Pilih <b>Pembayaran Virtual Account</b>.",
                        "Masukkan 16 digit nomor akun virtual pembayaran <b>" + account + "</b>.",
                        "Di halaman konfirmasi transfer akan muncul jumlah yang dibayarkan, no. rekening tujuan. Jika informasinya telah cocok, tekan <b>Benar</b>.",
                        "Transaksi berhasil.",
                    })
                };

            #endregion

            if (medium == PaymentMedium.Veritrans &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Other)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Transfer</b>",
                        "Pilih <b>Bank Lainnya</b>",
                        "Input Kode Bank <b>013</b> atau pilih bank <b>Permata Bank</b>",
                        "Input nomor rekening: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile/SMS/Internet Banking", new List<string>
                    {
                        "Lakukan seperti anda mentransfer ke <b>Bank Lain</b> pada umumnya",
                        "Input <b>013</b> sebagai Kode Bank atau pilih bank <b>Permata Bank</b>",
                        "Input nomor rekening: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Selesai"
                    }),
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.BCA)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Transaksi Lainnya</b>",
                        "Pilih <b>Transfer</b>",
                        "Pilih <b>Ke rekening BCA Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai"
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Login Mobile Banking",
                        "Pilih <b>m-Transfer</b>",
                        "Pilih <b>BCA Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Klik <b>Send</b>",
                        "Informasi VA akan ditampilkan",
                        "Klik <b>OK</b>",
                        "Input <b>PIN</b> Mobile Banking",
                        "Bukti bayar ditampilkan",
                        "Selesai"
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih <b>Transaksi Dana</b>",
                        "Pilih <b>Transfer Ke BCA Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Klik <b>Lanjutkan</b>",
                        "Input <b>Respon KeyBCA Appli 1</b>",
                        "Klik <b>Kirim</b>",
                        "Bukti bayar ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Mandiri)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Bayar/Beli</b>",
                        "Pilih <b>Lainnya</b>",
                        "Pilih <b>Multi Payment</b>",
                        "Input <b>70014</b> sebagai <b>Kode Institusi</b>",
                        "Input Virtual Account Number: <b>" + account + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai"
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Login Mobile Banking",
                        "Pilih <b>Bayar</b>",
                        "Pilih <b>Lainnya</b>",
                        "Input <b>Transferpay</b> sebagai <b>Penyedia Jasa</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Lanjut</b>",
                        "Input <b>OTP</b> dan <b>PIN</b>",
                        "Pilih <b>OK</b>",
                        "Bukti bayar ditampilkan",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih <b>Bayar</b>",
                        "Pilih <b>Multi Payment</b>",
                        "Input <b>Transferpay</b> sebagai <b>Penyedia Jasa</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Ceklis <b>IDR</b>",
                        "Klik <b>Lanjutkan</b>",
                        "Bukti bayar ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.BNI)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih <b>Menu Lain</b>",
                        "Pilih <b>Menu Transfer</b>",
                        "Pilih <b>Ke Rekening BNI</b>",
                        "Masukkan Nominal: <b>" + price + "</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil Bukti Pembayaran Anda.",
                        "Selesai.",
                    }),
                    new Tuple<string, List<string>>("SMS Banking", new List<string>
                    {
                        "Masuk Aplikasi SMS Banking BNI.",
                        "Pilih Menu <b>Transfer</b>",
                        "Pilih <b>Trf Rekening BNI</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Masukkan Jumlah Tagihan: <b>" + price + "</b>",
                        "Pilih <b>Proses</b>",
                        "Pada Pop Up Message, Pilih <b>Setuju</b>",
                        "Anda Akan Mendapatkan Pesan Konfirmasi.",
                        "Masukkan 2 Angka Dari <b>PIN Anda</b> Dengan Mengikuti Petunjuk Yang Tertera Pada Pesan.",
                        "Bukti Pembayaran Ditampilkan.",
                        "Selesai.",
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Pilih <b>Transfer</b>",
                        "Pilih <b>Antar Rekening BNI</b>",
                        "Pilih <b>Rekening Tujuan</b>",
                        "Pilih <b>Input Rekening Baru</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Klik <b>Lanjut</b>",
                        "Klik <b>Lanjut</b>",
                        "Masukkan Nominal Tagihan: <b>" + price + "</b>",
                        "Klik <b>Lanjut</b>",
                        "Periksa Detail Konfirmasi. Pastikan Data Sudah Benar.",
                        "Jika Sudah Benar, Masukkan <b>Password Transaksi</b>",
                        "Klik <b>Lanjut</b>",
                        "Bukti Pembayaran Ditampilkan.",
                        "Selesai.",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Masuk Internet Banking.",
                        "Pilih <b>Transaksi</b>",
                        "Pilih <b>Info dan Administrasi</b>",
                        "Pilih <b>Atur Rekening Tujuan</b>",
                        "Pilih <b>Tambah Rekening Tujuan</b>",
                        "Klik <b>Ok</b>",
                        "Masukkan Nomor Order Sebagai <b>Nama Singkat</b>: <b>Travorama</b>",
                        "Masukkan Nomor Virtual Account Sebagai Nomor Rekening: <b>" + account + "</b>",
                        "Lengkapi Semua Data Yang Diperlukan.",
                        "Klik <b>Lanjutkan</b>",
                        "Masukkan <b>Kode Otentikasi Token</b> lalu, <b>Proses</b>",
                        "Rekening Tujuan Berhasil Ditambahkan.",
                        "Pilih <b>Menu Transfer</b>",
                        "Pilih <b>Transfer Antar Rek. BNI</b>",
                        "Pilih Rekening Tujuan dengan <b>Nama Singkat</b> Yang Sudah Anda Tambahkan: <b>Travorama</b>",
                        "Masukkan Nominal: <b>" + price + "</b>",
                        "Masukkan <b>Kode Otentikasi Token</b>",
                        "Bukti Pembayaran Ditampilkan.",
                        "Selesai."
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Maybank)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Pembayaran/Top Up Pulsa</b>",
                        "Pilih <b>Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("SMS Banking", new List<string>
                    {
                        "SMS ke <b>69811</b>",
                        "Ketik <b>TRANSFER " + account + " " + price + "</b>",
                        "Kirim SMS",
                        "Anda akan mendapat balasan <b>Transfer dr rek &lt;nomor rekening anda&gt; ke rek " + account + " sebesar Rp. " + price + " Ketik &lt;karakter acak&gt;</b>",
                        "Balas SMS tersebut, ketik <b>&lt;karakter acak&gt;</b>",
                        "Kirim SMS",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih <b>Rekening dan Transaksi</b>",
                        "Pilih <b>Maybank Virtual Account</b>",
                        "Pilih <b>Sumber Tabungan</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Klik <b>Submit</b>",
                        "Input <b>SMS Token</b>",
                        "Bukti bayar ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Permata)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Transaksi Lainnya</b>",
                        "Pilih <b>Pembayaran</b>",
                        "Pilih <b>Pembayaran Lain-lain</b>",
                        "Pilih <b>Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Select <b>Benar</b>",
                        "Select <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Login Mobile Banking",
                        "Pilih <b>Pembayaran Tagihan</b>",
                        "Pilih <b>Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Input Nominal misal: <b>" + price + "</b>",
                        "Klik <b>Kirim</b>",
                        "Input <b>Token</b>",
                        "Klik <b>Kirim</b>",
                        "Bukti bayar akan ditampilkan",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih <b>Pembayaran Tagihan</b>",
                        "Pilih <b>Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Klik <b>Kirim</b>",
                        "Input <b>Token</b>",
                        "Klik <b>Kirim</b>",
                        "Bukti bayar akan ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.KEBHana)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Pembayaran</b>",
                        "Pilih <b>Lainnya</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih menu <b>Transfer</b> kemudian Pilih <b>Withdrawal Account Information</b>",
                        "Pilih <b>Account Number</b> anda",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Click <b>Submit</b>",
                        "Input <b>SMS Pin</b>",
                        "Bukti bayar akan ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.BRI)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Transaksi Lain</b>",
                        "Pilih Menu <b>Pembayaran</b>",
                        "Pilih Menu <b>Lain-lain</b>",
                        "Pilih Menu <b>BRIVA</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Login BRI Mobile",
                        "Pilih <b>Mobile Banking BRI</b>",
                        "Pilih Menu <b>Info</b>",
                        "Pilih Menu <b>BRIVA</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Masukkan Nominal: <b>" + price + "</b>",
                        "Klik <b>Kirim</b>",
                        "Masukkan <b>PIN Mobile</b>",
                        "Klik <b>Kirim</b>",
                        "Bukti bayar akan dikirim melalui sms",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih <b>Pembayaran</b>",
                        "Pilih <b>BRIVA</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Klik <b>Kirim</b>",
                        "Masukkan <b>Password</b>",
                        "Masukkan <b>mToken</b>",
                        "Klik <b>Kirim</b>",
                        "Bukti bayar akan ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.CIMB)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Pembayaran</b>",
                        "Pilih Menu <b>Lanjut</b>",
                        "Pilih Menu <b>Virtual Account</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Proses</b>",
                        "Data Virtual Account akan ditampilkan",
                        "Pilih <b>Proses</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Login Go Mobile",
                        "Pilih Menu <b>Transfer</b>",
                        "Pilih Menu <b>Other Rekening Ponsel/CIMB Niaga</b>",
                        "Pilih Sumber Dana yang akan digunakan",
                        "Pilih <b>Casa</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Masukkan Nominal: <b>" + price + "</b>",
                        "Klik <b>Lanjut</b>",
                        "Data Virtual Account akan ditampilkan",
                        "Masukkan <b>PIN Mobile</b>",
                        "Klik <b>Konfirmasi</b>",
                        "Bukti bayar akan dikirim melalui sms",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Internet Banking", new List<string>
                    {
                        "Login Internet Banking",
                        "Pilih <b>Bayar Tagihan</b>",
                        "Rekening Sumber - Pilih yang akan Anda digunakan",
                        "Jenis Pembayaran - Pilih <b>Virtual Account</b>",
                        "Untuk Pembayaran - Pilih <b>Masukkan Nomor Virtual Account</b>",
                        "Nomor Rekening Virtual: <b>" + price + "</b>",
                        "Isi <b>Remark</b> Jika diperlukan",
                        "Klik <b>Lanjut</b>",
                        "Data Virtual Account akan ditampilkan",
                        "Masukkan <b>mPIN</b>",
                        "Klik <b>Kirim</b>",
                        "Bukti bayar akan ditampilkan",
                        "Selesai"
                    })
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Danamon)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Pembayaran</b>",
                        "Pilih <b>Lainnya</b>",
                        "Pilih Menu <b>Virtual Account</b>",
                        "Input Nomor Virtual Account: <b>" + account + "</b>",
                        "Pilih <b>Benar</b>",
                        "Data Virtual Account akan ditampilkan",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile Banking", new List<string>
                    {
                        "Login D-Mobile",
                        "Pilih menu <b>Pembayaran</b>",
                        "Pilih menu <b>Virtual Account</b>",
                        "Pilih <b>Tambah Biller Baru Pembayaran</b>",
                        "Tekan <b>Lanjut</b>",
                        "Masukkan Nomor Virtual Account: <b>" + account + "</b>",
                        "Tekan <b>Ajukan</b>",
                        "Data Virtual Account akan ditampilkan",
                        "Masukkan <b>mPIN</b>",
                        "Pilih <b>Konfirmasi</b>",
                        "Bukti bayar akan dikirim melalui sms",
                        "Selesai"
                    }),
                };
            #endregion

            if (medium == PaymentMedium.Nicepay &&
                method == PaymentMethod.VirtualAccount &&
                submethod == PaymentSubmethod.Other)

                #region Content

                return new List<Tuple<string, List<string>>>
                {
                    new Tuple<string, List<string>>("ATM", new List<string>
                    {
                        "Pilih Menu <b>Transfer</b>",
                        "Pilih <b>Bank Lainnya</b>",
                        "Input Kode Bank <b>013</b> atau pilih bank <b>Permata Bank</b>",
                        "Input nomor rekening: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Pilih <b>Benar</b>",
                        "Pilih <b>Ya</b>",
                        "Ambil bukti bayar anda",
                        "Selesai",
                    }),
                    new Tuple<string, List<string>>("Mobile/SMS/Internet Banking", new List<string>
                    {
                        "Lakukan seperti anda mentransfer ke <b>Bank Lain</b> pada umumnya",
                        "Input <b>013</b> sebagai Kode Bank atau pilih bank <b>Permata Bank</b>",
                        "Input nomor rekening: <b>" + account + "</b>",
                        "Input Nominal: <b>" + price + "</b>",
                        "Selesai"
                    }),
                };
            #endregion

            return null;
        }
    }
}
