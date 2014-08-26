using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class RolesTableRecord : Lunggo.Framework.Database.TableRecord
    {
		public int? Id
		{
		    get { return _Id; }
		    set
		    {
		        _Id = value;
		        IncrementLog("Id");
		    }
		}
		public String Name
		{
		    get { return _Name; }
		    set
		    {
		        _Name = value;
		        IncrementLog("Name");
		    }
		}

		
		private int? _Id;
		private String _Name;


		public static RolesTableRecord CreateNewInstance()
        {
            var record = new RolesTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public RolesTableRecord()
        {
            ;
        }

        static RolesTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            TableName = "Roles";
        }

        private static void InitRecordMetadata()
        {
            RecordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Name", false),

            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            PrimaryKeys = RecordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }
    }
}
