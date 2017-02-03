using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class UserApproverTableRecord : Lunggo.Framework.Database.TableRecord
    {
        private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

        public String UserId
        {
            get { return _UserId; }
            set
            {
                _UserId = value;
                IncrementLog("UserId");
            }
        }
        public String ApproverId
        {
            get { return _ApproverId; }
            set
            {
                _ApproverId = value;
                IncrementLog("ApproverId");
            }
        }


        private String _UserId;
        private String _ApproverId;


        public static UserApproverTableRecord CreateNewInstance()
        {
            var record = new UserApproverTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

        public UserApproverTableRecord()
        {
            ;
        }

        static UserApproverTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "UserApprover";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("UserId", true),
				new ColumnMetadata("ApproverId", true),

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
