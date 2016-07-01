using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Antlr3.ST;
using Microsoft.Office.Interop.Excel;

namespace Lunggo.Configuration
{
    public enum DeploymentEnvironment
    {
        Local = 5,
        DV1 = 6,
        DV2 = 7,
        QA = 8,
        Production = 9
    }

    public class ConfigGenerator
    {
        private const DeploymentEnvironment Environment = DeploymentEnvironment.DV1;
        private const bool DeployHtmlTemplate = false;
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
        private const string JsConfigTemplatePath = (@"Template\JsConfig.js");
        private const string WebConfigDebugTemplatePath = (@"Template\Web.Debug.config");
        private const string WebConfigReleaseTemplatePath = (@"Template\Web.Release.config");
        private const string AppConfigDebugTemplatePath = (@"Template\App.Debug.config");
        private const string AppConfigReleaseTemplatePath = (@"Template\App.Release.config");
        
        private static readonly string ParentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        private static readonly string WorkspacePath = Directory.GetParent(ParentPath).FullName;
        private static readonly string ParentConfigPath = Path.Combine(ParentPath,ConfigDirectoryName);
        private static readonly string ParentLogDirectory = Path.Combine(ParentPath,LogDirectoryName);
        private static readonly string PathExcel = Path.Combine(ParentPath, ExcelConfigurationFile);
        
        private static readonly ConfigGenerator Instance = new ConfigGenerator(); 
        private String _currentExecutionDir;
        private Dictionary<String, String> _configDictionary;
        private static string _azureStorageConnString;

        public static void Main(String[] args)
        {
            String[] projectList =
            {
                "BackendWeb", 
                "CustomerWeb", 
                "WebAPI", 
                "WebJob.MystiflyQueueHandler",
                "WebJob.EmailQueueHandler", 
                "WebJob.FlightProcessor",
                "WebJob.FlightCrawlScheduler", 
                "WebJob-FlightSearch.FlightCrawler1",
                "WebJob-FlightSearch.FlightCrawler2", 
                "WebJob-FlightSearch.FlightCrawler3", 
                "WebJob-FlightSearch.FlightCrawler4", 
                "WebJob-FlightSearch.FlightCrawler5",
                "WebJob-FlightSearch.FlightCrawler6",
                "CloudApp.EticketHandler",
                "WebJob.BankTransferChecking",
                "CloudApp.CaptchaReader"
            };
            Console.WriteLine("####################Starting Configuration Generation");
            Console.WriteLine("####################Configuration for below projects will be generated : \n");

            foreach (var project in projectList)
            {
                Console.WriteLine(project);
            }
            Console.WriteLine();

            var generator = ConfigGenerator.GetInstance();
            generator.StartConfig(Environment, projectList);
            if (DeployHtmlTemplate)
            HtmlTemplateGenerator.StartHtmlGenerator(_azureStorageConnString);
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
            GenerateFiles(projectList);
        }

        private void SetCurrentExecutionDirectory()
        {
            _currentExecutionDir = Path.Combine(ParentLogDirectory, DateTime.UtcNow.ToString(FormatDate));
        }

