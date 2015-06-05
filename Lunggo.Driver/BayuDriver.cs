using System.CodeDom;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.ModelBinding;
using Lunggo.Flight.Crawler;
using Lunggo.Flight.Model;
using Lunggo.Framework.Blob;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Payment.Data;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.SnowMaker;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZendeskApi_v2.Models.AccountsAndActivities;
using ZendeskApi_v2.Models.Constants;
using ZendeskApi_v2.Models.Tickets;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Driver
{
    class BayuDriver
    {

        static void Main(string[] args)
        {

            Console.WriteLine( Regex.IsMatch("-100.00", @"^(-)?\d+(\.\d\d)?$"));
            Regex rex = new Regex(@"^\d+(\.\d\d)?$");
            Console.WriteLine(rex.IsMatch("-100.00"));
            TryNameValueCOllection();
            //new BayuDriver().Init();
            //new BayuDriver().testDeserialize();
            //new BayuDriver().testasync().Start();

            //var evaluator = new Regex("",RegexOptions.Compiled);

            Console.ReadLine();


            //new BayuDriver().getBlobsFromContainer();
        }
        public class CustomData
        {
            public long CreationTime;
            public int Name;
            public int ThreadNum;
        }

        public static void TryPerformanceCounter()
        {
            if (!PerformanceCounterCategory.Exists("test"))
            {
                var counters = new CounterCreationDataCollection();
                var ccdCounter1 = new CounterCreationData()
                {
                    CounterName = "counter1",
                    CounterType = PerformanceCounterType.SampleFraction
                };
                counters.Add(ccdCounter1);
                PerformanceCounterCategory.Create("test", "Help String", PerformanceCounterCategoryType.SingleInstance,
                    counters);



            }
            var perform = PerformanceCounterCategory.GetCategories("test");
            
        }
        //public static void WriteObjectContentInDocument(string path)
        //{
        //    // Create the object to serialize.
        //    Person p = new Person("Lynn", "Tsoflias", 9876);

        //    // Create the writer object.
        //    FileStream fs = new FileStream(path, FileMode.Create);
        //    XmlDictionaryWriter writer =
        //        XmlDictionaryWriter.CreateTextWriter(fs);

        //    DataContractSerializer ser =
        //        new DataContractSerializer(typeof(Person));

        //    // Use the writer to start a document.
        //    writer.WriteStartDocument(true);

        //    // Use the writer to write the root element.
        //    writer.WriteStartElement("Company");

        //    // Use the writer to write an element.
        //    writer.WriteElementString("Name", "Microsoft");

        //    // Use the serializer to write the start, 
        //    // content, and end data.
        //    ser.WriteStartObject(writer, p);
        //    ser.WriteObjectContent(writer, p);
        //    ser.WriteEndObject(writer);

        //    // Use the writer to write the end element and 
        //    // the end of the document.
        //    writer.WriteEndElement();
        //    writer.WriteEndDocument();

        //    // Close and release the writer resources.
        //    writer.Flush();
        //    fs.Flush();
        //    fs.Close();
        //}
        public static void TryCodeTypeDeclaration()
        {
            var ctd = new CodeTypeDeclaration("sd");
            ctd.Attributes = MemberAttributes.Public;
            ctd.IsStruct = true;
            ctd.IsClass = true;
        }

        public static void TryNameValueCOllection()
        {
            var nvc = new NameValueCollection() {{"a", 1.ToString()}};
            List<DateTime?> ldt = new List<DateTime?>();
            DateTime? dt = DateTime.Now;
            ldt.Add(dt);

                int i3 = 2147483647;
                Console.WriteLine(i3);
        }

        public static void TryCatch()
        {
            try
            {
                List<string> strings = new List<string>();
                Task[] taskArray = new Task[10];
                int nomorTest = 0;
                object lockob =  new object();
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew((Object obj) =>
                    {
                        var data = new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks };
                        data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                        Thread.Sleep(100);
                        lock (lockob)
                        {
                            nomorTest += 1;
                        strings.Add(string.Format ("Task #{0} created at {1} on thread #{2}. task id = {3}. nomor Test = {4}",
                                          data.Name, data.CreationTime, data.ThreadNum, Task.CurrentId, nomorTest));
                        }
                        
                    },
                                                         i);
                }
                Task.WhenAll(taskArray).ContinueWith(task => Console.WriteLine("endline"));
                Task.WaitAll(taskArray);
                foreach (string st in (from list in strings select list).AsParallel())
                {
                    Console.WriteLine(st);
                }
                var parent = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Parent task beginning.");
                    Task[] taskArray2 = new Task[10];
                    for (int i = 0; i < taskArray.Length; i++)
                    {
                        taskArray2[i] = Task.Factory.StartNew((Object obj ) =>
                        {
                            var data = new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks };
                            data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                            Console.WriteLine("Task #{0} created at {1} on thread #{2}. task id = {3}",
                                              data.Name, data.CreationTime, data.ThreadNum, Task.CurrentId);
                        }, i, TaskCreationOptions.AttachedToParent);
                    }

                    Task.WhenAny(taskArray2).ContinueWith(task => Console.WriteLine("ada yang selesai"));
                    //Task.WaitAll(taskArray2);
                });

                parent.Wait();
                Console.WriteLine("Parent task completed.");
                //XmlWriterTraceListener listener = new XmlWriterTraceListener("error.log");
                //listener.WriteLine("error log");
                //listener.Flush();
                //listener.Close();

                //TraceSource trace = new TraceSource("traceClass");
                //trace.Listeners.Add(listener);
                //trace.TraceTransfer(1,"",new Guid());

                //trace.TraceEvent(TraceEventType.Start, 1);

                //CancellationToken token = new CancellationToken(){};
                //token.ThrowIfCancellationRequested();
                //throw new ArgumentNullException();
            }
            catch (ArgumentNullException ex)
            {

                int.Parse("ds");
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Init()
        {
            InitConfigurationManager();
            InitUniqueIdGenerator();
            InitDatabaseService();
            InitTicketService();
            InitQueueService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            string serverMapPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName +"\\Config\\";
            var configDirectoryPath = serverMapPath;
            configManager.Init(configDirectoryPath);
        }


        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = ConfigManager.GetInstance().GetConfigValue("general", "seqGeneratorContainerName");
            var storageConnectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            var optimisticData = new BlobOptimisticDataStore(CloudStorageAccount.Parse(storageConnectionString), seqContainerName)
            {
                SeedValueInitializer = (sequenceName) => generator.GetIdInitialValue(sequenceName)
            };
            generator.Init(optimisticData);
            generator.BatchSize = 100;
        }

        private static void InitDatabaseService()
        {
            var database = DbService.GetInstance();
            database.Init();
        }
        private static void InitQueueService()
        {
            var queue = QueueService.GetInstance();
            queue.Init();
        }
        private static void InitTicketService()
        {
            var apiKey = ConfigManager.GetInstance().GetConfigValue("zendesk", "apikey");
            ITicketSupportClient ticket = new ZendeskTicketClient();
            ticket.init(apiKey);
            var TicketService = TicketSupportService.GetInstance();
            TicketService.Init(ticket);
        }
        public async Task testasync()
        {
            Task<string> s = testTaskString();
            string a = s.Result;
            Thread.Sleep(3000);
            Thread.Sleep(3000);
            Thread.Sleep(3000);
            Thread.Sleep(3000);
            Thread.Sleep(3000);
        }

        private async Task<string> testTaskString()
        {
            Thread.Sleep(3000);
            return "";
        }

        public void testAddQueue()
        {
            var queueService = QueueService.GetInstance();
            var _queue = queueService.GetQueueByReference("apibookingfailed");
            _queue.CreateIfNotExists();

            for (int i = 0; i < 10; i++)
            {
                BookingDetail TestClass = new BookingDetail();
                TestClass.Name = "nama"+i;
                TestClass.Email = "Email"+i;
                TestClass.DynamicTesting = 1;
                _queue.AddMessage(AzureQueueExtension.Serialize(TestClass));
            }
        }
        public class Order
        {
            public string Name { get; set; }

            public string OrderId { get; set; }
        }

        public void testQueueEmailHandler()
        {
            BookingDetail TestClass = new BookingDetail();
            TestClass.Name = "nama";
            TestClass.Email = "Email@email.com";
            TestClass.DynamicTesting = 1;


            List<Lunggo.Framework.SharedModel.FileInfo> files = new List<Lunggo.Framework.SharedModel.FileInfo>();
            files.Add(new Lunggo.Framework.SharedModel.FileInfo()
            {
                ContentType = "text/plain",
                FileName = "BayuDriver.cs",
                FileData =
                    File.ReadAllBytes(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName +
                          "\\BayuDriver.cs")
            });


            MailDetailForQueue testEmail = new MailDetailForQueue();
            testEmail.FromName = "Bayu";
            testEmail.FromMail = "bayualvian@hotmail.com";
            testEmail.RecipientList = new string[] { "Bayualvian@hotmail.com" };
            testEmail.Subject = "testing";
            testEmail.MailTemplate = MailTemplateEnum.TestHtmlWithAttachment;
            testEmail.MailObjectDetail = TestClass;
            testEmail.ListFileInfo = files;

            CloudQueueMessage TestCloudQueue = testEmail.SerializeToQueueMessage();
            var queueService = QueueService.GetInstance();
            var _queue = queueService.GetQueueByReference("emailqueue");
            _queue.CreateIfNotExists();
            _queue.AddMessage(TestCloudQueue);
        }

        public void testDeserialize()
        {
            TicketQueueMessage TestQueue = new TicketQueueMessage() { TicketPurpose = TicketPurpose.ApalagiGitu };
            BookingDetail TestClass = new BookingDetail();
            TestClass.Name = "nama";
            TestClass.Email = "Email@email.com";
            TestClass.DynamicTesting = 1;

            TestQueue.TicketObjectDetail = TestClass;

            //BookingDetail TestClass2 = new BookingDetail();
            //TestClass2.Name = "nama";
            //TestClass2.Email = "Email";
            //FileInfo Testticket = new FileInfo();
            //Testticket.FileName = "FileName";
            //Testticket.ContentType = "empty";
            //TestClass2.DynamicTesting = Testticket;



            CloudQueueMessage TestCloudQueue = TestQueue.SerializeToQueueMessage();
            var queueService = QueueService.GetInstance();
            var _queue = queueService.GetQueueByReference("ticketqueue");
            _queue.CreateIfNotExists();
            _queue.AddMessage(TestCloudQueue);
            //CloudQueueMessage TestCloudQueue2 = AzureQueueExtension.Serialize(TestClass2);
            var Hasil = JsonConvert.DeserializeObject<TicketQueueMessage>(TestCloudQueue.AsString);
            BookingDetail asd = (Hasil.TicketObjectDetail as JObject).ToObject<BookingDetail>();
            //var Hasil2 = TestCloudQueue2.Deserialize();
            Console.WriteLine(asd.DetailBooking);
            //Console.WriteLine(Hasil2.GetType());
        }
        
        public void testBooking()
        {
            //CIMBPaymentData data = new CIMBPaymentData();
            //data.PaymentType="cimb";


            //data.CustomerDetails.BillingAddress.Address="jalan";
            //data.CustomerDetails.BillingAddress.City="jakarta";
            //data.CustomerDetails.BillingAddress.CountryCode = "jakarta";
            //data.CustomerDetails.BillingAddress.Email = "jakarta";
            //data.CustomerDetails.BillingAddress.FirstName = "jakarta";
            //data.CustomerDetails.BillingAddress.LastName = "jakarta";
            //data.CustomerDetails.BillingAddress.Phone = "jakarta";
            //data.CustomerDetails.BillingAddress.PostalCode = "jakarta";

            //data.CustomerDetails.Email = "jakarta";
            //data.CustomerDetails.FirstName = "jakarta";
            //data.CustomerDetails.LastName = "jakarta";
            //data.CustomerDetails.Phone = "jakarta";

            //List<ItemDetail> ListItemDetailDummy = new List<ItemDetail>();


            //ItemDetail detailDummy = new ItemDetail();
            //detailDummy.Id ="123213";
            //detailDummy.Name="asdsad";
            //detailDummy.Price=123;
            //detailDummy.Quantity=123;

            //ItemDetail detailDummy2 = new ItemDetail();
            //detailDummy2.Id = "1232123";
            //detailDummy2.Name = "asdsad";
            //detailDummy2.Price = 123;
            //detailDummy2.Quantity = 123;

            //ListItemDetailDummy.Add(detailDummy);
            //ListItemDetailDummy.Add(detailDummy2);
            //data.ItemDetails = ListItemDetailDummy;

            //data.TransactionDetails.OrderId = "jakarta";
            //data.TransactionDetails.GrossAmount = 2;

            //data.CIMBClicks.Description = "jakarta";
            //string json = JsonConvert.SerializeObject(data);
            //string json2 = JsonConvert.SerializeObject(data.ConvertToDummyObject());
        }

        public void testCrawl()
        {
            //TicketSearch SearchParam = new TicketSearch();
            //SearchParam.IsReturn = true;
            //SearchParam.DepartFromCode = "CGK";
            //SearchParam.DepartToCode = "DPS";
            //SearchParam.DepartDate = new DateTime(2014, 8, 25);
            //SearchParam.ReturnDate = new DateTime(2014, 8, 28);
            //SearchParam.Adult = 1;
            //SearchParam.Child = 1;
            //SearchParam.Infant = 1;

            //ICrawler AirAsiaCrawler = new AirAsiaCrawler();
            //ICrawler CitilinkCrawler = new CitilinkCrawler();
            //ICrawler SriwijayaCrawler = new SriwijayaCrawler();
            //ICrawler LionAirCrawler = new LionAirCrawler();
            //List<FlightTicket> ListTicket = new List<FlightTicket>();

            //Stopwatch LionAirStopWatch = new Stopwatch();
            //Stopwatch AirAsiaStopWatch = new Stopwatch();
            //Stopwatch SriwijayaStopWatch = new Stopwatch();
            //Stopwatch CitilinkStopWatch = new Stopwatch();
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();

            //LionAirStopWatch.Start();
            //List<FlightTicket> ListTicketLionAir = LionAirCrawler.Search(SearchParam);
            //LionAirStopWatch.Stop();

            //AirAsiaStopWatch.Start();
            //List<FlightTicket> ListTicketAirAsia = AirAsiaCrawler.Search(SearchParam);
            //AirAsiaStopWatch.Stop();

            //SriwijayaStopWatch.Start();
            //List<FlightTicket> ListTicketSriwijaya = SriwijayaCrawler.Search(SearchParam);
            //SriwijayaStopWatch.Stop();

            //CitilinkStopWatch.Start();
            //List<FlightTicket> ListTicketCitilink = CitilinkCrawler.Search(SearchParam);
            //CitilinkStopWatch.Stop();

            //stopWatch.Stop();

            //ListTicket.AddRange(ListTicketLionAir);
            //ListTicket.AddRange(ListTicketAirAsia);
            //ListTicket.AddRange(ListTicketSriwijaya);
            //ListTicket.AddRange(ListTicketCitilink);


            //ListTicket.Sort(delegate(FlightTicket c, FlightTicket b)
            //{
            //    if (c.ListDepartDetail.First().DepartTime == null && b.ListDepartDetail.First().DepartTime == null) return 0;
            //    else if (c.ListDepartDetail.First().DepartTime == null) return -1;
            //    else if (b.ListDepartDetail.First().DepartTime == null) return 1;
            //    else return c.ListDepartDetail.First().DepartTime.CompareTo(b.ListDepartDetail.First().DepartTime);
            //});
            //ListTicket = ListTicket.OrderBy(x => x.ListDepartDetail.First().DepartTime).ToList();
            // Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;
            //TimeSpan lioair = LionAirStopWatch.Elapsed;
            //TimeSpan airasia = AirAsiaStopWatch.Elapsed;
            //TimeSpan sriwijaya = SriwijayaStopWatch.Elapsed;
            //TimeSpan citilink = CitilinkStopWatch.Elapsed;
            //int a = 1;
        }

        public void testTicket()
        {
            string test = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName +
                          "\\BayuDriver.cs";
            List<Lunggo.Framework.SharedModel.FileInfo> files = new List<Lunggo.Framework.SharedModel.FileInfo>();
            files.Add(new Lunggo.Framework.SharedModel.FileInfo()
            {
                ContentType = "text/plain",
                FileName = "BayuDriver.cs",
                FileData =
                    File.ReadAllBytes(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName +
                          "\\BayuDriver.cs")
            });
            files.Add(new Lunggo.Framework.SharedModel.FileInfo()
            {
                ContentType = "text/plain",
                FileName = "RamaDriver.cs",
                FileData =
                    File.ReadAllBytes(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName +
                          "\\RamaDriver.cs")
            });
            var ticket = new ZendeskTicket()
            {
                Subject = "ticket with attachments",
                Comment = new ZendeskComment() { Body = "testing requester" },
                Priority = TicketPriorities.Normal,
                Requester = new ZendeskRequester() { Email = "Bayualvian@hotmail.com", Name = "Bayu" }
            };
            var ticket2 = new ZendeskTicket()
            {
                Subject = "Failed booking attempt",
                Comment = new ZendeskComment() { Body = "Failed booking attempt for a member named:" + "nama" },
                Priority = TicketPriorities.Normal,
                Requester = new ZendeskRequester() { Email = "Bayualvian@hotmail.com", Name = "nama" }
            };
            TicketSupportService.GetInstance().CreateTicketAndReturnResponseStatus(ticket2);
            TicketSupportService.GetInstance().CreateTicketWithAttachmentAndReturnResponseStatus(ticket, files);
        }

        //public enum coba
        //{
        //    test = 1,
        //    test2 = 2
        //}

        //public string cobaEnum(coba tipe)
        //{
        //    object val = Convert.ChangeType(tipe, tipe.GetTypeCode());
        //    return val.ToString();
        //}
        /*void deleteBlob()
        {
            string fileName = "test/Capture.PNG";
            string containerName = "testdirectory/";
            new BlobStorageHandler().deleteBlob(@"http://127.0.0.1:10000/devstoreaccount1/testdirectory/test/Capture.PNG");
        }
        void getBlob()
        {
            string fileName = "test/Capture.PNG";
            string containerName = "testdirectory/";
            CloudBlob blob = new BlobStorageHandler().getBlobFromStorage(containerName + fileName);
            System.IO.Stream stream = new System.IO.MemoryStream();
            blob.DownloadToStream(stream);
            using(Stream inStream = stream)
            {
                using (Stream outStream = File.Create(@"C:\Users\bayu.alvian\Documents\Test2.ashx"))
                {
                    while (inStream.Position < inStream.Length)
                    {
                        outStream.WriteByte((byte)inStream.ReadByte());
                    }
                }
            }
        }
        void RenameBlobs()
        {
            string fileName = "file_41aba3e4-82d2-4376-ac1e-16be98286a7a.ashx";
            string containerName = "temp/";
            string containerName2 = "temp2/";
            new BlobStorageHandler().RenameBlobs(@"http://127.0.0.1:10000/devstoreaccount1/temp2/file_41aba3e4-82d2-4376-ac1e-16be98286a7a.ashx", @"http://127.0.0.1:10000/devstoreaccount1/temp/file_41aba3e4-82d2-4376-ac1e-16be98286a7a.ashx");
        }
        void getBlobsFromContainer()
        {
            foreach (var blob in new BlobStorageHandler().GetDirectoryList("testdirectory", ""))
            {
                Console.WriteLine(blob.Uri+"\n");
            }
            Console.ReadLine();
        }
         */
    }
}
