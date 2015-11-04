using Microsoft.WindowsAzure.Storage.Table;

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
