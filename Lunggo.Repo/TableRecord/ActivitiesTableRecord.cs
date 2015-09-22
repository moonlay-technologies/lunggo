using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivitiesTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public long? SupplierId
		{
		    get { return _SupplierId; }
		    set
		    {
		        _SupplierId = value;
		        IncrementLog("SupplierId");
		    }
		}
		public String ActivityName
		{
		    get { return _ActivityName; }
		    set
		    {
		        _ActivityName = value;
		        IncrementLog("ActivityName");
		    }
		}
		public String ActivityShortDesc
		{
		    get { return _ActivityShortDesc; }
		    set
		    {
		        _ActivityShortDesc = value;
		        IncrementLog("ActivityShortDesc");
		    }
		}
		public String HotelMeetLocation
		{
		    get { return _HotelMeetLocation; }
		    set
		    {
		        _HotelMeetLocation = value;
		        IncrementLog("HotelMeetLocation");
		    }
		}
		public String ToKnow
		{
		    get { return _ToKnow; }
		    set
		    {
		        _ToKnow = value;
		        IncrementLog("ToKnow");
		    }
		}
		public int? MaxGuest
		{
		    get { return _MaxGuest; }
		    set
		    {
		        _MaxGuest = value;
		        IncrementLog("MaxGuest");
		    }
		}
		public int? MinGuest
		{
		    get { return _MinGuest; }
		    set
		    {
		        _MinGuest = value;
		        IncrementLog("MinGuest");
		    }
		}
		public Double? LocationMeetUpLat
		{
		    get { return _LocationMeetUpLat; }
		    set
		    {
		        _LocationMeetUpLat = value;
		        IncrementLog("LocationMeetUpLat");
		    }
		}
		public Double? LocationMeetUpLang
		{
		    get { return _LocationMeetUpLang; }
		    set
		    {
		        _LocationMeetUpLang = value;
		        IncrementLog("LocationMeetUpLang");
		    }
		}
		public String OtherRules
		{
		    get { return _OtherRules; }
		    set
		    {
		        _OtherRules = value;
		        IncrementLog("OtherRules");
		    }
		}
		public String WeatherPolicy
		{
		    get { return _WeatherPolicy; }
		    set
		    {
		        _WeatherPolicy = value;
		        IncrementLog("WeatherPolicy");
		    }
		}
		public String VenueAddress
		{
		    get { return _VenueAddress; }
		    set
		    {
		        _VenueAddress = value;
		        IncrementLog("VenueAddress");
		    }
		}
		public Boolean? Publish
		{
		    get { return _Publish; }
		    set
		    {
		        _Publish = value;
		        IncrementLog("Publish");
		    }
		}
		public int? ActivityType
		{
		    get { return _ActivityType; }
		    set
		    {
		        _ActivityType = value;
		        IncrementLog("ActivityType");
		    }
		}
		public int? AreaCd
		{
		    get { return _AreaCd; }
		    set
		    {
		        _AreaCd = value;
		        IncrementLog("AreaCd");
		    }
		}

		
		private long? _ActivityId;
		private long? _SupplierId;
		private String _ActivityName;
		private String _ActivityShortDesc;
		private String _HotelMeetLocation;
		private String _ToKnow;
		private int? _MaxGuest;
		private int? _MinGuest;
		private Double? _LocationMeetUpLat;
		private Double? _LocationMeetUpLang;
		private String _OtherRules;
		private String _WeatherPolicy;
		private String _VenueAddress;
		private Boolean? _Publish;
		private int? _ActivityType;
		private int? _AreaCd;


		public static ActivitiesTableRecord CreateNewInstance()
        {
            var record = new ActivitiesTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivitiesTableRecord()
        {
            ;
        }

        static ActivitiesTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Activities";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityId", true),
				new ColumnMetadata("SupplierId", false),
				new ColumnMetadata("ActivityName", false),
				new ColumnMetadata("ActivityShortDesc", false),
				new ColumnMetadata("HotelMeetLocation", false),
				new ColumnMetadata("ToKnow", false),
				new ColumnMetadata("MaxGuest", false),
				new ColumnMetadata("MinGuest", false),
				new ColumnMetadata("LocationMeetUpLat", false),
				new ColumnMetadata("LocationMeetUpLang", false),
				new ColumnMetadata("OtherRules", false),
				new ColumnMetadata("WeatherPolicy", false),
				new ColumnMetadata("VenueAddress", false),
				new ColumnMetadata("Publish", false),
				new ColumnMetadata("ActivityType", false),
				new ColumnMetadata("AreaCd", false),

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
