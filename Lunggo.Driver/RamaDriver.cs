using System.Data;
using System.Data.SqlClient;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;     
using System.IO;
using System.Drawing;
using Lunggo.Framework.Http;
using Lunggo.Framework.Http.Rest;
using Lunggo.Framework.Util;
using Microsoft.WindowsAzure.Storage;
using StackExchange.Redis;


namespace Lunggo.Driver
{
    class RamaDriver
    {
        static void Main(string[] args)
        {
            //TestWhere();
            //TestTakeAndSkip();
            //TestDistinct();
            //TestSelect();
            //TestHttp();
            //TestDynamic();
            //TestDB1();
            //TestSnowMaker();
            //TestDB1();
            //TestQueue();
            //TestLoop();
            //TestRedis();

            const string hotelImageDirectory = "images/hotels";
            var isHttps = true;
            var urlBuilder = new UriBuilder
            {
                Path = hotelImageDirectory + "/" + "dodol.jpg",
                Host = "services.carsolize.com",
                Scheme = isHttps ? "https" : "http"
            };
            Console.WriteLine(urlBuilder.Uri.ToString());
        }

        static void TestRedis()
        {
            var connection = ConnectionMultiplexer.Connect("lunggodev.redis.cache.windows.net,ssl=true,password=3Zl2hElizwSap5pKp8xGX1s2vvvNsxeBW6DDErPYbIU=");
            var cache = connection.GetDatabase();
            cache.StringSet("rama", "adhitia");
            cache.StringSet("dodol", "garut");
            //cache.StringSet("runi", "anggia", TimeSpan.FromSeconds(60));

            var key1 = cache.StringGet("rama");
            var key2 = cache.StringGet("dodol");
            var key3 = (String)cache.StringGet("runi");

            var valueList = cache.StringGet(new RedisKey[] { "rama", "dodol", "runi" });
            Console.WriteLine("{0} {1} {2}",key1,key2,key3 ?? "null masa dong");
            foreach (var value in valueList)
            {
                Console.WriteLine(value.ToString() ?? "masa null sih");
            }
        }

        
    

        static void TestLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static void TestQueue()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var account = CloudStorageAccount.Parse(connectionString);
            var queueClient = account.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("testqueue");

            //queue.DeleteIfExists();
            queue.CreateIfNotExists();
            


            var message = new CloudQueueMessage("Hello, World 1");
            queue.AddMessage(message);
            message = new CloudQueueMessage("Hello, World 2");
            queue.AddMessage(message);

            /*
            for (int i = 0; i < 2; i++)
            {
                var retrievedMessage = queue.GetMessage();
                
                Console.WriteLine(retrievedMessage.AsString);
                //queue.DeleteMessage(retrievedMessage);
            }
            */
            

        }


