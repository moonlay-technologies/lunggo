using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityPackageReservationTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? RsvId
		{
		    get { return _RsvId; }
		    set
		    {
		        _RsvId = value;
		        IncrementLog("RsvId");
		    }
		}
		public long? PackageId
		{
		    get { return _PackageId; }
		    set
		    {
		        _PackageId = value;
		        IncrementLog("PackageId");
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
		public String Type
		{
		    get { return _Type; }
		    set
		    {
		        _Type = value;
		        IncrementLog("Type");
		    }
		}
		public int? Count
		{
		    get { return _Count; }
		    set
		    {
		        _Count = value;
		        IncrementLog("Count");
		    }
		}
		public Decimal? Amount
		{
		    get { return _Amount; }
		    set
		    {
		        _Amount = value;
		        IncrementLog("Amount");
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

		
		private long? _RsvId;
		private long? _PackageId;
		private long? _ActivityId;
		private String _Type;
		private int? _Count;
		private Decimal? _Amount;
		private String _RsvNo;


		public static ActivityPackageReservationTableRecord CreateNewInstance()
        {
            var record = new ActivityPackageReservationTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityPackageReservationTableRecord()
        {
            ;
        }

        static ActivityPackageReservationTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityPackageReservation";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvId", false),
				new ColumnMetadata("PackageId", false),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("Type", false),
				new ColumnMetadata("Count", false),
				new ColumnMetadata("Amount", false),
				new ColumnMetadata("RsvNo", false),

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
