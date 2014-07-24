using Lunggo.Flight.Crawler;
using Lunggo.Flight.Model;
using Lunggo.Framework.Blob;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Lunggo.Driver
{
    class BayuDriver
    {
        static void Main(string[] args)
        {

            TicketSearch SearchParam = new TicketSearch();
            SearchParam.IsReturn = true;
            SearchParam.DepartFromCode = "CGK";
            SearchParam.DepartToCode = "DPS";
            SearchParam.DepartDate = new DateTime(2014, 8, 25);
            SearchParam.ReturnDate = new DateTime(2014, 8, 28);
            SearchParam.Adult = 1;
            SearchParam.Child = 1;
            SearchParam.Infant = 1;
            
            ICrawler AirAsiaCrawler = new AirAsiaCrawler();
            ICrawler CitilinkCrawler = new CitilinkCrawler();
            ICrawler SriwijayaCrawler = new SriwijayaCrawler();
            ICrawler LionAirCrawler = new LionAirCrawler();
            List<FlightTicket> ListTicket = new List<FlightTicket>();

            Stopwatch LionAirStopWatch = new Stopwatch();
            Stopwatch AirAsiaStopWatch = new Stopwatch();
            Stopwatch SriwijayaStopWatch = new Stopwatch();
            Stopwatch CitilinkStopWatch = new Stopwatch();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //LionAirStopWatch.Start();
            //List<FlightTicket> ListTicketLionAir = LionAirCrawler.Search(SearchParam);
            //LionAirStopWatch.Stop();

            //AirAsiaStopWatch.Start();
            //List<FlightTicket> ListTicketAirAsia = AirAsiaCrawler.Search(SearchParam);
            //AirAsiaStopWatch.Stop();

            //SriwijayaStopWatch.Start();
            //List<FlightTicket> ListTicketSriwijaya = SriwijayaCrawler.Search(SearchParam);
            //SriwijayaStopWatch.Stop();

            CitilinkStopWatch.Start();
            List<FlightTicket> ListTicketCitilink = CitilinkCrawler.Search(SearchParam);
            CitilinkStopWatch.Stop();

            stopWatch.Stop();

            //ListTicket.AddRange(ListTicketLionAir);
            //ListTicket.AddRange(ListTicketAirAsia);
            //ListTicket.AddRange(ListTicketSriwijaya);
            //ListTicket.AddRange(ListTicketCitilink);


            ListTicket.Sort(delegate(FlightTicket c, FlightTicket b)
            {
                if (c.ListDepartDetail.First().DepartTime == null && b.ListDepartDetail.First().DepartTime == null) return 0;
                else if (c.ListDepartDetail.First().DepartTime == null) return -1;
                else if (b.ListDepartDetail.First().DepartTime == null) return 1;
                else return c.ListDepartDetail.First().DepartTime.CompareTo(b.ListDepartDetail.First().DepartTime);
            });
            //ListTicket = ListTicket.OrderBy(x => x.ListDepartDetail.First().DepartTime).ToList();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            TimeSpan lioair = LionAirStopWatch.Elapsed;
            TimeSpan airasia = AirAsiaStopWatch.Elapsed;
            TimeSpan sriwijaya = SriwijayaStopWatch.Elapsed;
            TimeSpan citilink = CitilinkStopWatch.Elapsed;
            int a = 1;

            //new BayuDriver().getBlobsFromContainer();
        }
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
