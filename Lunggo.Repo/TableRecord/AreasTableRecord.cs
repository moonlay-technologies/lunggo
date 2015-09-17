using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class AreasTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public int? AreaCd
		{
		    get { return _AreaCd; }
		    set
		    {
		        _AreaCd = value;
		        IncrementLog("AreaCd");
		    }
		}
		public String AreaName
		{
		    get { return _AreaName; }
		    set
		    {
		        _AreaName = value;
		        IncrementLog("AreaName");
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

		
		private int? _AreaCd;
		private String _AreaName;
		private String _LangCd;


		public static AreasTableRecord CreateNewInstance()
        {
            var record = new AreasTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public AreasTableRecord()
        {
            ;
        }

        static AreasTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Areas";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("AreaCd", true),
				new ColumnMetadata("AreaName", false),
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
