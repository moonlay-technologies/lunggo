using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UsedDiscountTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String RsvNo
		{
		    get { return _RsvNo; }
		    set
		    {
		        _RsvNo = value;
		        IncrementLog("RsvNo");
		    }
		}
		public String Name
		{
		    get { return _Name; }
		    set
		    {
		        _Name = value;
		        IncrementLog("Name");
		    }
		}
		public String Description
		{
		    get { return _Description; }
		    set
		    {
		        _Description = value;
		        IncrementLog("Description");
		    }
		}
		public String DisplayName
		{
		    get { return _DisplayName; }
		    set
		    {
		        _DisplayName = value;
		        IncrementLog("DisplayName");
		    }
		}
		public Decimal? Percentage
		{
		    get { return _Percentage; }
		    set
		    {
		        _Percentage = value;
		        IncrementLog("Percentage");
		    }
		}
		public Decimal? Constant
		{
		    get { return _Constant; }
		    set
		    {
		        _Constant = value;
		        IncrementLog("Constant");
		    }
		}
		public String CurrencyCd
		{
		    get { return _CurrencyCd; }
		    set
		    {
		        _CurrencyCd = value;
		        IncrementLog("CurrencyCd");
		    }
		}
		public Boolean? IsFlat
		{
		    get { return _IsFlat; }
		    set
		    {
		        _IsFlat = value;
		        IncrementLog("IsFlat");
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

		
		private String _RsvNo;
		private String _Name;
		private String _Description;
		private String _DisplayName;
		private Decimal? _Percentage;
		private Decimal? _Constant;
		private String _CurrencyCd;
		private Boolean? _IsFlat;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static UsedDiscountTableRecord CreateNewInstance()
        {
            var record = new UsedDiscountTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public UsedDiscountTableRecord()
        {
            ;
        }

        static UsedDiscountTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "UsedDiscount";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("Description", false),
				new ColumnMetadata("DisplayName", false),
				new ColumnMetadata("Percentage", false),
				new ColumnMetadata("Constant", false),
				new ColumnMetadata("CurrencyCd", false),
				new ColumnMetadata("IsFlat", false),
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