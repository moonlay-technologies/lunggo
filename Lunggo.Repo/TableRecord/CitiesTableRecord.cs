using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class CitiesTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public int? CityCd
		{
		    get { return _CityCd; }
		    set
		    {
		        _CityCd = value;
		        IncrementLog("CityCd");
		    }
		}
		public String CityName
		{
		    get { return _CityName; }
		    set
		    {
		        _CityName = value;
		        IncrementLog("CityName");
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

		
		private int? _CityCd;
		private String _CityName;
		private String _LangCd;


		public static CitiesTableRecord CreateNewInstance()
        {
            var record = new CitiesTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public CitiesTableRecord()
        {
            ;
        }

        static CitiesTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Cities";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("CityCd", true),
				new ColumnMetadata("CityName", false),
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
