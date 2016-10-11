using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class HotelRateTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? RoomId
		{
		    get { return _RoomId; }
		    set
		    {
		        _RoomId = value;
		        IncrementLog("RoomId");
		    }
		}
		public long? PriceId
		{
		    get { return _PriceId; }
		    set
		    {
		        _PriceId = value;
		        IncrementLog("PriceId");
		    }
		}
		public int? RoomCount
		{
		    get { return _RoomCount; }
		    set
		    {
		        _RoomCount = value;
		        IncrementLog("RoomCount");
		    }
		}
		public String RateKey
		{
		    get { return _RateKey; }
		    set
		    {
		        _RateKey = value;
		        IncrementLog("RateKey");
		    }
		}
		public String PaymentType
		{
		    get { return _PaymentType; }
		    set
		    {
		        _PaymentType = value;
		        IncrementLog("PaymentType");
		    }
		}
		public String Board
		{
		    get { return _Board; }
		    set
		    {
		        _Board = value;
		        IncrementLog("Board");
		    }
		}
		public Decimal? CancellationFee
		{
		    get { return _CancellationFee; }
		    set
		    {
		        _CancellationFee = value;
		        IncrementLog("CancellationFee");
		    }
		}
		public DateTime? CancellationStartTime
		{
		    get { return _CancellationStartTime; }
		    set
		    {
		        _CancellationStartTime = value;
		        IncrementLog("CancellationStartTime");
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
		private long? _RoomId;
		private long? _PriceId;
		private int? _RoomCount;
		private String _RateKey;
		private String _PaymentType;
		private String _Board;
		private Decimal? _CancellationFee;
		private DateTime? _CancellationStartTime;
		private int? _AdultCount;
		private int? _ChildCount;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static HotelRateTableRecord CreateNewInstance()
        {
            var record = new HotelRateTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public HotelRateTableRecord()
        {
            ;
        }

        static HotelRateTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "HotelRate";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("RoomId", false),
				new ColumnMetadata("PriceId", false),
				new ColumnMetadata("RoomCount", false),
				new ColumnMetadata("RateKey", false),
				new ColumnMetadata("PaymentType", false),
				new ColumnMetadata("Board", false),
				new ColumnMetadata("CancellationFee", false),
				new ColumnMetadata("CancellationStartTime", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
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
