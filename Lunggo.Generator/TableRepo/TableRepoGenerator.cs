using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Dapper;
using System.IO;
using Antlr3.ST;


namespace Lunggo.Generator.TableRepo
{
    class TableRepoGenerator
    {
        private static StringTemplate _tableRepoTemplate;
        private static StringTemplate _tableRecordTemplate;
        private static readonly String tableRepoTemplateFileName = @"Template\TableRepo.txt";
        private static readonly String tableRecordTemplateFileName = @"Template\TableRecord.txt";
        private readonly String _connectionString;
        private readonly String _xmlConfigurationPath;
        public String ConnectionString
        {
            get { return _connectionString; }
            set
            {
                throw new InvalidOperationException("Connection String can only be set through constructor");
            }
        }

        public String XmlConfigurationPath
        {
            get { return _xmlConfigurationPath; }
            set
            {
                throw new InvalidOperationException("XML Configuration path can only be set through constructor");
            }
        }

        static TableRepoGenerator()
        {
               Init();
        }
        
        static void Main(string[] args)
        {
            //String connectionString = @"Data Source=MARINABAY\SQL2012DC;Initial Catalog=Lunggo;User ID=sa;Password=Admin-pwd";
            //String connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=""D:\Bayu\Lunggo\Lunggo.Driver\dodol.mdf"";Integrated Security=True;";
            //String connectionString = @"Data Source=""playdb.cloudapp.net, 63778"";Initial Catalog=Travorama;Persist Security Info=True;User ID=developer;Password=Standar1234";
            //String connectionString = @"Server=tcp:esk54ibs1w.database.windows.net,1433;Database=travorama-qa;User ID=developer@esk54ibs1w;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            String connectionString = @"Server=tcp:travorama-development-sql-server.database.windows.net,1433;Database=travorama-dv2;User ID=developer@travorama-development-sql-server;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            String destinationPath = String.Empty;
            String configurationXmlPath = @"test.xml";
            
            var tableRepoGenerator = new TableRepoGenerator(connectionString,configurationXmlPath);
            tableRepoGenerator.Generate(destinationPath);
        }

        public TableRepoGenerator(String connectionString, String xmlConfigurationPath)
        {
            _connectionString = connectionString;
            _xmlConfigurationPath = xmlConfigurationPath;
        }

