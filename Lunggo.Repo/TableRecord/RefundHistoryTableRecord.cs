using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RefundHistoryTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public String RefundName
		{
		    get { return _RefundName; }
		    set
		    {
		        _RefundName = value;
		        IncrementLog("RefundName");
		    }
		}
		public String RefundDescription
		{
		    get { return _RefundDescription; }
		    set
		    {
		        _RefundDescription = value;
		        IncrementLog("RefundDescription");
		    }
		}
		public DateTime? RefundDate
		{
		    get { return _RefundDate; }
		    set
		    {
		        _RefundDate = value;
		        IncrementLog("RefundDate");
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
		public Boolean? RefundStatus
		{
		    get { return _RefundStatus; }
		    set
		    {
		        _RefundStatus = value;
		        IncrementLog("RefundStatus");
		    }
		}

		
		private String _RsvNo;
		private long? _ActivityId;
		private String _RefundName;
		private String _RefundDescription;
		private DateTime? _RefundDate;
		private Decimal? _RefundAmount;
		private Boolean? _RefundStatus;


		public static RefundHistoryTableRecord CreateNewInstance()
        {
            var record = new RefundHistoryTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RefundHistoryTableRecord()
        {
            ;
        }

        static RefundHistoryTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "RefundHistory";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("RefundName", false),
				new ColumnMetadata("RefundDescription", false),
				new ColumnMetadata("RefundDate", false),
				new ColumnMetadata("RefundAmount", false),
				new ColumnMetadata("RefundStatus", false),

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
