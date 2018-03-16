using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ReferralCreditTableRecord : Lunggo.Framework.Database.TableRecord
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
		public Decimal? ReferralCredit
		{
		    get { return _ReferralCredit; }
		    set
		    {
		        _ReferralCredit = value;
		        IncrementLog("ReferralCredit");
		    }
		}
		public DateTime? ExpDate
		{
		    get { return _ExpDate; }
		    set
		    {
		        _ExpDate = value;
		        IncrementLog("ExpDate");
		    }
		}

		
		private String _UserId;
		private Decimal? _ReferralCredit;
		private DateTime? _ExpDate;


		public static ReferralCreditTableRecord CreateNewInstance()
        {
            var record = new ReferralCreditTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ReferralCreditTableRecord()
        {
            ;
        }

        static ReferralCreditTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ReferralCredit";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("ReferralCredit", false),
				new ColumnMetadata("ExpDate", false),

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
