using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityDetailReservationTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
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
		public Boolean? IsPassportNeeded
		{
		    get { return _IsPassportNeeded; }
		    set
		    {
		        _IsPassportNeeded = value;
		        IncrementLog("IsPassportNeeded");
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
		public Boolean? IsDateOfBirthNeeded
		{
		    get { return _IsDateOfBirthNeeded; }
		    set
		    {
		        _IsDateOfBirthNeeded = value;
		        IncrementLog("IsDateOfBirthNeeded");
		    }
		}
		public Boolean? IsPassportIssueDateNeeded
		{
		    get { return _IsPassportIssueDateNeeded; }
		    set
		    {
		        _IsPassportIssueDateNeeded = value;
		        IncrementLog("IsPassportIssueDateNeeded");
		    }
		}
		public String OperatorName
		{
		    get { return _OperatorName; }
		    set
		    {
		        _OperatorName = value;
		        IncrementLog("OperatorName");
		    }
		}
		public String OperatorEmail
		{
		    get { return _OperatorEmail; }
		    set
		    {
		        _OperatorEmail = value;
		        IncrementLog("OperatorEmail");
		    }
		}
		public String OperatorPhone
		{
		    get { return _OperatorPhone; }
		    set
		    {
		        _OperatorPhone = value;
		        IncrementLog("OperatorPhone");
		    }
		}
		public String Zone
		{
		    get { return _Zone; }
		    set
		    {
		        _Zone = value;
		        IncrementLog("Zone");
		    }
		}
		public String Area
		{
		    get { return _Area; }
		    set
		    {
		        _Area = value;
		        IncrementLog("Area");
		    }
		}
		public Decimal? Rating
		{
		    get { return _Rating; }
		    set
		    {
		        _Rating = value;
		        IncrementLog("Rating");
		    }
		}
		public Boolean? HasPDFVoucher
		{
		    get { return _HasPDFVoucher; }
		    set
		    {
		        _HasPDFVoucher = value;
		        IncrementLog("HasPDFVoucher");
		    }
		}
		public Boolean? IsInstantConfirmation
		{
		    get { return _IsInstantConfirmation; }
		    set
		    {
		        _IsInstantConfirmation = value;
		        IncrementLog("IsInstantConfirmation");
		    }
		}
		public Boolean? MustPrinted
		{
		    get { return _MustPrinted; }
		    set
		    {
		        _MustPrinted = value;
		        IncrementLog("MustPrinted");
		    }
		}
		public String Duration
		{
		    get { return _Duration; }
		    set
		    {
		        _Duration = value;
		        IncrementLog("Duration");
		    }
		}
		public long? viewCount
		{
		    get { return _viewCount; }
		    set
		    {
		        _viewCount = value;
		        IncrementLog("viewCount");
		    }
		}
		public String ActivityMedia
		{
		    get { return _ActivityMedia; }
		    set
		    {
		        _ActivityMedia = value;
		        IncrementLog("ActivityMedia");
		    }
		}
		public Boolean? HasOperator
		{
		    get { return _HasOperator; }
		    set
		    {
		        _HasOperator = value;
		        IncrementLog("HasOperator");
		    }
		}
        public Boolean? IsOpenTrip
        {
            get { return _IsOpenTrip; }
            set
            {
                _HasOperator = value;
                IncrementLog("IsOpenTrip");
            }
        }

		
		private String _RsvNo;
		private long? _ActivityId;
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
		private Boolean? _IsPassportNeeded;
		private Boolean? _IsNameAccordingToPassport;
		private Boolean? _IsPhoneNumberAccordingToPassport;
		private String _Address;
		private Decimal? _Latitude;
		private Decimal? _Longitude;
		private String _Cancellation;
		private String _Category;
		private String _PriceDetail;
		private Boolean? _IsDateOfBirthNeeded;
		private Boolean? _IsPassportIssueDateNeeded;
		private String _OperatorName;
		private String _OperatorEmail;
		private String _OperatorPhone;
		private String _Zone;
		private String _Area;
		private Decimal? _Rating;
		private Boolean? _HasPDFVoucher;
		private Boolean? _IsInstantConfirmation;
		private Boolean? _MustPrinted;
		private String _Duration;
		private long? _viewCount;
		private String _ActivityMedia;
		private Boolean? _HasOperator;
		private Boolean? _IsOpenTrip;


		public static ActivityDetailReservationTableRecord CreateNewInstance()
        {
            var record = new ActivityDetailReservationTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityDetailReservationTableRecord()
        {
            ;
        }

        static ActivityDetailReservationTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityDetailReservation";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("ActivityId", false),
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
				new ColumnMetadata("IsPassportNeeded", false),
				new ColumnMetadata("IsNameAccordingToPassport", false),
				new ColumnMetadata("IsPhoneNumberAccordingToPassport", false),
				new ColumnMetadata("Address", false),
				new ColumnMetadata("Latitude", false),
				new ColumnMetadata("Longitude", false),
				new ColumnMetadata("Cancellation", false),
				new ColumnMetadata("Category", false),
				new ColumnMetadata("PriceDetail", false),
				new ColumnMetadata("IsDateOfBirthNeeded", false),
				new ColumnMetadata("IsPassportIssueDateNeeded", false),
				new ColumnMetadata("OperatorName", false),
				new ColumnMetadata("OperatorEmail", false),
				new ColumnMetadata("OperatorPhone", false),
				new ColumnMetadata("Zone", false),
				new ColumnMetadata("Area", false),
				new ColumnMetadata("Rating", false),
				new ColumnMetadata("HasPDFVoucher", false),
				new ColumnMetadata("IsInstantConfirmation", false),
				new ColumnMetadata("MustPrinted", false),
				new ColumnMetadata("Duration", false),
				new ColumnMetadata("viewCount", false),
				new ColumnMetadata("ActivityMedia", false),
				new ColumnMetadata("HasOperator", false),
				new ColumnMetadata("IsOpenTrip", false),

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
