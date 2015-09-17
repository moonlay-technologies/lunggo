using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightCrawlTargetTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public int? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public String OriginAirport
		{
		    get { return _OriginAirport; }
		    set
		    {
		        _OriginAirport = value;
		        IncrementLog("OriginAirport");
		    }
		}
		public String DestinationAirport
		{
		    get { return _DestinationAirport; }
		    set
		    {
		        _DestinationAirport = value;
		        IncrementLog("DestinationAirport");
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
		public int? InfantCount
		{
		    get { return _InfantCount; }
		    set
		    {
		        _InfantCount = value;
		        IncrementLog("InfantCount");
		    }
		}
		public String RequestedCabinClassCd
		{
		    get { return _RequestedCabinClassCd; }
		    set
		    {
		        _RequestedCabinClassCd = value;
		        IncrementLog("RequestedCabinClassCd");
		    }
		}
		public int? DaysAdvanceDepartureDateStart
		{
		    get { return _DaysAdvanceDepartureDateStart; }
		    set
		    {
		        _DaysAdvanceDepartureDateStart = value;
		        IncrementLog("DaysAdvanceDepartureDateStart");
		    }
		}
		public int? DaysAdvanceDepartureDateEnd
		{
		    get { return _DaysAdvanceDepartureDateEnd; }
		    set
		    {
		        _DaysAdvanceDepartureDateEnd = value;
		        IncrementLog("DaysAdvanceDepartureDateEnd");
		    }
		}
		public int? Timeout
		{
		    get { return _Timeout; }
		    set
		    {
		        _Timeout = value;
		        IncrementLog("Timeout");
		    }
		}

		
		private int? _Id;
		private String _OriginAirport;
		private String _DestinationAirport;
		private int? _AdultCount;
		private int? _ChildCount;
		private int? _InfantCount;
		private String _RequestedCabinClassCd;
		private int? _DaysAdvanceDepartureDateStart;
		private int? _DaysAdvanceDepartureDateEnd;
		private int? _Timeout;


		public static FlightCrawlTargetTableRecord CreateNewInstance()
        {
            var record = new FlightCrawlTargetTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightCrawlTargetTableRecord()
        {
            ;
        }

        static FlightCrawlTargetTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightCrawlTarget";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("OriginAirport", false),
				new ColumnMetadata("DestinationAirport", false),
				new ColumnMetadata("AdultCount", false),
				new ColumnMetadata("ChildCount", false),
				new ColumnMetadata("InfantCount", false),
				new ColumnMetadata("RequestedCabinClassCd", false),
				new ColumnMetadata("DaysAdvanceDepartureDateStart", false),
				new ColumnMetadata("DaysAdvanceDepartureDateEnd", false),
				new ColumnMetadata("Timeout", false),

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
