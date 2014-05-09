using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Lunggo.Framework.Database
{
    public interface IDBTableRepository<T> where T : TableRecord
    {
        void Insert(IDbConnection connection, T record);
        void Delete(IDbConnection connection, T record);
        void Update(IDbConnection connection, T record);
        IEnumerable<T> FindAll(IDbConnection connection);
        void DeleteAll(IDbConnection connection);
    }
}
