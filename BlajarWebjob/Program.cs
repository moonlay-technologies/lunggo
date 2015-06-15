using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace BlajarWebjob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var configuration = new JobHostConfiguration
            {
                DashboardConnectionString = "DefaultEndpointsProtocol=https;AccountName=lunggostoragedev;AccountKey=lFA73VFWzxbPnk3hCJ17KVv4TSrfwA4zjfzM8PGno95xygdUomFq+AaNsKBGLn1wfaLmQCayb7Yt5x7z8/6fOg==",
                StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=lunggostoragedev;AccountKey=lFA73VFWzxbPnk3hCJ17KVv4TSrfwA4zjfzM8PGno95xygdUomFq+AaNsKBGLn1wfaLmQCayb7Yt5x7z8/6fOg==",
            };
            var host = new JobHost(configuration);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
