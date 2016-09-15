using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Identity.AuthStore
{
    public class DapperAuthStore
    {
        public Client FindClient(string clientId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetClientByIdQuery.GetInstance().Execute(conn, new { Id = clientId }).SingleOrDefault();
                var client = ToClient(record);
                return client;
            }
        }

        public async Task<RefreshToken> GetExistingRefreshToken(RefreshToken token)
        {
            return await Task.Run(() =>
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var existingTokenRecord =
                        GetRefreshTokenByClientAndSubjectAndDeviceQuery.GetInstance()
                                .Execute(conn, new { token.Subject, token.ClientId, token.DeviceId })
                                .SingleOrDefault();
                    return ToRefreshToken(existingTokenRecord);
                }
            });
        }

        public async Task<bool> AddOrReplaceRefreshToken(RefreshToken token, bool ignoreDevice)
        {
            return await Task.Run(() =>
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    if (ignoreDevice)
                    {
                        var existingTokenRecords =
                            GetRefreshTokenByClientAndSubjectQuery.GetInstance()
                                .Execute(conn, new {token.Subject, token.ClientId});

                        Parallel.ForEach(existingTokenRecords, existingTokenRecord =>
                        {
                            var existingToken = ToRefreshToken(existingTokenRecord);
                            if (existingToken != null)
                                RemoveRefreshToken(existingToken).Wait();
                        });

                        return RefreshTokenTableRepo.GetInstance().Insert(conn, ToRefreshTokenRecord(token)) > 0;
                    }
                    else
                    {
                        var existingTokenRecords =
                            GetRefreshTokenByClientAndSubjectAndDeviceQuery.GetInstance()
                                .Execute(conn, new { token.Subject, token.ClientId, token.DeviceId });

                        Parallel.ForEach(existingTokenRecords, existingTokenRecord =>
                        {
                            var existingToken = ToRefreshToken(existingTokenRecord);
                            if (existingToken != null)
                                RemoveRefreshToken(existingToken).Wait();
                        });

                        return RefreshTokenTableRepo.GetInstance().Insert(conn, ToRefreshTokenRecord(token)) > 0;
                    }
                }
            });
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            return await Task.Run(() =>
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var refreshToken =
                        GetRefreshTokenByIdQuery.GetInstance().Execute(conn, new { Id = refreshTokenId });
                    if (refreshToken != null)
                        return
                            RefreshTokenTableRepo.GetInstance()
                                .Delete(conn, new RefreshTokenTableRecord { Id = refreshTokenId }) > 0;
                    return false;
                }
            });
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return await Task.Run(() =>
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    return
                        RefreshTokenTableRepo.GetInstance()
                            .Delete(conn, new RefreshTokenTableRecord { Id = refreshToken.Id }) > 0;
                }
            });
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return await Task.Run(() =>
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var refreshTokenRecord =
                        GetRefreshTokenByIdQuery.GetInstance()
                            .Execute(conn, new { Id = refreshTokenId })
                            .SingleOrDefault();
                    var refreshToken = ToRefreshToken(refreshTokenRecord);
                    return refreshToken;
                }
            });

        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return RefreshTokenTableRepo.GetInstance().FindAll(conn).Select(ToRefreshToken).ToList();
            }
        }

        private static Client ToClient(ClientTableRecord record)
        {
            if (record == null)
                return null;

            return new Client
            {
                Id = record.Id,
                Name = record.Name,
                Secret = record.Secret,
                ApplicationType = ApplicationTypeCd.Mnemonic(record.ApplicationTypeCd),
                IsActive = record.IsActive.GetValueOrDefault(),
                AllowedOrigin = record.AllowedOrigin
            };
        }

        private static ClientTableRecord ToClientRecord(Client client)
        {
            if (client == null)
                return null;

            return new ClientTableRecord
            {
                Id = client.Id,
                Name = client.Name,
                Secret = client.Secret,
                ApplicationTypeCd = ApplicationTypeCd.Mnemonic(client.ApplicationType),
                IsActive = client.IsActive,
                AllowedOrigin = client.AllowedOrigin
            };
        }

        private static RefreshToken ToRefreshToken(RefreshTokenTableRecord record)
        {
            if (record == null)
                return null;

            return new RefreshToken
            {
                Id = record.Id,
                Subject = record.Subject,
                ClientId = record.ClientId,
                DeviceId = record.DeviceId,
                IssueTime = record.IssueTime.GetValueOrDefault(),
                ExpireTime = record.ExpireTime.GetValueOrDefault(),
                ProtectedTicket = record.ProtectedTicket
            };
        }

        private static RefreshTokenTableRecord ToRefreshTokenRecord(RefreshToken token)
        {
            if (token == null)
                return null;

            return new RefreshTokenTableRecord
            {
                Id = token.Id,
                Subject = token.Subject,
                ClientId = token.ClientId,
                DeviceId = token.DeviceId,
                IssueTime = token.IssueTime,
                ExpireTime = token.ExpireTime,
                ProtectedTicket = token.ProtectedTicket
            };
        }
    }
}
