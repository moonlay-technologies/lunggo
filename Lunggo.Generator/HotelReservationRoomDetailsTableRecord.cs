using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class HotelReservationRoomDetailsTableRecord : Lunggo.Framework.Database.TableRecord
    {
		public short? RoomSeq
		{
		    get { return _RoomSeq; }
		    set
		    {
		        _RoomSeq = value;
		        IncrementLog("RoomSeq");
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
		public String GuestName
		{
		    get { return _GuestName; }
		    set
		    {
		        _GuestName = value;
		        IncrementLog("GuestName");
		    }
		}
		public short? GuestCount
		{
		    get { return _GuestCount; }
		    set
		    {
		        _GuestCount = value;
		        IncrementLog("GuestCount");
		    }
		}
		public String InsertBy
		{
		    get { return _InsertBy; }
		    set
		    {
		        _InsertBy = value;
		        IncrementLog("InsertBy");
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
		public String InsertPgId
		{
		    get { return _InsertPgId; }
		    set
		    {
		        _InsertPgId = value;
		        IncrementLog("InsertPgId");
		    }
		}
		public String UpdateBy
		{
		    get { return _UpdateBy; }
		    set
		    {
		        _UpdateBy = value;
		        IncrementLog("UpdateBy");
		    }
		}
		public DateTime? UpdateDate
		{
		    get { return _UpdateDate; }
		    set
		    {
		        _UpdateDate = value;
		        IncrementLog("UpdateDate");
		    }
		}
		public String UpdatePgId
		{
		    get { return _UpdatePgId; }
		    set
		    {
		        _UpdatePgId = value;
		        IncrementLog("UpdatePgId");
		    }
		}

		
		private short? _RoomSeq;
		private String _RsvNo;
		private String _GuestName;
		private short? _GuestCount;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static HotelReservationRoomDetailsTableRecord CreateNewInstance()
        {
            var record = new HotelReservationRoomDetailsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public HotelReservationRoomDetailsTableRecord()
        {
            ;
        }

        static HotelReservationRoomDetailsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "HotelReservationRoomDetails";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RoomSeq", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("GuestName", false),
				new ColumnMetadata("GuestCount", false),
				new ColumnMetadata("InsertBy", false),
				new ColumnMetadata("InsertDate", false),
				new ColumnMetadata("InsertPgId", false),
				new ColumnMetadata("UpdateBy", false),
				new ColumnMetadata("UpdateDate", false),
				new ColumnMetadata("UpdatePgId", false),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
