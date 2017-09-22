using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityPackageTableRecord : Lunggo.Framework.Database.TableRecord
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
		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public String PriceIncluded
		{
		    get { return _PriceIncluded; }
		    set
		    {
		        _PriceIncluded = value;
		        IncrementLog("PriceIncluded");
		    }
		}
		public String PriceExcluded
		{
		    get { return _PriceExcluded; }
		    set
		    {
		        _PriceExcluded = value;
		        IncrementLog("PriceExcluded");
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

		
		private long? _Id;
		private long? _ActivityId;
		private String _PriceIncluded;
		private String _PriceExcluded;
		private String _AdditionalNotes;


		public static ActivityPackageTableRecord CreateNewInstance()
        {
            var record = new ActivityPackageTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityPackageTableRecord()
        {
            ;
        }

        static ActivityPackageTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityPackage";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("PriceIncluded", false),
				new ColumnMetadata("PriceExcluded", false),
				new ColumnMetadata("AdditionalNotes", false),

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
