using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;

namespace Lunggo.ApCommon.Notifications
{
    public partial class NotificationService
    {
        private static readonly NotificationService Instance = new NotificationService();
        private bool _isInitialized;
        //private static readonly AzureNotificationClient Client = AzureNotificationClient.GetClientInstance();

        private NotificationService()
        {

        }
        public void Init(string connString, string hubName)
        {
            if (!_isInitialized)
            {
                //Client.Init(connString, hubName);
                _isInitialized = true;
            }
        }
        public static NotificationService GetInstance()
        {
            return Instance;
        }

        public string RegisterDevice(string notificationHandle, string deviceId, string userId)
        {
            //return Client.RegisterDevice(notificationHandle, deviceId);
            return RegisterDeviceExpoToDb(notificationHandle, deviceId, userId);
        }

        internal string RegisterDeviceExpoToDb(string notificationHandle, string deviceId, string userId)
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

        public string OperatorRegisterDevice(string notificationHandle, string deviceId, string userId)
        {
            //return Client.RegisterDevice(notificationHandle, deviceId);
            return OperatorRegisterDeviceExpoToDb(notificationHandle, deviceId, userId);
        }

        internal string OperatorRegisterDeviceExpoToDb(string notificationHandle, string deviceId, string userId)
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


        public bool SendNotificationsCustomer(string messageTitle, string messageBody, string userId)
        {
            var expoTokens = GetCustomerExpoTokenFromDb(userId);
            if (expoTokens == null || expoTokens.Count <  1)
            {
                return false;
            }

            int successCounter = 0;
            int failCounter = 0;

            foreach (var expoData in expoTokens)
            {
                var check = SendNotificationsExpo(messageTitle, messageBody, expoData.Handle);
                if (check)
                {
                    successCounter++;
                }
                else
                {
                    failCounter++;
                }                
            }
            return true;
        }

        public bool SendNotificationsOperator(string messageTitle, string messageBody, string userId)
        {
            var expoTokens = GetOperatorExpoTokenFromDb(userId);
            if (expoTokens == null || expoTokens.Count <  1)
            {
                return false;
            }

            int successCounter = 0;
            int failCounter = 0;

            foreach (var expoData in expoTokens)
            {
                var check = SendNotificationsExpo(messageTitle, messageBody, expoData.Handle);
                if (check)
                {
                    successCounter++;
                }
                else
                {
                    failCounter++;
                }                
            }
            return true;
        }
        

        internal bool SendNotificationsExpo(string messageTitle, string messageBody, string expoToken)
        {
            var pushNotificationClient = new RestClient("https://exp.host");
            var pushNotificationRequest = new RestRequest("/--/api/v2/push/send", Method.POST);
            var pushNotificationBody = new Notification();
            pushNotificationBody.Title = messageTitle;
            pushNotificationBody.Message = messageBody;
            pushNotificationBody.Receiver = expoToken;
            pushNotificationBody.Sound = "default";
            var pushNotificationBodyJson = pushNotificationBody.ConvertToNotificationJsonFormat();
            pushNotificationRequest.AddJsonBody(pushNotificationBodyJson);
            var pushNotificationResponse = pushNotificationClient.Execute(pushNotificationRequest);
            return pushNotificationResponse.StatusCode == HttpStatusCode.OK;
        }

        internal List<NotificationTableRecord> GetCustomerExpoTokenFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord();
                notificationRecord.UserId = userId;
                var userExpoToken = NotificationTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                return userExpoToken;
            }
        }

        internal List<NotificationOperatorTableRecord> GetOperatorExpoTokenFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationOperatorTableRecord();
                notificationRecord.UserId = userId;
                var userExpoToken = NotificationOperatorTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                return userExpoToken;
            }
        }


        public bool UpdateTags(string registrationId, string notificationHandle, Platform platform,
            Dictionary<string, string> tags)
        {
            //return Client.UpdateTags(registrationId, notificationHandle, platform, tags);
            return false;
        }

        public void DeleteRegistration(string expoHandle)
        {
            DeleteRegistrationFromDb(expoHandle);
        }

        public void DeleteRegistrationFromDb(string expoHandle)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord();
                notificationRecord.Handle = expoHandle;
                NotificationTableRepo.GetInstance().Delete(conn, notificationRecord);
            }
        }

        public void OperatorDeleteRegistration(string expoHandle)
        {
            OperatorDeleteRegistrationFromDb(expoHandle);
        }

        public void OperatorDeleteRegistrationFromDb(string expoHandle)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationOperatorTableRecord();
                notificationRecord.Handle = expoHandle;
                NotificationOperatorTableRepo.GetInstance().Delete(conn, notificationRecord);
            }
        }

        public void PushNotification(Notification notification, Dictionary<string, string> tags)
        {
            //Client.PushNotification(notification, tags);
        }

        public void PushSilentNotification(object data, Dictionary<string, string> tags)
        {
            //Client.PushSilentNotification(data, tags);
        }

        public bool CheckAvailableExpoToken(string expoToken)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord();
                notificationRecord.Handle = expoToken;
                var result = NotificationTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                return result.Count > 0;
            }
        }

        public bool CheckAvailableOperatorExpoToken(string expoToken)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationOperatorTableRecord();
                notificationRecord.Handle = expoToken;
                var result = NotificationOperatorTableRepo.GetInstance().Find(conn, notificationRecord).ToList();
                return result.Count > 0;
            }
        }
    }
}
