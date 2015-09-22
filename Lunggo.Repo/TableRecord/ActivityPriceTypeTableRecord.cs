using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityPriceTypeTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public String PriceTypeName
		{
		    get { return _PriceTypeName; }
		    set
		    {
		        _PriceTypeName = value;
		        IncrementLog("PriceTypeName");
		    }
		}
		public String PriceTypeMetric
		{
		    get { return _PriceTypeMetric; }
		    set
		    {
		        _PriceTypeMetric = value;
		        IncrementLog("PriceTypeMetric");
		    }
		}
		public String LangCd
		{
		    get { return _LangCd; }
		    set
		    {
		        _LangCd = value;
		        IncrementLog("LangCd");
		    }
		}

		
		private long? _ActivityPriceTypeId;
		private long? _ActivityId;
		private String _PriceTypeName;
		private String _PriceTypeMetric;
		private String _LangCd;


		public static ActivityPriceTypeTableRecord CreateNewInstance()
        {
            var record = new ActivityPriceTypeTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityPriceTypeTableRecord()
        {
            ;
        }

        static ActivityPriceTypeTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityPriceType";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityPriceTypeId", true),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("PriceTypeName", false),
				new ColumnMetadata("PriceTypeMetric", false),
				new ColumnMetadata("LangCd", true),

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
