using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override List<BookingStatusInfo> GetBookingStatus()
        {
            var statusInfos = new List<BookingStatusInfo>();
            statusInfos.AddRange(GetQueues(QueueCategory.Ticketed));
            statusInfos.AddRange(GetQueues(QueueCategory.ScheduleChange));
            RemoveQueues(statusInfos);
            return statusInfos;
        }

        private static IEnumerable<BookingStatusInfo> GetQueues(QueueCategory category)
        {
            var request = new AirMessageQueueRQ
            {
                CategoryId = category,
                SessionId = Client.SessionId,
                Target = Client.Target,
                ExtensionData = null
            };
            var result = new List<BookingStatusInfo>();
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.MessageQueues(request);
                done = true;
                if (response.Success && !response.Errors.Any())
                {
                    result.AddRange(response.MessageItems.Select(item => new BookingStatusInfo
                    {
                        BookingId = item.UniqueID,
                        BookingStatus = MapMessageCategory(category),
                        TimeLimit = DateTime.MinValue
                    }));
                }
                else
                {
                    if (response.Errors.Any())
                    {
                        if (retry <= 3 && 
                            (response.Errors.Select(error => error.Code).Contains("ERMQU001") ||
                            response.Errors.Select(error => error.Code).Contains("ERMQU003")))
                        {
                            Client.CreateSession();
                            request.SessionId = Client.SessionId;
                            retry++;
                            done = false;
                        }
                        else
                        {
                            if (response.Errors.Select(error => error.Code).Contains("ERMQU001") ||
                                response.Errors.Select(error => error.Code).Contains("ERMQU003"))
                            {
                                //TODO Flight Error logging
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static void RemoveQueues(IEnumerable<BookingStatusInfo> infos)
        {
            var items = infos.Select(info => new Item
            {
                UniqueId = info.BookingId,
                CategoryId = MapMessageCategory(info.BookingStatus),
                ExtensionData = null
            }).ToArray();
            var request = new AirRemoveMessageQueueRQ
            {
                Items = items,
                SessionId = Client.SessionId,
                Target = Client.Target,
                ExtensionData = null
            };
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.RemoveMessageQueues(request);
                done = true;
                if (response.Success && !response.Errors.Any())
                {

                }
                else
                {
                    if (response.Errors.Any())
                    {
                        if (retry <= 3 && 
                            (response.Errors.Select(error => error.Code).Contains("ERRMQ001") ||
                            response.Errors.Select(error => error.Code).Contains("ERRMQ006")))
                        {
                            Client.CreateSession();
                            request.SessionId = Client.SessionId;
                            retry++;
                            done = false;
                        }
                        else
                        {
                            if (response.Errors.Select(error => error.Code).Contains("ERRMQ001") ||
                                response.Errors.Select(error => error.Code).Contains("ERRMQ006"))
                            {
                                //TODO Flight Error logging
                            }
                        }
                    }
                }
            }
        }

        private static BookingStatus MapMessageCategory(QueueCategory category)
        {
            switch (category)
            {
                case QueueCategory.Ticketed:
                    return BookingStatus.Ticketed;
                case QueueCategory.ScheduleChange:
                    return BookingStatus.ScheduleChanged;
                default:
                    return BookingStatus.Undefined;
            }
        }

        private static QueueCategory MapMessageCategory(BookingStatus status)
        {
            switch (status)
            {
                case BookingStatus.Ticketed:
                    return QueueCategory.Ticketed;
                case BookingStatus.ScheduleChanged:
                    return QueueCategory.ScheduleChange;
                default:
                    return QueueCategory.Urgent;
            }
        }
    }
}
