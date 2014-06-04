using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lunggo.Configuration.Config
{
    public class ConfigGenerator
    {
        Dictionary<string, int> VersionIndex = new Dictionary<string, int>() { { "local", 5 }, { "development", 6 }, { "production", 7 } };
        string usedVersion = "local";
        static string parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        static string parentConfigPath = parentPath + @"\Config";
        static string parentResultPath = @"\ConfigLog";
        string pathExcel = parentPath + @"\Lunggo_Config.xlsx";
        string[] directoryPaths = { @"\CustomerWeb", @"\Framework" };
        string fileExtension = "*.properties";
        int ExcelColumnGeneratedNameIndex = 4;
        int ExcelRowDefaultStart = 2;
        int ExcelSheetNumber = 1;
        string formatDate = "ddMMyyyyhhmmss";
        public void startConfig()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Range TheExcelFile = GetExcelFile();
                Dictionary<string, string> DictionaryConfig = readAllRowOfExcel(TheExcelFile);
                DateTime Date = DateTime.Now;
                ReadAndCopyProcess(DictionaryConfig, Date.ToString(formatDate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        Microsoft.Office.Interop.Excel.Range GetExcelFile()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(this.pathExcel);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[this.ExcelSheetNumber];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            return xlRange;
        }
        Dictionary<string, string> readAllRowOfExcel(Microsoft.Office.Interop.Excel.Range xlRange)
        {
            try
            {
                Dictionary<string, string> DictionaryConfig = new Dictionary<string, string>();
                int excelRowCount = xlRange.Rows.Count;
                int ColumnValueIndex = VersionIndex[this.usedVersion];
                for (int row = this.ExcelRowDefaultStart; row <= excelRowCount; row++)
                {
                    string VariableName = xlRange.Cells[row, this.ExcelColumnGeneratedNameIndex].Value2;
                    string ResultValue = xlRange.Cells[row, ColumnValueIndex].Value2;
                    if (isCellsNotNull(VariableName, ResultValue))
                        DictionaryConfig.Add(VariableName, ResultValue);
                }
                return DictionaryConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        bool isCellsNotNull(string VariableName, string ResultValue)
        {
            return (!string.IsNullOrEmpty(VariableName) && !string.IsNullOrEmpty(ResultValue));
        }
        void ReadAndCopyProcess(Dictionary<string, string> DictionaryConfig, string DateTime)
        {
            foreach (string directoryPath in this.directoryPaths)
            {
                string[] AllFilesInPath = getAllPathFilesByDirectoryName(directoryPath);
                ReadAndWriteXMLInAllFiles(AllFilesInPath, DictionaryConfig, DateTime);
            }
        }
        string[] getAllPathFilesByDirectoryName(string directoryPath)
        {
            string[] AllFilesInPath = System.IO.Directory.GetFiles(parentConfigPath + directoryPath, this.fileExtension);
            return AllFilesInPath;
        }
        void ReadAndWriteXMLInAllFiles(string[] AllFilesInPath, Dictionary<string, string> DictionaryConfig, string DateTime)
        {
            foreach (string aFilePath in AllFilesInPath)
            {
                ReadAndWriteXMLInFile(aFilePath, DictionaryConfig, DateTime);
            }
        }
        void ReadAndWriteXMLInFile(string aFilePath, Dictionary<string, string> DictionaryConfig, string DateTime)
        {
            Dictionary<string, string> AllKeyAndValueOfFile = getAllKeyAndValueFromFileInPath(aFilePath);
            for (int row = 0; row < AllKeyAndValueOfFile.Count; row++)
            {
                KeyValuePair<string, string> aKeyAndValue = AllKeyAndValueOfFile.ElementAt(row);
                AllKeyAndValueOfFile[aKeyAndValue.Key] = CheckAndReplaceValueIfKeyExistInExcelDictionary(aKeyAndValue, DictionaryConfig);
            }
            WriteNexXMLFile(aFilePath, AllKeyAndValueOfFile, DateTime);
        }
        Dictionary<string, string> getAllKeyAndValueFromFileInPath(string aFilePath)
        {
            var doc = XDocument.Load(aFilePath);
            var rootNodes = doc.Root.DescendantNodes().OfType<XElement>();
            var allItems = rootNodes.ToDictionary(n => n.Attribute("name").Value, n => n.Value);
            return allItems;
        }
        string CheckAndReplaceValueIfKeyExistInExcelDictionary(KeyValuePair<string, string> KeyAndValue, Dictionary<string, string> DictionaryConfig)
        {
            string value = IfKeyExist(KeyAndValue.Value, DictionaryConfig) ? DictionaryConfig.Where(x => x.Key == KeyAndValue.Value).Select(x => x.Value).FirstOrDefault() : KeyAndValue.Value;
            return value;
        }
        bool IfKeyExist(string value, Dictionary<string, string> DictionaryConfig)
        {
            bool ifExist = DictionaryConfig.Keys.Contains(value);
            return ifExist;
        }
        void WriteNexXMLFile(string aFilePath, Dictionary<string, string> AllKeyAndValueOfFiles, string DateTime)
        {
            XmlDocument doc = GenerateXMLFile(AllKeyAndValueOfFiles);
            string path = generatePathToSave(aFilePath,DateTime);
            createNewPathIfDirectoryNotExistByFilePath(path);
            doc.Save(path);
        }
        XmlDocument GenerateXMLFile(Dictionary<string, string> AllKeyAndValueOfFiles)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement parent = doc.CreateElement("xml");
            foreach (var Element in AllKeyAndValueOfFiles)
            {
                XmlElement id = doc.CreateElement("Setting");
                id.SetAttribute("name", Element.Key);
                id.InnerText = Element.Value;
                parent.AppendChild(id);
            }
            doc.AppendChild(parent);
            return doc;
        }
        string generatePathToSave(string aFilePath, string DateTime)
        {
            string filename = Path.GetFileName(aFilePath);
            string FullDirectoryPathOfFile = aFilePath.Replace(filename,"");
            string SubConfigDirectory = FullDirectoryPathOfFile.Replace(parentConfigPath, "").Replace(@"\","");
            string ProjectParentConfigPath = parentConfigPath.Replace("Lunggo.Configuration", "Lunggo." + SubConfigDirectory);
            string newPath =ProjectParentConfigPath+@"\"+filename;
            //string subConfigDirectory = aFilePath.Replace(parentConfigPath, "");
            return newPath;
            //return aFilePath.Replace(FullDirectoryPathOfFile, ProjectParentConfigPath + parentResultPath + DateTime+@"\"+SubConfigDirectory+"-");
        }
        void createNewPathIfDirectoryNotExistByFilePath(string filepath)
        {
            string directoryPath = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }
    }
}
