using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class CartsTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String CartId
		{
		    get { return _CartId; }
		    set
		    {
		        _CartId = value;
		        IncrementLog("CartId");
		    }
		}
		public String RsvNoList
		{
		    get { return _RsvNoList; }
		    set
		    {
		        _RsvNoList = value;
		        IncrementLog("RsvNoList");
		    }
		}

		
		private String _CartId;
		private String _RsvNoList;


		public static CartsTableRecord CreateNewInstance()
        {
            var record = new CartsTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public CartsTableRecord()
        {
            ;
        }

        static CartsTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Carts";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("CartId", true),
				new ColumnMetadata("RsvNoList", false),

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
