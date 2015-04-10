using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravolutionaryWebServiceTest.Travolutionary.WebService;

namespace TravolutionaryWebServiceTest
{
    class ProgramOriginal
    {
        

        private static string Login(string userName, string password)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var loginRequest = new DynamicDataServiceRqst
                {
                    TypeOfService = ServiceType.Unknown,
                    RequestType = ServiceRequestType.Login,
                    Credentials = new Credentials
                    {
                        UserName = userName,
                        Password = password,
                    },
                };

                var loginResponse = cli.ServiceRequest(loginRequest);

                if (loginResponse.Errors != null && loginResponse.Errors.Any())
                {
                    foreach (var error in loginResponse.Errors)
                    {
                        Console.Error.WriteLine("Login Failed.");
                        Console.Out.WriteLine(error.Message);
                    }
                    return null;
                }

                return loginResponse.SessionID;
            }
        }
    }
}
