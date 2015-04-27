using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Lunggo.Configuration.MailTemplate;
using Microsoft.Office.Interop.Excel;

namespace Lunggo.Configuration
{
    public enum DeploymentEnvironment
    {
        Local = 5,
        Development = 6,
        Production = 7
    }

    public class ConfigGenerator
    {
        private const string FileExtension = "*.properties";
        private const string FinalProjectConfigFile = "application.properties";
        private const string RootProject = "Lunggo";
        private const string ConfigDirectoryName = "Config";
        private const string ConfigurationProject = "Configuration";
        private const string CommonDirectoryName = "Common";
        private const string LogDirectoryName = "Log";
        private const string ExcelConfigurationFile = "Lunggo_Config.xlsx";
        private const int ExcelColumnGeneratedNameIndex = 4;
        private const int ExcelRowDefaultStart = 2;
        private const int ExcelSheetNumber = 1;
        private const string FormatDate = "yyyyMMdd-HHmmss";
        
        private static readonly string ParentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private static readonly string WorkspacePath = Directory.GetParent(ParentPath).FullName;
        private static readonly string ParentConfigPath = Path.Combine(ParentPath,ConfigDirectoryName);
        private static readonly string ParentLogDirectory = Path.Combine(ParentPath,LogDirectoryName);
        private static readonly string PathExcel = Path.Combine(ParentPath, ExcelConfigurationFile);
        
        private static readonly ConfigGenerator Instance = new ConfigGenerator(); 
        private String _currentExecutionDir;
        private Dictionary<String, String> _configDictionary; 

        public static void Main(String[] args)
        {
            String[] projectList = { "BackendWeb", "CustomerWeb", "Driver", "WebJob.EmailQueueHandler", "WebJob.TicketQueueHandler", "WebAPI" };
            Console.WriteLine("####################Starting Configuration Generation");
            Console.WriteLine("####################Configuration for below projects will be generated : \n");

            foreach (var project in projectList)
            {
                Console.WriteLine(project);
            }
            Console.WriteLine();

            var generator = ConfigGenerator.GetInstance();
            generator.StartConfig(DeploymentEnvironment.Development, projectList);
            //new MailTemplateGenerator().StartMailGenerator();
            Console.WriteLine("####################Config Generation is Finished");
        }

        private ConfigGenerator()
        {
            ;
        }

        public static ConfigGenerator GetInstance()
        {
            return Instance;
        }

        public void StartConfig(DeploymentEnvironment environment, String[] projectList)
        {
            SetCurrentExecutionDirectory();
            SetConfigDictionary(GetExcelFile(),environment);
            ReadAndCopyProcess(projectList);
        }

        private void SetCurrentExecutionDirectory()
        {
            _currentExecutionDir = Path.Combine(ParentLogDirectory, DateTime.Now.ToString(FormatDate));
        }

        private void SetConfigDictionary(Range excelFile, DeploymentEnvironment environment)
        {
            _configDictionary = ReadAllRowOfExcel(excelFile, environment);
        }

        private Range GetExcelFile()
        {
            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            var xlWorkbook = xlApp.Workbooks.Open(PathExcel);
            var xlWorksheet = xlWorkbook.Sheets[ExcelSheetNumber];
            var xlRange = xlWorksheet.UsedRange;
            return xlRange;
        }

        private Dictionary<string, string> ReadAllRowOfExcel(Range xlRange, DeploymentEnvironment environment)
        {
            var dictionaryConfig = new Dictionary<string, string>();
            var excelRowCount = xlRange.Rows.Count;
            var columnValueIndex = (int) environment;

            for (var row = ExcelRowDefaultStart; row <= excelRowCount; row++)
            {
                string variableName = xlRange.Cells[row, ExcelColumnGeneratedNameIndex].Text;
                string resultValue = xlRange.Cells[row, columnValueIndex].Text;
                if (IsCellsNotNull(variableName, resultValue))
                    dictionaryConfig.Add(variableName, resultValue);
            }
            return dictionaryConfig;
        }

        private bool IsCellsNotNull(string variableName, string resultValue)
        {
            return (!string.IsNullOrEmpty(variableName) && !string.IsNullOrEmpty(resultValue));
        }

        private void ReadAndCopyProcess(IEnumerable<String> projectList)
        {
            foreach (var project in projectList)
            {
                Console.WriteLine("####################Generating Configuration for {0}", project);
                CreateProjectLogDirectory(project);
                var configFile = CreateProjectConfigurationFile(project);
                WriteConfigurationFileToProject(configFile, project);
                WriteConfigurationFileToLog(configFile, project);
                var nonConfigFiles = GetAllNonConfigFilesByProjectName(project);
                foreach (var nonConfigFile in nonConfigFiles)
                {
                    WriteNonConfigurationFileToProject(nonConfigFile, project);
                    WriteNonConfigurationFileToLog(nonConfigFile, project);
                }
                var commonFiles = GetAllCommonFiles();
                foreach (var commonFile in commonFiles)
                {
                    WriteCommonFileToProject(commonFile, project);
                    WriteCommonFileToLog(commonFile, project);
                }
            }
        }

