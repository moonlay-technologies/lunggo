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
        public NotificationDbService _db { get; set; }
        private static readonly NotificationService Instance = new NotificationService();
        private bool _isInitialized;
        //private static readonly AzureNotificationClient Client = AzureNotificationClient.GetClientInstance();

        public NotificationService() : this(null)
        {

        }

        public NotificationService(NotificationDbService notificationDbService)
        {
            _db = notificationDbService ?? new NotificationDbService();
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
            if (string.IsNullOrWhiteSpace(notificationHandle) || string.IsNullOrWhiteSpace(userId)) 
            {
                return null;
            }
            //return Client.RegisterDevice(notificationHandle, deviceId);
            return _db.RegisterDeviceExpoToDb(notificationHandle, deviceId, userId);
        }

      
        public string OperatorRegisterDevice(string notificationHandle, string deviceId, string userId)
        {
            //return Client.RegisterDevice(notificationHandle, deviceId);
            if (string.IsNullOrWhiteSpace(notificationHandle) || string.IsNullOrWhiteSpace(userId)) 
            {
                return null;
            }
            return _db.OperatorRegisterDeviceExpoToDb(notificationHandle, deviceId, userId);
        }

        public bool SendNotificationsCustomer(string messageTitle, string messageBody, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }
            var expoTokens = _db.GetCustomerExpoTokenFromDb(userId);
            if (expoTokens == null || expoTokens.Count <  1)
            {
                return false;
            }
            foreach (var expoToken in expoTokens)
            {
                var check = SendNotificationsExpo(messageTitle, messageBody, expoToken);            
            }
            return true;
        }

        public bool SendNotificationsOperator(string messageTitle, string messageBody, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }
            var expoTokens = _db.GetOperatorExpoTokenFromDb(userId);
            if (expoTokens == null || expoTokens.Count <  1)
            {
                return false;
            }
            foreach (var expoToken in expoTokens)
            {
                var check = SendNotificationsExpo(messageTitle, messageBody, expoToken);           
            }
            return true;
        }
        

        public virtual bool SendNotificationsExpo(string messageTitle, string messageBody, string expoToken)
        {
            if (string.IsNullOrWhiteSpace(expoToken))
            {
                return false;
            }
            var pushNotificationClient = new RestClient("https://exp.host");
            var pushNotificationRequest = new RestRequest("/--/api/v2/push/send", Method.POST);
            var pushNotificationBody = new Notification();
            pushNotificationBody.Title = messageTitle;
            pushNotificationBody.Message = messageBody;
            pushNotificationBody.Receiver = expoToken;
            pushNotificationBody.Sound = "default";
            var pushNotificationBodyJson = ConvertToNotificationJsonFormat(pushNotificationBody);
            pushNotificationRequest.AddJsonBody(pushNotificationBodyJson);
            var pushNotificationResponse = pushNotificationClient.Execute(pushNotificationRequest);
            return pushNotificationResponse.StatusCode == HttpStatusCode.OK;
        }

        public bool DeleteRegistration(string expoHandle)
        {
            if (string.IsNullOrWhiteSpace(expoHandle))
            {
                return false;
            }
            _db.DeleteRegistrationFromDb(expoHandle);
            return true;
        }

        public bool OperatorDeleteRegistration(string expoHandle)
        {
            if (string.IsNullOrWhiteSpace(expoHandle))
            {
                return false;
            }
            _db.OperatorDeleteRegistrationFromDb(expoHandle);
            return true;
        }

        public NotificationJsonFormat ConvertToNotificationJsonFormat(Notification notification)
        {
            if (notification == null)
            {
                return null;
            }
            var notificationJsonFormat = new NotificationJsonFormat();
            notificationJsonFormat.to = notification.Receiver;
            notificationJsonFormat.body = notification.Message;
            notificationJsonFormat.sound = notification.Sound;
            notificationJsonFormat.title = notification.Title;
            return notificationJsonFormat;
        }
    }
}
