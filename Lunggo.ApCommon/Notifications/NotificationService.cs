using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lunggo.ApCommon.Notifications
{
    public partial class NotificationService
    {
        private static readonly NotificationService Instance = new NotificationService();
        private bool _isInitialized;

        private NotificationService()
        {

        }
        public void Init(string connString, string hubName)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }
        public static NotificationService GetInstance()
        {
            return Instance;
        }

        public void RegisterDevice(string token, string deviceId, string userId, params string[] tags)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var deviceRegistered = NotificationTableRepo.GetInstance().Find1(conn, new NotificationTableRecord { Handle = token });
                NotificationTableRecord TableRecord = prepareQueryParams(token, deviceId, userId, tags);
                if (deviceRegistered == null)
                {
                    NotificationTableRepo.GetInstance().Insert(conn, TableRecord);
                }
                else
                {
                    NotificationTableRepo.GetInstance().Update(conn, TableRecord);
                }
            }
        }

        public void DeleteRegistration(string token)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var deletedId = NotificationTableRepo.GetInstance().Delete(conn, new NotificationTableRecord { Handle = token });
            }
        }

        private static NotificationTableRecord prepareQueryParams(string token, string deviceId, string userId, string[] tags)
        {
            var tagString = string.Join(",", tags);
            tagString = "," + tagString + ",";
            var TableRecord = new NotificationTableRecord
            {
                Handle = token,
                DeviceId = deviceId,
                UserId = userId,
                Tags = tagString
            };
            return TableRecord;
        }
    }
}
