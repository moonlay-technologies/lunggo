using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Notifications
{
    public class NotificationDbService
    {
        internal virtual string RegisterDeviceExpoToDb(string notificationHandle, string deviceId, string userId)
        {
            var RegisterId = "";
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {                
                var notificationRecord = new NotificationTableRecord
                {
                    Handle = notificationHandle,
                    DeviceId = deviceId,
                    UserId = userId
                };
                if (CheckAvailableExpoToken(notificationHandle))
                {
                    NotificationTableRepo.GetInstance().Update(conn, notificationRecord);
                }
                else
                {
                    NotificationTableRepo.GetInstance().Insert(conn, notificationRecord);
                }
            }
            return RegisterId;
        }

        internal virtual bool CheckAvailableExpoToken(string expoToken)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord();
                notificationRecord.Handle = expoToken;
                var result = NotificationTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                return result.Count > 0;
            }
        }

        internal virtual string OperatorRegisterDeviceExpoToDb(string notificationHandle, string deviceId, string userId)
        {
            var RegisterId = "";
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {                
                var notificationRecord = new NotificationOperatorTableRecord
                {
                    Handle = notificationHandle,
                    DeviceId = deviceId,
                    UserId = userId
                };
                if (CheckAvailableOperatorExpoToken(notificationHandle))
                {
                    NotificationOperatorTableRepo.GetInstance().Update(conn, notificationRecord);
                }
                else
                {
                    NotificationOperatorTableRepo.GetInstance().Insert(conn, notificationRecord);
                }
            }
            return RegisterId;
        }

        internal virtual bool CheckAvailableOperatorExpoToken(string expoToken)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationOperatorTableRecord();
                notificationRecord.Handle = expoToken;
                var result = NotificationOperatorTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                return result.Count > 0;
            }
        }

        internal virtual List<string> GetCustomerExpoTokenFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord();
                notificationRecord.UserId = userId;
                var userExpoToken = NotificationTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                if (userExpoToken == null || userExpoToken.Count < 1)
                {
                    return null;
                }
                return userExpoToken.Select(a => a.Handle).ToList();
            }
        }

        internal virtual List<string> GetOperatorExpoTokenFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationOperatorTableRecord();
                notificationRecord.UserId = userId;
                var userExpoToken = NotificationOperatorTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                if (userExpoToken == null || userExpoToken.Count < 1)
                {
                    return null;
                }
                return userExpoToken.Select(a => a.Handle).ToList();
            }
        }

        internal virtual void DeleteRegistrationFromDb(string expoHandle)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord();
                notificationRecord.Handle = expoHandle;
                NotificationTableRepo.GetInstance().Delete(conn, notificationRecord);
            }
        }

        internal virtual void OperatorDeleteRegistrationFromDb(string expoHandle)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationOperatorTableRecord();
                notificationRecord.Handle = expoHandle;
                NotificationOperatorTableRepo.GetInstance().Delete(conn, notificationRecord);
            }
        }
    }
}
