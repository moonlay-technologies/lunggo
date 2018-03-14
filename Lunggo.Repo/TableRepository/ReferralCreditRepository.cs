using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class ReferralCreditTableRepo : TableDao<ReferralCreditTableRecord>, IDbTableRepository<ReferralCreditTableRecord> 
    {
		private static readonly ReferralCreditTableRepo Instance = new ReferralCreditTableRepo("ReferralCredit");
        
        private ReferralCreditTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static ReferralCreditTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, ReferralCreditTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, ReferralCreditTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, ReferralCreditTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public ReferralCreditTableRecord Find1(IDbConnection connection, ReferralCreditTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<ReferralCreditTableRecord> Find(IDbConnection connection, ReferralCreditTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<ReferralCreditTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, ReferralCreditTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, ReferralCreditTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, ReferralCreditTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public ReferralCreditTableRecord Find1(IDbConnection connection, ReferralCreditTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<ReferralCreditTableRecord> Find(IDbConnection connection, ReferralCreditTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<ReferralCreditTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}