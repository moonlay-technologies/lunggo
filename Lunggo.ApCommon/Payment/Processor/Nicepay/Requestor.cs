using System.IO;
using System.Net;
using System.Text;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        public static string Post_Http(string SingleString, string API_url)
        {

            WebRequest request = WebRequest.Create(API_url);
            //request.Timeout = 10000
            request.Method = "POST";
            string postData = SingleString;
            byte[] byteArray = Encoding.UTF8.GetBytes(SingleString);
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;

        }
    }
}