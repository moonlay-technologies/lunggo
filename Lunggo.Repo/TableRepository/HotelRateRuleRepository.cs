using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class HotelRateRuleTableRepo : TableDao<HotelRateRuleTableRecord>, IDbTableRepository<HotelRateRuleTableRecord> 
    {
		private static readonly HotelRateRuleTableRepo Instance = new HotelRateRuleTableRepo("HotelRateRule");
        
        private HotelRateRuleTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static HotelRateRuleTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, HotelRateRuleTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, HotelRateRuleTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, HotelRateRuleTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public HotelRateRuleTableRecord Find1(IDbConnection connection, HotelRateRuleTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<HotelRateRuleTableRecord> Find(IDbConnection connection, HotelRateRuleTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<HotelRateRuleTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, HotelRateRuleTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, HotelRateRuleTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, HotelRateRuleTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public HotelRateRuleTableRecord Find1(IDbConnection connection, HotelRateRuleTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<HotelRateRuleTableRecord> Find(IDbConnection connection, HotelRateRuleTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<HotelRateRuleTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}