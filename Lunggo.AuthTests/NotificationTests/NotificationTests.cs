using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Notifications;
using Lunggo.Repository.TableRecord;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lunggo.AuthTests.NotificationTests
{
    [TestClass]
    public class NotificationTests
    {
        [TestMethod]
        public void Register_device_should_return_null_when_expoToken_is_null_or_whitespace()
        {
            string expoToken = null;
            string userId = "14707";
            string deviceId = null;
            var notificationService = new NotificationService();
            var result = notificationService.RegisterDevice(expoToken, deviceId, userId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Operator_Register_device_should_return_null_when_expoToken_is_null_or_whitespace()
        {
            string expoToken = null;
            string userId = "14707";
            string deviceId = null;
            var notificationService = new NotificationService();
            var result = notificationService.RegisterDevice(expoToken, deviceId, userId);
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void Register_device_should_return_null_when_userId_is_null_or_whitespace()
        {
            string expoToken = "testExpoToken";
            string userId = null;
            var notificationService = new NotificationService();
            var result = notificationService.RegisterDevice(expoToken, null, userId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Operator_Register_device_should_return_null_when_userId_is_null_or_whitespace()
        {
            string expoToken = "testExpoToken";
            string userId = null;
            var notificationService = new NotificationService();
            var result = notificationService.OperatorRegisterDevice(expoToken, null, userId);
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void Send_notification_to_custommer_should_return_false_when_userId_is_null_or_whitespace()
        {
            string userId = null;
            string messageTitle = "test";
            string messageBody = "test";
            var notificationService = new NotificationService();
            var result = notificationService.SendNotificationsCustomer(messageTitle, messageBody, userId);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Send_notification_to_custommer_should_return_true_when_success()
        {
            string userId = "test";
            string messageTitle = "test";
            string messageBody = "test";
            var mockNotifDbService = new Mock<NotificationDbService>();
            mockNotifDbService.Setup(a => a.GetCustomerExpoTokenFromDb(userId)).Returns(new List<string> {"test"});
            var notificationService = new Mock<NotificationService>(mockNotifDbService.Object);
            notificationService.Setup(a => a.SendNotificationsExpo("test", "test", "test")).Returns(true);
            var result = notificationService.Object.SendNotificationsCustomer(messageTitle, messageBody, userId);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Send_notification_to_operator_should_return_false_when_userId_is_null_or_whitespace()
        {
            string userId = null;
            string messageTitle = "test";
            string messageBody = "test";
            var notificationService = new NotificationService();            
            var result = notificationService.SendNotificationsOperator(messageTitle, messageBody, userId);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Send_notification_to_customer_should_return_false_when_userId_is_not_registered()
        {
            string userId = "unregistered";
            string messageTitle = "test";
            string messageBody = "test";
            
            //mock userId getter
            var mockNotifDbService = new Mock<NotificationDbService>();            
            mockNotifDbService.Setup(a => a.GetCustomerExpoTokenFromDb(userId)).Returns((List<string>)null);
            var notificationService = new NotificationService(mockNotifDbService.Object);
            var result = notificationService.SendNotificationsCustomer(messageTitle, messageBody, userId);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Send_notification_to_operator_should_return_false_when_userId_not_registered()
        {
            string userId = "unregistered";
            string messageTitle = "test";
            string messageBody = "test";
            var mockNotifDbService = new Mock<NotificationDbService>();            
            mockNotifDbService.Setup(a => a.GetOperatorExpoTokenFromDb(userId)).Returns((List<string>)null);
            var notificationService = new NotificationService(mockNotifDbService.Object);
            var result = notificationService.SendNotificationsOperator(messageTitle, messageBody, userId);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Send_notification_expo_should_return_false_when_expoToken_is_null_or_whitespace()
        {
            string expoToken = null;
            string messageTitle = "test";
            string messageBody = "test";
            var notificationService = new NotificationService();
            var result = notificationService.SendNotificationsExpo(messageTitle, messageBody, expoToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Delete_registration_should_return_false_when_expoToken_is_null_or_whitespace()
        {
            string expoToken = null;
            var notificationService = new NotificationService();
            var result = notificationService.DeleteRegistration(expoToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Operator_delete_registration_should_return_false_when_expoToken_is_null_or_whitespace()
        {
            string expoToken = null;
            var notificationService = new NotificationService();
            var result = notificationService.OperatorDeleteRegistration(expoToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Convert_to_Notification_Json_Format_should_return_null_when_input_Notification_is_null()
        {
            Notification input = null;
            var notificationService = new NotificationService();
            var result = notificationService.ConvertToNotificationJsonFormat(input);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_to_Notification_Json_Format_should_return_Notification_Json_Format_when_valid()
        {
            Notification input = new Notification
            {
                Receiver = "test receiver",
                Sound = "test sound",
                Message = "test message",
                Title = "test title"
            };
            var notificationService = new NotificationService();
            var actualResult = notificationService.ConvertToNotificationJsonFormat(input);
            NotificationJsonFormat expectedResult = new NotificationJsonFormat
            {
                to = "test receiver",
                sound = "test sound",
                body = "test message",
                title = "test title"
            };
            Assert.AreEqual(expectedResult.to, actualResult.to);
            Assert.AreEqual(expectedResult.sound, actualResult.sound);
            Assert.AreEqual(expectedResult.body, actualResult.body);
            Assert.AreEqual(expectedResult.title, actualResult.title);
        }
    }
}