        public void Generate(String destinationPath)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                IEnumerable<String> tableNames = GetTableNames();
                foreach (var tableName in tableNames)
                {
                    CreateFiles(conn, tableName, destinationPath);
                }
            }
        }

        private IEnumerable<String> GetTableNames()
        {
            return ReadXmlConfiguration();
        }

        private IEnumerable<String> ReadXmlConfiguration()
        {
            XElement configFiles = XElement.Load(XmlConfigurationPath);
            IEnumerable<XElement> tables = configFiles.Elements();
            return tables.Select(p => p.Attribute("name").Value);
        }

        private static void Init()
        {
            InitTableRecordTemplate();
            InitTableRepoTemplate();
        }

        private static void InitTableRepoTemplate()
        {
            _tableRepoTemplate = new StringTemplate(ReadFileToEnd(tableRepoTemplateFileName));
        }

        private static void InitTableRecordTemplate()
        {
            _tableRecordTemplate = new StringTemplate(ReadFileToEnd(tableRecordTemplateFileName));
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

        private void CreateFiles(IDbConnection conn,String tableName, String destinationPath)
        {
            var columnDefinitions = GetColumnDefinitions(conn, tableName);
            var typeList = GetMappedColumnsDefinition(columnDefinitions).ToList();
            WriteTableRepoFile(tableName, destinationPath);
            WriteTableRecordFile(typeList, tableName, destinationPath);
        }

        private void WriteTableRepoFile(String tableName, String destinationPath)
        {
            var model = new
            {
                TableName = tableName,
            };
            _tableRepoTemplate.Reset();
            _tableRepoTemplate.SetAttribute("TableName",tableName);

            var fileContent = _tableRepoTemplate.ToString();
            SaveFile(fileContent, GetTableRepoFileName(tableName));
        }

        private void WriteTableRecordFile(IEnumerable<ColumnDefinition> typeList, String tableName, String destinationPath)
        {
            var model = new
            {
                TableName = tableName,
                ColumnsDefinition = typeList,
            };

            var dummyList = new List<Lala>
            {
                new Lala {ColumnName = "Id", ColumnType = "int?"},
                new Lala {ColumnName = "HireDate", ColumnType = "Datetime?"},
                new Lala {ColumnName = "Blob", ColumnType = "Byte[]"}, 
                new Lala {ColumnName = "Amount", ColumnType = "long?"}, 
            };

            _tableRecordTemplate.Reset();
            _tableRecordTemplate.SetAttribute("TableName", tableName);
            _tableRecordTemplate.SetAttribute("orders", typeList);

            var fileContent = _tableRecordTemplate.ToString();
            Console.WriteLine(fileContent);
            SaveFile(fileContent, GetTableRecordFileName(tableName));
        }

        private String GetTableRecordFileName(String tableName)
        {
            var nameBuilder = new StringBuilder().AppendFormat("{0}TableRecord.cs", tableName);
            return nameBuilder.ToString();
        }

        private String GetTableRepoFileName(String tableName)
        {
            var nameBuilder = new StringBuilder().AppendFormat("{0}Repository.cs", tableName);
            return nameBuilder.ToString();
        }

        private void SaveFile(String fileContent, String destinationPath)
        {
            File.WriteAllText(@"..\..\" + destinationPath,fileContent);
        }

        private IEnumerable<ColumnDefinition> GetMappedColumnsDefinition(IEnumerable<InformationSchemaColumnDefinition> originColumnsDefinition )
        {
            var columnsDefinition = originColumnsDefinition.Select(
                definition => new ColumnDefinition
                {
                    ColumnType = MapToClrType(definition), 
                    OriginDefinition = definition
                }
            ).ToList();

            return columnsDefinition;
        }

        private String MapToClrType(InformationSchemaColumnDefinition definition)
        {
            String sqlType = definition.DATA_TYPE;
            String clrType = String.Empty;
            if (sqlType.Equals("bigint", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "long?";
            }
            else if (sqlType.Equals("binary", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Byte[]";
            }
            else if (sqlType.Equals("bit", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Boolean?";
            }
            else if (sqlType.Equals("date", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "DateTime?";
            }
            else if (sqlType.Equals("datetime", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "DateTime?";
            }
            else if (sqlType.Equals("datetime2", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "DateTime?";
            }
            else if (sqlType.Equals("datetimeoffset", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "DateTimeOffset?";
            }
            else if (sqlType.Equals("decimal", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Decimal?";
            }
            else if (sqlType.Equals("float", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Double?";
            }
            else if (sqlType.Equals("int", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "int?";
            }
            else if (sqlType.Equals("money", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Decimal?";
            }
            else if (sqlType.Equals("nchar", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "String";
            }
            else if (sqlType.Equals("numeric", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Decimal?";
            }
            else if (sqlType.Equals("nvarchar", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "String";
            }
            else if (sqlType.Equals("real", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Single?";
            }
            else if (sqlType.Equals("rowversion", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Byte[]";
            }
            else if (sqlType.Equals("smallint", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "short?";
            }
            else if (sqlType.Equals("smallmoney", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Decimal?";
            }
            else if (sqlType.Equals("sql_variant", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Object";
            }
            else if (sqlType.Equals("time", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "TimeSpan?";
            }
            else if (sqlType.Equals("tinyint", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Byte?";
            }
            else if (sqlType.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Guid?";
            }
            else if (sqlType.Equals("varbinary", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Byte[]";
            }
            else if (sqlType.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase))
            {
                clrType = "Byte[]";
            }
            else
            {
                var messageBuilder = new StringBuilder();
                messageBuilder.AppendFormat("Sql Type {0} is not supported", sqlType);
                throw new InvalidDataException(messageBuilder.ToString());
            }
            return clrType;
        }

        private IEnumerable<InformationSchemaColumnDefinition> GetColumnDefinitions(IDbConnection conn, String tableName)
        {
            var columnsDefinition = GetBasicColumnsDefinition(conn,tableName).ToList();
            SetPrimaryKeys(columnsDefinition, conn, tableName);
            return columnsDefinition;
        }

        private IEnumerable<InformationSchemaColumnDefinition> GetBasicColumnsDefinition(IDbConnection conn, String tableName)
        {
            var queryString = CreateBasicColumnDefinitionsQueryString();
            var queryParams = new
            {
                TABLE_NAME = tableName
            };
            return SqlMapper.Query<InformationSchemaColumnDefinition>(conn, queryString, queryParams as object, null, true, null, CommandType.Text); 
        }

        private void SetPrimaryKeys(List<InformationSchemaColumnDefinition> columnsDefinition, IDbConnection conn, String tableName)
        {
            IEnumerable<String> pkColumns = GetPrimaryKeyColumns(conn, tableName);
            foreach (var pkName in pkColumns)
            {
                var colDef = columnsDefinition.Find(p => p.COLUMN_NAME == pkName);
                if (colDef != null)
                {
                    colDef.IsPrimaryKey = true;    
                }
            }
        }

        private IEnumerable<String> GetPrimaryKeyColumns(IDbConnection conn, String tableName)
        {
            var queryString = CreatePrimaryKeysDefinitionQueryString();
            var queryParams = new
            {
                TABLE_NAME = tableName
            };
            return SqlMapper.Query(conn, queryString, queryParams, null, true, null, CommandType.Text).Select(p => (String) p.ColumnName);
        }

        private String CreatePrimaryKeysDefinitionQueryString()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                "SELECT ColumnName = col.column_name FROM information_schema.table_constraints tc INNER JOIN information_schema.key_column_usage col ");
            queryBuilder.Append("ON col.Constraint_Name = tc.Constraint_Name AND col.Constraint_schema = tc.Constraint_schema ");
            queryBuilder.AppendFormat("WHERE tc.Constraint_Type = 'Primary Key' AND col.Table_name = @TABLE_NAME");
            return queryBuilder.ToString();
        }

        private String CreateBasicColumnDefinitionsQueryString()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLE_NAME");
            return queryBuilder.ToString();
        }
    }

    public class Lala
    {
        public String ColumnType { get; set; }
        public String ColumnName { get; set; }
    }
}
