using Lunggo.Configuration.Config;
using Lunggo.Configuration.MailTemplate;
using Lunggo.Framework.Blob;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TableStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lunggo.Configuration
{
    class Program
    {
        void init()
        {
            IBlobStorageClient iBlobStorageClient = new AzureBlobStorageClient();
            iBlobStorageClient.init("UseDevelopmentStorage=true");
            BlobStorageService blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.init(iBlobStorageClient);

            ITableStorageClient storageClient = new AzureTableStorageClient();
            storageClient.init("UseDevelopmentStorage=true");
            TableStorageService tableStorageService = TableStorageService.GetInstance();
            tableStorageService.init(storageClient);

            IMailClient mailCLient = new MandrillMailClient();
            mailCLient.init("PqYl3epQtNkIypjknECYEw");
            MailService mailService = MailService.GetInstance();
            mailService.init(mailCLient);
        }
        
        static void Main(string[] args)
        {
            new Program().init();
            new MailTemplateGenerator().StartMailGenerator();
            //new ConfigGenerator().startConfig();
        }
        
    }
}
