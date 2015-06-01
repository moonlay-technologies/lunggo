using Lunggo.Framework.Mail;
using Lunggo.Framework.TableStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Configuration.MailTemplate
{
    public class MailTemplateGenerator
    {
        string DefaultRowKey = "default";
        string DefaultTableName = "mailTemplate";
        string fileExtension = "*.html";
        static string parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        static string parentMailGeneratorPath = parentPath+@"\MailTemplate\TemplateFiles";
        public void StartMailGenerator()
        {
            try
            {
                string[] AllFilesInPath = GetAllTemplateFiles();
                ReadHTMLAndWriteTableFromAllFiles(AllFilesInPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string[] GetAllTemplateFiles()
        {
            string[] AllFilesInPath = System.IO.Directory.GetFiles(parentMailGeneratorPath, this.fileExtension);
            return AllFilesInPath;
        }
        
        void ReadHTMLAndWriteTableFromAllFiles(string[] AllFilesInPath)
        {
            foreach (string aFilePath in AllFilesInPath)
            {
                ReadHTMLAndWriteTableFromFile(aFilePath);
            }
        }
        void ReadHTMLAndWriteTableFromFile(string aFilePath)
        {
            KeyValuePair<string, string> FileNameAndTemplate = getFileNameAndTemplate(aFilePath);
            InsertTemplateToTable(FileNameAndTemplate);
        }
        KeyValuePair<string, string> getFileNameAndTemplate(string aFilePath)
        {
            string Template = System.IO.File.ReadAllText(aFilePath);
            string FileName = Path.GetFileNameWithoutExtension(aFilePath);
            return new KeyValuePair<string, string>(FileName,Template);
        }
        void InsertTemplateToTable(KeyValuePair<string, string> FileNameAndTemplate)
        {
            var emp = new MailTemplateModel() { Template = FileNameAndTemplate.Value, PartitionKey = FileNameAndTemplate.Key, RowKey = this.DefaultRowKey };
            ITableStorageClient tableStorageClient =  new AzureTableStorageClient();
            tableStorageClient.init("DefaultEndpointsProtocol=https;AccountName=lunggostoragedev;AccountKey=lFA73VFWzxbPnk3hCJ17KVv4TSrfwA4zjfzM8PGno95xygdUomFq+AaNsKBGLn1wfaLmQCayb7Yt5x7z8/6fOg==");
            TableStorageService.GetInstance().Init(tableStorageClient);
            TableStorageService.GetInstance().InsertOrReplaceEntityToTableStorage(emp, this.DefaultTableName);
        }
    }
}
