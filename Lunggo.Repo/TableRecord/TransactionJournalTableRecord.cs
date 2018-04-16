using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class TransactionJournalTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public DateTime? Time
		{
		    get { return _Time; }
		    set
		    {
		        _Time = value;
		        IncrementLog("Time");
		    }
		}
		public String ToAccountNo
		{
		    get { return _ToAccountNo; }
		    set
		    {
		        _ToAccountNo = value;
		        IncrementLog("ToAccountNo");
		    }
		}
		public String FromAccountNo
		{
		    get { return _FromAccountNo; }
		    set
		    {
		        _FromAccountNo = value;
		        IncrementLog("FromAccountNo");
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
		public Decimal? BalanceAfter
		{
		    get { return _BalanceAfter; }
		    set
		    {
		        _BalanceAfter = value;
		        IncrementLog("BalanceAfter");
		    }
		}
		public String Remark
		{
		    get { return _Remark; }
		    set
		    {
		        _Remark = value;
		        IncrementLog("Remark");
		    }
		}

		
		private String _Id;
		private DateTime? _Time;
		private String _ToAccountNo;
		private String _FromAccountNo;
		private Decimal? _Amount;
		private Decimal? _BalanceAfter;
		private String _Remark;


		public static TransactionJournalTableRecord CreateNewInstance()
        {
            var record = new TransactionJournalTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public TransactionJournalTableRecord()
        {
            ;
        }

        static TransactionJournalTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "TransactionJournal";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Time", false),
				new ColumnMetadata("ToAccountNo", false),
				new ColumnMetadata("FromAccountNo", false),
				new ColumnMetadata("Amount", false),
				new ColumnMetadata("BalanceAfter", false),
				new ColumnMetadata("Remark", false),

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
