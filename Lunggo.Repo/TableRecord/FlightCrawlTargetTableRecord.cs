using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class FlightCrawlTargetTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		

		public static FlightCrawlTargetTableRecord CreateNewInstance()
        {
            var record = new FlightCrawlTargetTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public FlightCrawlTargetTableRecord()
        {
            ;
        }

        static FlightCrawlTargetTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "FlightCrawlTarget";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
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
