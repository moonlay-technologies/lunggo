using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class VoucherRecipientsTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? VoucherRecipientId
		{
		    get { return _VoucherRecipientId; }
		    set
		    {
		        _VoucherRecipientId = value;
		        IncrementLog("VoucherRecipientId");
		    }
		}
		public String VoucherCode
		{
		    get { return _VoucherCode; }
		    set
		    {
		        _VoucherCode = value;
		        IncrementLog("VoucherCode");
		    }
		}
		public String Email
		{
		    get { return _Email; }
		    set
		    {
		        _Email = value;
		        IncrementLog("Email");
		    }
		}

		
		private long? _VoucherRecipientId;
		private String _VoucherCode;
		private String _Email;


		public static VoucherRecipientsTableRecord CreateNewInstance()
        {
            var record = new VoucherRecipientsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public VoucherRecipientsTableRecord()
        {
            ;
        }

        static VoucherRecipientsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "VoucherRecipients";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("VoucherRecipientId", true),
				new ColumnMetadata("VoucherCode", false),
				new ColumnMetadata("Email", false),

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
