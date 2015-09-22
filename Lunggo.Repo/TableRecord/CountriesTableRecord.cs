using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class CountriesTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public int? CountryCd
		{
		    get { return _CountryCd; }
		    set
		    {
		        _CountryCd = value;
		        IncrementLog("CountryCd");
		    }
		}
		public String CountryName
		{
		    get { return _CountryName; }
		    set
		    {
		        _CountryName = value;
		        IncrementLog("CountryName");
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

		
		private int? _CountryCd;
		private String _CountryName;
		private String _LangCd;


		public static CountriesTableRecord CreateNewInstance()
        {
            var record = new CountriesTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public CountriesTableRecord()
        {
            ;
        }

        static CountriesTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Countries";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("CountryCd", true),
				new ColumnMetadata("CountryName", false),
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
