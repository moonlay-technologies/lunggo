using Lunggo.Framework.Core;
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
    internal class RazorMailTemplateEngine : MailTemplateEngine
    {
        private string _mailTable;
        private string _rowKey;
        internal override void Init(string mailTableName, string mailRowKey)
        {
            _mailTable = mailTableName;
            _rowKey = mailRowKey;
        }
        internal override string GetEmailTemplate<T>(T objectParam , string partitionKey)
        {
            var template = GetEmailTemplateByPartitionKey(partitionKey);
            var result = Razor.Parse(template, objectParam, partitionKey);
            return result;
        }
        private string GetEmailTemplateByPartitionKey(string partitionKey)
        {
            var table = TableStorageService.GetInstance().GetTableByReference(this._mailTable);
            var query = (from tabel in table.CreateQuery<MailTemplateModel>()
                         where tabel.PartitionKey == partitionKey && tabel.RowKey == this._rowKey
                         select tabel).FirstOrDefault();
            var mailTemplate = (query != null) ? query.Template : null;

            return mailTemplate;
        }
    }
}
