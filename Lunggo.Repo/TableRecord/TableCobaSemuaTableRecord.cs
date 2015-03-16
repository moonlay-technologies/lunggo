using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Repository.TableRecord
{
    public class TableCobaSemuaTableRecord: Lunggo.Framework.Database.TableRecord
    {

        private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

        public static TableCobaSemuaTableRecord CreateNewInstance()
        {
            var record = new TableCobaSemuaTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

        private Int64 _Id;
        public Int64 Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                IncrementLog("Id");
            }
        }
        private Byte[] _binaryType;
        public Byte[] binaryType
        {
            get { return _binaryType; }
            set
            {
                _binaryType = value;
                IncrementLog("binaryType");
            }
        }
        private Boolean _bitType;
        public Boolean bitType
        {
            get { return _bitType; }
            set
            {
                _bitType = value;
                IncrementLog("bitType");
            }
        }
        private DateTime _date;
        public DateTime date
        {
            get { return _date; }
            set
            {
                _date = value;
                IncrementLog("date");
            }
        }
        private DateTime _datetimeType;
        public DateTime datetimeType
        {
            get { return _datetimeType; }
            set
            {
                _datetimeType = value;
                IncrementLog("datetimeType");
            }
        }
        private DateTime _datetime2Type;
        public DateTime datetime2Type
        {
            get { return _datetime2Type; }
            set
            {
                _datetime2Type = value;
                IncrementLog("datetime2Type");
            }
        }
        private DateTimeOffset _datetimeoffsetType;
        public DateTimeOffset datetimeoffsetType
        {
            get { return _datetimeoffsetType; }
            set
            {
                _datetimeoffsetType = value;
                IncrementLog("datetimeoffsetType");
            }
        }
        private Decimal _decimalType;
        public Decimal decimalType
        {
            get { return _decimalType; }
            set
            {
                _decimalType = value;
                IncrementLog("decimalType");
            }
        }
        private Double _floatType;
        public Double floatType
        {
            get { return _floatType; }
            set
            {
                _floatType = value;
                IncrementLog("floatType");
            }
        }
        private Int32 _intType;
        public Int32 intType
        {
            get { return _intType; }
            set
            {
                _intType = value;
                IncrementLog("intType");
            }
        }
        private Decimal _moneyType;
        public Decimal moneyType
        {
            get { return _moneyType; }
            set
            {
                _moneyType = value;
                IncrementLog("moneyType");
            }
        }
        private String _ncharType;
        public String ncharType
        {
            get { return _ncharType; }
            set
            {
                _ncharType = value;
                IncrementLog("ncharType");
            }
        }
        private Decimal _numericType;
        public Decimal numericType
        {
            get { return _numericType; }
            set
            {
                _numericType = value;
                IncrementLog("numericType");
            }
        }
        private String _nvarcharType;
        public String nvarcharType
        {
            get { return _nvarcharType; }
            set
            {
                _nvarcharType = value;
                IncrementLog("nvarcharType");
            }
        }
        private Char _nvarchar1Type;
        public Char nvarchar1Type
        {
            get { return _nvarchar1Type; }
            set
            {
                _nvarchar1Type = value;
                IncrementLog("nvarchar1Type");
            }
        }
        private Char _nchar1Type;
        public Char nchar1Type
        {
            get { return _nchar1Type; }
            set
            {
                _nchar1Type = value;
                IncrementLog("nchar1Type");
            }
        }
        private Single _realType;
        public Single realType
        {
            get { return _realType; }
            set
            {
                _realType = value;
                IncrementLog("realType");
            }
        }
        private Byte[] _rowversionType;
        public Byte[] rowversionType
        {
            get { return _rowversionType; }
            set
            {
                _rowversionType = value;
                IncrementLog("rowversionType");
            }
        }
        private Int16 _smallintType;
        public Int16 smallintType
        {
            get { return _smallintType; }
            set
            {
                _smallintType = value;
                IncrementLog("smallintType");
            }
        }
        private Decimal _smallmoneyType;
        public Decimal smallmoneyType
        {
            get { return _smallmoneyType; }
            set
            {
                _smallmoneyType = value;
                IncrementLog("smallmoneyType");
            }
        }
        private TimeSpan _timeType;
        public TimeSpan timeType
        {
            get { return _timeType; }
            set
            {
                _timeType = value;
                IncrementLog("timeType");
            }
        }
        private Byte _tinyintType;
        public Byte tinyintType
        {
            get { return _tinyintType; }
            set
            {
                _tinyintType = value;
                IncrementLog("tinyintType");
            }
        }
        private Guid _uniqueidentifierType;
        public Guid uniqueidentifierType
        {
            get { return _uniqueidentifierType; }
            set
            {
                _uniqueidentifierType = value;
                IncrementLog("uniqueidentifierType");
            }
        }
        private Byte[] _varbinaryType;
        public Byte[] varbinaryType
        {
            get { return _varbinaryType; }
            set
            {
                _varbinaryType = value;
                IncrementLog("varbinaryType");
            }
        }
        private Byte[] _varbinary1Type;
        public Byte[] varbinary1Type
        {
            get { return _varbinary1Type; }
            set
            {
                _varbinary1Type = value;
                IncrementLog("varbinary1Type");
            }
        }
        private Byte[] _binary1Type;
        public Byte[] binary1Type
        {
            get { return _binary1Type; }
            set
            {
                _binary1Type = value;
                IncrementLog("binary1Type");
            }
        }

        private TableCobaSemuaTableRecord()
        {
            ;
        }

        static TableCobaSemuaTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "TableCobaSemua";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
        {
            new ColumnMetadata("Id", true),
            new ColumnMetadata("binaryType", false),
            new ColumnMetadata("bitType",false),
            new ColumnMetadata("date", false),
            new ColumnMetadata("datetimeType", false),
            new ColumnMetadata("datetime2Type", true),
            new ColumnMetadata("datetimeoffsetType", false),
            new ColumnMetadata("decimalType",false),
            new ColumnMetadata("floatType", false),
            new ColumnMetadata("intType", false),
            new ColumnMetadata("moneyType", true),
            new ColumnMetadata("ncharType", false),
            new ColumnMetadata("numericType",false),
            new ColumnMetadata("nvarcharType", false),
            new ColumnMetadata("nvarchar1Type", false),
            new ColumnMetadata("nchar1Type", true),
            new ColumnMetadata("realType", false),
            new ColumnMetadata("rowversionType",false),
            new ColumnMetadata("smallintType", false),
            new ColumnMetadata("smallmoneyType", false),
            new ColumnMetadata("timeType", true),
            new ColumnMetadata("tinyintType", false),
            new ColumnMetadata("uniqueidentifierType",false),
            new ColumnMetadata("varbinaryType", false),
            new ColumnMetadata("varbinary1Type", false),
            new ColumnMetadata("binary1Type", false)
        };
        }

        private static void InitPrimaryKeysMetadata()
        {
            _primaryKeys = _recordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }

        public override List<ColumnMetadata> GetMetadata()
        {
            return _recordMetadata;
        }

        public override string GetTableName()
        {
            return _tableName;
        }

        public override List<ColumnMetadata> GetPrimaryKeys()
        {
            return _primaryKeys;
        }
        
    }
}
