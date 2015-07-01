using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.ApCommon.Identity.Role;
using Lunggo.ApCommon.Query;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.AspNet.Identity;


namespace Lunggo.ApCommon.Identity.RoleStore
{
    public class DapperRoleStore<TRole> : IRoleStore<TRole> where TRole : IdentityRole<String>, new() 
    {

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = RolesTableRepo.GetInstance();
                    role.Id = RoleIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
                    var toBeInsertedRecord = ToRolesTableRecord(role);
                    repo.Insert(connection, toBeInsertedRecord);
                }
            });
        }

        private RolesTableRecord ToRolesTableRecord(TRole role)
        {
            var record = new RolesTableRecord
            {
                Id = role.Id,
                Name = role.Name
            };
            return record;
        }

        private RolesTableRecord ToRolesTableRecordPkOnly(TRole role)
        {
            var record = new RolesTableRecord
            {
                Id = role.Id
            };
            return record;
        }

        private TRole ToRole(GetRoleByAnyQueryRecord record)
        {
            var role = new TRole
            {

                Name = record.Name,
                Id = record.Id
            };
            return role;
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = RolesTableRepo.GetInstance();
                    var toBeDeletedRecord = ToRolesTableRecordPkOnly(role);
                    repo.Delete(connection, toBeDeletedRecord);
                }
            });

        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetRoleByIdQuery.GetInstance();
                    var record = query.Execute(connection, new {Id = roleId}).SingleOrDefault();
                    var role = ToRole(record);
                    return role;
                }
            });
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var query = GetRoleByNameQuery.GetInstance();
                    var record = query.Execute(connection, new { Name = roleName }).SingleOrDefault();
                    var role = ToRole(record);
                    return role;
                }
            });
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() =>
            {
                using (var connection = DbService.GetInstance().GetOpenConnection())
                {
                    var repo = RolesTableRepo.GetInstance();
                    var toBeInsertedRecord = ToRolesTableRecord(role);
                    repo.Update(connection, toBeInsertedRecord);
                }
            });
        }

        public void Dispose()
        {
            ;
        }
    }
}