        private void WriteConfigurationFileToProject(XmlDocument file, String projectName)
        {
            file.Save(Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", FinalProjectConfigFile));
        }

        private void WriteConfigurationFileToLog(XmlDocument file, String projectName)
        {
            file.Save(Path.Combine(_currentExecutionDir, projectName, FinalProjectConfigFile));
        }

        private void WriteNonConfigurationFileToProject(string file, String projectName)
        {
            var sourcePath = Path.Combine(WorkspacePath, RootProject + "." + ConfigurationProject, "Config", projectName, file);
            var destinationPath = Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", file);
            File.Copy(sourcePath, destinationPath, true);
        }
        private void WriteNonConfigurationFileToLog(string file, String projectName)
        {
            var sourcePath = Path.Combine(WorkspacePath, RootProject + "." + ConfigurationProject, "Config", projectName, file);
            var destinationPath = Path.Combine(_currentExecutionDir, projectName, FinalProjectConfigFile);
            File.Copy(sourcePath, destinationPath, true);
        }

        private void WriteCommonFileToProject(string file, String projectName)
        {
            var sourcePath = Path.Combine(WorkspacePath, RootProject + "." + ConfigurationProject, "Config", CommonDirectoryName, file);
            var destinationPath = Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", file);
            File.Copy(sourcePath, destinationPath, true);
        }
        private void WriteCommonFileToLog(string file, String projectName)
        {
            var sourcePath = Path.Combine(WorkspacePath, RootProject + "." + ConfigurationProject, "Config", CommonDirectoryName, file);
            var destinationPath = Path.Combine(_currentExecutionDir, projectName, file);
            File.Copy(sourcePath, destinationPath, true);
        }

        private void CreateProjectLogDirectory(String projectName)
        {
            Directory.CreateDirectory(Path.Combine(_currentExecutionDir, projectName));
        }

        private XmlDocument CreateProjectConfigurationFile(String projectName)
        {
            var appliedConfig = GetAppliedConfig(projectName);
            var doc = GenerateXmlConfigFile(appliedConfig);
            return doc;
        }

        private XmlDocument GenerateXmlConfigFile(Dictionary<String,Dictionary<String,String>> appliedConfig)
        {
            var doc = new XmlDocument();
            var xmlDeclaration = doc.CreateXmlDeclaration("1.0","UTF-8",null);
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
            var configurationRoot = doc.CreateElement("Settings");
            doc.AppendChild(configurationRoot);

            foreach (var configCollection in appliedConfig)
            {
                foreach (var config in configCollection.Value)
                {
                    var configNode = doc.CreateElement("Setting");
                    configNode.SetAttribute("file", configCollection.Key);
                    configNode.SetAttribute("name", config.Key);
                    configNode.InnerText = config.Value;
                    configurationRoot.AppendChild(configNode);
                }
            }
            return doc;
        }

        private IEnumerable<string> GetAllConfigFilesByProjectName(string projectName)
        {
            var allFilesInPath = Directory.GetFiles(Path.Combine(ParentConfigPath,projectName), FileExtension);
            return allFilesInPath;
        }

        private IEnumerable<string> GetAllNonConfigFilesByProjectName(string projectName)
        {
            var allFilesInPath = Directory.GetFiles(Path.Combine(ParentConfigPath, projectName));
            var allConfigFilesInPath = Directory.GetFiles(Path.Combine(ParentConfigPath, projectName), FileExtension);
            var allNonConfigFilesInPath = allFilesInPath.Except(allConfigFilesInPath);
            return allNonConfigFilesInPath.Select(Path.GetFileName);
        }

        private IEnumerable<string> GetAllCommonFiles()
        {
            var allFilesInPath = Directory.GetFiles(Path.Combine(ParentConfigPath, CommonDirectoryName));
            return allFilesInPath.Select(Path.GetFileName);
        }

        private Dictionary<String,Dictionary<String,String>> GetAppliedConfig(String projectName)
        {
            var appliedConfig = new Dictionary<String, Dictionary<String, String>>();
            foreach (var aFilePath in GetAllConfigFilesByProjectName(projectName))
            {
                ApplyConfigDictionary(aFilePath, appliedConfig);
            }
            return appliedConfig;
        }

        private void ApplyConfigDictionary(string configFile, IDictionary<string, Dictionary<String,String>> appliedConfig)
        {
            var allKeyAndValueOfFile = GetAllKeyAndValueFromFileInPath(configFile);
            for (var row = 0; row < allKeyAndValueOfFile.Count; row++)
            {
                var aKeyAndValue = allKeyAndValueOfFile.ElementAt(row);
                allKeyAndValueOfFile[aKeyAndValue.Key] = CheckAndReplaceValueIfKeyExistInExcelDictionary(aKeyAndValue.Value);
            }

            appliedConfig.Add(Path.GetFileNameWithoutExtension(configFile),allKeyAndValueOfFile);
        }

        private Dictionary<string, string> GetAllKeyAndValueFromFileInPath(string aFilePath)
        {
            var doc = XElement.Load(aFilePath);
            var configNodes = doc.Elements();
            var allItems = configNodes.ToDictionary(n => n.Attribute("name").Value, n => n.Value);
            return allItems;
        }

        private string CheckAndReplaceValueIfKeyExistInExcelDictionary(String variableName)
        {
            var value = IsKeyExist(variableName, _configDictionary) ? _configDictionary[variableName] : variableName;
            return value;
        }

        private bool IsKeyExist(string value, Dictionary<string, string> configDictionary)
        {
            return configDictionary.Keys.Contains(value);
        }
    }
}
