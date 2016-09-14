using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class CampaignVoucherTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String VoucherCode
		{
		    get { return _VoucherCode; }
		    set
		    {
		        _VoucherCode = value;
		        IncrementLog("VoucherCode");
		    }
		}
		public long? CampaignId
		{
		    get { return _CampaignId; }
		    set
		    {
		        _CampaignId = value;
		        IncrementLog("CampaignId");
		    }
		}
		public int? RemainingCount
		{
		    get { return _RemainingCount; }
		    set
		    {
		        _RemainingCount = value;
		        IncrementLog("RemainingCount");
		    }
		}
		public Boolean? IsSingleUsage
		{
		    get { return _IsSingleUsage; }
		    set
		    {
		        _IsSingleUsage = value;
		        IncrementLog("IsSingleUsage");
		    }
		}

		
		private String _VoucherCode;
		private long? _CampaignId;
		private int? _RemainingCount;
		private Boolean? _IsSingleUsage;


		public static CampaignVoucherTableRecord CreateNewInstance()
        {
            var record = new CampaignVoucherTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public CampaignVoucherTableRecord()
        {
            ;
        }

        static CampaignVoucherTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "CampaignVoucher";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("VoucherCode", true),
				new ColumnMetadata("CampaignId", false),
				new ColumnMetadata("RemainingCount", false),
				new ColumnMetadata("IsSingleUsage", false),

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
