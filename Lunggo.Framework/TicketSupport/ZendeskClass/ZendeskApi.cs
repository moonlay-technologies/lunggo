using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Lunggo.Framework.SharedModel;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public interface IZendeskApi
    {
        ITickets Tickets { get; }
        IAttachments Attachments { get; }

        string ZendeskUrl { get; }
    }

    public class ZendeskApi : IZendeskApi
    {
        public ITickets Tickets { get; set; }
        public IAttachments Attachments { get; set; }

        public string ZendeskUrl { get; set; }

        /// <summary>
        /// Constructor that takes 3 params.
        /// </summary>
        /// <param name="yourZendeskUrl">Will be formated to "https://yoursite.zendesk.com/api/v2"</param>
        /// <param name="user"></param>
        /// <param name="password">LEAVE BLANK IF USING TOKEN</param>
        /// <param name="apiToken">Optional Param which is used if specified instead of the password</param>
        public ZendeskApi(string yourZendeskUrl, string user, string password = "", string apiToken = "")
        {
            var formattedUrl = GetFormattedZendeskUrl(yourZendeskUrl).AbsoluteUri;

            Tickets = new Tickets(formattedUrl, user, password, apiToken);
            Attachments = new Attachments(formattedUrl, user, password, apiToken);

            ZendeskUrl = formattedUrl;
        }

                /// <summary>
        /// Constructor that takes 3 params.
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="yourZendeskUrl">Will be formated to "https://yoursite.zendesk.com/api/v2"</param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public ZendeskApi(IWebProxy proxy, string yourZendeskUrl, string user, string password)
            : this(yourZendeskUrl, user, password)
        {
            if (proxy == null) return;

            ((Tickets)Tickets).Proxy = proxy;
            ((Attachments)Attachments).Proxy = proxy;
        }
        Uri GetFormattedZendeskUrl(string yourZendeskUrl)
        {
            yourZendeskUrl = yourZendeskUrl.ToLower();

            //Make sure the Authority is https://
            if (yourZendeskUrl.StartsWith("http://"))
                yourZendeskUrl = yourZendeskUrl.Replace("http://", "https://");

            if (!yourZendeskUrl.StartsWith("https://"))
                yourZendeskUrl = "https://" + yourZendeskUrl;

            if (!yourZendeskUrl.EndsWith("/api/v2"))
            {
                //ensure that url ends with ".zendesk.com/api/v2"
                yourZendeskUrl = yourZendeskUrl.Split(new[] { ".zendesk.com" }, StringSplitOptions.RemoveEmptyEntries)[0] + ".zendesk.com/api/v2";
            }

            return new Uri(yourZendeskUrl);
        }

        public string GetLoginUrl(string name, string email, string authenticationToken, string returnToUrl = "")
        {
            string url = string.Format("{0}/access/remoteauth/", ZendeskUrl);

            string timestamp = GetUnixEpoch(DateTime.UtcNow).ToString();


            string message = string.Format("{0}|{1}|||||{2}|{3}", name, email, authenticationToken, timestamp);
            //string message = name + email + token + timestamp;
            string hash = Md5(message);

            string result = url + "?name=" + HttpUtility.UrlEncode(name) +
                            "&email=" + HttpUtility.UrlEncode(email) +
                            "&timestamp=" + timestamp +
                            "&hash=" + hash;

            if (returnToUrl.Length > 0)
                result += "&return_to=" + returnToUrl;

            return result;
        }

        private double GetUnixEpoch(DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() -
                           new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalSeconds;
        }


        public string Md5(string strChange)
        {
            //Change the syllable into UTF8 code
            byte[] pass = Encoding.UTF8.GetBytes(strChange);

            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(pass);
            string strPassword = ByteArrayToHexString(md5.Hash);
            return strPassword;
        }

        public string ByteArrayToHexString(byte[] Bytes)
        {
            // important bit, you have to change the byte array to hex string or zenddesk will reject
            StringBuilder Result;
            string HexAlphabet = "0123456789abcdef";

            Result = new StringBuilder();

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }

    }
}
