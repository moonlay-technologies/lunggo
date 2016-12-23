using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class HotelReservationDetailsTableRecord : Lunggo.Framework.Database.TableRecord
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
		public int? HotelCd
		{
		    get { return _HotelCd; }
		    set
		    {
		        _HotelCd = value;
		        IncrementLog("HotelCd");
		    }
		}
		public String HotelName
		{
		    get { return _HotelName; }
		    set
		    {
		        _HotelName = value;
		        IncrementLog("HotelName");
		    }
		}
		public DateTime? CheckInDate
		{
		    get { return _CheckInDate; }
		    set
		    {
		        _CheckInDate = value;
		        IncrementLog("CheckInDate");
		    }
		}
		public DateTime? CheckOutDate
		{
		    get { return _CheckOutDate; }
		    set
		    {
		        _CheckOutDate = value;
		        IncrementLog("CheckOutDate");
		    }
		}
		public int? AdultCount
		{
		    get { return _AdultCount; }
		    set
		    {
		        _AdultCount = value;
		        IncrementLog("AdultCount");
		    }
		}
		public int? ChildCount
		{
		    get { return _ChildCount; }
		    set
		    {
		        _ChildCount = value;
		        IncrementLog("ChildCount");
		    }
		}
		public String SpecialRequest
		{
		    get { return _SpecialRequest; }
		    set
		    {
		        _SpecialRequest = value;
		        IncrementLog("SpecialRequest");
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

		
		private long? _Id;
		private String _RsvNo;
		private int? _HotelCd;
		private String _HotelName;
		private DateTime? _CheckInDate;
		private DateTime? _CheckOutDate;
		private int? _AdultCount;
		private int? _ChildCount;
		private String _SpecialRequest;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static HotelReservationDetailsTableRecord CreateNewInstance()
        {
            var record = new HotelReservationDetailsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public HotelReservationDetailsTableRecord()
        {
            ;
        }

        static HotelReservationDetailsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "HotelReservationDetails";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("HotelCd", false),
				new ColumnMetadata("HotelName", false),
				new ColumnMetadata("CheckInDate", false),
				new ColumnMetadata("CheckOutDate", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
				new ColumnMetadata("SpecialRequest", false),
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
