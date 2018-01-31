using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.SmsGateway
{
    public class SmsGateway
    {

        public const string _userName = "93zbgq";
        public const string _password = "Standar1234";

        public IRestResponse SendSms(string phoneNumber, string message)
        {
            var smsGatewayClient = new RestClient("https://reguler.zenziva.net");
            var smsGatewayRequest = new RestRequest("/apps/smsapi.php", Method.GET);
            smsGatewayRequest.AddParameter("userkey", _userName, ParameterType.GetOrPost);
            smsGatewayRequest.AddParameter("passkey", _password, ParameterType.GetOrPost);
            smsGatewayRequest.AddParameter("nohp", phoneNumber, ParameterType.GetOrPost);
            smsGatewayRequest.AddParameter("pesan", message, ParameterType.GetOrPost);
            var smsGatewayResponse = smsGatewayClient.Execute(smsGatewayRequest);
            return smsGatewayResponse;
        }

        public void SendBroadcastSms(List<string>phoneNumbers, string message)
        {
            foreach(var phoneNumber in phoneNumbers)
            {
                var smsGatewayClient = new RestClient("https://reguler.zenziva.net");
                var smsGatewayRequest = new RestRequest("/apps/smsapi.php", Method.GET);
                smsGatewayRequest.AddParameter("userkey", "yi2e1z", ParameterType.GetOrPost);
                smsGatewayRequest.AddParameter("passkey", "Standar1234", ParameterType.GetOrPost);
                smsGatewayRequest.AddParameter("nohp", phoneNumber, ParameterType.GetOrPost);
                smsGatewayRequest.AddParameter("pesan", message, ParameterType.GetOrPost);
                var smsGatewayResponse = smsGatewayClient.Execute(smsGatewayRequest);
            }
        }
    }
}
