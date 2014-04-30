using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public class MailTemplateEngine : IMailTemplateEngine
    {
        private string _defaultMailTable = "mailtemplate";
        private string _defaultRowKey = "default";

        public string GetEmailTemplate<T>(T objectParam , string partitionKey)
        {
            try 
            {
                string template = GetEmailTemplateByPartitionKey(partitionKey);
                string result = Razor.Parse(template, objectParam);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetEmailTemplateByPartitionKey(string partitionKey)
        {
            CloudTable table = TableStorageService.GetInstance().GetTableByReference(this._defaultMailTable);
            var query = (from tabel in table.CreateQuery<MailTemplateModel>()
                         where tabel.PartitionKey == partitionKey && tabel.RowKey == this._defaultRowKey
                         select tabel).FirstOrDefault();
            string mailTemplate = (query as MailTemplateModel).Template;
            return mailTemplate;
        }
    }
}