        static void TestDB1()
        {
            const string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=""C:\Users\Rama.Adhitia\documents\visual studio 2013\Projects\Lunggo\Lunggo.Driver\dodol.mdf"";Integrated Security=True;";
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var simplyRedList = SampleQuery.GetInstance().Execute(con, null);

                foreach (var simplyRed in simplyRedList)
                {
                    Console.WriteLine("{0} {1}",simplyRed.FirstName,simplyRed.LastName);
                }
            }
        }

        static void TestDB()
        {

            String connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=""D:\Bayu\Lunggo\Lunggo.Driver\dodol.mdf"";Integrated Security=True";
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                //TestInsert(con);
                TestSelect(con);
                //try
                //{
                //    TableCobaSemuaTableRecord cobarecord = TableCobaSemuaTableRecord.CreateNewInstance();
                //    TableCobaSemuaRepository repo = TableCobaSemuaRepository.GetInstance();
                //    var get = repo.FindAll(con);
                //    Console.WriteLine("update {0} records successfully", get);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex);
                //}
                /* 
                try
                {
                    newPersonRecord = PersonTableRecord.CreateNewInstance();
                    newPersonRecord.PersonID = 71;
                    var delete = repo.Delete(con, newPersonRecord);
                    Console.WriteLine("delete {0} records successfully", delete);
                    newPersonRecord.PersonID = 72;
                    delete = repo.Delete(con, newPersonRecord);
                    Console.WriteLine("delete {0} records successfully", delete);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                 */ 

                /*
                try
                {
                    newPersonRecord.FirstName = "Amar";
                    newPersonRecord.LastName = "Dithia";
                    newPersonRecord.EnrollmentDate = new DateTime(1970,12,1);
                    newPersonRecord.HireDate = new DateTime(1970, 12, 2);
                    var update = repo.Update(con, newPersonRecord);
                    Console.WriteLine("update {0} record successfully", update);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                

                try
                {
                    newPersonRecord.PersonID = 134;
                    var delete = repo.Delete(con, newPersonRecord);
                    Console.WriteLine("delete {0} record successfully", delete);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                try
                {
                    List<PersonTableRecord> personList = repo.FindAll(con).ToList();
                    foreach (var p in personList)
                    {
                        Console.Write("{0} {1} {2} {3} {4}", p.PersonID, p.FirstName, p.LastName, p.HireDate, p.EnrollmentDate);
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {   
                    Console.WriteLine(ex);
                }

                try
                {
                    var delete = repo.DeleteAll(con);
                    Console.WriteLine("delete {0} record successfully", delete);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                
                 */
                con.Close();
            }
            Console.WriteLine("Finished");
                  
        }

        static void TestInsert(IDbConnection con)
        {
            try
            {
                var repo = PersonTableRepo.GetInstance();
                var insert = 0;
                var newPersonRecord = PersonTableRecord.CreateNewInstance();
                newPersonRecord.PersonID = 78;
                newPersonRecord.FirstName = "Jon";
                newPersonRecord.LastName = "Connington";
                newPersonRecord.EnrollmentDate = new DateTime(2013, 12, 2);
                newPersonRecord.HireDate = new DateTime(2012, 12, 1);
                newPersonRecord.LalaDate = new DateTime(2012, 12, 3);
                insert = repo.Insert(con, newPersonRecord);
                Console.WriteLine("insert {0} records successfully", insert);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }    
        }

        static void TestSelect(IDbConnection con)
        {
            try
            {
                var repo = PersonTableRepo.GetInstance();
                List<PersonTableRecord> personList = repo.FindAll(con).ToList();
                foreach (var p in personList)
                {
                    var iRecord = p.AsInterface();
                    Console.Write("{0} {1} {2} {3} {4} {5} {6} {7}", p.PersonID, p.FirstName, p.LastName, p.HireDate, p.EnrollmentDate, iRecord.ManuallyCreated, iRecord.IsSet("FirstName"), iRecord.IsSet("LalaDate"));
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void TestDynamic()
        {
            JustOnlyAPoco poco = new JustOnlyAPoco
            {
                LastName = "Adhitia",
                FirstName = "Rama",
                HireDate = new DateTime(2014, 1, 1),
                EnrollmentDate = new DateTime(2014,3,8)
            };

            var obj = TypeUtil.ToAnonymousType(poco);
            Console.WriteLine(obj.LastName);
            Console.WriteLine(obj.FirstName);
            Console.WriteLine(obj.HireDate);
            Console.WriteLine(obj.EnrollmentDate);
        }

        static void TestHttp()
        {
            Http http = new Http();
            IRestRequest request = new RestRequest();
            SetupHttp(http);
            SetupRequestPost(request);
            IHttpRequest httpRequest = HttpRequest.CreateHttpRequest(request);
            IHttpResponse response = http.SendRequest(httpRequest);
            Console.WriteLine(response.RawString);
        }

        static void SetupRequest(IRestRequest request)
        {
            request.RequestUri = "/14yxzql1";
            request.Method = Framework.Http.HttpMethod.GET;
            request.AddParameter("stayDay", "9");
            request.AddHeader(HttpRequestHeaderConstant.UserAgent, "Rama Http Client");
            request.AddHeader("rama-special-request-header","heyhoo");
        }

        static void SetupRequestPost(IRestRequest request)
        {
            String jsonValue =
                @"{
    ""key"": ""hzWYbZ1BJPyLp90ng26b1Q"",
    ""message"": {
        ""html"": ""<p>Hello Mandrill</p>"",
        ""subject"": ""Async 5 Message from Moonlay.com"",
        ""from_email"": ""Rama.Adhitia@moonlay.com"",
        ""from_name"": ""Rama Adhitia"",
        ""to"": [
			{
                ""email"": ""ramaadhitia@yahoo.com"",
                ""name"": ""Rama Adhitia"",
                ""type"": ""to""
            },
            {
                ""email"": ""if15040@gmail.com"",
                ""name"": ""Rama Adhitia If15040"",
                ""type"": ""cc""
            }
        ],
        ""headers"": {
            ""Reply-To"": ""Rama.Adhitia@moonlay.com""
        },
        ""important"": true,
        ""track_opens"": true,
        ""track_clicks"": true,
        ""auto_text"": false,
        ""auto_html"": false,
        ""inline_css"": true,
        ""url_strip_qs"": false,
        ""preserve_recipients"": true,
        ""view_content_link"": true,
        ""bcc_address"": null,
        ""tracking_domain"": null,
        ""signing_domain"": null,
        ""return_path_domain"": null,
        ""merge"": false,
        ""global_merge_vars"": null,
        ""merge_vars"": null,
        ""tags"": [
            ""test-email""
        ],
        ""subaccount"": null,
        ""google_analytics_domains"": null,
        ""google_analytics_campaign"": null,
        ""metadata"": {
            ""website"": ""www.moonlay.com""
        },
        ""recipient_metadata"": null,
        ""attachments"": null,
        ""images"": null
    },
    ""async"": true,
    ""ip_pool"": null,
    ""send_at"": null
}";
            byte[] byteArray = new byte[] { (byte)65, (byte)66, (byte)67 };
            request.RequestUri = "/14yxzql1";
            request.Method = Framework.Http.HttpMethod.POST;
            //request.AddHeader(HttpRequestHeaderConstant.UserAgent, "Rama Http Client");
            //request.AddHeader("rama-special-request-header", "heyhoo");
            request.AddBody(jsonValue, Encoding.UTF8, "text/json");
            /*
            request.AddFile("file_upload", "dodol.jpg", byteArray, "application/octet-stream");
            request.AddParameter("stayDay", "9");
            request.AddParameter("name", "Rama Adhitia");
            request.AddParameter("mailAddress", "ramaadhitia@gmail.com");
            */
        }

        static void SetupHttp(Http http)
        {
            http.Timeout = 10000;
            //http.CookieContainer = null;
            http.BaseAddress = "http://requestb.in";
            //http.Proxy = null;
        }

        static void TestSelect()
        {
            DirectoryInfo[] dirs = new DirectoryInfo(@"C:\Users\Rama.Adhitia\Box Sync\ebook").GetDirectories(); 
            IEnumerable<String> fluent1 = FontFamily.Families.Select(n => n.Name);
            String[] names = { "Tom", "Dick", "Harry", "Mary", "Jay"};
            var fluent2 = FontFamily.Families.Where((f) => f.IsStyleAvailable(FontStyle.Strikeout)).Select(n => new { n.Name, LineSpacing = n.GetLineSpacing(FontStyle.Bold) });
            var fluent3 =
                dirs.Where((d) => (d.Attributes & FileAttributes.System) == 0)
                    .Select(
                        (n) => new
                        {
                            DirectoryName = n.FullName,
                            Created = n.CreationTime,

                            Files =
                                n.GetFiles().Where((f) => (f.Attributes & FileAttributes.System) == 0)
                                            .Select(
                                                (f) => new
                                                {
                                                    Filename = f.Name,
                                                    f.Length
                                                }
                                            )
                        }
                    );
           var fluent4 = names.SelectMany( (n) =>
                    {
                        IList<String> list = new List<String>();
                        foreach(var s in names)
                        {
                            list.Add(n + " vs " + s);
                        }
                        return list;
                    }
               );               


            foreach(String s in fluent1)
            {
                Console.WriteLine("{0}", s);
            }
            OutputDelimiter();
            foreach(var s in fluent2)
            {
                Console.WriteLine("{0} {1}", s.Name, s.LineSpacing);
            }
            OutputDelimiter();
            foreach (var s in fluent3)
            {
                Console.WriteLine("{0}", s.DirectoryName);
                foreach(var n in s.Files)
                {
                    Console.WriteLine("{0} {1}", n.Filename, n.Length);
                }
                OutputDelimiter();
            }

            foreach(var s in fluent4)
            {
                Console.WriteLine(s);
            }

        }

        static void TestTakeAndSkip()
        {
            int[] numbers = { 3, 5, 2, 234, 4, 1 };
            var takeWhileSmall = numbers.TakeWhile((n) => n < 100);

            foreach(int angka in takeWhileSmall)
            {
                Console.WriteLine("{0}", angka);
            }
            OutputDelimiter();
        }

        static void TestDistinct()
        {
            char[] distinctLetters = "HelloWorld".Distinct().ToArray();
            String s = new String(distinctLetters);
            Console.WriteLine("{0}", s);
        }

        static void TestWhere()
        {
            String[] names = { "Tom", "Dick", "Harry", "Mary", "Jay"};
            
            IEnumerable<String> fluent1 = names.Where((n) => n.EndsWith("k"));
            IEnumerable<String> fluent2 = names.Where((n,i) => (i % 2) == 0);
            IEnumerable<String> query1 = from n in names
                                        where n.EndsWith("k")
                                        select n;
            IEnumerable<String> query2 = from n in names
                                         where n.Length > 3
                                         let u = n.ToUpper()
                                         select u;

            foreach(String s in fluent1)
            {
                Console.WriteLine("{0}", s);
            }
            OutputDelimiter();
            foreach(String s in fluent2)
            {
                Console.WriteLine("{0}", s);
            }
            OutputDelimiter();
            foreach (String s in query1)
            {
                Console.WriteLine("{0}", s);
            }
            OutputDelimiter();
            foreach (String s in query2)
            {
                Console.WriteLine("{0}", s);
            }

        }

        static void OutputDelimiter()
        {
            Console.WriteLine("=======================================================");
        }

        static void TestFormUrlEncodedContent()
        {
            using (var client = new HttpClient())
            {
                String baseAddress = "http://requestb.in";
                String addressSegment = "/10w2i7h1";
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromMinutes(5);
               

                var values = new[]
                {
                    new KeyValuePair<String,String>("username","ramaadhitia@gmail.com"),
                    new KeyValuePair<String,String>("password","lalalala")
                };
                
                var result = client.PostAsync(addressSegment, new FormUrlEncodedContent(values)).Result;
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
                
            }
        }

        static void TestMultipartContent()
        {
            using (var client = new HttpClient())
            {
                String baseAddress = "http://requestb.in";
                String addressSegment = "/10w2i7h1";
                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = TimeSpan.FromMinutes(5);

                using (var content = new MultipartFormDataContent())
                {
                    var values = new[]
                    {
                        new KeyValuePair<String,String>("username","ramaadhitia@gmail.com"),
                        new KeyValuePair<String,String>("password","lalalala")
                    };
                    
                    String filePath = @"C:\Users\Rama.Adhitia\Pictures\lili.png";
                    String filePath2 = @"C:\Users\Rama.Adhitia\Pictures\lala.jpg";
                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }

                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath));
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        FileName = Path.GetFileName(filePath),
                        Name = Path.GetFileNameWithoutExtension(filePath)
                    };
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    content.Add(fileContent);

                    fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(filePath2));
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        FileName = Path.GetFileName(filePath2),
                        Name = Path.GetFileNameWithoutExtension(filePath2)
                    };
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    content.Add(fileContent);

                    var result = client.PostAsync(addressSegment, content).Result;
                    Console.WriteLine(result.Content.ReadAsStringAsync().Result);
                }
            }
        }
    }

    class Dummy
    {
        public List<HttpFile> Files { get; set; }
    }

    class JustOnlyAPoco
    {
        public String LastName {get; set;}
        public String FirstName {get; set;}
        public DateTime HireDate {get; set;}
        public DateTime EnrollmentDate {get; set;}
    }


    public class SimplyRed
    {
        private bool _firstChanged = false;
        private bool _lastChanged = false;

        private String _firstName;
        private String _lastName;

        private SimplyRed()
        {
            ;
        }

        public static SimplyRed GetNewInstance()
        {
            return new SimplyRed();
        }

        public String FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                _firstChanged = true;
            }
        }

        public String LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                _lastChanged = true;
            }
        }

        public bool GetFirstChanged()
        {
            return _firstChanged;
        }

        public bool GetLastChanged()
        {
            return _lastChanged;
        }
    }

    public class SampleQuery : QueryBase<SampleQuery,SampleQueryRecord>
    {
        private SampleQuery()
        {
            
        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT FirstName,LastName FROM Person");
            return queryBuilder.ToString();
        }
    }

    public class SampleQueryRecord : QueryRecord
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}
