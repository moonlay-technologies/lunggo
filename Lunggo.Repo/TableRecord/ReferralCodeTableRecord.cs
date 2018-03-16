using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ReferralCodeTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String ReferralCode
		{
		    get { return _ReferralCode; }
		    set
		    {
		        _ReferralCode = value;
		        IncrementLog("ReferralCode");
		    }
		}
		public String ReferrerCode
		{
		    get { return _ReferrerCode; }
		    set
		    {
		        _ReferrerCode = value;
		        IncrementLog("ReferrerCode");
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

		
		private String _UserId;
		private String _ReferralCode;
		private String _ReferrerCode;
		private Decimal? _ReferralCredit;


		public static ReferralCodeTableRecord CreateNewInstance()
        {
            var record = new ReferralCodeTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ReferralCodeTableRecord()
        {
            ;
        }

        static ReferralCodeTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ReferralCode";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("UserId", true),
				new ColumnMetadata("ReferralCode", false),
				new ColumnMetadata("ReferrerCode", false),
				new ColumnMetadata("ReferralCredit", false),

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
