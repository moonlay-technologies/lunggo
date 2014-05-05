using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public class MailTemplateModel : TableEntity
    {
        public string Template { get; set; }
        public MailTemplateModel(){
            RowKey = "default";
        }

    }
}
