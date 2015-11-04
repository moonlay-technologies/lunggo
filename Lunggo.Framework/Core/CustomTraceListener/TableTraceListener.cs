using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.Framework.Core.CustomTraceListener
{
    public class TableTraceListener : TraceListener
    {
        #region Fields      

        private readonly string _connectionString;
        private readonly string _diagnosticsTable = "CustomLogTable";

        [ThreadStatic] private static StringBuilder _messageBuffer;

        private readonly object _initializationSection = new object();
        private bool _isInitialized;

        private CloudTableClient _tableStorage;
        private readonly object _traceLogAccess = new object();
        private readonly List<LogEntry> _traceLog = new List<LogEntry>();

        #endregion

        #region Constructors

        public TableTraceListener()
            : this("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString")
        {
        }

        public TableTraceListener(string connectionString)
            : base("TableTraceListener")
        {
            _connectionString = connectionString;
        }

        public TableTraceListener(string connectionString, string tableName)
            : base("TableTraceListener")
        {
            _connectionString = connectionString;
            _diagnosticsTable = tableName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Flushes the entries to the storage table
        /// </summary>
        public override void Flush()
        {
            if (!_isInitialized)
            {
                lock (_initializationSection)
                {
                    if (!_isInitialized)
                    {
                        Initialize();
                    }
                }
            }

            var context = _tableStorage.GetTableReference(_diagnosticsTable);
            //TableOperation.InsertOrReplace(ObjectParam)
            lock (_traceLogAccess)
            {
                _traceLog.ForEach(entry => context.Execute(TableOperation.InsertOrReplace(entry)));
                _traceLog.Clear();
            }

            //if (context.Entities.Count > 0)
            //{
            //    context.BeginSaveChangesWithRetries(SaveChangesOptions.None,
            //        (ar) => context.EndSaveChangesWithRetries(ar), null);
            //}
        }

        /// <summary>
        /// Creates the storage table object
        /// </summary>
        private void Initialize()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableStorage = account.CreateCloudTableClient();
            _tableStorage.GetTableReference(_diagnosticsTable).CreateIfNotExists();
            _isInitialized = true;
        }

        public override bool IsThreadSafe
        {
            get { return true; }
        }

        #region Trace and Write Methods

        /// <summary>
        /// Writes the message to a string buffer
        /// </summary>
        /// <param name="message">the Message</param>
        public override void Write(string message)
        {
            if (_messageBuffer == null)
                _messageBuffer = new StringBuilder();

            _messageBuffer.Append(message);
        }

        /// <summary>
        /// Writes the message with a line breaker to a string buffer
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            if (_messageBuffer == null)
                _messageBuffer = new StringBuilder();

            _messageBuffer.AppendLine(message);
        }

        /// <summary>
        /// Appends the trace information and message
        /// </summary>
        /// <param name="eventCache">the Event Cache</param>
        /// <param name="source">the Source</param>
        /// <param name="eventType">the Event Type</param>
        /// <param name="id">the Id</param>
        /// <param name="message">the Message</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);
            AppendEntry(id, eventType, eventCache, source);
        }

        /// <summary>
        /// Adds the trace information to a collection of LogEntry objects
        /// </summary>
        /// <param name="id">the Id</param>
        /// <param name="eventType">the Event Type</param>
        /// <param name="eventCache">the EventCache</param>
        private void AppendEntry(int id, TraceEventType eventType, TraceEventCache eventCache, string source)
        {
            if (_messageBuffer == null)
                _messageBuffer = new StringBuilder();

            var message = _messageBuffer.ToString();
            _messageBuffer.Length = 0;

            if (message.EndsWith(Environment.NewLine))
                message = message.Substring(0, message.Length - Environment.NewLine.Length);

            if (message.Length == 0)
                return;

            var entry = new LogEntry
            {
                PartitionKey = string.Format("{0:D10}", eventCache.Timestamp >> 30),
                RowKey = string.Format("{0:D19}", eventCache.Timestamp),
                EventTickCount = eventCache.Timestamp,
                Level = (int) eventType,
                EventId = id,
                Pid = eventCache.ProcessId,
                Tid = eventCache.ThreadId,
                Role = source,
                Message = message
            };
            lock (_traceLogAccess)
                _traceLog.Add(entry);
        }

        #endregion

        #endregion
    }

    //public class LogEntry : TableServiceEntity//, Obsolate
    public class LogEntry : TableEntity
    {
        public long EventTickCount { get; set; }
        public int Level { get; set; }
        public int EventId { get; set; }
        public int Pid { get; set; }
        public string Tid { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
    }
}
