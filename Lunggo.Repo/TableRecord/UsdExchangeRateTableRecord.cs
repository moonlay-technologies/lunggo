using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UsdExchangeRateTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String CurrencyCd
		{
		    get { return _CurrencyCd; }
		    set
		    {
		        _CurrencyCd = value;
		        IncrementLog("CurrencyCd");
		    }
		}
		public Decimal? ExchangeRate
		{
		    get { return _ExchangeRate; }
		    set
		    {
		        _ExchangeRate = value;
		        IncrementLog("ExchangeRate");
		    }
		}

		
		private String _CurrencyCd;
		private Decimal? _ExchangeRate;


		public static UsdExchangeRateTableRecord CreateNewInstance()
        {
            var record = new UsdExchangeRateTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public UsdExchangeRateTableRecord()
        {
            ;
        }

        static UsdExchangeRateTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "UsdExchangeRate";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("CurrencyCd", true),
				new ColumnMetadata("ExchangeRate", false),

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
