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
        public List<string> RecipientList { get; set; }
        public List<string> CCList { get; set; }
        public List<string> BCCList { get; set; }
        public List<FileInfo> ListFileInfo { get; set; }
        public string Subject { get; set; }
        private string _from_Mail = "System@Lunggo.com";
        public string From_Mail
        {
            get { return _from_Mail; }
            set { _from_Mail = value; }
        }
        private string _from_Name = "Lunggo System";
        public string From_Name
        {
            get { return _from_Name; }
            set { _from_Name = value; }
        }
        
    }
}
