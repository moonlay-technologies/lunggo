using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class SubscriberTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String Email
		{
		    get { return _Email; }
		    set
		    {
		        _Email = value;
		        IncrementLog("Email");
		    }
		}
		public Boolean? IsValidated
		{
		    get { return _IsValidated; }
		    set
		    {
		        _IsValidated = value;
		        IncrementLog("IsValidated");
		    }
		}
		public String HashLink
		{
		    get { return _HashLink; }
		    set
		    {
		        _HashLink = value;
		        IncrementLog("HashLink");
		    }
		}

		
		private String _Email;
		private Boolean? _IsValidated;
		private String _HashLink;


		public static SubscriberTableRecord CreateNewInstance()
        {
            var record = new SubscriberTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public SubscriberTableRecord()
        {
            ;
        }

        static SubscriberTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Subscriber";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Email", true),
				new ColumnMetadata("IsValidated", false),
				new ColumnMetadata("HashLink", false),

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
