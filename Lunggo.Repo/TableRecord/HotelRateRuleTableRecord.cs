using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class HotelRateRuleTableRecord : Lunggo.Framework.Database.TableRecord
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
		public int? ConstraintCount
		{
		    get { return _ConstraintCount; }
		    set
		    {
		        _ConstraintCount = value;
		        IncrementLog("ConstraintCount");
		    }
		}
		public int? Priority
		{
		    get { return _Priority; }
		    set
		    {
		        _Priority = value;
		        IncrementLog("Priority");
		    }
		}
		public String BookingDates
		{
		    get { return _BookingDates; }
		    set
		    {
		        _BookingDates = value;
		        IncrementLog("BookingDates");
		    }
		}
		public String BookingDays
		{
		    get { return _BookingDays; }
		    set
		    {
		        _BookingDays = value;
		        IncrementLog("BookingDays");
		    }
		}
		public String StayDates
		{
		    get { return _StayDates; }
		    set
		    {
		        _StayDates = value;
		        IncrementLog("StayDates");
		    }
		}
		public String StayDurations
		{
		    get { return _StayDurations; }
		    set
		    {
		        _StayDurations = value;
		        IncrementLog("StayDurations");
		    }
		}
		public int? MaxAdult
		{
		    get { return _MaxAdult; }
		    set
		    {
		        _MaxAdult = value;
		        IncrementLog("MaxAdult");
		    }
		}
		public int? MinAdult
		{
		    get { return _MinAdult; }
		    set
		    {
		        _MinAdult = value;
		        IncrementLog("MinAdult");
		    }
		}
		public int? MaxChild
		{
		    get { return _MaxChild; }
		    set
		    {
		        _MaxChild = value;
		        IncrementLog("MaxChild");
		    }
		}
		public int? MinChild
		{
		    get { return _MinChild; }
		    set
		    {
		        _MinChild = value;
		        IncrementLog("MinChild");
		    }
		}
		public String Boards
		{
		    get { return _Boards; }
		    set
		    {
		        _Boards = value;
		        IncrementLog("Boards");
		    }
		}
		public String Countries
		{
		    get { return _Countries; }
		    set
		    {
		        _Countries = value;
		        IncrementLog("Countries");
		    }
		}
		public String Destinations
		{
		    get { return _Destinations; }
		    set
		    {
		        _Destinations = value;
		        IncrementLog("Destinations");
		    }
		}
		public String RoomTypes
		{
		    get { return _RoomTypes; }
		    set
		    {
		        _RoomTypes = value;
		        IncrementLog("RoomTypes");
		    }
		}
		public String HotelStars
		{
		    get { return _HotelStars; }
		    set
		    {
		        _HotelStars = value;
		        IncrementLog("HotelStars");
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
		private int? _ConstraintCount;
		private int? _Priority;
		private String _BookingDates;
		private String _BookingDays;
		private String _StayDates;
		private String _StayDurations;
		private int? _MaxAdult;
		private int? _MinAdult;
		private int? _MaxChild;
		private int? _MinChild;
		private String _Boards;
		private String _Countries;
		private String _Destinations;
		private String _RoomTypes;
		private String _HotelStars;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static HotelRateRuleTableRecord CreateNewInstance()
        {
            var record = new HotelRateRuleTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public HotelRateRuleTableRecord()
        {
            ;
        }

        static HotelRateRuleTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "HotelRateRule";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("ConstraintCount", false),
				new ColumnMetadata("Priority", false),
				new ColumnMetadata("BookingDates", false),
				new ColumnMetadata("BookingDays", false),
				new ColumnMetadata("StayDates", false),
				new ColumnMetadata("StayDurations", false),
				new ColumnMetadata("MaxAdult", false),
				new ColumnMetadata("MinAdult", false),
				new ColumnMetadata("MaxChild", false),
				new ColumnMetadata("MinChild", false),
				new ColumnMetadata("Boards", false),
				new ColumnMetadata("Countries", false),
				new ColumnMetadata("Destinations", false),
				new ColumnMetadata("RoomTypes", false),
				new ColumnMetadata("HotelStars", false),
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
