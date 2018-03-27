using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityReservationContentTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? ActivityId
		{
		    get { return _ActivityId; }
		    set
		    {
		        _ActivityId = value;
		        IncrementLog("ActivityId");
		    }
		}
		public String Title
		{
		    get { return _Title; }
		    set
		    {
		        _Title = value;
		        IncrementLog("Title");
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

		
		private long? _ActivityId;
		private String _Title;
		private String _Description;


		public static ActivityReservationContentTableRecord CreateNewInstance()
        {
            var record = new ActivityReservationContentTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityReservationContentTableRecord()
        {
            ;
        }

        static ActivityReservationContentTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityReservationContent";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("Title", false),
				new ColumnMetadata("Description", false),

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
