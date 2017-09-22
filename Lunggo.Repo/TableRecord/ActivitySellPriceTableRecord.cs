using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivitySellPriceTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? PackageId
		{
		    get { return _PackageId; }
		    set
		    {
		        _PackageId = value;
		        IncrementLog("PackageId");
		    }
		}
		public long? AgeId
		{
		    get { return _AgeId; }
		    set
		    {
		        _AgeId = value;
		        IncrementLog("AgeId");
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

		
		private long? _PackageId;
		private long? _AgeId;
		private Decimal? _Price;


		public static ActivitySellPriceTableRecord CreateNewInstance()
        {
            var record = new ActivitySellPriceTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivitySellPriceTableRecord()
        {
            ;
        }

        static ActivitySellPriceTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivitySellPrice";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("PackageId", false),
				new ColumnMetadata("AgeId", false),
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
