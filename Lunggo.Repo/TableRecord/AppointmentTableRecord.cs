using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class AppointmentTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public String RsvNo
		{
		    get { return _RsvNo; }
		    set
		    {
		        _RsvNo = value;
		        IncrementLog("RsvNo");
		    }
		}
		public String AppointmentStatus
		{
		    get { return _AppointmentStatus; }
		    set
		    {
		        _AppointmentStatus = value;
		        IncrementLog("AppointmentStatus");
		    }
		}
		public long? ActivityRsvId
		{
		    get { return _ActivityRsvId; }
		    set
		    {
		        _ActivityRsvId = value;
		        IncrementLog("ActivityRsvId");
		    }
		}
		public String OperatorId
		{
		    get { return _OperatorId; }
		    set
		    {
		        _OperatorId = value;
		        IncrementLog("OperatorId");
		    }
		}
		public DateTime? InsertDate
		{
		    get { return _InsertDate; }
		    set
		    {
		        _InsertDate = value;
		        IncrementLog("InsertDate");
		    }
		}

		
		private long? _Id;
		private String _RsvNo;
		private String _AppointmentStatus;
		private long? _ActivityRsvId;
		private String _OperatorId;
		private DateTime? _InsertDate;


		public static AppointmentTableRecord CreateNewInstance()
        {
            var record = new AppointmentTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public AppointmentTableRecord()
        {
            ;
        }

        static AppointmentTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Appointment";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("AppointmentStatus", false),
				new ColumnMetadata("ActivityRsvId", false),
				new ColumnMetadata("OperatorId", false),
				new ColumnMetadata("InsertDate", false),

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
