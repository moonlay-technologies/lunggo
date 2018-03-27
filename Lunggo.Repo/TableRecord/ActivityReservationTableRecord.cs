using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityReservationTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public DateTime? Date
		{
		    get { return _Date; }
		    set
		    {
		        _Date = value;
		        IncrementLog("Date");
		    }
		}
		public int? TicketCount
		{
		    get { return _TicketCount; }
		    set
		    {
		        _TicketCount = value;
		        IncrementLog("TicketCount");
		    }
		}
		public String SelectedSession
		{
		    get { return _SelectedSession; }
		    set
		    {
		        _SelectedSession = value;
		        IncrementLog("SelectedSession");
		    }
		}
		public String UserId
		{
		    get { return _UserId; }
		    set
		    {
		        _UserId = value;
		        IncrementLog("UserId");
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
		public Boolean? IsPdfUploaded
		{
		    get { return _IsPdfUploaded; }
		    set
		    {
		        _IsPdfUploaded = value;
		        IncrementLog("IsPdfUploaded");
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
		public String ActivityDuration
		{
		    get { return _ActivityDuration; }
		    set
		    {
		        _ActivityDuration = value;
		        IncrementLog("ActivityDuration");
		    }
		}
		public long? ViewCount
		{
		    get { return _ViewCount; }
		    set
		    {
		        _ViewCount = value;
		        IncrementLog("ViewCount");
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

		
		private long? _Id;
		private String _RsvNo;
		private long? _ActivityId;
		private DateTime? _Date;
		private int? _TicketCount;
		private String _SelectedSession;
		private String _UserId;
		private String _BookingStatusCd;
		private Boolean? _IsPdfUploaded;
		private String _ActivityName;
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
		private int? _AmountDuration;
		private String _UnitDuration;
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
		private String _ActivityDuration;
		private long? _ViewCount;
		private String _ActivityMedia;


		public static ActivityReservationTableRecord CreateNewInstance()
        {
            var record = new ActivityReservationTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityReservationTableRecord()
        {
            ;
        }

        static ActivityReservationTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityReservation";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RsvNo", false),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("Date", false),
				new ColumnMetadata("TicketCount", false),
				new ColumnMetadata("SelectedSession", false),
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("BookingStatusCd", false),
				new ColumnMetadata("IsPdfUploaded", false),
				new ColumnMetadata("ActivityName", false),
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
				new ColumnMetadata("AmountDuration", false),
				new ColumnMetadata("UnitDuration", false),
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
				new ColumnMetadata("ActivityDuration", false),
				new ColumnMetadata("ViewCount", false),
				new ColumnMetadata("ActivityMedia", false),

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
