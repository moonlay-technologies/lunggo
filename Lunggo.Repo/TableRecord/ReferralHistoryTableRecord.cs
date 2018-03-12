using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ReferralHistoryTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String UserId
		{
		    get { return _UserId; }
		    set
		    {
		        _UserId = value;
		        IncrementLog("UserId");
		    }
		}
		public String ReferreeId
		{
		    get { return _ReferreeId; }
		    set
		    {
		        _ReferreeId = value;
		        IncrementLog("ReferreeId");
		    }
		}
		public String History
		{
		    get { return _History; }
		    set
		    {
		        _History = value;
		        IncrementLog("History");
		    }
		}
		public Decimal ReferralCredit
		{
		    get { return _ReferralCredit; }
		    set
		    {
		        _ReferralCredit = value;
		        IncrementLog("ReferralCredit");
		    }
		}
		public DateTime? TimeStamp
		{
		    get { return _TimeStamp; }
		    set
		    {
		        _TimeStamp = value;
		        IncrementLog("TimeStamp");
		    }
		}

		
		private String _UserId;
		private String _ReferreeId;
		private String _History;
		private Decimal _ReferralCredit;
		private DateTime? _TimeStamp;


		public static ReferralHistoryTableRecord CreateNewInstance()
        {
            var record = new ReferralHistoryTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ReferralHistoryTableRecord()
        {
            ;
        }

        static ReferralHistoryTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ReferralHistory";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("ReferreeId", false),
				new ColumnMetadata("History", false),
				new ColumnMetadata("ReferralCredit", false),
				new ColumnMetadata("TimeStamp", false),

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
