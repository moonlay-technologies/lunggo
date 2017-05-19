using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.SharedModel;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebJob.BookingAutomation
{
    public partial class Program
    {
        public static void SaveBookingLogToStorage(string supplierName, FlightBookApiResponse bookResponse)
        {
            var prevData = GetBookingLog() + "\r\n ";
            var message = "";
            if (bookResponse != null && bookResponse.IsValid == true)
            {
                message = "Success";
            }
            else
            {
                message = "Failed";
            }
            if (bookResponse == null)
            {
                prevData = prevData + supplierName + " : " + message + " , Response NULL ";
            }
            else
            {
                prevData = prevData + supplierName + " : " + message + " Error Code : " + bookResponse.ErrorCode;    
            }
            
            var byteData = Encoding.ASCII.GetBytes(prevData);
            SaveLogToBlob(byteData);
        }

        public static string GetBookingLog()
        {
            var blob = BlobStorageService.GetInstance().GetByteArrayByFileInContainer("BookingLog.txt", "FlightBookingLog");
            string result = Encoding.UTF8.GetString(blob);
            return result;
        }

        public static void SaveLogToBlob(byte[] data)
        {
            BlobStorageService.GetInstance().WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    Container = "FlightBookingLog",
                    FileInfo = new FileInfo
                    {
                        ContentType = "application/text",
                        FileData = data,
                        FileName = "BookingLog.txt"
                    }
                },
                SaveMethod = SaveMethod.Force
            });
            Console.WriteLine("Done saving booking log");
        }

        public static void SaveDateSearch(string stringDate)
        {
            string fixData = "";
            if (!isFirst)
            {
                var prevData = GetBookingLog();
                fixData = prevData + " \r\r\n " + "Date : " + stringDate + "\r\n" + "========================";
            }
            else
            {
                fixData = "Date" + stringDate + "\r\n" + "========================";
            }
            var btyeData = Encoding.ASCII.GetBytes(fixData);
            SaveLogToBlob(btyeData);
            isFirst = false;
        }

        public static void FlightUnavailable(string supplierName)
        {
            var prevData = GetBookingLog() + "\r\n ";

            prevData = prevData + supplierName + " : No Flights Available";
            var byteData = Encoding.ASCII.GetBytes(prevData);
            SaveLogToBlob(byteData);
        }
    }

}
