using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace WebJob3
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config =
                new JobHostConfiguration(
                    "DefaultEndpointsProtocol=https;AccountName=lunggostorageprod;AccountKey=hxf/W4AzJPk4aIO4w1qxTxCRGBSJvBDOfhkcWD+T0O4/G2lYcbSeBzj2qLKtEF9pf1CYNh0cBgsTzYu615CEUQ==");
            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
