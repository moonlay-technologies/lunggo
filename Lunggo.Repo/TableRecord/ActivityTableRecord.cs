using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String Name
		{
		    get { return _Name; }
		    set
		    {
		        _Name = value;
		        IncrementLog("Name");
		    }
		}
		public String Description
		{
		    get { return _Description; }
		    set
		    {
		        _Description = value;
		        IncrementLog("Description");
		    }
		}
		public String City
		{
		    get { return _City; }
		    set
		    {
		        _City = value;
		        IncrementLog("City");
		    }
		}
		public String Country
		{
		    get { return _Country; }
		    set
		    {
		        _Country = value;
		        IncrementLog("Country");
		    }
		}
		public String OperationTime
		{
		    get { return _OperationTime; }
		    set
		    {
		        _OperationTime = value;
		        IncrementLog("OperationTime");
		    }
		}
		public String ImportantNotice
		{
		    get { return _ImportantNotice; }
		    set
		    {
		        _ImportantNotice = value;
		        IncrementLog("ImportantNotice");
		    }
		}
		public String Warning
		{
		    get { return _Warning; }
		    set
		    {
		        _Warning = value;
		        IncrementLog("Warning");
		    }
		}
		public String AdditionalNotes
		{
		    get { return _AdditionalNotes; }
		    set
		    {
		        _AdditionalNotes = value;
		        IncrementLog("AdditionalNotes");
		    }
		}
		public Boolean? IsFixedDate
		{
		    get { return _IsFixedDate; }
		    set
		    {
		        _IsFixedDate = value;
		        IncrementLog("IsFixedDate");
		    }
		}
		public Boolean? IsRedemptionNeeded
		{
		    get { return _IsRedemptionNeeded; }
		    set
		    {
		        _IsRedemptionNeeded = value;
		        IncrementLog("IsRedemptionNeeded");
		    }
		}
		public long? RefundRegulationId
		{
		    get { return _RefundRegulationId; }
		    set
		    {
		        _RefundRegulationId = value;
		        IncrementLog("RefundRegulationId");
		    }
		}
		public Boolean? IsPassportNumberNeeded
		{
		    get { return _IsPassportNumberNeeded; }
		    set
		    {
		        _IsPassportNumberNeeded = value;
		        IncrementLog("IsPassportNumberNeeded");
		    }
		}
		public Boolean? IsNameAccordingToPassport
		{
		    get { return _IsNameAccordingToPassport; }
		    set
		    {
		        _IsNameAccordingToPassport = value;
		        IncrementLog("IsNameAccordingToPassport");
		    }
		}
		public Boolean? IsPhoneNumberAccordingToPassport
		{
		    get { return _IsPhoneNumberAccordingToPassport; }
		    set
		    {
		        _IsPhoneNumberAccordingToPassport = value;
		        IncrementLog("IsPhoneNumberAccordingToPassport");
		    }
		}
		public String Address
		{
		    get { return _Address; }
		    set
		    {
		        _Address = value;
		        IncrementLog("Address");
		    }
		}
		public Decimal? Latitude
		{
		    get { return _Latitude; }
		    set
		    {
		        _Latitude = value;
		        IncrementLog("Latitude");
		    }
		}
		public Decimal? Longitude
		{
		    get { return _Longitude; }
		    set
		    {
		        _Longitude = value;
		        IncrementLog("Longitude");
		    }
		}
		public String Cancellation
		{
		    get { return _Cancellation; }
		    set
		    {
		        _Cancellation = value;
		        IncrementLog("Cancellation");
		    }
		}
		public String Category
		{
		    get { return _Category; }
		    set
		    {
		        _Category = value;
		        IncrementLog("Category");
		    }
		}
		public String PriceDetail
		{
		    get { return _PriceDetail; }
		    set
		    {
		        _PriceDetail = value;
		        IncrementLog("PriceDetail");
		    }
		}
		public int? AmountDuration
		{
		    get { return _AmountDuration; }
		    set
		    {
		        _AmountDuration = value;
		        IncrementLog("AmountDuration");
		    }
		}
		public String UnitDuration
		{
		    get { return _UnitDuration; }
		    set
		    {
		        _UnitDuration = value;
		        IncrementLog("UnitDuration");
		    }
		}

		
		private long? _Id;
		private String _Name;
		private String _Description;
		private String _City;
		private String _Country;
		private String _OperationTime;
		private String _ImportantNotice;
		private String _Warning;
		private String _AdditionalNotes;
		private Boolean? _IsFixedDate;
		private Boolean? _IsRedemptionNeeded;
		private long? _RefundRegulationId;
		private Boolean? _IsPassportNumberNeeded;
		private Boolean? _IsNameAccordingToPassport;
		private Boolean? _IsPhoneNumberAccordingToPassport;
		private String _Address;
		private Decimal? _Latitude;
		private Decimal? _Longitude;
		private String _Cancellation;
		private String _Category;
		private String _PriceDetail;
		private int? _AmountDuration;
		private String _UnitDuration;


		public static ActivityTableRecord CreateNewInstance()
        {
            var record = new ActivityTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityTableRecord()
        {
            ;
        }

        static ActivityTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Activity";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("Description", false),
				new ColumnMetadata("City", false),
				new ColumnMetadata("Country", false),
				new ColumnMetadata("OperationTime", false),
				new ColumnMetadata("ImportantNotice", false),
				new ColumnMetadata("Warning", false),
				new ColumnMetadata("AdditionalNotes", false),
				new ColumnMetadata("IsFixedDate", false),
				new ColumnMetadata("IsRedemptionNeeded", false),
				new ColumnMetadata("RefundRegulationId", false),
				new ColumnMetadata("IsPassportNumberNeeded", false),
				new ColumnMetadata("IsNameAccordingToPassport", false),
				new ColumnMetadata("IsPhoneNumberAccordingToPassport", false),
				new ColumnMetadata("Address", false),
				new ColumnMetadata("Latitude", false),
				new ColumnMetadata("Longitude", false),
				new ColumnMetadata("Cancellation", false),
				new ColumnMetadata("Category", false),
				new ColumnMetadata("PriceDetail", false),
				new ColumnMetadata("AmountDuration", false),
				new ColumnMetadata("UnitDuration", false),

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
