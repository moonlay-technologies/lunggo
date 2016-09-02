using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class ClientTableRecord : Lunggo.Framework.Database.TableRecord
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
		public String Secret
		{
		    get { return _Secret; }
		    set
		    {
		        _Secret = value;
		        IncrementLog("Secret");
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
		public String ApplicationTypeCd
		{
		    get { return _ApplicationTypeCd; }
		    set
		    {
		        _ApplicationTypeCd = value;
		        IncrementLog("ApplicationTypeCd");
		    }
		}
		public Boolean? IsActive
		{
		    get { return _IsActive; }
		    set
		    {
		        _IsActive = value;
		        IncrementLog("IsActive");
		    }
		}
		public int? RefreshTokenLifeTime
		{
		    get { return _RefreshTokenLifeTime; }
		    set
		    {
		        _RefreshTokenLifeTime = value;
		        IncrementLog("RefreshTokenLifeTime");
		    }
		}
		public String AllowedOrigin
		{
		    get { return _AllowedOrigin; }
		    set
		    {
		        _AllowedOrigin = value;
		        IncrementLog("AllowedOrigin");
		    }
		}

		
		private String _Id;
		private String _Secret;
		private String _Name;
		private String _ApplicationTypeCd;
		private Boolean? _IsActive;
		private int? _RefreshTokenLifeTime;
		private String _AllowedOrigin;


		public static ClientTableRecord CreateNewInstance()
        {
            var record = new ClientTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public ClientTableRecord()
        {
            ;
        }

        static ClientTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "Client";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Id", true),
				new ColumnMetadata("Secret", false),
				new ColumnMetadata("Name", false),
				new ColumnMetadata("ApplicationTypeCd", false),
				new ColumnMetadata("IsActive", false),
				new ColumnMetadata("RefreshTokenLifeTime", false),
				new ColumnMetadata("AllowedOrigin", false),

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