        private void SetConfigDictionary(Range excelFile, DeploymentEnvironment environment)
        {
            _configDictionary = ReadAllRowOfExcel(excelFile, environment);
            excelFile.Application.ActiveWorkbook.Close();
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
                {
                    dictionaryConfig.Add(variableName, resultValue);
                    if (variableName == "@@.*.azureStorage.connectionString@@")
                        _azureStorageConnString = resultValue;
                }
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
            if (projectName.Contains("WebJob"))
                file.Save(Path.Combine(WorkspacePath, RootProject + "." + projectName, FinalProjectConfigFile));
            else
                file.Save(Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", FinalProjectConfigFile));
        }

        private void WriteConfigurationFileToLog(XmlDocument file, String projectName)
        {
            file.Save(Path.Combine(_currentExecutionDir, projectName, FinalProjectConfigFile));
        }

        private void WriteNonConfigurationFileToProject(string file, String projectName)
        {
            var sourcePath = Path.Combine(WorkspacePath, RootProject + "." + ConfigurationProject, "Config", projectName, file);
            var destinationPath = projectName.Contains("WebJob") 
                ? Path.Combine(WorkspacePath, RootProject + "." + projectName, file)
                : Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", file);
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
            var destinationPath = projectName.Contains("WebJob")
                ? Path.Combine(WorkspacePath, RootProject + "." + projectName, "", file)
                : Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", file);
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

        public void GenerateFiles(String[] projectList)
        {
            GenerateJsConfigFile();
            GenerateWebConfigDebugFile();
            GenerateWebConfigReleaseFile();
            GenerateAppConfigDebugFile();
            GenerateAppConfigReleaseFile();
        }

        private void GenerateJsConfigFile()
        {
            var apiUrl = _configDictionary["@@.*.api.apiUrl@@"];
            var rootUrl = _configDictionary["@@.*.general.rootUrl@@"];
            var mobileUrl = "http://" + _configDictionary["@@.*.general.mobileUrl@@"];
            const string hotelPath = @"/v1/hotels";
            const string roomPath = @"/v1/rooms";
            const string flightPath = @"/v1/flight";
            const string flightSelectPath = @"/v1/flight/select";
            const string flightRevalidatePath = @"/v1/flight/revalidate";
            const string flightBookPath = @"/v1/flight/book";
            const string flightPayPath = @"/v1/payment/pay";
            const string flightRulesPath = @"/v1/flight/rules";
            const string autocompleteHotelLocationPath = @"/v1/autocomplete/hotellocation/";
            const string autocompleteAirportPath = @"/v1/autocomplete/airports/";
            const string autocompleteAirlinePath = @"/v1/autocomplete/airlines/";
            const string checkVoucherPath = @"/v1/payment/checkvoucher";
            const string subscribePath = @"/v1/newsletter/subscribe";
            const string loginPath = @"/v1/login";
            const string getProfilePath = @"/v1/profile";
            const string registerPath = @"/v1/register";
            const string resetPasswordPath = @"/v1/account/resetpassword";
            const string forgotPasswordPath = @"/v1/forgot";
            const string changePasswordPath = @"/v1/changepassword";
            const string changeProfilePath = @"/v1/profile";
            const string trxHistoryPath = @"/v1/trxhistory";
            const string getReservationPath = @"/v1/rsv/";
            const string resendConfirmationEmailPath = @"/v1/account/resendconfirmationemail";
            const string transferPaymentPath = @"/v1/payment/transferfee";
            var veritransTokenPath = _configDictionary["@@.*.veritrans.tokenEndPoint@@"];
            var veritransClientKey = _configDictionary["@@.*.veritrans.clientKey@@"];



            var fileTemplate = new StringTemplate(ReadFileToEnd(JsConfigTemplatePath));
            fileTemplate.Reset();
            fileTemplate.SetAttribute("apiUrl", apiUrl);
            fileTemplate.SetAttribute("rootUrl", rootUrl);
            fileTemplate.SetAttribute("hotelPath", hotelPath);
            fileTemplate.SetAttribute("roomPath", roomPath);
            fileTemplate.SetAttribute("flightPath", flightPath);
            fileTemplate.SetAttribute("flightSelectPath", flightSelectPath);
            fileTemplate.SetAttribute("flightRevalidatePath", flightRevalidatePath);
            fileTemplate.SetAttribute("flightBookPath", flightBookPath);
            fileTemplate.SetAttribute("flightPayPath", flightPayPath);
            fileTemplate.SetAttribute("flightRulesPath", flightRulesPath);
            fileTemplate.SetAttribute("autocompleteHotelLocationPath", autocompleteHotelLocationPath);
            fileTemplate.SetAttribute("autocompleteAirportPath", autocompleteAirportPath);
            fileTemplate.SetAttribute("autocompleteAirlinePath", autocompleteAirlinePath);
            fileTemplate.SetAttribute("checkVoucherPath", checkVoucherPath);
            fileTemplate.SetAttribute("subscribePath", subscribePath);
            fileTemplate.SetAttribute("trxHistoryPath", trxHistoryPath);
            fileTemplate.SetAttribute("getReservationPath", getReservationPath);
            fileTemplate.SetAttribute("loginPath", loginPath);
            fileTemplate.SetAttribute("getProfilePath", getProfilePath);
            fileTemplate.SetAttribute("registerPath", registerPath);
            fileTemplate.SetAttribute("resetPasswordPath", resetPasswordPath);
            fileTemplate.SetAttribute("forgotPasswordPath", forgotPasswordPath);
            fileTemplate.SetAttribute("changePasswordPath", changePasswordPath);
            fileTemplate.SetAttribute("changeProfilePath", changeProfilePath);
            fileTemplate.SetAttribute("resendConfirmationEmailPath", resendConfirmationEmailPath);
            fileTemplate.SetAttribute("veritransTokenPath", veritransTokenPath);
            fileTemplate.SetAttribute("veritransClientKey", veritransClientKey);
            fileTemplate.SetAttribute("transferPaymentPath", transferPaymentPath);
            fileTemplate.SetAttribute("mobileUrl", mobileUrl);

            var fileContent = fileTemplate.ToString();
            string[] projectList = { "BackendWeb", "CustomerWeb" };
            SaveFile("JsConfig.js", fileContent, projectList);
        }

        private void GenerateWebConfigDebugFile()
        {
            var mystiflyApiEndPoint = _configDictionary["@@.*.mystifly.apiEndPoint@@"];

            var fileTemplate = new StringTemplate(ReadFileToEnd(WebConfigDebugTemplatePath));
            fileTemplate.Reset();
            fileTemplate.SetAttribute("mystiflyApiEndPoint", mystiflyApiEndPoint);

            var fileContent = fileTemplate.ToString();
            string[] projectList = { "BackendWeb", "CustomerWeb", "WebAPI" };
            SaveRootFile("Web.Debug.config", fileContent, projectList);
        }

        private void GenerateWebConfigReleaseFile()
        {
            var mystiflyApiEndPoint = _configDictionary["@@.*.mystifly.apiEndPoint@@"];

            var fileTemplate = new StringTemplate(ReadFileToEnd(WebConfigReleaseTemplatePath));
            fileTemplate.Reset();
            fileTemplate.SetAttribute("mystiflyApiEndPoint", mystiflyApiEndPoint);

            var fileContent = fileTemplate.ToString();
            string[] projectList = { "BackendWeb", "CustomerWeb", "WebAPI" };
            SaveRootFile("Web.Release.config", fileContent, projectList);
        }

        private void GenerateAppConfigDebugFile()
        {
            var mystiflyApiEndPoint = _configDictionary["@@.*.mystifly.apiEndPoint@@"];

            var fileTemplate = new StringTemplate(ReadFileToEnd(AppConfigDebugTemplatePath));
            fileTemplate.Reset();
            fileTemplate.SetAttribute("mystiflyApiEndPoint", mystiflyApiEndPoint);

            var fileContent = fileTemplate.ToString();
            string[] projectList =
            {
                "WebJob.EmailQueueHandler", 
                "WebJob.MystiflyQueueHandler",
                "WebJob.FlightProcessor",
                "WebJob.FlightCrawlScheduler",
                "WebJob-FlightSearch.FlightCrawler1", 
                "WebJob-FlightSearch.FlightCrawler2", 
                "WebJob-FlightSearch.FlightCrawler3", 
                "WebJob-FlightSearch.FlightCrawler4",
                "WebJob-FlightSearch.FlightCrawler5",
                "WebJob-FlightSearch.FlightCrawler6"
            };
            SaveRootFile("App.Debug.config", fileContent, projectList);
        }

        private void GenerateAppConfigReleaseFile()
        {
            var mystiflyApiEndPoint = _configDictionary["@@.*.mystifly.apiEndPoint@@"];

            var fileTemplate = new StringTemplate(ReadFileToEnd(AppConfigReleaseTemplatePath));
            fileTemplate.Reset();
            fileTemplate.SetAttribute("mystiflyApiEndPoint", mystiflyApiEndPoint);

            var fileContent = fileTemplate.ToString();
            string[] projectList =
            {
                "WebJob.EmailQueueHandler",
                "WebJob.MystiflyQueueHandler",
                "WebJob.FlightProcessor",
                "WebJob.FlightCrawlScheduler",
                "WebJob-FlightSearch.FlightCrawler1",
                "WebJob-FlightSearch.FlightCrawler2",
                "WebJob-FlightSearch.FlightCrawler3",
                "WebJob-FlightSearch.FlightCrawler4",
                "WebJob-FlightSearch.FlightCrawler5",
                "WebJob-FlightSearch.FlightCrawler6"
            };
            SaveRootFile("App.Release.config", fileContent, projectList);
        }

        private static String ReadFileToEnd(String fileName)
        {
            using (var templateFile = new StreamReader(fileName))
            {
                try
                {
                    return templateFile.ReadToEnd();
                }
                finally
                {
                    templateFile.Close();
                }
            }
        }

        private void SaveFile(string fileName, string fileContent, IEnumerable<string> projectNames)
        {
            foreach (var projectName in projectNames)
            {
                File.WriteAllText(Path.Combine(WorkspacePath, RootProject + "." + projectName, "Config", fileName),
                    fileContent);
            }
        }

        private void SaveRootFile(string fileName, string fileContent, IEnumerable<string> projectNames)
        {
            foreach (var projectName in projectNames)
            {
                File.WriteAllText(Path.Combine(WorkspacePath, RootProject + "." + projectName, fileName),
                    fileContent);
            }
        }
    }
}
