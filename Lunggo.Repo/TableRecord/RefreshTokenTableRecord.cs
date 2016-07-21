using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RefreshTokenTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public String Subject
		{
		    get { return _Subject; }
		    set
		    {
		        _Subject = value;
		        IncrementLog("Subject");
		    }
		}
		public String ClientId
		{
		    get { return _ClientId; }
		    set
		    {
		        _ClientId = value;
		        IncrementLog("ClientId");
		    }
		}
        public String DeviceId
        {
            get { return _DeviceId; }
            set
            {
                _DeviceId = value;
                IncrementLog("DeviceId");
            }
        }
		public DateTime? IssueTime
		{
		    get { return _IssueTime; }
		    set
		    {
		        _IssueTime = value;
		        IncrementLog("IssueTime");
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
		public String ProtectedTicket
		{
		    get { return _ProtectedTicket; }
		    set
		    {
		        _ProtectedTicket = value;
		        IncrementLog("ProtectedTicket");
		    }
		}

		
		private String _Id;
		private String _Subject;
		private String _ClientId;
		private String _DeviceId;
		private DateTime? _IssueTime;
		private DateTime? _ExpireTime;
		private String _ProtectedTicket;


		public static RefreshTokenTableRecord CreateNewInstance()
        {
            var record = new RefreshTokenTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RefreshTokenTableRecord()
        {
            ;
        }

        static RefreshTokenTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "RefreshToken";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Subject", false),
				new ColumnMetadata("ClientId", false),
				new ColumnMetadata("DeviceId", false),
				new ColumnMetadata("IssueTime", false),
				new ColumnMetadata("ExpireTime", false),
				new ColumnMetadata("ProtectedTicket", false),

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
