using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ResetPasswordTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String CountryCallCd
		{
		    get { return _CountryCallCd; }
		    set
		    {
		        _CountryCallCd = value;
		        IncrementLog("CountryCallCd");
		    }
		}
		public String PhoneNumber
		{
		    get { return _PhoneNumber; }
		    set
		    {
		        _PhoneNumber = value;
		        IncrementLog("PhoneNumber");
		    }
		}
		public String OtpHash
		{
		    get { return _OtpHash; }
		    set
		    {
		        _OtpHash = value;
		        IncrementLog("OtpHash");
		    }
		}
		public DateTime? ExpireTime
		{
		    get { return _ExpireTime; }
		    set
		    {
		        _ExpireTime = value;
		        IncrementLog("ExpireTime");
		    }
		}

		
		private String _CountryCallCd;
		private String _PhoneNumber;
		private String _OtpHash;
		private DateTime? _ExpireTime;


		public static ResetPasswordTableRecord CreateNewInstance()
        {
            var record = new ResetPasswordTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ResetPasswordTableRecord()
        {
            ;
        }

        static ResetPasswordTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ResetPassword";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("CountryCallCd", false),
				new ColumnMetadata("PhoneNumber", false),
				new ColumnMetadata("OtpHash", false),
				new ColumnMetadata("ExpireTime", false),

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
