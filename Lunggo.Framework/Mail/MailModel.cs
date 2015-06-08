using Lunggo.Framework.SharedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public class MailModel
    {
        public string[] RecipientList { get; set; }
        public string[] CcList { get; set; }
        public string[] BccList { get; set; }
        public List<FileInfo> ListFileInfo { get; set; }
        public string Subject { get; set; }
        private string _fromMail = "System@Lunggo.com";
        public string FromMail
        {
            get { return _fromMail; }
            set { _fromMail = value; }
        }
        private string _fromName = "Lunggo System";
        public string FromName
        {
            get { return _fromName; }
            set { _fromName = value; }
        }
        
    }
}
