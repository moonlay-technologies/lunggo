
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        private partial class TiketClientHandler
        {
            internal GetLionCaptcha GetLionCaptcha()
            {
                var client = CreateTiketClient();
                var url = "/flight_api/getLionCaptcha?token=" + token + "&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var captchaData = JsonExtension.Deserialize<GetLionCaptcha>(response.Content);
                var temp = captchaData;

                Console.WriteLine("Fisnihed Get Lion Captcha Data");
                //Operate Data which useful for Order Data
            }
        }
    }
}
