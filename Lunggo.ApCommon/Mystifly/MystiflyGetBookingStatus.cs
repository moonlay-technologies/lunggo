using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override GetBookingStatusResult GetBookingStatus()
        {
            using (var client = new MystiflyClientHandler())
            {
                var result = new GetBookingStatusResult();
                var takeRequest = new AirMessageQueueRQ
                {
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };
                AirMessageQueueRS takeResponse;
                var items = new List<Item>();
                
                takeRequest.CategoryId = QueueCategory.Booking;
                takeResponse = client.MessageQueues(takeRequest);
                if (takeResponse.Success)
                {
                    items.AddRange(takeResponse.MessageItems.Select(message => new Item
                    {
                        UniqueId = message.UniqueID,
                        CategoryId = QueueCategory.Booking,
                        ExtensionData = null
                    }));
                    result.BookingStatusInfos.AddRange(takeResponse.MessageItems.Select(item => new BookingStatusInfo
                    {
                        BookingId = item.UniqueID,
                        BookingStatus = BookingStatus.Booked,
                        TimeLimit = item.TktTimeLimit
                    }));
                }

                takeRequest.CategoryId = QueueCategory.Cancelled;
                takeResponse = client.MessageQueues(takeRequest);
                if (takeResponse.Success)
                {
                    items.AddRange(takeResponse.MessageItems.Select(message => new Item
                    {
                        UniqueId = message.UniqueID,
                        CategoryId = QueueCategory.Cancelled,
                        ExtensionData = null
                    }));
                    result.BookingStatusInfos.AddRange(takeResponse.MessageItems.Select(item => new BookingStatusInfo
                    {
                        BookingId = item.UniqueID,
                        BookingStatus = BookingStatus.Cancelled,
                        TimeLimit = null
                    }));
                }

                takeRequest.CategoryId = QueueCategory.Ticketed;
                takeResponse = client.MessageQueues(takeRequest);
                if (takeResponse.Success)
                {
                    items.AddRange(takeResponse.MessageItems.Select(message => new Item
                    {
                        UniqueId = message.UniqueID,
                        CategoryId = QueueCategory.Ticketed,
                        ExtensionData = null
                    }));
                    result.BookingStatusInfos.AddRange(takeResponse.MessageItems.Select(item => new BookingStatusInfo
                    {
                        BookingId = item.UniqueID,
                        BookingStatus = BookingStatus.Ticketed,
                        TimeLimit = null
                    }));
                }

                takeRequest.CategoryId = QueueCategory.ScheduleChange;
                takeResponse = client.MessageQueues(takeRequest);
                if (takeResponse.Success)
                {
                    items.AddRange(takeResponse.MessageItems.Select(message => new Item
                    {
                        UniqueId = message.UniqueID,
                        CategoryId = QueueCategory.ScheduleChange,
                        ExtensionData = null
                    }));
                    result.ChangedScheduleBooking.AddRange(takeResponse.MessageItems.Select(item => item.UniqueID));
                }

                takeRequest.CategoryId = QueueCategory.Urgent;
                takeResponse = client.MessageQueues(takeRequest);
                if (takeResponse.Success)
                {
                    items.AddRange(takeResponse.MessageItems.Select(message => new Item
                    {
                        UniqueId = message.UniqueID,
                        CategoryId = QueueCategory.Urgent,
                        ExtensionData = null
                    }));
                    result.BookingStatusInfos.AddRange(takeResponse.MessageItems.Select(item => new BookingStatusInfo
                    {
                        BookingId = item.UniqueID,
                        BookingStatus = BookingStatus.Ticketed,
                        TimeLimit = null
                    }));
                }

                var deleteRequest = new AirRemoveMessageQueueRQ
                {
                    Items = items.ToArray(),
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };

                var x = client.RemoveMessageQueues(deleteRequest);

                return result;
            }
            
        }
    }
}
