using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class PriceTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public long? OrderId
		{
		    get { return _OrderId; }
		    set
		    {
		        _OrderId = value;
		        IncrementLog("OrderId");
		    }
		}
		public long? MarginId
		{
		    get { return _MarginId; }
		    set
		    {
		        _MarginId = value;
		        IncrementLog("MarginId");
		    }
		}
		public Decimal? SupplierPrice
		{
		    get { return _SupplierPrice; }
		    set
		    {
		        _SupplierPrice = value;
		        IncrementLog("SupplierPrice");
		    }
		}
		public String SupplierCurrencyCd
		{
		    get { return _SupplierCurrencyCd; }
		    set
		    {
		        _SupplierCurrencyCd = value;
		        IncrementLog("SupplierCurrencyCd");
		    }
		}
		public Decimal? SupplierRate
		{
		    get { return _SupplierRate; }
		    set
		    {
		        _SupplierRate = value;
		        IncrementLog("SupplierRate");
		    }
		}
		public Decimal? OriginalPriceIdr
		{
		    get { return _OriginalPriceIdr; }
		    set
		    {
		        _OriginalPriceIdr = value;
		        IncrementLog("OriginalPriceIdr");
		    }
		}
		public Decimal? MarginNominal
		{
		    get { return _MarginNominal; }
		    set
		    {
		        _MarginNominal = value;
		        IncrementLog("MarginNominal");
		    }
		}
		public Decimal? FinalPriceIdr
		{
		    get { return _FinalPriceIdr; }
		    set
		    {
		        _FinalPriceIdr = value;
		        IncrementLog("FinalPriceIdr");
		    }
		}
		public Decimal? LocalPrice
		{
		    get { return _LocalPrice; }
		    set
		    {
		        _LocalPrice = value;
		        IncrementLog("LocalPrice");
		    }
		}
		public String LocalCurrencyCd
		{
		    get { return _LocalCurrencyCd; }
		    set
		    {
		        _LocalCurrencyCd = value;
		        IncrementLog("LocalCurrencyCd");
		    }
		}
		public Decimal? LocalRate
		{
		    get { return _LocalRate; }
		    set
		    {
		        _LocalRate = value;
		        IncrementLog("LocalRate");
		    }
		}
		public String InsertBy
		{
		    get { return _InsertBy; }
		    set
		    {
		        _InsertBy = value;
		        IncrementLog("InsertBy");
		    }
		}
		public DateTime? InsertDate
		{
		    get { return _InsertDate; }
		    set
		    {
		        _InsertDate = value;
		        IncrementLog("InsertDate");
		    }
		}
		public String InsertPgId
		{
		    get { return _InsertPgId; }
		    set
		    {
		        _InsertPgId = value;
		        IncrementLog("InsertPgId");
		    }
		}
		public String UpdateBy
		{
		    get { return _UpdateBy; }
		    set
		    {
		        _UpdateBy = value;
		        IncrementLog("UpdateBy");
		    }
		}
		public DateTime? UpdateDate
		{
		    get { return _UpdateDate; }
		    set
		    {
		        _UpdateDate = value;
		        IncrementLog("UpdateDate");
		    }
		}
		public String UpdatePgId
		{
		    get { return _UpdatePgId; }
		    set
		    {
		        _UpdatePgId = value;
		        IncrementLog("UpdatePgId");
		    }
		}

		
		private long? _Id;
		private long? _OrderId;
		private long? _MarginId;
		private Decimal? _SupplierPrice;
		private String _SupplierCurrencyCd;
		private Decimal? _SupplierRate;
		private Decimal? _OriginalPriceIdr;
		private Decimal? _MarginNominal;
		private Decimal? _FinalPriceIdr;
		private Decimal? _LocalPrice;
		private String _LocalCurrencyCd;
		private Decimal? _LocalRate;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static PriceTableRecord CreateNewInstance()
        {
            var record = new PriceTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public PriceTableRecord()
        {
            ;
        }

        static PriceTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Price";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("OrderId", false),
				new ColumnMetadata("MarginId", false),
				new ColumnMetadata("SupplierPrice", false),
				new ColumnMetadata("SupplierCurrencyCd", false),
				new ColumnMetadata("SupplierRate", false),
				new ColumnMetadata("OriginalPriceIdr", false),
				new ColumnMetadata("MarginNominal", false),
				new ColumnMetadata("FinalPriceIdr", false),
				new ColumnMetadata("LocalPrice", false),
				new ColumnMetadata("LocalCurrencyCd", false),
				new ColumnMetadata("LocalRate", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),

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
