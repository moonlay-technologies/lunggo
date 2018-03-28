using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ReservationStatusHistoryTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String BookingStatusCd
		{
		    get { return _BookingStatusCd; }
		    set
		    {
		        _BookingStatusCd = value;
		        IncrementLog("BookingStatusCd");
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

		
		private String _RsvNo;
		private String _BookingStatusCd;
		private DateTime? _TimeStamp;


		public static ReservationStatusHistoryTableRecord CreateNewInstance()
        {
            var record = new ReservationStatusHistoryTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ReservationStatusHistoryTableRecord()
        {
            ;
        }

        static ReservationStatusHistoryTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ReservationStatusHistory";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("BookingStatusCd", false),
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
