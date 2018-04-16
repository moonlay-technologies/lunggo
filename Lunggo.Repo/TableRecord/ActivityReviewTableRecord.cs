using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityReviewTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String UserId
		{
		    get { return _UserId; }
		    set
		    {
		        _UserId = value;
		        IncrementLog("UserId");
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
		public String Review
		{
		    get { return _Review; }
		    set
		    {
		        _Review = value;
		        IncrementLog("Review");
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

		
		private String _UserId;
		private String _RsvNo;
		private long? _ActivityId;
		private String _Review;
		private DateTime? _Date;


		public static ActivityReviewTableRecord CreateNewInstance()
        {
            var record = new ActivityReviewTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityReviewTableRecord()
        {
            ;
        }

        static ActivityReviewTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityReview";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("UserId", false),
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("Review", false),
				new ColumnMetadata("Date", false),

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
