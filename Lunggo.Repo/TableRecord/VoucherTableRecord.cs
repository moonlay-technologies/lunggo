using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class VoucherTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String VoucherId
		{
		    get { return _VoucherId; }
		    set
		    {
		        _VoucherId = value;
		        IncrementLog("VoucherId");
		    }
		}
		public String ValidEmail
		{
		    get { return _ValidEmail; }
		    set
		    {
		        _ValidEmail = value;
		        IncrementLog("ValidEmail");
		    }
		}
		public DateTime? ExpiryDate
		{
		    get { return _ExpiryDate; }
		    set
		    {
		        _ExpiryDate = value;
		        IncrementLog("ExpiryDate");
		    }
		}
		public Boolean? IsUsed
		{
		    get { return _IsUsed; }
		    set
		    {
		        _IsUsed = value;
		        IncrementLog("IsUsed");
		    }
		}

		
		private String _VoucherId;
		private String _ValidEmail;
		private DateTime? _ExpiryDate;
		private Boolean? _IsUsed;


		public static VoucherTableRecord CreateNewInstance()
        {
            var record = new VoucherTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public VoucherTableRecord()
        {
            ;
        }

        static VoucherTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightVoucher";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("VoucherId", true),
				new ColumnMetadata("ValidEmail", false),
				new ColumnMetadata("ExpiryDate", false),
				new ColumnMetadata("IsUsed", false),

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
