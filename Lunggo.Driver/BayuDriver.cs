using Lunggo.Framework.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Driver
{
    class BayuDriver
    {
        static void Main(string[] args)
        {
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
