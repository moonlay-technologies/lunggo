using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityPriceTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? ActivityPriceTypeId
		{
		    get { return _ActivityPriceTypeId; }
		    set
		    {
		        _ActivityPriceTypeId = value;
		        IncrementLog("ActivityPriceTypeId");
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
		public Decimal? Price
		{
		    get { return _Price; }
		    set
		    {
		        _Price = value;
		        IncrementLog("Price");
		    }
		}

		
		private long? _ActivityPriceTypeId;
		private DateTime? _Date;
		private Decimal? _Price;


		public static ActivityPriceTableRecord CreateNewInstance()
        {
            var record = new ActivityPriceTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityPriceTableRecord()
        {
            ;
        }

        static ActivityPriceTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityPrice";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityPriceTypeId", true),
				new ColumnMetadata("Date", true),
				new ColumnMetadata("Price", false),

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
