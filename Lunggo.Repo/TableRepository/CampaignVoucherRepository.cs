using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class CampaignVoucherTableRepo : TableDao<CampaignVoucherTableRecord>, IDbTableRepository<CampaignVoucherTableRecord> 
    {
		private static readonly CampaignVoucherTableRepo Instance = new CampaignVoucherTableRepo("CampaignVoucher");
        
        private CampaignVoucherTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static CampaignVoucherTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, CampaignVoucherTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, CampaignVoucherTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, CampaignVoucherTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public CampaignVoucherTableRecord Find1(IDbConnection connection, CampaignVoucherTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<CampaignVoucherTableRecord> Find(IDbConnection connection, CampaignVoucherTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<CampaignVoucherTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, CampaignVoucherTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, CampaignVoucherTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, CampaignVoucherTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public CampaignVoucherTableRecord Find1(IDbConnection connection, CampaignVoucherTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<CampaignVoucherTableRecord> Find(IDbConnection connection, CampaignVoucherTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<CampaignVoucherTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}