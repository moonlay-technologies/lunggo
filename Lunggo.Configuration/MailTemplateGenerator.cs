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
                TableStorageService.GetInstance().Init("DefaultEndpointsProtocol=https;AccountName=lunggostoragedv1;AccountKey=b4+GLz5z4ySRlzaXucbvShtxSXXmkJlQid5rR6HgxCUeKIF47oUGzsBxGPwrKZrOkzpUyIOcJsS3wO8k8rY9nQ==");
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
            TableStorageService.GetInstance().InsertOrReplaceEntityToTableStorage(emp, this.DefaultTableName);
        }
    }
}
