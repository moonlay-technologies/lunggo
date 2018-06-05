using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RefundCustomerTableRecord : Lunggo.Framework.Database.TableRecord
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
		public Decimal? RefundAmount
		{
		    get { return _RefundAmount; }
		    set
		    {
		        _RefundAmount = value;
		        IncrementLog("RefundAmount");
		    }
		}
		public String RefundStatus
		{
		    get { return _RefundStatus; }
		    set
		    {
		        _RefundStatus = value;
		        IncrementLog("RefundStatus");
		    }
		}
		public DateTime? RefundProcessDate
		{
		    get { return _RefundProcessDate; }
		    set
		    {
		        _RefundProcessDate = value;
		        IncrementLog("RefundProcessDate");
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

		
		private String _RsvNo;
		private Decimal? _RefundAmount;
		private String _RefundStatus;
		private DateTime? _RefundProcessDate;
		private DateTime? _UpdateDate;


		public static RefundCustomerTableRecord CreateNewInstance()
        {
            var record = new RefundCustomerTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RefundCustomerTableRecord()
        {
            ;
        }

        static RefundCustomerTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "RefundCustomer";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("RefundAmount", false),
				new ColumnMetadata("RefundStatus", false),
				new ColumnMetadata("RefundProcessDate", false),
				new ColumnMetadata("UpdateDate", false),

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
