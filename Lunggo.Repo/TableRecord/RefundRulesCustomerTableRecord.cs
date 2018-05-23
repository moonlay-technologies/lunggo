using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RefundRulesCustomerTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public long? RuleId
		{
		    get { return _RuleId; }
		    set
		    {
		        _RuleId = value;
		        IncrementLog("RuleId");
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
		public String RuleName
		{
		    get { return _RuleName; }
		    set
		    {
		        _RuleName = value;
		        IncrementLog("RuleName");
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
		public Decimal? ValuePercentage
		{
		    get { return _ValuePercentage; }
		    set
		    {
		        _ValuePercentage = value;
		        IncrementLog("ValuePercentage");
		    }
		}
		public Decimal? ValueConstant
		{
		    get { return _ValueConstant; }
		    set
		    {
		        _ValueConstant = value;
		        IncrementLog("ValueConstant");
		    }
		}
		public Decimal? MinValue
		{
		    get { return _MinValue; }
		    set
		    {
		        _MinValue = value;
		        IncrementLog("MinValue");
		    }
		}
		public Double? PayDateLimit
		{
		    get { return _PayDateLimit; }
		    set
		    {
		        _PayDateLimit = value;
		        IncrementLog("PayDateLimit");
		    }
		}
		public String PayState
		{
		    get { return _PayState; }
		    set
		    {
		        _PayState = value;
		        IncrementLog("PayState");
		    }
		}

		
		private long? _RuleId;
		private long? _ActivityId;
		private String _RuleName;
		private String _Description;
		private Decimal? _ValuePercentage;
		private Decimal? _ValueConstant;
		private Decimal? _MinValue;
		private Double? _PayDateLimit;
		private String _PayState;


		public static RefundRulesCustomerTableRecord CreateNewInstance()
        {
            var record = new RefundRulesCustomerTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RefundRulesCustomerTableRecord()
        {
            ;
        }

        static RefundRulesCustomerTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "RefundRulesCustomer";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("RuleId", false),
				new ColumnMetadata("ActivityId", false),
				new ColumnMetadata("RuleName", false),
				new ColumnMetadata("Description", false),
				new ColumnMetadata("ValuePercentage", false),
				new ColumnMetadata("ValueConstant", false),
				new ColumnMetadata("MinValue", false),
				new ColumnMetadata("PayDateLimit", false),
				new ColumnMetadata("PayState", false),

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
