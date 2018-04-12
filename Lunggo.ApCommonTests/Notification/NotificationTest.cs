using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Notifications;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using System.Data;

namespace Lunggo.ApCommonTests.Notification
{
    [TestClass]
    class NotificationTest
    {
        public void ManipulateDB(Action<IDbConnection> callback)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                callback(conn);
            }
        }

        public void AssertRegisterDevice(string dummyHandle = "TEST_HANDLE", string dummyDeviceId = "TEST_DEVICEID", string dummyUserId = "TEST_USERID", params string[] tags)
        {
            NotificationTableRecord actualResult;
            NotificationService.GetInstance().RegisterDevice(dummyHandle, dummyDeviceId, dummyUserId);
            ManipulateDB((conn) =>
            {
                actualResult = NotificationTableRepo.GetInstance().Find1(conn, new NotificationTableRecord { Handle = dummyHandle });
                NotificationTableRepo.GetInstance().Delete(conn, new NotificationTableRecord { Handle = dummyHandle });
                Assert.IsTrue(dummyHandle == actualResult.Handle &&
                            dummyDeviceId == actualResult.DeviceId &&
                            dummyUserId == actualResult.UserId
                );
            });
        }

        [TestMethod]
        public void Should_Success_On_Register_New_Device()
        {
            ManipulateDB((conn) =>
            {
               NotificationTableRepo.GetInstance().Delete(conn, new NotificationTableRecord { Handle = "TEST_HANDLE" });
            });
            AssertRegisterDevice();
        }

        [TestMethod]
        public void Should_Success_On_Register_Existing_Device_With_Changed_Paramter()
        {
            ManipulateDB((conn) =>
            {
               NotificationTableRepo.GetInstance().Insert(conn, new NotificationTableRecord { Handle = "TEST_HANDLE", DeviceId = "TEST_DEVICEID" });
            });
            AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID_2");
        }

        [TestMethod]
        public void Should_Error_On_Register_Device_When_Handle_Params_Is_Null_Or_Empty_Or_Whitespace()
        {
            AssertRegisterDevice("", "TEST_DEVICEID");
            AssertRegisterDevice(null, "TEST_DEVICEID");
            AssertRegisterDevice(" ", "TEST_DEVICEID");
        }

        [TestMethod]
        public void Should_Error_On_Register_Device_When_DeviceID_Params_Is_Null_Or_Empty_Or_Whitespace()
        {
            AssertRegisterDevice("TEST_HANDLE", "");
            AssertRegisterDevice("TEST_HANDLE", null);
            AssertRegisterDevice("TEST_HANDLE", " ");

        }

        // [TestMethod]
        // public void Should_Error_On_Register_Device_When_UserID_Params_Is_Null_Or_Empty_Or_Whitespace()
        // {

        //     AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", null);
        //     AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", "");
        //     AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", " ");
        // }

        // [TestMethod]
        // public void Should_RegisterDuplicateDeviceWithOneDifferentParam()
        // {
        //     AssertRegisterDevice("TEST_HANDLE_2", "TEST_DEVICEID", "TEST_USERID");
        //     AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID_2", "TEST_USERID");
        //     AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", "TEST_USERID_2");
        // }

        [TestMethod]
        public void Should_Success_When_Register_Device_With_Tags()
        {
            AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", "TEST_USERID", "tag1");
            AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", "TEST_USERID", "tag1", "tag2");
            AssertRegisterDevice("TEST_HANDLE", "TEST_DEVICEID", "TEST_USERID", "tag1", "tag2", "tag3");
        }
    }
}
