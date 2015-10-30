using System;
using System.Collections.Generic;
using System.IO;
using Lunggo.Framework.Mail;
using Lunggo.Framework.TableStorage;

namespace Lunggo.Configuration
{
    public class HtmlTemplateGenerator
    {
        private const string DefaultRowKey = "default";
        private const string FileExtension = "*.cshtml";
        static readonly string ParentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        static readonly string ParentTemplateGeneratorPath = ParentPath+@"\HtmlTemplate";
        public static void StartHtmlGenerator(string azureStorageConnString)
        {
            TableStorageService.GetInstance().Init(azureStorageConnString);
            var allFilesInPath = GetAllTemplateFiles();
            ReadHtmlAndWriteTableFromAllFiles(allFilesInPath);
        }
        static IEnumerable<string> GetAllTemplateFiles()
        {
            string[] allFilesInPath = System.IO.Directory.GetFiles(ParentTemplateGeneratorPath, FileExtension);
            return allFilesInPath;
        }
        
        static void ReadHtmlAndWriteTableFromAllFiles(IEnumerable<string> allFilesInPath)
        {
            foreach (string aFilePath in allFilesInPath)
            {
                ReadHtmlAndWriteTableFromFile(aFilePath);
            }
        }
        static void ReadHtmlAndWriteTableFromFile(string aFilePath)
        {
            KeyValuePair<string, string> fileNameAndTemplate = GetFileNameAndTemplate(aFilePath);
            InsertTemplateToTable(fileNameAndTemplate);
        }
        static KeyValuePair<string, string> GetFileNameAndTemplate(string aFilePath)
        {
            string template = System.IO.File.ReadAllText(aFilePath);
            string fileName = Path.GetFileNameWithoutExtension(aFilePath).ToLower();
            return new KeyValuePair<string, string>(fileName,template);
        }
        static void InsertTemplateToTable(KeyValuePair<string, string> fileNameAndTemplate)
        {
            var emp = new MailTemplateModel() { Template = fileNameAndTemplate.Value, PartitionKey = fileNameAndTemplate.Key, RowKey = DefaultRowKey };
            TableStorageService.GetInstance().InsertOrReplaceEntityToTableStorage(emp, "HtmlTemplate");
        }
    }
}
