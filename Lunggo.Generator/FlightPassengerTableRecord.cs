using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightPassengerTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? PassengerId
		{
		    get { return _PassengerId; }
		    set
		    {
		        _PassengerId = value;
		        IncrementLog("PassengerId");
		    }
		}
		public long? TripId
		{
		    get { return _TripId; }
		    set
		    {
		        _TripId = value;
		        IncrementLog("TripId");
		    }
		}
		public String FirstName
		{
		    get { return _FirstName; }
		    set
		    {
		        _FirstName = value;
		        IncrementLog("FirstName");
		    }
		}
		public String LastName
		{
		    get { return _LastName; }
		    set
		    {
		        _LastName = value;
		        IncrementLog("LastName");
		    }
		}
		public DateTime? BirthDate
		{
		    get { return _BirthDate; }
		    set
		    {
		        _BirthDate = value;
		        IncrementLog("BirthDate");
		    }
		}
		public String Gender
		{
		    get { return _Gender; }
		    set
		    {
		        _Gender = value;
		        IncrementLog("Gender");
		    }
		}
		public String PassengerTypeCd
		{
		    get { return _PassengerTypeCd; }
		    set
		    {
		        _PassengerTypeCd = value;
		        IncrementLog("PassengerTypeCd");
		    }
		}
		public String CountryCd
		{
		    get { return _CountryCd; }
		    set
		    {
		        _CountryCd = value;
		        IncrementLog("CountryCd");
		    }
		}
		public String IdCardNo
		{
		    get { return _IdCardNo; }
		    set
		    {
		        _IdCardNo = value;
		        IncrementLog("IdCardNo");
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

		
		private long? _PassengerId;
		private long? _TripId;
		private String _FirstName;
		private String _LastName;
		private DateTime? _BirthDate;
		private String _Gender;
		private String _PassengerTypeCd;
		private String _CountryCd;
		private String _IdCardNo;
		private String _InsertBy;
		private DateTime? _InsertDate;
		private String _InsertPgId;
		private String _UpdateBy;
		private DateTime? _UpdateDate;
		private String _UpdatePgId;


		public static FlightPassengerTableRecord CreateNewInstance()
        {
            var record = new FlightPassengerTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightPassengerTableRecord()
        {
            ;
        }

        static FlightPassengerTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightPassenger";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("PassengerId", true),
				new ColumnMetadata("TripId", false),
				new ColumnMetadata("FirstName", false),
				new ColumnMetadata("LastName", false),
				new ColumnMetadata("BirthDate", false),
				new ColumnMetadata("Gender", false),
				new ColumnMetadata("PassengerTypeCd", false),
				new ColumnMetadata("CountryCd", false),
				new ColumnMetadata("IdCardNo", false),
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
