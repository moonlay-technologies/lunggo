using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ActivityReservationStepOperatorTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

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
		public String StepName
		{
		    get { return _StepName; }
		    set
		    {
		        _StepName = value;
		        IncrementLog("StepName");
		    }
		}
		public String StepDescription
		{
		    get { return _StepDescription; }
		    set
		    {
		        _StepDescription = value;
		        IncrementLog("StepDescription");
		    }
		}
		public DateTime? StepDate
		{
		    get { return _StepDate; }
		    set
		    {
		        _StepDate = value;
		        IncrementLog("StepDate");
		    }
		}
		public Decimal? StepAmount
		{
		    get { return _StepAmount; }
		    set
		    {
		        _StepAmount = value;
		        IncrementLog("StepAmount");
		    }
		}
		public Boolean? StepStatus
		{
		    get { return _StepStatus; }
		    set
		    {
		        _StepStatus = value;
		        IncrementLog("StepStatus");
		    }
		}

		
		private String _RsvNo;
		private long? _ActivityId;
		private String _StepName;
		private String _StepDescription;
		private DateTime? _StepDate;
		private Decimal? _StepAmount;
		private Boolean? _StepStatus;


		public static ActivityReservationStepOperatorTableRecord CreateNewInstance()
        {
            var record = new ActivityReservationStepOperatorTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ActivityReservationStepOperatorTableRecord()
        {
            ;
        }

        static ActivityReservationStepOperatorTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "ActivityReservationStepOperator";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RsvNo", true),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("StepName", false),
				new ColumnMetadata("StepDescription", false),
				new ColumnMetadata("StepDate", false),
				new ColumnMetadata("StepAmount", false),
				new ColumnMetadata("StepStatus", false),

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
