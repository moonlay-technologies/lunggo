using System;
using System.Collections.Generic;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using System.Data;

namespace Lunggo.Repository.TableRepository
{
	public class HotelReservationDetailsTableRepo : TableDao<HotelReservationDetailsTableRecord>, IDbTableRepository<HotelReservationDetailsTableRecord> 
    {
		private static readonly HotelReservationDetailsTableRepo Instance = new HotelReservationDetailsTableRepo("HotelReservationDetails");
        
        private HotelReservationDetailsTableRepo(String tableName) : base(tableName)
        {
            ;
        }

		public static HotelReservationDetailsTableRepo GetInstance()
        {
            return Instance;
        }

        public int Insert(IDbConnection connection, HotelReservationDetailsTableRecord record)
        {
            return Insert(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public int Delete(IDbConnection connection, HotelReservationDetailsTableRecord record)
        {
            return Delete(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public int Update(IDbConnection connection, HotelReservationDetailsTableRecord record)
        {
            return Update(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public HotelReservationDetailsTableRecord Find1(IDbConnection connection, HotelReservationDetailsTableRecord record)
        {
            return Find1(connection, record, CommandDefinition.GetDefaultDefinition());
        }

		public IEnumerable<HotelReservationDetailsTableRecord> Find(IDbConnection connection, HotelReservationDetailsTableRecord record)
        {
            return Find(connection, record, CommandDefinition.GetDefaultDefinition());
        }

        public IEnumerable<HotelReservationDetailsTableRecord> FindAll(IDbConnection connection)
        {
            return FindAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int DeleteAll(IDbConnection connection)
        {
            return DeleteAll(connection, CommandDefinition.GetDefaultDefinition());
        }

        public int Insert(IDbConnection connection, HotelReservationDetailsTableRecord record, CommandDefinition definition)
        {
            return InsertInternal(connection, record, definition);
        }

        public int Delete(IDbConnection connection, HotelReservationDetailsTableRecord record, CommandDefinition definition)
        {
            return DeleteInternal(connection, record, definition);
        }

        public int Update(IDbConnection connection, HotelReservationDetailsTableRecord record, CommandDefinition definition)
        {
            return UpdateInternal(connection, record, definition);
        }

		public HotelReservationDetailsTableRecord Find1(IDbConnection connection, HotelReservationDetailsTableRecord record, CommandDefinition definition)
        {
			return Find1Internal(connection, record, definition);
        }

		public IEnumerable<HotelReservationDetailsTableRecord> Find(IDbConnection connection, HotelReservationDetailsTableRecord record, CommandDefinition definition)
        {
			return FindInternal(connection, record, definition);
        }

        public int DeleteAll(IDbConnection connection, CommandDefinition definition)
        {
            return DeleteAllInternal(connection, definition);
        }

        public IEnumerable<HotelReservationDetailsTableRecord> FindAll(IDbConnection connection, CommandDefinition definition)
        {
            return FindAllInternal(connection, definition);
        }
	}	
}